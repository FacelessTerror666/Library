using Library.Domain;
using Library.Domain.Jobs;
using Library.Parser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

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
            services.AddMyConfig(Configuration);

            services.AddTransient<IJobFactory, JobFactory>();
            services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();

            services.AddTransient<AuthoCancelJob>();
            services.AddTransient<BookParserJob>();

            services.AddSingleton(new JobSchedule(
               jobType: typeof(AuthoCancelJob),
               cronExpression: "0 0 0/12 * * ?"));
            services.AddSingleton(new JobSchedule(
               jobType: typeof(BookParserJob),
               cronExpression: "0 25 13 * * ?"));

            services.AddHostedService<QuartzHostedService>();
            services.AddHostedService<RoleInitializeHostedService>();

            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
