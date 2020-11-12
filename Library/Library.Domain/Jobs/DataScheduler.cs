using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.Jobs
{
    public class QuartzHostedService : IHostedService
    {
        //public static async void Start(IServiceProvider serviceProvider)
        //{
        //    IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //    scheduler.JobFactory = serviceProvider.GetRequiredService<JobFactory>();
        //    await scheduler.Start();

        //    IJobDetail jobDetail = JobBuilder.Create<DataJob>().Build();
        //    ITrigger trigger = TriggerBuilder.Create()
        //        .WithIdentity("MailingTrigger", "default")
        //        .StartAt(DateTimeOffset.Now/*.AddMinutes(5)*/)
        //        .WithSimpleSchedule(x => x
        //        .WithIntervalInMinutes(1)/*InHours(12)*/
        //        .RepeatForever())
        //        .Build();

        //    await scheduler.ScheduleJob(jobDetail, trigger);
        //}

        private readonly ISchedulerFactory schedulerFactory;
        private readonly IJobFactory jobFactory;
        private readonly IEnumerable<JobSchedule> jobSchedules;

        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<JobSchedule> jobSchedules)
        {
            this.schedulerFactory = schedulerFactory;
            this.jobSchedules = jobSchedules;
            this.jobFactory = jobFactory;
        }

        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = jobFactory;

            foreach (var jobSchedule in jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
}
