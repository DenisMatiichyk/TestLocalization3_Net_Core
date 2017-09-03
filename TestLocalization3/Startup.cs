using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestLocalization3.Localization;

namespace TestLocalization3
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-AU"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("en"),
                    new CultureInfo("es-ES"),
                    new CultureInfo("es-MX"),
                    new CultureInfo("es"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("fr"),
                    new CultureInfo("ru-RU"),
                    new CultureInfo("ru"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
            });

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }



            //var supportedCultures = new[]
            //{
            //    new CultureInfo("en-US"),
            //            new CultureInfo("en-AU"),
            //            new CultureInfo("en-GB"),
            //            new CultureInfo("en"),
            //            new CultureInfo("es-ES"),
            //            new CultureInfo("es-MX"),
            //            new CultureInfo("es"),
            //            new CultureInfo("fr-FR"),
            //            new CultureInfo("fr"),
            //            new CultureInfo("ru-RU"),
            //            new CultureInfo("ru"),
            //};

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                //DefaultRequestCulture = new RequestCulture("en-US"),
                //// Formatting numbers, dates, etc.
                //SupportedCultures = supportedCultures,
                //// UI strings that we have localized.
                //SupportedUICultures = supportedCultures
            });
            // app.UseCulture();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithCulture",
                template: "{ui-culture}/{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvcWithDefaultRoute();
        }

    }
}
