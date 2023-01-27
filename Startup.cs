#region Library
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ChatWithSignal.Service;
using ChatWithSignal.Infrastructure;
using ChatWithSignal.Infrastructure.EntityFramework;
using ChatWithSignal.Infrastructure.Interface;
using ChatWithSignal.Service.Interface;
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Service.Server;
using ChatWithSignal.Service.ChatHub;
#endregion

namespace ChatWithSignal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region підключення конфігу з appsettings.json
            Configuration.Bind("Project", new Config());
            #endregion

            #region EF
            services.AddScoped<IProfileRepository, EFProfileRepository>();
            services.AddScoped<IChatsRepository, EFChatsRepository>();
            services.AddScoped<IGroupRepository, EFGroupRepository>();
            #endregion

            #region Service
            services.AddScoped<IMessengerService, MessengerService>();
            services.AddScoped<IProfileService, ProfileService>();
            #endregion

            #region Підключаєм контекст Бази Даних
            services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(Config.DBConnectionString));
            #endregion

            #region Налаштовуємо identity систему
            services.AddIdentity<Profile, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
            #endregion

            #region Налаштовуємо authentication cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "SCAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Identity/Login";
                options.AccessDeniedPath = "/Identity/Accessdenied";
                options.SlidingExpiration = true;
            });
            #endregion

            #region Підключення сервісів передачі у реальному часі
            services.AddSignalR();
            #endregion

            #region Додаємо підтримку controllers і views (MVC)
            services.AddControllersWithViews();//Підтримка останньою версією asp.net core
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Показ детальної інформації про помилки в процесі розробки
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
            #endregion

            #region Підключення підтримки статичних файлів в програмі (css, js і тд.)
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            #endregion

            #region Підключаємо систему маршутизації
            app.UseRouting();
            #endregion

            #region Підключаємо авторизацію і аутентифікацію
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            #region Реєструємо потрібні маршрути (endpoints)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MainHub>("/chat");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
        }
    }
}
