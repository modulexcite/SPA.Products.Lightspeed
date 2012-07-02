using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Generic.Util.Logging;

namespace Products.App.Infrastructure
{
    public class NLogger : ILogger
    {
        private Logger _logger;

        public NLogger(string currentClass)
        {
            _logger = LogManager.GetLogger(currentClass);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception x)
        {
            Error(x);
        }

        public void Error(string message, Exception x)
        {
            _logger.ErrorException(message, x);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(Exception x)
        {
            Fatal(x);
        }
    }
}
