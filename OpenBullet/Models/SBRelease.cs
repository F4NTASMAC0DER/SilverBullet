using System;
using System.Text.RegularExpressions;

namespace OpenBullet.Models
{
    public class SBRelease
    {
        public string Name { get; set; }

        public Assets[] Assets { get; set; }

        public Version Ver => Version.Parse(Regex.Match(Name, @"\d+(\.\d+)+").Value);
    }

    public class Assets
    {
        public int download_count { get; set; }
    }
}
