using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Wing.APM
{
    public class DiagnsticListenerObserver : IObserver<DiagnosticListener>
    {
        private IEnumerable<IDiagnosticListener> _listeners;
        private readonly ILogger<DiagnsticListenerObserver> _logger;

        public DiagnsticListenerObserver(IEnumerable<IDiagnosticListener> listeners, ILogger<DiagnsticListenerObserver> logger)
        {
            _listeners = listeners;
            _logger = logger;
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {
            _logger.LogError(error, "Apm Observer异常");
        }

        public void OnNext(DiagnosticListener listener)
        {
            IDiagnosticListener diagnosticListener = _listeners.Where(x => x.Name == listener.Name).FirstOrDefault();

            if (diagnosticListener != null)
            {
                listener.Subscribe(diagnosticListener);
            }
        }
    }
}
