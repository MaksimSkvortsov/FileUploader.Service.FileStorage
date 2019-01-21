using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploader.Service.FileStorage.Data;
using FileUploader.Service.FileStorage.Infrastructure;
using FileUploader.Service.FileStorage.Serivces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileUploader.Service.FileStorage
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //message bus
            var messageBusConfig = Configuration.GetConnectionString("ServiceBus");
            services.AddSingleton<IServiceBus>(new AzureServiceBus(messageBusConfig, "file-storage"));

            //storage
            var storageConnectionString = Configuration.GetConnectionString("Storage");
            services.AddSingleton<IStorage>(new AzureStorage(storageConnectionString));

            //database
            var connection = Configuration.GetConnectionString("Database");
            services.AddDbContext<FileStorageContext>(options => options.UseSqlServer(connection));

            //background file processing
            services.AddHostedService<FileProcessSerivce>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder => { builder.AllowAnyOrigin(); builder.AllowAnyMethod(); builder.AllowAnyHeader(); });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
