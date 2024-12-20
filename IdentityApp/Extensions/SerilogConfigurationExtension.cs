using System.Collections.ObjectModel;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using System.Data;
using Serilog;

namespace IdentityApp.Extensions
{
    public static class SerilogConfigurationExtension
    {
        [Obsolete]
        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) => lc
                    .Enrich.FromLogContext()
                    .WriteTo.OpenTelemetry(options =>
                    {
                        options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
                        var headers = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"]?.Split(',') ?? [];
                        foreach (var header in headers)
                        {
                            var (key, value) = header.Split('=') switch
                            {
                            [string k, string v] => (k, v),
                                var v => throw new Exception($"Invalid header format {v}")
                            };

                            options.Headers.Add(key, value);
                        }
                        options.ResourceAttributes.Add("service.name", "apiservice");

                        var (otelResourceAttribute, otelResourceAttributeValue) = builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]?.Split('=') switch
                        {
                        [string k, string v] => (k, v),
                            _ => throw new Exception($"Invalid header format {builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]}")
                        };

                        options.ResourceAttributes.Add(otelResourceAttribute, otelResourceAttributeValue);

                    })
                    .WriteTo.MSSqlServer(
                        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "RequestLogs",
                            AutoCreateSqlTable = true,
                        },
                        columnOptions: GetColumnOptions(),
                        restrictedToMinimumLevel: LogEventLevel.Error)
                    .ReadFrom.Configuration(ctx.Configuration)
            );
            return builder;
        }

        private static ColumnOptions GetColumnOptions()
        {
            return new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn
                    { 
                        ColumnName = "HttpMethod", 
                        PropertyName = "RequestMethod", 
                        DataType = SqlDbType.NVarChar, 
                        DataLength = 10 
                    },

                    new SqlColumn
                    { 
                        ColumnName = "HttpRequestPath", 
                        PropertyName = "RequestPath", 
                        DataType = SqlDbType.NVarChar, 
                        DataLength = 128 
                    },

                    new SqlColumn
                    { 
                        ColumnName = "HttpRequestMessage", 
                        PropertyName = "Message", 
                        DataType = SqlDbType.NVarChar, 
                        DataLength = 500 
                    },

                    new SqlColumn
                    { 
                        ColumnName = "HttpStatusCode", 
                        PropertyName = "StatusCode", 
                        DataType = SqlDbType.Int
                    },

                    new SqlColumn
                    { 
                        ColumnName = "HttpRequestSource",
                        PropertyName = "RequestSource", 
                        DataType = SqlDbType.NVarChar, 
                        DataLength = 40 }
                }
            };
        }
    }
}
