using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Text;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;

namespace RuriLib
{
    /// <summary>
    /// SilverBullet config compiler
    /// </summary>
    public class ScriptCompiler : IDisposable
    {
        /// <summary>
        /// Gets a System.CodeDom.Compiler.CodeDomProvider instance for the specified language.
        /// </summary>
        /// <param name="lang">The language name.</param>
        public ScriptCompiler(CodeProviderLanguage lang = CodeProviderLanguage.CS)
        {
            if (lang == CodeProviderLanguage.CS)
            {
                Compiler = new CSharpCodeProvider();
            }
            else
            {
                //
            }
            Options = new CompilerParameters()
            {
                GenerateExecutable = true,
                GenerateInMemory = false,
                WarningLevel = 3
            };
        }

        private CSharpCodeProvider Compiler;
        private CompilerParameters Options;

        public static string AssemblyVersion = "1.0.0.0";

        public static string AssemblyFileVersion = "1.0.0.0";

        public const string DirCompile = "Compiled";

        /// <summary>
        /// Executable output file path 
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Compiler title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Compiler icon path
        /// </summary>
        public string IconPath { get; set; }
        /// <summary>
        /// Compiler message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Compiler message color
        /// </summary>
        public string MessageColor { get; set; }
        /// <summary>
        /// Compiler author color
        /// </summary>
        public string AuthorColor { get; set; }
        /// <summary>
        /// Compiler wordlist color
        /// </summary>
        public string WordlistColor { get; set; }
        /// <summary>
        /// Compiler bots color
        /// </summary>
        public string BotsColor { get; set; }
        /// <summary>
        /// Compiler custom input color
        /// </summary>
        public string CustomInputColor { get; set; }
        /// <summary>
        /// Compiler cpm color
        /// </summary>
        public string CPMColor { get; set; }

        /// <summary>
        /// Compiler progress color
        /// </summary>
        public string ProgressColor { get; set; }

        /// <summary>
        /// Compiler hits color
        /// </summary>
        public string HitsColor { get; set; }

        /// <summary>
        /// Compiler custom color
        /// </summary>
        public string CustomColor { get; set; }

        /// <summary>
        /// Compiler to check color
        /// </summary>
        public string ToCheckColor { get; set; }

        /// <summary>
        /// Compiler fails color
        /// </summary>
        public string FailsColor { get; set; }

        /// <summary>
        /// Compiler retries color
        /// </summary>
        public string RetriesColor { get; set; }

        /// <summary>
        /// Compiler ocrRate color
        /// </summary>
        public string OcrRateColor { get; set; }

        /// <summary>
        /// Compiler proxies color
        /// </summary>
        public string ProxiesColor { get; set; }

        /// <summary>
        /// svb file
        /// </summary>
        public string SvbConfig { get; set; }

        public Config Config { get; set; }

        public string HitInformationFormat { get; set; }

        public string LicenseSource { get; set; }

        /// <summary>
        /// command-line arguments
        /// </summary>
        /// <param name="option">arguments</param>
        public void AddOption(string option)
        {
            AddOptions(new[] { option });
        }

        /// <summary>
        /// command-line arguments
        /// </summary>
        /// <param name="options">arguments</param>
        public void AddOptions(string[] options)
        {
            foreach (var option in options)
            {
                Options.CompilerOptions += $"{option} ";
            }
        }

        public void AddReference(string reference)
        {
            Options.ReferencedAssemblies.Add(reference);
        }

        public void AddReferences(string[] references)
        {
            Options.ReferencedAssemblies.AddRange(references);
        }

