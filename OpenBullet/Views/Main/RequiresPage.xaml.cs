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
    /// Interaction logic for RequiresPage.xaml
    /// </summary>
    public partial class RequiresPage : Page
    {
        public RequiresPage()
        {
            InitializeComponent();
        }

        Requires[] requires;

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
                        data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/Requires.json");
                    }
                }).ContinueWith(_ =>
                {
                    if (string.IsNullOrWhiteSpace(data))
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            waitingLabel.Visibility = Visibility.Visible;
                            waitingLabel.Content = "ERROR (CHECK YOUR NETWORK CONNECTION)";
                        });
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            waitingLabel.Visibility = Visibility.Collapsed;
                        });
                    }
                    requires = IOManager.DeserializeObject<Requires[]>(data);
                    try { SetRequirements(); } catch { }
                })) ;
            }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { }
            catch (Exception ex)
            {
                waitingLabel.Visibility = Visibility.Visible;
                waitingLabel.Content = "ERROR (CHECK YOUR NETWORK CONNECTION)";
            }
        }

        private async void SetRequirements()
        {
            if (requires == null || requires.Length <= 0) return;
            for (var i = 0; i < requires.Length; i++)
            {
                try
                {
                    await Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        var uc = new UserControlSupport()
                        {
                            Width = requires[i].Width,
                            Height = requires[i].Height,
                            SupportName = requires[i].Name,
                            Margin = new Thickness(0, 0, 8, 8),
                            BackgroundButton = (SolidColorBrush)brushConverter.ConvertFrom(requires[i].Color),
                            Url = requires[i].Address,
                            border = { ToolTip = requires[i].ToolTip },
                        };
                        uc.SetImage(new Uri(requires[i].Image));
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
