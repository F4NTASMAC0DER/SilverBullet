using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using PluginFramework;
using PluginFramework.Attributes;
using RuriLib;
using RuriLib.LS;

namespace SilverBullet.Plugin
{
    public class CloudScraper : BlockBase, IBlockPlugin
    {

        public CloudScraper()
        {
            Label = nameof(CloudScraper);
        }

        public string Name => nameof(CloudScraper);

        public LinearGradientBrush LinearGradientBrush =>
            new LinearGradientBrush(new GradientStopCollection()
            {
                new GradientStop("#F38020".ColorConverter() , 1)
            });

        public bool LightForeground => false;

        private string variableName;
        [Text("VariableName:", "return user agent")]
        public string VariableName
        {
            get { return variableName; }
            set
            {
                variableName = value;
                OnPropertyChanged();
            }
        }

        private string url;
        [Text("Url:")]
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged();
            }
        }

        public override void Process(BotData data)
        {
            base.Process(data);

            var arg = Url;
            if (data.UseProxies)
            {
                arg += $" {data.Proxy.Type.ToString().ToLower()}://{data.Proxy.Proxy}";
            }
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo("bin\\CloudScraper.exe", arg)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var processOutput = process.StandardOutput.ReadToEnd();
            try { if (!process.HasExited) process.Kill(); } catch { }

            string[] pOutArray;
            if ((pOutArray = processOutput.Split('\n')).Any(o => o == "bypassed = true\r"))
            {
                var output = pOutArray.FirstOrDefault(o => o.StartsWith("cookie = \""))
                      .Split(new[] { '"' }, 2)[1];
                foreach (var cookie in output.Split(';').AsParallel())
                {
                    if (cookie == "\r" || cookie == "\n") continue;
                    try
                    {
                        var name = cookie.Split(new[] { '=' }, 2)[0];
                        if (data.Cookies.ContainsKey(name))
                        {
                            data.Cookies[name] = cookie.Split(new[] { '=' }, 2)[1];
                        }
                        else
                        {
                            data.Cookies.Add(name, cookie.Split(new[] { '=' }, 2)[1]);
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Log(new LogEntry(ex.Message, Colors.PaleVioletRed));
                    }
                }
            }
            else
            {
                if (processOutput.Contains("Cannot connect to proxy."))
                {
                    InsertVariable(data, false, "Cannot connect to proxy.", VariableName);
                    return;
                }
                else if (processOutput.Contains("Missing dependencies for SOCKS support."))
                {
                    InsertVariable(data, false, "Missing dependencies for SOCKS support.", VariableName);
                    return;
                }
                InsertVariable(data, false, "an error occurred", VariableName);
                throw new Exception("an error occurred\nbypassed = false");
            }

            InsertVariable(data, false, pOutArray.FirstOrDefault(o => o.StartsWith("useragent =")).Split(new[] { '=' }, 2)[1].Replace("\r", string.Empty), VariableName);
        }

        public override string ToLS(bool indent = true)
        {
            var writer = new BlockWriter(GetType(), indent, Disabled);

            writer.Label(Label)
            .Token(nameof(CloudScraper))
            .Literal(Url);

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

            Url = LineParser.ParseLiteral(ref input, nameof(Url));

            // Parse the variable/capture name
            try { VariableName = LineParser.ParseToken(ref input, TokenType.Literal, true); }
            catch { throw new ArgumentException("Variable name not specified"); }

            return this;
        }
    }
}