        public void InjectPluginLoader()
        {
            beginRunMethod += "foreach (var plugin in LoadPluginsFromResource()) { try { BlockParser.BlockMappings.Add(plugin.Name, plugin.GetType()); } catch (Exception ex) { Console.WriteLine($\"{ex.Message}{Environment.NewLine}{plugin.Name}.dll\", Color.Red);}}";
            AddMethods(new[] { "private static IEnumerable<IBlockPlugin> LoadPluginsFromResource(){List<IBlockPlugin> list = new List<IBlockPlugin>(); var executingAssembly = Assembly.GetExecutingAssembly();var manifestResourceNames=executingAssembly.GetManifestResourceNames();for (int i = 0; i < manifestResourceNames.Length; i++){Assembly assembly=null;using (Stream stream = executingAssembly.GetManifestResourceStream(manifestResourceNames[i])){if (stream == null)continue;byte[] assemblyRawBytes = new byte[stream.Length];stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);assembly = Assembly.Load(assemblyRawBytes);}LoadDependencies(assembly.GetReferencedAssemblies());foreach (Type type in assembly.GetTypes()){if (type.GetInterface(\"IBlockPlugin\") == typeof(IBlockPlugin) && type.GetTypeInfo().IsSubclassOf(typeof(BlockBase))){list.Add(Activator.CreateInstance(type) as IBlockPlugin);}}}return list;}private static void LoadDependencies(IEnumerable<AssemblyName> assemblies){using (IEnumerator<AssemblyName> enumerator = assemblies.GetEnumerator()){while (enumerator.MoveNext()){AssemblyName asm = enumerator.Current;if (!AppDomain.CurrentDomain.GetAssemblies().Any((Assembly a) => a.GetName().FullName == asm.FullName)){try{AppDomain.CurrentDomain.Load(asm);LoadDependencies(Assembly.Load(asm).GetReferencedAssemblies());}catch{}}}}}" });
        }

