using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;

using KeywordsApp.Data;
using KeywordsApp.Models;
using KeywordsApp.Areas.Identity;
using KeywordsApp.Areas.Identity.Services;
using KeywordsApp.Models.File;
using KeywordsApp.Hubs;
using KeywordsApp.HostedServices;

namespace KeywordsApp
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
            services.AddControllersWithViews();

            services.AddIdentity<UserEntity, IdentityRole>(options =>
             {
                 options.Password.RequireDigit = true;
                 options.Password.RequireUppercase = true;
             })
            .AddEntityFrameworkStores<KeywordContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.Configure<UploadFormOptions>(Configuration);


            services.AddRazorPages();
            services.AddSignalR();
            services.AddHostedService<ParserService>();
            services.AddSingleton<IGoogleParser, GoogleParser>();

            services.AddScoped<IUserClaimsPrincipalFactory<UserEntity>, UserClaimsPrincipalFactory>();

            services.AddDbContext<KeywordContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("KeywordConnection"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Dev & Test
            if (!env.IsProduction())
            {
                CreateDbIfNotExists(app);
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=File}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<ParserHub>("/parser");
            });

        }

        private static void CreateDbIfNotExists(IApplicationBuilder host)
        {
            using (var scope = host.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<KeywordContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}
