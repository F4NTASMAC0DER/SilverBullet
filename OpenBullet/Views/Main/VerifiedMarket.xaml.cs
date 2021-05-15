using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using OpenBullet.Views.UserControls;
using RuriLib;
using RuriLib.Models;

namespace OpenBullet.Views.Main
{
    /// <summary>
    /// Interaction logic for VerifiedMarket.xaml
    /// </summary>
    public partial class VerifiedMarket : Page
    {
        public VerifiedMarket()
        {
            InitializeComponent();
            itemsControl.ItemsSource = marketCollection;
        }

        private Market[] markets;

        private ObservableCollection<UserControlMarket> marketCollection = new ObservableCollection<UserControlMarket>();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (marketCollection.Count <= 0) { waitingLabel.Visibility = Visibility.Visible; }
                else { waitingLabel.Visibility = Visibility.Collapsed; }

                var data = string.Empty;
                using (Task.Run(() =>
                {
                    using (var wc = new WebClient())
                    {
                        wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
                        data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/VerifiedMarket.json");
                    }
                }).ContinueWith(_ =>
                {
                    if (string.IsNullOrWhiteSpace(data))
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            waitingLabel.Visibility = Visibility.Visible;
                            searchBoxDockPanel.Visibility = Visibility.Collapsed;
                            waitingLabel.Content = "ERROR";
                        });
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            waitingLabel.Visibility = Visibility.Collapsed;
                            searchBoxDockPanel.Visibility = Visibility.Visible;
                        });
                    }
                    markets = IOManager.DeserializeObject<Market[]>(data);
                    Dispatcher.Invoke(() =>
                    {
                        SB.MainWindow.SilverZonePage.vm.VerifiedMarketBadge = markets.Length > 99 ? "99+" : markets.Length.ToString();
                        var badge = markets.Length + int.Parse(SB.MainWindow.SilverZonePage.vm.SupportersBadge.Replace("+", ""));
                        SB.MainWindow.silverZoneBadged.Badge = badge > 99 ? "99+" : badge.ToString();
                    });
                    try { SetMarkets(); } catch { }
                })) ;
            }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { }
            catch (Exception ex)
            {
                waitingLabel.Visibility = Visibility.Visible;
                searchBoxDockPanel.Visibility = Visibility.Collapsed;
                waitingLabel.Content = "ERROR";
            }
        }

        private async void SetMarkets()
        {
            if (markets == null || markets.Length <= 0) return;
            for (var i = 0; i < markets.Length; i++)
            {
                try
                {
                    await Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        var uc = new UserControlMarket()
                        {
                            Date = markets[i].Date,
                            Category = markets[i].Category,
                            Seller = markets[i].Seller,
                            Margin = new Thickness(0, 0, 0, 10),
                            ContentMarket = markets[i].Content
                        };
                        uc.SetContent(uc.ContentMarket);
                        uc.SetIcon(new Uri(markets[i].Icon));
                        if (!marketCollection.Any(u => u.Seller == uc.Seller && u.Date == uc.Date && u.ContentMarket == uc.ContentMarket))
                        {
                            marketCollection.Add(uc);
                        }
                    });
                }
                catch { }
            }
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                itemsControl.ItemsSource = marketCollection.Where(m => m.ContentMarket.ToLower()
                .Contains(serachTextBox.Text.ToLower()));
            }
        }

        private void serachTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (serachTextBox.Text.Length == 0)
            {
                itemsControl.ItemsSource = marketCollection;
            }
        }
    }
}
