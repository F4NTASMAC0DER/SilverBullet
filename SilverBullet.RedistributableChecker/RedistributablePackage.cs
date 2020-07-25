using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SilverBullet.RedistributableChecker
{
    public static class RedistributablePackage
    {
        public static string[] GetInstalledVersion()
        {
            var subKeyVc = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Dependencies\", false);
            var versionList = new List<string>();

            Parallel.ForEach(subKeyVc.GetSubKeyNames().Where(s => s.StartsWith("{")), currSubKey =>
              {
                  using (RegistryKey key = subKeyVc.OpenSubKey(currSubKey))
                  {
                      var value = key.GetValue("DisplayName");
                      if (value != null)
                      {
                          var valString = value.ToString();
                          if (valString.Contains("Microsoft Visual C++ 2015 x86 Debug Runtime"))
                          {
                              versionList.Add("VC++ 2015 x86 Debug Runtime Installed");
                          }
                          else if (valString.Contains("Microsoft Visual C++ 2015 Redistributable (x86)"))
                          {
                              versionList.Add("VC++ 2015 x86 Installed");
                          }
                          else if (valString.Contains("Microsoft Visual C++ 2015 x64 Debug Runtime"))
                          {
                              versionList.Add("VC++ 2015 x64 Debug Runtime Installed");
                          }
                          else if (valString.Contains("Microsoft Visual C++ 2015 Redistributable (x64)"))
                          {
                              versionList.Add("VC++ 2015 Redistributable x64 Installed");
                          }
                          else if (valString.Contains("Microsoft Visual C++ 2017 x64 Debug Runtime"))
                          {
                              versionList.Add("VC++ 2017 x64 Debug Runtime Installed");
                          }
                          else if (valString.Contains("Microsoft Visual C++ 2017 x86 Debug Runtime"))
                          {
                              versionList.Add("VC++ 2017 x86 Debug Runtime Installed");
                          }
                          else if (versionList.Contains("Microsoft Visual C++ 2017 Redistributable (x86)"))
                          {
                              versionList.Add("VC++ 2017 Redistributable x86 Installed");
                          }
                          else if (versionList.Contains("Microsoft Visual C++ 2017 Redistributable (x64)"))
                          {
                              versionList.Add("VC++ 2017 Redistributable x64 Installed");
                          }
                          if (versionList.Count == 8) return;
                      }
                  }
              });
            return versionList.ToArray();
        }
    }
}
