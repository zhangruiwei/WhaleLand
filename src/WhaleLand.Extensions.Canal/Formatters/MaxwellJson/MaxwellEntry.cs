﻿using System.Collections.Generic;

namespace WhaleLand.Extensions.Canal.Formatters.MaxwellJson
{
    public class MaxwellEntry
    {
        public string database { get; set; }

        public string table { get; set; }

        public string type { get; set; }

        public Dictionary<string, dynamic> data { get; set; }

        public Dictionary<string, dynamic> old { get; set; }

        public long ts { get; set; }

        public long xid { get; set; }

        public string gtid { get; set; }

        public long xoffset { get; set; }

        public string position { get; set; }
    }
}
