using System;
using Library.Database;
using Library.Database.Entities;
using Library.Database.Interfaces;
using Library.Domain;
using Library.Domain.Jobs;
using Library.Domain.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Library
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
            services.AddDbContextPool<LibraryDbContext>(options => 
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<JobFactory>();
            services.AddTransient<DataJob>();
            services.AddTransient<IAuthoCancel, AuthoCancel>();

            services.AddTransient(typeof(IBookRepository<>), typeof(BookRepository<>));
            services.AddTransient(typeof(IOrderRepository<>), typeof(OrderRepository<>));

            services.AddIdentity<User, RoleInitialize>()
                .AddEntityFrameworkStores<LibraryDbContext>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                     .CreateScope();

            var services = serviceScope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<RoleInitialize>>();
                    await RoleInitialize.InitializeAsync(userManager, rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }

            DataScheduler.Start(serviceScope.ServiceProvider);
        }
    }
}
