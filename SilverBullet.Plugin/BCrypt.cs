using System;
using PluginFramework;
using PluginFramework.Attributes;
using RuriLib;
using RuriLib.LS;

namespace SilverBullet.Plugin
{
    public class BCrypt : BlockBase, IBlockPlugin
    {
        public BCrypt()
        {
            Label = nameof(BCrypt);
        }

        public string Name => nameof(BCrypt);

        public string Color => "#A02A2F";

        public bool LightForeground => false;

        private string variableName, input, salt;

        [Text("VariableName:")]
        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; OnPropertyChanged(); }
        }

        [Text("Input:")]
        public string Input
        {
            get { return input; }
            set { input = value; OnPropertyChanged(); }
        }

        [Text("Salt:")]
        public string Salt
        {
            get { return salt; }
            set { salt = value; OnPropertyChanged(); }
        }

        public override void Process(BotData data)
        {
            base.Process(data);
            string result;
            if (string.IsNullOrEmpty(Salt))
                result = global::BCrypt.Net.BCrypt.HashPassword(ReplaceValues(Input, data));
            else result = global::BCrypt.Net.BCrypt.HashPassword(ReplaceValues(Input, data), ReplaceValues(Salt, data));
            InsertVariable(data, false, result, VariableName);
        }

        public override string ToLS(bool indent = true)
        {
            var writer = new BlockWriter(GetType(), indent, Disabled);

            writer.Label(Label)
             .Token(nameof(BCrypt))
             .Literal(input)
             .Literal(Salt);

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

            Input = LineParser.ParseLiteral(ref input, nameof(Input));
            Salt = LineParser.ParseLiteral(ref input, nameof(Salt));

            if (LineParser.ParseToken(ref input, TokenType.Arrow, false) == "")
                return this;

            // Parse the variable/capture name
            try { VariableName = LineParser.ParseToken(ref input, TokenType.Literal, true); }
            catch { throw new ArgumentException("Variable name not specified"); }

            return this;
        }

    }
}
