using Library.Database;
using Library.Database.Entities;
using Library.Database.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Services;
using Library.Parser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Domain
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddMyConfig(
                this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<LibraryDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, RoleInitialize>()
                 .AddEntityFrameworkStores<LibraryDbContext>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IBookParserService, BookParserService>();

            return services;
        }
    }
}