        /// <summary>
        /// Invoke compilation.
        /// </summary>
        /// <returns></returns>
        public CompilerResults Execute()
        {
            Options.OutputAssembly = Output;
            if (!string.IsNullOrWhiteSpace(IconPath) && File.Exists(IconPath) && Path.GetExtension(IconPath) == ".ico")
            {
                AddOption($"/win32icon:{IconPath}");
            }

            AddFields(new[]
            {
                      "private static string SvbConfig = " + ToLiteral(SvbConfig) + ";" ,
            "\r\n        private static int write;\r\n\t\t\r\n\t\t" ,
             "private static string Title=\"" + Title + "\";\r\n" ,
             "        private static string message=\"" + Message + "\";" ,
             "\r\n        private static Color messageColor = GetColor(\"" + MessageColor + "\");" ,
             "\r\n        private static Color authorColor = GetColor(\"" + AuthorColor + "\");\r\n" ,
             "        private static Color wordlistColor = GetColor(\"" + WordlistColor + "\");\r\n" ,
             "        private static Color botsColor = GetColor(\"" + BotsColor + "\");\r\n" ,
             "        private static Color customInputColor = GetColor(\"" + CustomInputColor + "\");\r\n" ,
             "        private static Color cpmColor = GetColor(\"" + CPMColor + "\");\r\n" ,
             "        private static Color progressColor = GetColor(\"" + ProgressColor + "\");\r\n" ,
             "        private static Color hitsColor = GetColor(\"" + HitsColor + "\");\r\n" ,
             "        private static Color customColor = GetColor(\"" + CustomColor + "\");\r\n" ,
             "        private static Color toCheckColor = GetColor(\"" + ToCheckColor + "\");\r\n" ,
             "        private static Color failsColor = GetColor(\"" + FailsColor + "\");\r\n" +
             "        private static Color retriesColor = GetColor(\"" + RetriesColor + "\");\r\n" ,
             "        private static Color ocrRateColor = GetColor(\"" + OcrRateColor + "\");\r\n" ,
             "        private static Color proxiesColor = GetColor(\"" + ProxiesColor + "\");"
            });
            AddMethods(new[]
            {
                "\r\nprivate static void FoundHit(IRunnerMessaging obj, Hit hit)\r\n        {\r\n\r\n            // If an output file was specified, print them to the output file as well\r\n            if (outFile != string.Empty)\r\n            {\r\n                lock (FileLocker.GetLock(outFile))\r\n                {\r\n                    File.AppendAllText(outFile, $\"[{ DateTime.Now}]"+ HitInformationFormat +"{Environment.NewLine}\");\r\n                }\r\n            }\r\n        }",
            });
            var invokeLic = string.Empty;
            if (!string.IsNullOrWhiteSpace(LicenseSource))
            {
                var matches = Regex.Matches(LicenseSource, "using(.*?);");
                if (matches.Count > 0)
                {
                    var usings = matches.Cast<Match>().Select(m => m.Groups[1].Value);
                    usings.ToList().ForEach(u => LicenseSource = LicenseSource.ReplaceFirst($"using{u};", string.Empty));
                    AddUsings(usings.ToArray());
                }


                invokeLic = LicenseSource.Split(new[] { "Invoke]" },
                    StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split('(')[0].Replace("\r\n", string.Empty);

                if (!string.IsNullOrWhiteSpace(invokeLic))
                {
                    var splitLic = LicenseSource.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var index = splitLic.IndexOf(splitLic.First(l => l.StartsWith(invokeLic)));
                    splitLic.RemoveAt(index - 1);
                    LicenseSource = string.Join("\r\n", splitLic.ToArray());
                }

                invokeLic = invokeLic.Split(' ').Last();
                AddMethods(new[] { LicenseSource }, true);
            }
            return Compiler.CompileAssemblyFromSource(Options,
                usings +
                assemblies +
                @namespace +
                programClass +
                fields +
                properties +
                optionsClass +
                beginMainMethod +
                (!string.IsNullOrWhiteSpace(invokeLic) ? $"{invokeLic}();" : string.Empty) +
                endMainMethod +
                methods +
                beginRunMethod +
                endRunMethod +
                endSource);
        }

        /// <summary>
        /// Get compiler result
        /// </summary>
        /// <param name="compilerResults">results of compilation</param>
        /// <returns>result</returns>
        public (string, bool) GetResult(CompilerResults compilerResults)
        {
            var asmResult = compilerResults.PathToAssembly;
            if (!compilerResults.Errors.HasErrors)
                return ($"Compiled Successfully On Path: {asmResult}", false);
            var errors = new StringBuilder("Compiler Errors :\r\n");
            foreach (CompilerError error in compilerResults.Errors)
            {
                errors.AppendFormat("Line {0},{1}\t: {2}\n",
                    error.Line, error.Column, error.ErrorText);
            }
            return (errors.ToString(), true);
        }

        /// <summary>
        /// Copy refs in compiled\bin
        /// </summary>
        public void CopyReferencesAndDependencies()
        {
            string[] refs = new string[Options.ReferencedAssemblies.Count];
            Options.ReferencedAssemblies.CopyTo(refs, 0);
            var references = refs.Where(r => File.Exists(r)).ToList();
            if (Config.SeleniumPresent)
            {
                references.AddRange(DirExtensions.GetFiles("bin", "*.exe", SearchOption.TopDirectoryOnly)
                    .Where(f => f.ToLower().EndsWith("driver")));
            }
            if (ScriptExtension.HasBlock(Config.Script, "CloudScraper"))
            {
                references.Add("bin\\CloudScraper.exe");
            }
            for (var i = 0; i < references.Count; i++)
            {
                try
                {
                    var dstRef = $"{DirCompile}\\bin\\{Path.GetFileName(references[i])}";
                    if (File.Exists(dstRef)) continue;
                    File.Copy(references[i], dstRef);
                }
                catch { }
            }
            if (Config.OcrNeeded)
            {
                try
                {
                    foreach (var data in Directory.GetFiles("tessdata"))
                    {
                        if (!Directory.Exists($"{DirCompile}\\bin\\tessdata"))
                            Directory.CreateDirectory($"{DirCompile}\\bin\\tessdata");
                        File.Copy(data, $"{DirCompile}\\bin\\tessdata");
                    }
                }
                catch { }
            }
        }

        public void CopySettings()
        {
            if (!Directory.Exists("Settings")) return;
            if (!Directory.Exists($"{DirCompile}\\Settings")) Directory.CreateDirectory($"{DirCompile}\\Settings");
            foreach (var sett in Directory.GetFiles("Settings"))
            {
                var fileName = Path.GetFileName(sett);
                if (File.Exists($"{DirCompile}\\Settings\\{fileName}")) continue;
                File.Copy(sett, $"{DirCompile}\\Settings\\{fileName}");
            }
        }

        public void CreateRunner(string name, Config config)
        {
            if (!config.Settings.NeedsProxies)
            {
                var configPath = "\"bin\\" + name + ".exe\"";
                File.WriteAllText($"{DirCompile}\\{name} Runner.bat", $@"@echo off
{configPath} --bots 1 --output ""hits.txt"" --useproxies False --wordlist ""<WORDLIST PATH>"" --wltype Default
pause");
            }
            else
            {
                var configPath = "\"bin\\" + name + ".exe\"";
                File.WriteAllText($"{DirCompile}\\{name} Runner.bat", $@"@echo off
{configPath} --bots 1 --output ""hits.txt"" --proxies ""<PROXY PATH>"" --ptype Http --useproxies True --wordlist ""<WORDLIST PATH>"" --wltype Default
pause");
            }
        }

        public bool Supports(GeneratorSupport generatorSupport)
        {
            return Compiler.Supports(generatorSupport);
        }

        public int AddEmbeddedResource(string value)
        {
            return Options.EmbeddedResources.Add(value);
        }

        public void AddEmbeddedResource(string[] value)
        {
            Options.EmbeddedResources.AddRange(value);
        }

        private string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                Compiler.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                return writer.ToString();
            }
        }

        public void Dispose()
        {
            Compiler.Dispose();
        }

        public void AddUsings(string[] usings)
        {
            foreach (var @using in usings)
            {
                var usi = $"using {@using.Trim()};";
                if (!this.usings.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(u => u.Trim() == usi))
                    this.usings += $"{usi}\r\n";
            }
        }

        public void AddFields(string[] fields, bool newLine = false)
        {
            foreach (var field in fields)
            {
                this.fields += $"{field}{(!newLine ? string.Empty : "\r\n")}";
            }
        }

        public void AddProperties(string[] properties, bool newLine = false)
        {
            foreach (var prop in properties)
            {
                this.properties += $"{prop}{(!newLine ? string.Empty : "\r\n")}";
            }
        }

        public void AddMethods(string[] methods, bool newLine = false)
        {
            foreach (var method in methods)
            {
                this.methods += method + (!newLine ? string.Empty : "\r\n");
            }
        }

        private string usings = "using System.Reflection;" +
        "\r\nusing System;" +
        "\r\nusing System.Collections.Generic;" +
        "\r\nusing System.Drawing;" +
        "\r\nusing System.IO;" +
        "\r\nusing System.Linq;" +
        "\r\nusing System.Threading;" +
        "\r\nusing CommandLine;" +
        "\r\nusing CommandLine.Text;" +
        "\r\nusing Extreme.Net;" +
        "\r\nusing RuriLib;" +
        "\r\nusing RuriLib.Models;" +
        "\r\nusing RuriLib.Runner;" +
        "\r\nusing RuriLib.ViewModels;" +
        "\r\nusing RuriLib.LS;" +
        "\r\nusing PluginFramework;" +
        "\r\nusing Console = Colorful.Console;" +
        "\r\n\r\n";

        private string assemblies = "[assembly: AssemblyTitle(\"SilverBulletCLI\")]" +
            "\r\n[assembly: AssemblyDescription(\"\")]" +
            "\r\n[assembly: AssemblyConfiguration(\"\")]" +
            "\r\n[assembly: AssemblyProduct(\"SilverBulletCLI\")]" +
            "\r\n[assembly: AssemblyCopyright(\"Copyright ©  2021\")]" +
            "\r\n[assembly: AssemblyVersion(\"" + AssemblyVersion + "\")]" +
            "\r\n[assembly: AssemblyFileVersion(\"" + AssemblyFileVersion + "\")]";

        private string @namespace = "\r\n\r\nnamespace SilverBulletCLI";

        private string programClass =
            "\r\n{\r\n    " +
            "class Program" +
            "\r\n    " +
            "{\r\n\r\n        ";

        private string fields =
            "private static string asmLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);" +
            "\r\n        private static string envFile = Directory.GetParent(asmLoc).FullName + @\"\\Settings\\Environment.ini\";" +
            "\r\n        private static string settFile = Directory.GetParent(asmLoc).FullName + @\"\\Settings\\RLSettings.json\";" +
            "\r\n        private static string outFile = \"\";" +
            "\r\n        private static Random random = new Random();\r\n        ";

        private string properties = "\r\n\r\n\r\n        public static EnvironmentSettings Env { get; set; }" +
            "\r\n        public static RLSettingsViewModel RLSettings { get; set; }" +
            "\r\n        public static RunnerViewModel Runner { get; set; }" +
            "\r\n        public static bool Verbose { get; set; } = false;" +
            "\r\n        public static string ProxyFile { get; set; }" +
            "\r\n        public static ProxyType ProxyType { get; set; }\r\n\t";

        private readonly string optionsClass =
            "\r\n\r\n        class Options\r\n        " +
            "{\r\n            " +
            "//[Option('c', \"config\", Required = true, HelpText = \"Configuration file to be processed.\")]" +
            "\r\n            //public string ConfigFile { get; set; }" +
            "\r\n\r\n            [Option('w', \"wordlist\", Required = true, HelpText = \"Wordlist file to be processed.\")]" +
            "\r\n            public string WordlistFile { get; set; }" +
            "\r\n\r\n            [Option('o', \"output\", Default = \"\", HelpText = \"Output file for the hits.\")]" +
            "\r\n            public string OutputFile { get; set; }" +
            "\r\n\r\n            [Option(\"wltype\", Required = true, HelpText = \"Type of the wordlist loaded (see Environment.ini for all allowed types).\")]" +
            "\r\n            public string WordlistType { get; set; }" +
            "\r\n\r\n            [Option(\"useproxies\", HelpText = \"Enable / Disable the usage of proxies (uses config default if not set).\")]" +
            "\r\n            public bool? UseProxies { get; set; }" +
            "\r\n\r\n            [Option('p', \"proxies\", Default = null, HelpText = \"Proxy file to be processed.\")]" +
            "\r\n            public string ProxyFile { get; set; }" +
            "\r\n\r\n            [Option(\"ptype\", Default = ProxyType.Http, HelpText = \"Type of proxies loaded (Http, Socks4, Socks4a, Socks5).\")]" +
            "\r\n            public ProxyType ProxyType { get; set; }" +
            "\r\n\r\n            [Option('v', \"verbose\", Default = false, HelpText = \"Prints all bots behaviour.\")]" +
            "\r\n            public bool Verbose { get; set; }" +
            "\r\n\r\n            [Option('s', \"skip\", Default = 1, HelpText = \"Number of lines to skip in the Wordlist.\")]" +
            "\r\n            public int Skip { get; set; }" +
            "\r\n\r\n            [Option('b', \"bots\", Default = 0, HelpText = \"Number of concurrent bots working. If not specified, the config default will be used.\")]" +
            "\r\n            public int BotsNumber { get; set; }" +
            "\r\n\r\n            [Usage(ApplicationAlias = \"SilverBulletCLI.exe\")]" +
            "\r\n            public static IEnumerable<Example> Examples" +
            "\r\n            {\r\n" +
            "                get\r\n" +
            "                {\r\n                    " +
            "return new List<Example>() {" +
            "\r\n                        " +
            "new Example(\"Simple POC CLI that executes a Runner.\"," +
            "\r\n                            new Options {" +
            "\r\n                                WordlistFile = \"rockyou.txt\"," +
            "\r\n                                WordlistType = \"Default\"," +
            "\r\n                                ProxyFile = \"proxies.txt\"," +
            "\r\n                                OutputFile = \"hits.txt\"," +
            "\r\n                                ProxyType = ProxyType.Http," +
            "\r\n                                UseProxies = true," +
            "\r\n                                Verbose = false," +
            "\r\n                                Skip = 1," +
            "\r\n                                BotsNumber = 1" +
            "\r\n                            }" +
            "\r\n                        )" +
            "\r\n                    };" +
            "\r\n                }" +
            "\r\n            }" +
            "\r\n        }" +
            "\r\n\r\n        ";
        private string beginMainMethod =
            "static void Main(string[] args)" +
            "\r\n        {" +
            "\r\n\r\n            " +
            "Console.Clear();" +
            "\r\n\r\n            " +
            "WriteName();" +
            "\r\n            " +
            "Console.WriteLine();\r\n            " +
            "Console.WriteLine();\r\n            " +
            "Console.WriteLine(\" [+] SB CLI by MOHAMM4DX\", Color.White);\r\n\r\n   ";

        private readonly string endMainMethod = "\r\n         // Read Environment file\r\n            Env = IOManager.ParseEnvironmentSettings(envFile);" +
            "\r\n\r\n            // Read Settings file\r\n            if (!File.Exists(settFile)) IOManager.SaveSettings(settFile, new RLSettingsViewModel());" +
            "\r\n            RLSettings = IOManager.LoadSettings<RLSettingsViewModel>(settFile);" +
            "\r\n\r\n            // Initialize the Runner (and hook event handlers)" +
            "\r\n            Runner = new RunnerViewModel(Env, RLSettings, random);" +
            "\r\n            Runner.AskCustomInputs += delegate { Runner.CustomInputsInitialized = true; };" +
            "\r\n            Runner.DispatchAction += DispatchAction;" +
            "\r\n            Runner.FoundHit += FoundHit;" +
            "\r\n            Runner.MessageArrived += MessageArrived;" +
            "\r\n            Runner.ReloadProxies += ReloadProxies;" +
            "\r\n            Runner.SaveProgress += SaveProgress;" +
            "\r\n            Runner.WorkerStatusChanged += WorkerStatusChanged;" +
            "\r\n\r\n\t\t\tif(!string.IsNullOrEmpty(message))" +
            "\t\r\n\t\t\t\t{\r\n\t\t\t\t\tConsole.WriteLine($\" [+] Message: {message}\",messageColor);" +
            "\r\n\t\t\t\t}\r\n\r\n" +
            "            // Parse the Options\r\n            Parser.Default.ParseArguments<Options>(args)\r\n              .WithParsed<Options>(opts => Run(opts))\r\n              .WithNotParsed<Options>((errs) => HandleParseError(errs));" +
            "\r\n\r\n            Console.ReadKey();\r\n        }";
        private string methods = "\r\n\r\n        private static void WorkerStatusChanged(IRunnerMessaging obj)\r\n        {\r\n            // Nothing to do here since the Title is updated every 100 ms anyways\r\n        }\r\n\r\n        private static void SaveProgress(IRunnerMessaging obj)\r\n        {\r\n            // TODO: Implement progress saving (maybe to a file)\r\n        }\r\n\r\n        private static void ReloadProxies(IRunnerMessaging obj)\r\n        {\r\n            // Set Proxies\r\n            if (ProxyFile == null) return;\r\n            var proxies = File.ReadLines(ProxyFile)\r\n                    .Select(p => new CProxy(p, ProxyType))\r\n                    .ToList();\r\n\r\n            List<CProxy> toAdd;\r\n            if (Runner.Config.Settings.OnlySocks) toAdd = proxies.Where(x => x.Type != ProxyType.Http).ToList();\r\n            else if (Runner.Config.Settings.OnlySsl) toAdd = proxies.Where(x => x.Type == ProxyType.Http).ToList();\r\n            else toAdd = proxies;\r\n\r\n            Runner.ProxyPool = new ProxyPool(toAdd);\r\n\r\n        }\r\n\r\n        private static void MessageArrived(IRunnerMessaging obj, LogLevel level, string message, bool prompt, int timeout)\r\n        {\r\n            // Do not print anything if no verbose argument was declared\r\n            if (!Verbose) return;\r\n\r\n            write++;\r\n            // Print the message to the screen\r\n            Console.WriteLine($\"[{DateTime.Now}][{level}] {message}\", level == LogLevel.Warning ? Color.Orange : Color.Tomato);\r\n        }\r\n\r\n        \r\n\r\n        private static void DispatchAction(IRunnerMessaging obj, Action action)\r\n        {\r\n            // No need to delegate the action to the UI thread in CLI, so just invoke it\r\n            action.Invoke();\r\n        }\r\n\r\n        private static void AskCustomInputs(IRunnerMessaging obj)\r\n        {\r\n            // Ask all the custom inputs in the console and set their values in the Runner\r\n            foreach (var input in Runner.Config.Settings.CustomInputs)\r\n            {\r\n                Console.WriteLine($\" Set custom input ({input.Description}): \", customInputColor);\r\n                Console.SetCursorPosition(1, Console.CursorTop);\r\n                Runner.CustomInputs.Add(new KeyValuePair<string, string>(input.VariableName, Console.ReadLine()));\r\n            }\r\n        }\r\n\r\n\r\n        private static void HandleParseError(IEnumerable<Error> errs)\r\n        {\r\n\r\n        }\r\n\r\n        private static void LoadOptions(Options opts)\r\n        {\r\n            // Load the user-defined options into local variables or into the local Runner instance\r\n            Verbose = opts.Verbose;\r\n            outFile = opts.OutputFile;\r\n            ProxyFile = opts.ProxyFile;\r\n            ProxyType = opts.ProxyType;\r\n            Runner.SetConfig(IOManager.DeserializeConfig(SvbConfig), false);\r\n            Runner.SetWordlist(new Wordlist(opts.WordlistFile, opts.WordlistFile, opts.WordlistType, \"\"));\r\n            Runner.StartingPoint = opts.Skip;\r\n            if (opts.BotsNumber <= 0) Runner.BotsAmount = Runner.Config.Settings.SuggestedBots;\r\n            else Runner.BotsAmount = opts.BotsNumber;\r\n\r\n            if (opts.ProxyFile != null && opts.UseProxies != null)\r\n            {\r\n                Runner.ProxyMode = (bool)opts.UseProxies ? ProxyMode.On : ProxyMode.Off;\r\n            }\r\n        }\r\n\r\n        private static void LogErrorAndExit(string message)\r\n        {\r\n            Console.WriteLine($\"ERROR: {message}\", Color.Tomato);\r\n            Console.ReadLine();\r\n            Environment.Exit(0);\r\n        }\r\n\r\n        private static void SetTitle(string title = \"\")\r\n        {\r\n\t\t\tvar customTitle= nameof(SilverBulletCLI);\r\n\t\t\tif(!string.IsNullOrWhiteSpace(Title)) \r\n\t\t\t{\r\n\t\t\t\tcustomTitle +=\" | \"+ Title;\r\n\t\t\t}\r\n            Console.Title = $\"{customTitle} - {Runner.Master.Status} | \" +\r\n                $\"Config: {Runner.ConfigName}{(title != \"\" ? (\" | \" + title) : title)}\";\r\n        }\r\n\r\n        private static void WriteName()\r\n        {\r\n            Console.WriteLine();\r\n            Console.WriteLine(\"       ██████  ██▓ ██▓     ██▒   █▓▓█████  ██▀███      ▄▄▄▄    █    ██  ██▓     ██▓    ▓█████ ▄▄▄█████▓\", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"     ▒██    ▒ ▓██▒▓██▒    ▓██░   █▒▓█   ▀ ▓██ ▒ ██▒   ▓█████▄  ██  ▓██▒▓██▒    ▓██▒    ▓█   ▀ ▓  ██▒ ▓▒\", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"     ░ ▓██▄   ▒██▒▒██░     ▓██  █▒░▒███   ▓██ ░▄█ ▒   ▒██▒ ▄██▓██  ▒██░▒██░    ▒██░    ▒███   ▒ ▓██░ ▒░\", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"       ▒   ██▒░██░▒██░      ▒██ █░░▒▓█  ▄ ▒██▀▀█▄     ▒██░█▀  ▓▓█  ░██░▒██░    ▒██░    ▒▓█  ▄ ░ ▓██▓ ░ \", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"     ▒██████▒▒░██░░██████▒   ▒▀█░  ░▒████▒░██▓ ▒██▒   ░▓█  ▀█▓▒▒█████▓ ░██████▒░██████▒░▒████▒  ▒██▒ ░ \", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"     ▒ ▒▓▒ ▒ ░░▓  ░ ▒░▓  ░   ░ ▐░  ░░ ▒░ ░░ ▒▓ ░▒▓░   ░▒▓███▀▒░▒▓▒ ▒ ▒ ░ ▒░▓  ░░ ▒░▓  ░░░ ▒░ ░  ▒ ░░   \", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"     ░ ░▒  ░ ░ ▒ ░░ ░ ▒  ░   ░ ░░   ░ ░  ░  ░▒ ░ ▒░   ▒░▒   ░ ░░▒░ ░ ░ ░ ░ ▒  ░░ ░ ▒  ░ ░ ░  ░    ░    \", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"     ░  ░  ░   ▒ ░  ░ ░        ░░     ░     ░░   ░     ░    ░  ░░░ ░ ░   ░ ░     ░ ░      ░     ░      \", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"           ░   ░      ░  ░      ░     ░  ░   ░         ░         ░         ░  ░    ░  ░   ░  ░         \", Color.FromArgb(255, 226, 29, 29));\r\n            Console.WriteLine(\"                               ░                            ░                                          \", Color.FromArgb(255, 226, 29, 29));\r\n        }\r\n\r\n        private static void UpdateStats()\r\n        {\r\n            if (Console.CursorTop > 25)\r\n            {\r\n                UpdateLine(10 + write, $\" [~] CPM: {Runner.CPM}\", cpmColor);\r\n                UpdateLine(9 + write, $\" [~] Progress: {Runner.ProgressCount} / {Runner.WordlistSize} ({Runner.Progress}%)\", progressColor);\r\n                UpdateLine(8 + write, $\" [~] Hits: {Runner.HitCount}\", hitsColor);\r\n                UpdateLine(7 + write, $\" [~] Custom: {Runner.CustomCount}\", customColor);\r\n                UpdateLine(6 + write, $\" [~] ToCheck: {Runner.ToCheckCount}\", toCheckColor);\r\n                UpdateLine(5 + write, $\" [~] Fails: {Runner.FailCount}\", failsColor);\r\n                UpdateLine(4 + write, $\" [~] Retries: {Runner.RetryCount}\", retriesColor);\r\n                UpdateLine(3 + write, $\" [~] OcrRate: {Runner.OcrRate}\", ocrRateColor);\r\n                UpdateLine(2 + write, $\" [~] Proxies: {Runner.AliveProxiesCount} / {Runner.TotalProxiesCount}\", proxiesColor);\r\n                return;\r\n            }\r\n            Console.WriteLine($\" [~] CPM: {Runner.CPM}\", cpmColor);\r\n            Console.WriteLine($\" [~] Progress: {Runner.ProgressCount} / {Runner.WordlistSize} ({Runner.Progress}%)\", progressColor);\r\n            Console.WriteLine($\" [~] Hits: {Runner.HitCount}\", hitsColor);\r\n            Console.WriteLine($\" [~] Custom: {Runner.CustomCount}\", customColor);\r\n            Console.WriteLine($\" [~] ToCheck: {Runner.ToCheckCount}\", toCheckColor);\r\n            Console.WriteLine($\" [~] Fails: {Runner.FailCount}\", failsColor);\r\n            Console.WriteLine($\" [~] Retries: {Runner.RetryCount}\", retriesColor);\r\n            Console.WriteLine($\" [~] OcrRate: {Runner.OcrRate}\", ocrRateColor);\r\n            Console.WriteLine($\" [~] Proxies: {Runner.AliveProxiesCount} / {Runner.TotalProxiesCount}\", proxiesColor);\r\n            Console.WriteLine();\r\n        }\r\n\r\n        public static void UpdateLine(int lineNumber, string newText, Color color)\r\n        {\r\n            Console.CursorVisible = false;\r\n            int currentLineCursor = Console.CursorTop;\r\n            Console.SetCursorPosition(0, currentLineCursor - lineNumber);\r\n            Console.Write(newText, color);\r\n            Console.SetCursorPosition(0, currentLineCursor);\r\n        }\r\n\t\t\r\n\t\tprivate static Color GetColor(string color)\r\n\t\t{\r\n\t\t\tvar Rgb=color.Trim().Replace(\" \",\"\").Split(',');\r\n\t\t\treturn Color.FromArgb(255,int.Parse(Rgb[0]),int.Parse(Rgb[1]),int.Parse(Rgb[2]));\r\n\t\t}";

        private string beginRunMethod = "\r\n\r\n        private static void Run(Options opts)\r\n        {\r\n            SetTitle();\r\n";
        private readonly string endRunMethod = "\r\n            // Set Runner settings from the specified Options\r\n            LoadOptions(opts);\r\n\r\n            // Create the hits file\r\n            if (opts.OutputFile != string.Empty)\r\n            {\r\n                File.Create(outFile).Close();\r\n                Console.WriteLine($\" [+] The Hits File is {outFile}\", Color.Aquamarine);\r\n            }\r\n\r\n            Console.WriteLine($\" [+] Author: {Runner.Config.Settings.Author}\", authorColor);\r\n            Console.WriteLine($\" [+] Wordlist: {Runner.WordlistName}\", wordlistColor);\r\n            if (Runner.UseProxies)\r\n            {\r\n                Console.WriteLine($\" [+] Proxylist: {opts.ProxyFile}\", Color.FromArgb(255, 181, 194, 225));\r\n                Console.WriteLine($\" [+] ProxyType: {opts.ProxyType}\", Color.FromArgb(255, 181, 194, 225));\r\n            }\r\n            Console.WriteLine($\" [+] Bots: {Runner.BotsAmount}\", botsColor);\r\n            Console.WriteLine();\r\n\r\n            if (Runner.Config.Settings.CustomInputs?.Count > 0)\r\n            {\r\n                AskCustomInputs(null);\r\n            }\r\n            Console.WriteLine();\r\n\r\n            // Start the runner\r\n            Runner.Start();\r\n\r\n            SetTitle();\r\n\r\n            // Wait until it finished\r\n            while (Runner.Busy)\r\n            {\r\n                Thread.Sleep(100);\r\n                UpdateStats();\r\n            }\r\n\r\n            // Print colored finish message\r\n            SetTitle(\"Finished\");\r\n            Console.WriteLine($\"Finished.\", Color.LightGreen);\r\n            //Console.Write($\"{Runner.HitCount} hits, \", Color.GreenYellow);\r\n            //Console.Write($\"{Runner.CustomCount} custom, \", Color.DarkOrange);\r\n            //Console.WriteLine($\"{Runner.ToCheckCount} to check.\", Color.Aquamarine);\r\n\r\n            // Prevent console from closing until the user presses return, then close\r\n            Console.WriteLine(\"Press any key to exit.\", Color.White);\r\n            Console.CursorVisible = true;\r\n            Console.ReadLine();\r\n            Environment.Exit(0);\r\n        }";
        private readonly string endSource = "\r\n\t}\r\n}\r\n";

        public enum CodeProviderLanguage
        {
            CS, VB
        }
    }
}
