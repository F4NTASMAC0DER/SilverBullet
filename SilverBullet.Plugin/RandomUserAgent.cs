using System;
using PluginFramework;
using PluginFramework.Attributes;
using RuriLib;
using RuriLib.LS;
using SpiceRandomUserAgent;

namespace SilverBullet.Plugin
{
    public class RandomUserAgent : BlockBase, IBlockPlugin
    {
        public string Name => "RandomUA";

        public string Color => "#3CC1E2";

        public bool LightForeground => false;

        public RandomUserAgent()
        {
            Label = "RandomUA";
        }

        private string variableName;
        [Text("VariableName:")]
        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; OnPropertyChanged(); }
        }

        private string typeUA;
        [Dropdown("Select:", options = new string[] { "Random",
            "Chrome",
            "FireFox",
            "Android",
            "AOL",
            "IceWeasel",
            "IE",
            "iPad",
            "iPhone",
            "Linux",
            "Mac",
            "Maxthon",
            "Mobile",
            "Mozilla",
            "OpearMini",
            "Opera",
            "Safari",
            "Windows",
        })]
        public string TypeUA
        {
            get { return typeUA; }
            set { typeUA = value; OnPropertyChanged(); }
        }

        public override void Process(BotData data)
        {
            base.Process(data);
            var ua = typeof(SpiceRUA).GetMethod($"Get{TypeUA}UA")
                 .Invoke(null, null) as string;
            InsertVariable(data, false, ua, VariableName);
        }

        public override string ToLS(bool indent = true)
        {
            var writer = new BlockWriter(GetType(), indent, Disabled);

            writer.Label(Label)
                .Token("RandomUA")
                .Token(TypeUA);

            if (!writer.CheckDefault(VariableName, nameof(VariableName)))
            {
                writer.Arrow()
                    .Token("VAR")
                    .Literal(VariableName)
                    .Indent();
            }

            return writer.ToString();
        }

        public override BlockBase FromLS(string line)
        {
            // Trim the line
            var input = line.Trim();

            // Parse the label
            if (input.StartsWith("#"))
                Label = LineParser.ParseLabel(ref input);

            TypeUA = LineParser.ParseToken(ref input, TokenType.Parameter, false);

            if (LineParser.ParseToken(ref input, TokenType.Arrow, false) == "")
                return this;

            // Parse the variable/capture name
            try { VariableName = LineParser.ParseToken(ref input, TokenType.Literal, true); }
            catch { throw new ArgumentException("Variable name not specified"); }

            return this;
        }
    }
}
