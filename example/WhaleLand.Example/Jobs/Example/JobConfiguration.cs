namespace WhaleLand.Example.Jobs.Example
{
    public class JobConfiguration
    {
        public static JobConfiguration Build(Quartz.JobDataMap jobData)
        {
            var config = new JobConfiguration();
            config.DelayMinutes = jobData.GetInt("DelayMinutes");
            return config;
        }

        public int DelayMinutes { get; set; }
    }
}
