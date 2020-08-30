using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.HostedServices;
using BankingApplication.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryWrapper;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace BankingApplication
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
            services.Configure<CookiePolicyOptions>(options =>
            {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => false;
            });
            services.AddDbContext<BankAppContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("BankAppContext")));
            //Repository Service (Wrapper around a generic repository pattern)
            services.AddTransient<Wrapper>();
            services.AddCors();
            services.AddSession(options =>
            {
                // Make the session cookie essential.
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });
            //BillPayService for background checking bills
            services.AddHostedService<BillPayService>();
            //CheckLockService for background checking locked accounts
            services.AddHostedService<CheckLockService>();
            services.AddControllersWithViews().AddNewtonsoftJson();
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
            //Custom error pages middleware
            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
