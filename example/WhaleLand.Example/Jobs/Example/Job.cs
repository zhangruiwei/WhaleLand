using Quartz;
using System;
using System.Threading.Tasks;

namespace WhaleLand.Example.Jobs.Example
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private JobConfiguration _jobConfiguration;

        public async Task Execute(IJobExecutionContext context)
        {
            _jobConfiguration = JobConfiguration.Build(context.JobDetail.JobDataMap);

            Console.WriteLine(_jobConfiguration.DelayMinutes);

            await Task.Run(async () =>
            {
                await Task.Delay(10);
            });

        }
    }
}
