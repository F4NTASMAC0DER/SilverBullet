using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using OpenBullet.ViewModels;

namespace OpenBullet.Views.Main
{
    /// <summary>
    /// Interaction logic for SilverZone.xaml
    /// </summary>
    public partial class SilverZone : Page
    {
        private Supporters supportersPage = new Supporters();
        private VerifiedMarket verifiedMarketPage = new VerifiedMarket();

        public SilverZoneViewModel vm;

        public SilverZone(SilverZoneViewModel viewModel = null)
        {
            InitializeComponent();
            DataContext = vm = viewModel ?? new SilverZoneViewModel();
            menuOptionSupporters_MouseDown(this, null);
        }

        #region Menu Options

        private void menuOptionSupporters_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = supportersPage;
            menuOptionSelected(menuOptionSupporters);
        }

        private void menuOptionVerifiedMarket_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = verifiedMarketPage;
            menuOptionSelected(menuOptionVerifiedMarket);
        }

        private void menuOptionSelected(object sender)
        {
            foreach (var child in topMenu.Children)
            {
                try
                {
                    Label option;
                    if (child is Badged badged)
                    {
                        option = badged.Content as Label;
                    }
                    else
                    {
                        option = (Label)child;
                    }
                    option.Foreground = Utils.GetBrush("ForegroundMain");
                }
                catch { }
            }
           ((Label)sender).Foreground = Utils.GetBrush("ForegroundGood");
        }

        #endregion

        public int GetBadge()
        {
            string data;
            int supCount, veriMarketCount;
            using (var wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");

                #region Supporters
                data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/Supporters.json");
                supCount = Regex.Matches(data, "\"Name\":\"").Count;
                #endregion

                #region Verified market
                data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/VerifiedMarket.json");
                veriMarketCount = Regex.Matches(data, "\"Content\":\"").Count;
                #endregion
            }
            if (vm != null)
            {
                vm.SupportersBadge = supCount > 99 ? "99+" : supCount.ToString();
                vm.VerifiedMarketBadge = veriMarketCount > 99 ? "99+" : veriMarketCount.ToString();
            }
            return supCount + veriMarketCount;
        }
    }
}
