using System;

namespace WhaleLand.Extensions.Idempotency
{
    public class IdempotencyOption : IIdempotencyOption
    {
        public TimeSpan Druation { get; set; } = TimeSpan.FromMinutes(5);

        public string CacheRegion { get; set; } = "Idempotency";
    }
}
