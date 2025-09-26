using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace dotnetconsulting.IoCConsole;

internal static class NLogExtentions
{
    extension (ILoggingBuilder loggingBuilder)
    {
        internal void AddMyNLog()
        {
            string nlogConfigFileName = Path.Combine(AppContext.BaseDirectory, "nlog.config");
            if (File.Exists(nlogConfigFileName))
            {
                loggingBuilder.AddNLog(nlogConfigFileName);
            }
        }
    }
}