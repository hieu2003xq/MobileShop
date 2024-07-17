using Serilog;
using Serilog.Sinks.ElmahIo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ado.thuVienLog
{
    public class managerLogging
    {
        private static readonly Lazy<ILogger> LazyLogger = new Lazy<ILogger>(() =>
        {
            
            return new LoggerConfiguration()
                .WriteTo.Console()
                .Enrich.WithProperty("IP", "192.168.0.1")
                .Enrich.FromLogContext()
                .WriteTo.File("D:\\ado\\ado\\logging/logfile.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Properties:j}{NewLine}{Exception}")
                .WriteTo.ElmahIo(new ElmahIoSinkOptions("34e838629a0f41ce800b8cd6f797ae68", new Guid("b9090af1-ea9b-460d-ad0b-d5825270cc2f")))
                .CreateLogger();
        });
        private static readonly Lazy<ILogger> LazyLogger1 = new Lazy<ILogger>(() =>
        {

            return new LoggerConfiguration()
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .WriteTo.ElmahIo(new ElmahIoSinkOptions("34e838629a0f41ce800b8cd6f797ae68", new Guid("b9090af1-ea9b-460d-ad0b-d5825270cc2f")))
                .CreateLogger();
        });

        public static ILogger Logger => LazyLogger.Value;
        public static ILogger Logger1 => LazyLogger1.Value;

    }
}