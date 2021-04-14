using System.Collections.Generic;

namespace RuriLib.Models
{
    public struct CheckResult
    {
        public BotData BotData { get; set; }

        public List<LogEntry> BotLog { get; set; }
    }
}
