using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Threading.Tasks;

namespace Library.Parser
{
    public class BookParserJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public BookParserJob(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var parsCount = 50;
                var parser = scope.ServiceProvider.GetService<IBookParserService>();
                await parser.Pars(parsCount);
            }
            await Task.CompletedTask;
        }
    }
}
