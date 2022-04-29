using Confluent.Kafka;
using System;

namespace WhaleLand.Extensions.HealthChecks
{
    public static class HealthCheckBuilderKafkaExtensions
    {
        public static HealthCheckBuilder AddKafkaCheck(this HealthCheckBuilder builder, string name, ProducerConfig configuration)
        {
            Guard.ArgumentNotNull(nameof(builder), builder);

            return AddKafkaCheck(builder, name, configuration, "healthcheck", builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddKafkaCheck(this HealthCheckBuilder builder, string name, ProducerConfig configuration, string topic)
        {
            Guard.ArgumentNotNull(nameof(builder), builder);

            return AddKafkaCheck(builder, name, configuration, topic, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddKafkaCheck(this HealthCheckBuilder builder, string name, ProducerConfig configuration, string topic, TimeSpan cacheDuration)
        {
            IProducer<string, string> _producer = null;

            builder.AddCheck($"KafkaCheck({name})", async () =>
            {
                try
                {
                    if (_producer == null)
                    {
                        _producer = new ProducerBuilder<string, string>(configuration).Build();
                    }

                    var message = new Message<string, string>()
                    {
                        Key = "healthcheck-key",
                        Value = $"Check Kafka healthy on {DateTime.UtcNow}"
                    };

                    var result = await _producer.ProduceAsync(topic, message);

                    if (result.Status == PersistenceStatus.NotPersisted)
                    {
                        return HealthCheckResult.Unhealthy($"Message is not persisted or a failure is raised on health check for kafka.");
                    }

                    return HealthCheckResult.Healthy("Healthy");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy(ex.Message);
                }

            }, cacheDuration);

            return builder;


        }
    }
}
