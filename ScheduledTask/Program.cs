using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Quartz;
using Quartz.Impl;
using Quartz.Job;

namespace ScheduledTask
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();
                IJobDetail job = JobBuilder.Create<MyJob>().WithIdentity("job1", "group1").Build();
                ITrigger trigger = TriggerBuilder.Create()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(22,30))
                    .ForJob("job1","group1")
                    .Build();
                scheduler.ScheduleJob(job, trigger);
                Thread.Sleep(TimeSpan.FromSeconds(500));
                scheduler.Shutdown();
            }
            catch (SchedulerException se) {
                Console.WriteLine(se);
            }
            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }
    }
    public class MyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello," + System.DateTime.Now.ToString());
        }
    }
}
