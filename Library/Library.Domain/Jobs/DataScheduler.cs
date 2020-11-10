using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;

namespace Library.Domain.Jobs
{
    public static class DataScheduler
    {
        public static async void Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetRequiredService<JobFactory>();
            await scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<DataJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MailingTrigger", "default")
                .StartAt(DateTimeOffset.Now/*.AddMinutes(5)*/)
                .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)/*InHours(12)*/
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
