using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog;

namespace qtree.core.website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.LoadConfiguration("NLog.config");
            var log = LogManager.GetCurrentClassLogger();
            log.Debug("Program - starting up debug");
            log.Trace("Program Trace - starting up trace");
            log.Error("Program - starting up error");
            log.Fatal("Program - starting up fatal");
            log.Info("Program - starting up info");

            CreateWebHostBuilder(args).Build().Run();
            log.Debug("Program - shutting down");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.ConfigureLogging((host, builder) =>
                //{
                //    host.HostingEnvironment.ConfigureNLog("nlog.config");
                //    builder.SetMinimumLevel(LogLevel.Trace);
                //})
                .UseStartup<Startup>();
    }
}
