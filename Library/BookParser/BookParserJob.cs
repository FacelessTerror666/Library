using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Library.Parser
{
    public class BookParserJob : IJob
    {
        private readonly IServiceProvider serviceProvider;

        public BookParserJob(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var parsCount = 50;
                var parser = scope.ServiceProvider.GetService<IBookParserService>();
                await parser.Pars(parsCount);
            }
            await Task.CompletedTask;
        }
    }
}
