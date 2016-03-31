using System;
using Serilog.Events;

namespace Serilog.Sinks.Slack.TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Slack("XXXXXXXXX", "xxxxxxxxxx", LevelAlias.Maximum)
                .CreateLogger();

            try
            {
                log.Information("This is an information message!");
                log.Error("This is an error message!");
                log.Fatal("This is an fatal message!");
            }
            catch (Exception exception)
            {
                log.Fatal(exception, "Exception catched at Main.");
            }
        }
    }
}
