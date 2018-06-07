using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Logging;

namespace Publisher
{
    public class Log4NetProvider : ILoggerProvider, ILoggerFactory
    {

        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers =
            new ConcurrentDictionary<string, Log4NetLogger>();


        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void AddProvider(ILoggerProvider provider)
        {

        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name);
        }
    }
    public class Log4NetLogger : ILogger
    {
        private readonly string _name;
        private readonly ILog _log;
        public Log4NetLogger(string name)
        {
            _name = name;
            _log = log4net.LogManager.GetLogger(name);
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            System.Exception exception, Func<TState, System.Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        _log.Debug(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message);
                        break;
                    case LogLevel.Information:
                        _log.Info(message);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message);
                        break;
                    default:
                        _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _log.Info(message, exception);
                        break;
                }
            }
        }
    }
}
