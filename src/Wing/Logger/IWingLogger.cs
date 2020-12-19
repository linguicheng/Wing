using System;
using Microsoft.Extensions.Logging;

namespace Wing.Logger
{
    public interface IWingLogger<T>
    {
        void Critical(string message, params object[] args);

        void Critical(Exception exception, string message, params object[] args);

        void Critical(EventId eventId, string message, params object[] args);

        void Critical(EventId eventId, Exception exception, string message, params object[] args);

        void Debug(EventId eventId, Exception exception, string message, params object[] args);

        void Debug(EventId eventId, string message, params object[] args);

        void Debug(Exception exception, string message, params object[] args);

        void Debug(string message, params object[] args);

        void Error(string message, params object[] args);

        void Error(Exception exception, string message, params object[] args);

        void Error(EventId eventId, Exception exception, string message, params object[] args);

        void Error(EventId eventId, string message, params object[] args);

        void Info(EventId eventId, string message, params object[] args);

        void Info(Exception exception, string message, params object[] args);

        void Info(EventId eventId, Exception exception, string message, params object[] args);

        void Info(string message, params object[] args);

        void Trace(string message, params object[] args);

        void Trace(Exception exception, string message, params object[] args);

        void Trace(EventId eventId, string message, params object[] args);

        void Trace(EventId eventId, Exception exception, string message, params object[] args);

        void Warn(EventId eventId, string message, params object[] args);

        void Warn(EventId eventId, Exception exception, string message, params object[] args);

        void Warn(string message, params object[] args);

        void Warn(Exception exception, string message, params object[] args);
    }
}
