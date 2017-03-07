using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Sinks.Slack.Core.TestConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Slack("https://hooks.slack.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX"
                //,(LogEvent l) => l.RenderMessage()
                )
                .CreateLogger();

            try
            {
                log.Verbose("This is an verbose message!");
                log.Debug("This is an debug message!");
                log.Information("This is an information message!");
                log.Warning("This is an warning message!");
                log.Error("This is an error message!");
                throw new Exception("This is an exception!");
            }
            catch (Exception exception)
            {
                log.Fatal(exception, "Exception catched at Main.");
            }
        }
    }
}
