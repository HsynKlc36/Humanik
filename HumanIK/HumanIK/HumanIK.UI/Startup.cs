using FluentValidation;
using FluentValidation.AspNetCore;
using FormHelper;
using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES.Entities;
using HumanIK.REPOSITORIES.Abstract;
using HumanIK.REPOSITORIES.Concrete;
using HumanIK.REPOSITORIES.Context;
using HumanIK.REPOSITORIES.FluentValidations;
using HumanIK.UI.Areas.Admin.Models;
using HumanIK.UI.Areas.Manager.Models;
using HumanIK.UI.Areas.Employee.Models;
using HumanIK.UI.Data.FluentValidations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HumanIK.UI
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddFormHelper();
            services.AddDbContext<IKDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("humanik")));
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<IKDbContext>().AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddScoped<IValidator<Company>, CompanyValidator>();
            services.AddSingleton<IAppLevelVariables, AppLevelVariables>();
            services.AddScoped<IValidator<ManagerViewModel>, ManagerValidator>();
            services.AddScoped<IValidator<Expense>, ExpenseValidator>();
            services.AddScoped<IValidator<EmployeeViewModel>, EmployeeValidator>(); services.AddScoped<IValidator<AdvanceViewModel>, AdvanceValidator>();
            services.AddScoped<IValidator<PermissionViewModel>, PermissionValidator>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IOTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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
            app.UseFormHelper();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapAreaControllerRoute(
                //name: "Admin",
                //areaName: "Admin",
                //pattern: "Yonetim/{controller=Admin}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                   name: "Manager",
                   pattern: "{area:exists}/{controller=Manager}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                   name: "Employee",
                   pattern: "{area:exists}/{controller=Employee}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");

            });
        }
    }
}
