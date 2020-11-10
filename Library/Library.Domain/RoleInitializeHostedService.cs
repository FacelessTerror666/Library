using Library.Database;
using Library.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class RoleInitializeHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public RoleInitializeHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleInitialize>>();
            await RoleInitialize.InitializeAsync(userManager, rolesManager);

            //using var serviceScope = app.ApplicationServices
            //    .GetRequiredService<IServiceScopeFactory>()
            //         .CreateScope();

            //var services = serviceScope.ServiceProvider;
            //try
            //{
            //    var userManager = services.GetRequiredService<UserManager<User>>();
            //    var rolesManager = services.GetRequiredService<RoleManager<RoleInitialize>>();
            //    await RoleInitialize.InitializeAsync(userManager, rolesManager);
            //}
            //catch (Exception ex)
            //{
            //    var logger = services.GetRequiredService<ILogger<Program>>();
            //    logger.LogError(ex, "An error occurred while seeding the database.");
            //}
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
