using RuriLib.Runner;

namespace RuriLib.Models
{
    public struct MultiRunInput
    {
        public RunnerBotViewModel Bot { get; set; }

        public BotData BotData { get; set; }

        public RunnerViewModel Runner { get; set; }

        public long Index { get; set; }
    }
}
