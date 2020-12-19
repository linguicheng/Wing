using System;
using Microsoft.Extensions.Logging;

namespace Wing.Logger
{
    public class WingLogger<T> : IWingLogger<T>
    {
        private readonly ILogger<T> _logger;

        public WingLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Critical(string message, params object[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void Critical(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }

        public void Critical(EventId eventId, string message, params object[] args)
        {
            _logger.LogCritical(eventId, message, args);
        }

        public void Critical(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(eventId, exception, message, args);
        }

        public void Debug(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogDebug(eventId, exception, message, args);
        }

        public void Debug(EventId eventId, string message, params object[] args)
        {
            _logger.LogDebug(eventId, message, args);
        }

        public void Debug(Exception exception, string message, params object[] args)
        {
            _logger.LogDebug(exception, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void Error(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        public void Error(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogError(eventId, exception, message, args);
        }

        public void Error(EventId eventId, string message, params object[] args)
        {
            _logger.LogError(eventId, message, args);
        }

        public void Info(EventId eventId, string message, params object[] args)
        {
            _logger.LogInformation(eventId, message, args);
        }

        public void Info(Exception exception, string message, params object[] args)
        {
            _logger.LogInformation(exception, message, args);
        }

        public void Info(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogInformation(eventId, exception, message, args);
        }

        public void Info(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void Trace(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void Trace(Exception exception, string message, params object[] args)
        {
            _logger.LogTrace(exception, message, args);
        }

        public void Trace(EventId eventId, string message, params object[] args)
        {
            _logger.LogTrace(eventId, message, args);
        }

        public void Trace(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogTrace(eventId, exception, message, args);
        }

        public void Warn(EventId eventId, string message, params object[] args)
        {
            _logger.LogWarning(eventId, message, args);
        }

        public void Warn(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(eventId, exception, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void Warn(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }
    }
}
