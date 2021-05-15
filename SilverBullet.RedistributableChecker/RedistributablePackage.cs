using System.Collections.Generic;
using Microsoft.Win32;

namespace SilverBullet.RedistributableChecker
{
    /// <summary>
    ///	Class to detect installed Microsoft Redistributable Packages.
    /// </summary>
    /// <see cref="//https://stackoverflow.com/questions/12206314/detect-if-visual-c-redistributable-for-visual-studio-2012-is-installed"/>
    public static class RedistributablePackage
    {
        public static string[] GetInstalledVersion()
        {
            var versionList = new List<string>();

            var parametersVc2015to2019x86 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\DevDiv\VC\Servicing\14.0\RuntimeMinimum", false);
            if (parametersVc2015to2019x86 != null)
            {
                var vc2015to2019x86Version = parametersVc2015to2019x86.GetValue("Version");
                if (((string)vc2015to2019x86Version).StartsWith("14"))
                {
                    versionList.Add("VC2015to2019x86");
                }
            }

            var parametersVc2015to2019x64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\DevDiv\VC\Servicing\14.0\RuntimeMinimum", false);
            if (parametersVc2015to2019x64 != null)
            {
                var vc2015to2019x64Version = parametersVc2015to2019x64.GetValue("Version");
                if (((string)vc2015to2019x64Version).StartsWith("14"))
                {
                    versionList.Add("VC2015to2019x64");
                }
            }

            var paths2017x86 = new List<string>
                            {
                            @"Installer\Dependencies\,,x86,14.0,bundle",
                            @"Installer\Dependencies\VC,redist.x86,x86,14.16,bundle" //changed in 14.16.x
                                    };
            foreach (var path in paths2017x86)
            {
                var parametersVc2017x86 = Registry.ClassesRoot.OpenSubKey(path, false);
                if (parametersVc2017x86 == null) continue;
                var vc2017x86Version = parametersVc2017x86.GetValue("Version");
                if (vc2017x86Version != null)
                {
                    if (((string)vc2017x86Version).StartsWith("14"))
                    {
                        versionList.Add("VC2017x86");
                        break;
                    }
                }
            }

            var paths2017x64 = new List<string>
                            {
                            @"Installer\Dependencies\,,amd64,14.0,bundle",
                            @"Installer\Dependencies\VC,redist.x64,amd64,14.16,bundle" //changed in 14.16.x
                                    };
            foreach (var path in paths2017x64)
            {
                var parametersVc2017x64 = Registry.ClassesRoot.OpenSubKey(path, false);
                if (parametersVc2017x64 == null) continue;
                var vc2017x64Version = parametersVc2017x64.GetValue("Version");
                if (vc2017x64Version != null)
                {
                    if (((string)vc2017x64Version).StartsWith("14"))
                    {
                        versionList.Add("VC2017x64");
                        break;
                    }
                }
            }

            var parametersVc2015x86 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Dependencies\{e2803110-78b3-4664-a479-3611a381656a}", false);
            if (parametersVc2015x86 != null)
            {
                var vc2015x86Version = parametersVc2015x86.GetValue("Version");
                if (((string)vc2015x86Version).StartsWith("14"))
                {
                    versionList.Add("VC2015x86");
                }
            }
            var parametersVc2015x64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Installer\Dependencies\{d992c12e-cab2-426f-bde3-fb8c53950b0d}", false);
            if (parametersVc2015x64 != null)
            {
                var vc2015x64Version = parametersVc2015x64.GetValue("Version");
                if (((string)vc2015x64Version).StartsWith("14"))
                {
                    versionList.Add("VC2015x64");
                }
            }
            return versionList.ToArray();
        }
    }
}
