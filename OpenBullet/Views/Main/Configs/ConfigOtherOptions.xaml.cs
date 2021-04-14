using System.Windows.Controls;
using System.Windows.Input;
using OpenBullet.Views.Main.Configs.OtherOptions;

namespace OpenBullet.Views.Main.Configs
{
    /// <summary>
    /// Logica di interazione per ConfigOtherOptions.xaml
    /// </summary>
    public partial class ConfigOtherOptions : Page
    {
        public General GeneralPage = new General();
        public Requests RequestsPage = new Requests();
        public Proxies ProxiesPage = new Proxies();
        public Inputs InputsPage = new Inputs();
        public Data DataPage = new Data();
        public Selenium SeleniumPage = new Selenium();
        public Compile CompilePage = new Compile();

        public ConfigOtherOptions()
        {
            InitializeComponent();

            menuOptionGeneral_MouseDown(this, null);
        }

        #region Menu Options
        private void menuOptionGeneral_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = GeneralPage;
            menuOptionSelected(menuOptionGeneral);
        }

        private void menuOptionRequests_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = RequestsPage;
            menuOptionSelected(menuOptionRequests);
        }

        private void menuOptionProxies_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = ProxiesPage;
            menuOptionSelected(menuOptionProxies);
        }

        private void menuOptionInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = InputsPage;
            menuOptionSelected(menuOptionInput);
        }

        private void menuOptionData_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = DataPage;
            menuOptionSelected(menuOptionData);
        }

        private void menuOptionSelenium_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = SeleniumPage;
            menuOptionSelected(menuOptionSelenium);
        }

        private void menuOptionCompile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = CompilePage;
            menuOptionSelected(sender);
        }

        private void menuOptionSelected(object sender)
        {
            foreach (Label child in topMenu.Children)
            {
                try
                {
                    child.Foreground = Utils.GetBrush("ForegroundMain");
                }
                catch { }
            }
            ((Label)sender).Foreground = Utils.GetBrush("ForegroundGood");
        }


        #endregion
    }
}
