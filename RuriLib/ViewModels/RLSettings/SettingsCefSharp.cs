using System.Collections.Generic;

namespace RuriLib.ViewModels
{
    /// <summary>
    /// Provides cefsharp-related settings.
    /// </summary>
    public class SettingsCefSharp : ViewModelBase
    {
        private bool packLoadingDisabled;
        ///<summary>
        ///Set to true to disable loading of pack files for resources and locales. A resource
        ///bundle handler must be provided for the browser and render processes via CefApp::GetResourceBundleHandler()
        ///if loading of pack files is disabled. Also configurable using the "disable-pack-loading"
        ///command- line switch.
        ///</summary>
        public bool PackLoadingDisabled
        {
            get => packLoadingDisabled;
            set { packLoadingDisabled = value; OnPropertyChanged(); }
        }

        private bool ignoreCertificateErrors = true;
        /// <summary>
        ///Set to true in order to completely ignore SSL certificate errors. This is NOT
        ///recommended.
        /// </summary>
        public bool IgnoreCertificateErrors
        {
            get => ignoreCertificateErrors;
            set
            {
                ignoreCertificateErrors = value; OnPropertyChanged();
            }
        }

        private string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36";
        ///<summary>
        ///Value that will be returned as the User-Agent HTTP header. If empty the default
        ///User-Agent string will be used. Also configurable using the "user-agent" command-line
        ///switch.
        ///</summary>
        public string UserAgent
        {
            get => userAgent;
            set
            {
                userAgent = value; OnPropertyChanged();
            }
        }

        ///<summary>
        ///Add custom command line arguments to this collection, they will be added in OnBeforeCommandLineProcessing.
        ///The CefSettings.CommandLineArgsDisabled value can be used to start with an empty
        ///command-line object. Any values specified in CefSettings that equate to command-line
        ///arguments will be set before this method is called.
        ///</summary>
        public Dictionary<string, string> CmdLineArgs { get; }

    }
}
