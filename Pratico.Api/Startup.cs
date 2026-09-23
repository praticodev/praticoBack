using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Pratico.Api.Configuration;
using Pratico.Data.Context;
using Pratico.Worker.OutBox;
using System;
using System.Reflection;

namespace Pratico.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        AWSOptions awsOptions;

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PraticoContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultPostgresConnectionSupa"));
                options.EnableSensitiveDataLogging();
            });

            var assembly = AppDomain.CurrentDomain.Load("Pratico.Business");
            services.AddMediatR(assembly);
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperConfig>();
            }, NullLoggerFactory.Instance);
            services.AddSingleton(mapperConfiguration);
            services.AddScoped<IMapper>(sp => mapperConfiguration.CreateMapper(sp.GetService));
            services.AddApiConfig();
            services.AddSwaggerConfig();
            services.AddIdentityConfig(Configuration);
            services.ResolveDependencies();
            //services.AddOutBoxWorker(Configuration);
            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfig();
            app.UseApiConfig(env);
        }


    }
}
