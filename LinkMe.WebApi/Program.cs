using LinkMe.Application.Logic.Abstractions;
using LinkMe.WebApi.Middlewares;
using LinkMe.Application;
using Serilog;
using LinkMe.Infrastructure.Auth;
using LinkMe.WebApi.Application.Auth;
using LinkMe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace LinkMe.WebApi
{
    public class Program
    {
        public static string APP_NAME = "LinkeMe.WebApi";

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", APP_NAME)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile("appsettings.Development.local.json");
            }

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .Enrich.WithProperty("Application", APP_NAME)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDatabaseCache();
            builder.Services.AddSqlDatabase(builder.Configuration.GetConnectionString("MainDbSql")!);
            builder.Services.AddControllersWithViews(options =>
            {
                if (!builder.Environment.IsDevelopment())
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }
            }).AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.AddJwtAuth(builder.Configuration);
            builder.Services.AddJwtAuthenticationDataProvider(builder.Configuration);
            builder.Services.AddPasswordManager();

            builder.Services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblyContaining(typeof(BaseCommandHandler));
            });

            builder.Services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(x =>
                {
                    var name = x.FullName;
                    if (name != null)
                    {
                        name = name.Replace("+", "_");
                    }

                    return name;
                });
            });

            builder.Services.AddAntiforgery(o =>
            {
                o.HeaderName = "X-XSRF-TOKEN";
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddValidators();

            builder.Services.AddCors();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
                .WithOrigins(app.Configuration.GetValue<string>("WebAppBaseUrl") ?? "")
                .WithOrigins(app.Configuration.GetSection("AdditionalCorsOrigins").Get<string[]>() ?? new string[0])
                .WithOrigins((Environment.GetEnvironmentVariable("AdditionalCorsOrigins") ?? "").Split(',').Where(h => !string.IsNullOrEmpty(h)).Select(h => h.Trim()).ToArray())
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod());

            app.UseExceptionResultMiddleware();

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
