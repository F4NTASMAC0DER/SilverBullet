using System.Collections.Generic;
using Noesis.Javascript;

namespace RuriLib.SB.JS
{
    /// <summary>
    /// javascript engine (model)
    /// </summary>
    public class Engine
    {
        /// <summary></summary>
        /// <param name="params">variables</param>
        /// <param name="context"></param>
        public Engine(JavascriptContext context,Dictionary<string, object> @params)
        {
            Parameters = @params;
            JavascriptContext = context;
        }

        /// <summary>
        /// js variables
        /// </summary>
        public Dictionary<string, object> Parameters { get; private set; } =
            new Dictionary<string, object>();

        /// <summary>javascript engine</summary>
        public JavascriptContext JavascriptContext { get; private set; }

    }
}
