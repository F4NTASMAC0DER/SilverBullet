using System.Collections.Generic;
using Noesis.Javascript;
using RuriLib.LS;
using RuriLib.Models;

namespace RuriLib.SB.JS
{
    /// <summary>
    /// JavaScript Engine
    /// </summary>
    public class JsEngine
    {
        private List<Engine> Engines = new List<Engine>();
        private readonly object createEngineLock = new object();
        private readonly object createEngineDisposedLock = new object();
        private readonly object removeEngineLock = new object();

        /// <summary>
        /// Get or create javascript context (engine)
        /// </summary>
        /// <param name="data">bot data</param>
        /// <returns></returns>
        public JavascriptContext GetOrCreateEngine(BotData data)
        {
            if (data.BotsAmount > Engines.Count)
            {
                lock (createEngineLock)
                {
                    for (var b = 0; b <= data.BotsAmount - Engines.Count; b++)
                    {
                        Engines.Add(CreateJsEngine(data));
                    }
                }
            }
            lock (removeEngineLock)
            {
                while (Engines.Count > data.BotsAmount)
                {
                    var index = Engines.Count - 1;
                    try { Engines[index].JavascriptContext.Dispose(); } catch { }
                    try { Engines.RemoveAt(index); } catch { }
                }
            }
            var context = Engines[data.BotNumber - 1].JavascriptContext;
            SetParameter(context, data);
            return context;
        }

        private Engine CreateJsEngine(BotData data)
        {
            var context = new JavascriptContext();
            var engine = new Engine(context, SetParameter(context, data));
            return engine;
        }

        /// <summary>
        /// Set variables
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<string, object> SetParameter(JavascriptContext context,
            BotData data)
        {
            Dictionary<string, object> @params = new Dictionary<string, object>();

            context.SetParameter("console", new SysConsole());
            // Add in all the variables
            data.Variables.All.ForEach(variable =>
            {
                try
                {
                    switch (variable.Type)
                    {
                        case CVar.VarType.List:
                            @params.Add(variable.Name, (variable.Value as List<string>).ToArray());
                            context.SetParameter(variable.Name, (variable.Value as List<string>).ToArray());
                            break;

                        default:
                            @params.Add(variable.Name, variable.Value.ToString());
                            context.SetParameter(variable.Name, variable.Value.ToString());
                            break;
                    }
                }
                catch { }
            });
            return @params;
        }

        /// <summary>
        /// Dispose and remove js engines
        /// </summary>
        public void DisposeEngines()
        {
            try
            {
                Engines.ForEach(e =>
               {
                   try
                   {
                       e.JavascriptContext.Dispose();
                       Engines.RemoveAt(0);
                   }
                   catch { Engines.RemoveAt(0); }
               });
            }
            catch { }
        }
    }
}
