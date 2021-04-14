using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace OpenBullet
{
    public static class CheckUpdate
    {
        public static T Run<T>(string url = "https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/SilverBulletUpdater/SBUpdate.json")
        {
            var data = string.Empty;
            using (var wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
                data = wc.DownloadString(url);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
    public class SBUpdate
    {
        public string Version { get; set; }

        public bool Available { get; set; }

        public string Message { get; set; }

        public string DownloadUrl { get; set; }

        public List<ReleaseNotes> ReleaseNotes { get; set; } = new List<ReleaseNotes>();

        public Donate[] Donate { get; set; }
    }
    public class ReleaseNotes
    {
        public string Note { get; set; }
    }
    public class Donate
    {
        public string Address { get; set; }
    }
    public class Release
    {
        public string Name { get; set; }

        public Assets[] Assets { get; set; }

        public string Body { get; set; }


        public Version Ver => Version.Parse(Regex.Match(Name, @"\d+(\.\d+)+").Value);
        public bool Available => Ver > Version.Parse(SB.Version);
    }

    public class Assets
    {
        public string Browser_Download_Url { get; set; }
    }
}
