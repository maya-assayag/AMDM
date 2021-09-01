using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AMDM.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using AMDM.Services;
using AMDM.Hubs;

namespace AMDM
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
            services.AddMvc();
            //services.AddNodeServices();
            services.AddSignalR(cfg => cfg.EnableDetailedErrors = true);

            services.AddTransient<UserService>();
            services.AddTransient<TraineeService>();
            services.AddTransient<TrainingService>();
            services.AddTransient<TicketTypeService>();
            services.AddTransient<TicketService>();

            services.AddControllersWithViews();

            services.AddDbContext<AMDMContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AMDMContext")));

            services.AddSession(options => 
            { 
                options.IdleTimeout = TimeSpan.FromMinutes(10); 
            }
            );
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                { 
                    options.LoginPath = "/Users/Login";
                    options.AccessDeniedPath = "/Users/AccessDenied"; 
                   
                }
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<TrainingHub>("/traininghub");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=LandingPage}");
                endpoints.MapHub<TrainingHub>("/traininghub");

                //pattern: "{controller=Users}/{action=Login}/{id?}");
            });
        }
    }
}
