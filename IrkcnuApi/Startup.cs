using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IrkcnuApi.Models;
using IrkcnuApi.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;


namespace IrkcnuApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .AddJsonOptions(options => options.UseMemberCasing())
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
           services.AddSwaggerGen(c =>
            {
                //A hack to link the upload functionality
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IRcknuAPI",
                    Description = "An upload tool using WebApi Core, Swagger,KendoUI and MongoDB",
                    Contact = new OpenApiContact
                    {
                        Name = "Upload Service",
                        Email = string.Empty,
                        Url = new Uri("https://localhost:5001/upload/index"),
                    }

                });
                //c.ResolveConflictingActions (apiDescriptions => apiDescriptions.First());
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            });


            services.Configure<IrckcnuDatabaseSettings>(Configuration.GetSection(nameof(IrckcnuDatabaseSettings)));
            services.AddSingleton<IIrckcnuDatabaseSettings>(sp => sp.GetRequiredService<IOptions<IrckcnuDatabaseSettings>>().Value);
            services.AddSingleton<ArtikelService>();
            services.AddSingleton<ImportService>();
            services.AddKendo();
            
       

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
            app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IRcknuApi");
                c.RoutePrefix = string.Empty;

            });

            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
        }
    }
}
