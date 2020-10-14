using System.Collections.Generic;
using System.Reflection;

namespace OpenBullet.ViewModels
{
    public class SBSettingsViewModel
    {
        public SBSettingsGeneral General { get; set; } = new SBSettingsGeneral();
        public OBSettingsSounds Sounds { get; set; } = new OBSettingsSounds();
        public OBSettingsSources Sources { get; set; } = new OBSettingsSources();
        public OBSettingsThemes Themes { get; set; } = new OBSettingsThemes();

        public void Reset()
        {
            General.Reset();
            Sounds.Reset();
            Sources.Reset();
            Themes.Reset();
        }
    }
}
