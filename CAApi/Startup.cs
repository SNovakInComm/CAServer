﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using CAApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CAApi.Filters;
using CAApi.Models;

namespace CAApi
{
    public class Startup
    {
        private readonly int? _httpsPort;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            if(env.IsDevelopment())
            {
                var lauchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile($"Properties{System.IO.Path.DirectorySeparatorChar}launchSettings.json")
                    .Build();
                _httpsPort = lauchJsonConfig.GetValue <int> ("iissettings:iisexpress:sslport");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            services.AddIdentity<UserEntity, UserRoleEntity>()
                .AddEntityFrameworkStores<CADBContext, Guid>()
                .AddDefaultTokenProviders();
            */

            services.AddDbContext<CADBContext>( options => 
            {
                options.UseSqlServer("Data Source=POR-PC0J936L;Initial Catalog=CA;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });

            services.AddMvc(opt =>
           {
               opt.Filters.Add(typeof(JsonExceptionFilter));

               opt.SslPort = _httpsPort;
               opt.Filters.Add(typeof(RequireHttpsAttribute));

               var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
               
               opt.OutputFormatters.Remove(jsonFormatter);
               opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
           });

            services.AddRouting( opt => opt.LowercaseUrls = true );

            services.AddApiVersioning( opt =>
            {
                opt.ApiVersionReader = new MediaTypeApiVersionReader();
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
