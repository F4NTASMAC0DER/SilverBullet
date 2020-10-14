using Newtonsoft.Json;
using OpenBullet.ViewModels;
using RuriLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBullet
{
    public class SBIOManager
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public static void SaveSettings(string settingsFile, SBSettingsViewModel settings)
        {
            File.WriteAllText(settingsFile, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

        public static SBSettingsViewModel LoadSettings(string settingsFile)
        {
            return JsonConvert.DeserializeObject<SBSettingsViewModel>(File.ReadAllText(settingsFile));
        }

        public static void CheckRequiredPlugins(IEnumerable<string> available, Config config)
        {
            foreach (var required in config.Settings.RequiredPlugins)
            {
                if (!available.Contains(required))
                {
                    throw new Exception($"This config requires the plugin {required} which is missing from the Plugins folder and hence cannot be opened!");
                }
            }
        }
    }
}
