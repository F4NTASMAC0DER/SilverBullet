using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using OpenBullet.Views.UserControls;
using RuriLib;
using RuriLib.Models;

namespace OpenBullet.Views.Main
{
    /// <summary>
    /// Interaction logic for Support.xaml
    /// </summary>
    public partial class Supporters : Page
    {
        public Supporters()
        {
            InitializeComponent();
        }

        SupportersModel[] supporters;

        BrushConverter brushConverter = new BrushConverter();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (wrapPanel.Children.Count <= 0) { waitingLabel.Visibility = Visibility.Visible; }
                else { waitingLabel.Visibility = Visibility.Collapsed; }

                var data = string.Empty;
                using (Task.Run(() =>
                {
                    using (var wc = new WebClient())
                    {
                        wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
                        data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/Supporters.json");
                    }
                }).ContinueWith(_ =>
               {
                   if (string.IsNullOrWhiteSpace(data))
                   {
                       Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                       {
                           waitingLabel.Visibility = Visibility.Visible;
                           waitingLabel.Content = "ERROR";
                       });
                   }
                   else
                   {
                       Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                       {
                           waitingLabel.Visibility = Visibility.Collapsed;
                       });
                   }
                   supporters = IOManager.DeserializeObject<SupportersModel[]>(data);

                   Dispatcher.Invoke(() =>
                   {
                       SB.MainWindow.SilverZonePage.vm.SupportersBadge = supporters.Length > 99 ? "999+" : supporters.Length.ToString();
                       var badge = supporters.Length + int.Parse(SB.MainWindow.SilverZonePage.vm.VerifiedMarketBadge.Replace("+", ""));
                       SB.MainWindow.silverZoneBadged.Badge = badge > 99 ? "99+" : badge.ToString();
                   });

                   try { SetSupporters(); } catch { }
               })) ;
            }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { }
            catch (Exception ex)
            {
                waitingLabel.Visibility = Visibility.Visible;
                waitingLabel.Content = "ERROR";
            }
        }

        private async void SetSupporters()
        {
            if (supporters == null || supporters.Length <= 0) return;
            for (var i = 0; i < supporters.Length; i++)
            {
                try
                {
                    await Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                      {
                          var uc = new UserControlSupport()
                          {
                              Width = 200,
                              Height = 200,
                              SupportName = supporters[i].Name,
                              Margin = new Thickness(0, 0, 8, 8),
                              BackgroundButton = (SolidColorBrush)brushConverter.ConvertFrom(supporters[i].Color),
                              Url = supporters[i].Address
                          };
                          uc.SetImage(new Uri(supporters[i].Logo));
                          if (!wrapPanel.Children.OfType<UserControlSupport>().Any(u => u.Url == uc.Url))
                          {
                              wrapPanel.Children.Add(uc);
                          }
                      });
                }
                catch { }
            }
        }
    }
}
