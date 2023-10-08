using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.Injection;
using Wing.Persistence.Saga;
using Wing.Saga.Grpc;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing.Saga.Server
{
    public class TranRetryHostedService : BackgroundService
    {
        private static readonly object _lock = new object();
        private readonly ILogger<TranRetryHostedService> _logger;
        private readonly IServiceFactory _serviceFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJson _json;
        private readonly SagaOptions _sagaOptions;
        private Timer _timer;
        private bool _wait = false;

        public TranRetryHostedService(ILogger<TranRetryHostedService> logger,
            IServiceFactory serviceFactory,
            IHttpClientFactory httpClientFactory,
            IJson json,
            SagaOptions sagaOptions)
        {
            _logger = logger;
            _serviceFactory = serviceFactory;
            _httpClientFactory = httpClientFactory;
            _json = json;
            _sagaOptions = sagaOptions;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saga重试服务开始运行...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            _logger.LogInformation("Saga重试服务停止运行...");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var retrySeconds = App.Configuration["Saga:RetrySeconds"];
            var seconds = string.IsNullOrWhiteSpace(retrySeconds) ? 300 : Convert.ToDouble(retrySeconds);
            _timer = new Timer(x =>
              {
                  lock (_lock)
                  {
                      if (_wait)
                      {
                          return;
                      }

                      _wait = true;
                      Scoped.Create(scoped =>
                      {
                          try
                          {
                              var sagaTranService = scoped.ServiceProvider.GetRequiredService<ISagaTranService>();
                              var unitService = scoped.ServiceProvider.GetRequiredService<ISagaTranUnitService>();
                              var failedTrans = sagaTranService.GetFailedData();
                              if (failedTrans == null || !failedTrans.Any())
                              {
                                  return;
                              }

                              foreach (var tran in failedTrans)
                              {
                                  try
                                  {
                                      Persistence.Saga.ResponseData result;
                                      if (tran.Policy == TranPolicy.Forward)
                                      {
                                          if (tran.BreakerCount > 0 && tran.CommittedCount >= tran.BreakerCount)
                                          {
                                              continue;
                                          }

                                          result = Commit(tran, unitService);
                                      }
                                      else if (tran.Policy == TranPolicy.Backward)
                                      {
                                          if (tran.BreakerCount > 0 && tran.CancelledCount >= tran.BreakerCount)
                                          {
                                              continue;
                                          }

                                          result = Cancel(tran, unitService);
                                      }
                                      else if (tran.Policy == TranPolicy.Custom)
                                      {
                                          if (tran.BreakerCount > 0 && tran.CancelledCount >= tran.BreakerCount)
                                          {
                                              continue;
                                          }

                                          if (tran.CustomCount > tran.CommittedCount)
                                          {
                                              result = Commit(tran, unitService);
                                          }
                                          else
                                          {
                                              result = Cancel(tran, unitService);
                                          }
                                      }
                                  }
                                  catch (Exception ex)
                                  {
                                      _logger.LogError(ex, "Saga重试服务执行异常");
                                  }
                              }
                          }
                          catch (Exception ex)
                          {
                              _logger.LogError(ex, "Saga重试服务执行异常");
                          }
                          finally
                          {
                              _wait = false;
                          }
                      });
                  }
              }, null, TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(seconds));
            return Task.CompletedTask;
        }

        private Grpc.RetryData BuildGrpcRetryData(string tranId, ISagaTranUnitService unitService, bool isCommit)
        {
            List<SagaTranUnit> failedUnits;
            if (isCommit)
            {
                failedUnits = unitService.GetFailedData(tranId);
            }
            else
            {
                failedUnits = unitService.GetSuccessData(tranId);
            }

            if (failedUnits == null || !failedUnits.Any())
            {
                return null;
            }

            var retryData = new Grpc.RetryData
            {
                TranId = tranId
            };
            failedUnits.ForEach(x =>
            {
                retryData.SagaTranUnits.Add(new Grpc.RetryTranUnit
                {
                    Id = x.Id,
                    ParamsValue = ByteString.CopyFrom(x.ParamsValue),
                    UnitNamespace = x.UnitNamespace
                });
            });
            return retryData;
        }

        private Persistence.Saga.RetryData BuildRetryData(string tranId, ISagaTranUnitService unitService, bool isCommit)
        {
            List<SagaTranUnit> failedUnits;
            if (isCommit)
            {
                failedUnits = unitService.GetFailedData(tranId);
            }
            else
            {
                failedUnits = unitService.GetSuccessData(tranId);
            }

            if (failedUnits == null || !failedUnits.Any())
            {
                return null;
            }

            var retryData = new Persistence.Saga.RetryData
            {
                TranId = tranId,
                SagaTranUnits = new List<Persistence.Saga.RetryTranUnit>()
            };
            failedUnits.ForEach(x =>
            {
                retryData.SagaTranUnits.Add(new Persistence.Saga.RetryTranUnit
                {
                    Id = x.Id,
                    ParamsValue = x.ParamsValue,
                    UnitNamespace = x.UnitNamespace
                });
            });
            return retryData;
        }

        private Persistence.Saga.ResponseData Commit(SagaTran tran, ISagaTranUnitService unitService)
        {
            Persistence.Saga.ResponseData result = new Persistence.Saga.ResponseData();
            if (tran.ServiceType == ServiceOptions.Grpc)
            {
                var grpcRetryData = BuildGrpcRetryData(tran.Id, unitService, true);
                if (grpcRetryData == null)
                {
                    return null;
                }

                var grpcResult = _serviceFactory.Invoke(tran.ServiceName, serviceAddr =>
                {
                    var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
                    var client = new TranRetry.TranRetryClient(channel);
                    return client.Commit(grpcRetryData, SetHeaders());
                });
                result.Success = grpcResult.Success;
                result.Msg = grpcResult.Msg;
                return result;
            }

            var retryData = BuildRetryData(tran.Id, unitService, true);
            if (retryData == null)
            {
                return null;
            }

            result = _serviceFactory.Invoke(tran.ServiceName, serviceAddr =>
            {
                var client = _httpClientFactory.CreateClient(tran.ServiceName);
                SetHeaders(client.DefaultRequestHeaders);
                var responseData = client.PostAsync($"{serviceAddr}/Wing/Saga/TranRetry/Commit", new JsonContent(retryData)).GetAwaiter().GetResult();
                if (responseData.IsSuccessStatusCode)
                {
                    return _json.Deserialize<Persistence.Saga.ResponseData>(responseData.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }

                return null;
            });

            return result;
        }

        private Persistence.Saga.ResponseData Cancel(SagaTran tran, ISagaTranUnitService unitService)
        {
            Persistence.Saga.ResponseData result = new Persistence.Saga.ResponseData();
            if (tran.ServiceType == ServiceOptions.Grpc)
            {
                var grpcRetryData = BuildGrpcRetryData(tran.Id, unitService, false);
                if (grpcRetryData == null)
                {
                    return null;
                }

                var grpcResult = _serviceFactory.Invoke(tran.ServiceName, serviceAddr =>
                {
                    var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
                    var client = new TranRetry.TranRetryClient(channel);
                    return client.Cancel(grpcRetryData, SetHeaders());
                });
                result.Success = grpcResult.Success;
                result.Msg = grpcResult.Msg;
                return result;
            }

            var retryData = BuildRetryData(tran.Id, unitService, false);
            if (retryData == null)
            {
                return null;
            }

            result = _serviceFactory.Invoke(tran.ServiceName, serviceAddr =>
            {
                var client = _httpClientFactory.CreateClient(tran.ServiceName);
                SetHeaders(client.DefaultRequestHeaders);
                var responseData = client.PostAsync($"{serviceAddr}/Wing/Saga/TranRetry/Cancel", new JsonContent(retryData)).GetAwaiter().GetResult();
                if (responseData.IsSuccessStatusCode)
                {
                    return _json.Deserialize<Persistence.Saga.ResponseData>(responseData.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }

                return null;
            });

            return result;
        }

        private Metadata SetHeaders()
        {
            if (_sagaOptions == null
                || _sagaOptions.Headers == null
                || !_sagaOptions.Headers.Any())
            {
                return null;
            }

            var metadata = new Metadata();
            foreach (var item in _sagaOptions.Headers)
            {
                metadata.Add(item.Key, item.Value);
            }

            return metadata;
        }

        private void SetHeaders(HttpRequestHeaders headers)
        {
            if (_sagaOptions == null
                || _sagaOptions.Headers == null
                || !_sagaOptions.Headers.Any())
            {
                return;
            }

            foreach (var item in _sagaOptions.Headers)
            {
                headers.Add(item.Key, item.Value);
            }
        }
    }
}
