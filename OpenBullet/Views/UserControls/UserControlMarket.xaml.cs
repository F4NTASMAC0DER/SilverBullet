using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AngleSharp.Text;
using RuriLib;

namespace OpenBullet.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlMarket.xaml
    /// </summary>
    public partial class UserControlMarket : UserControl
    {
        public UserControlMarket()
        {
            InitializeComponent();
            DataContext = this;
        }

        private BrushConverter converter = new BrushConverter();

        public void SetIcon(Uri imgSource)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = imgSource;
            bitmap.EndInit();
            icon.Source = bitmap;
        }

        public string Seller { get; set; }

        public string Date { get; set; }

        public string Category { get; set; }

        public string ContentMarket { get; set; }

        private Regex regexUrl = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public void SetContent(string content)
        {
            var contents = content.Split('\n');
            if (contents == null || content.Length == 0)
            {
                contents = new[] { content };
            }
            foreach (var cont in contents)
            {
                var tmpCont = cont;
                var dockPanel = new DockPanel();
                contentPanel.Children.Add(dockPanel);

                var urls = regexUrl.Matches(tmpCont);
                for (var i = 0; i < urls.Count; i++)
                {
                    var url = urls[i].Value;
                    var text = tmpCont.GetUntilOrEmpty(url);
                    tmpCont = tmpCont.ReplaceFirst(text + url, string.Empty);
                    var link = CreateHyperLink(url, text);
                    dockPanel.Children.Add(link);
                }
                if (!string.IsNullOrWhiteSpace(tmpCont))
                {
                    dockPanel.Children.Add(CreateTextBlock(tmpCont));
                }
                else if (string.IsNullOrEmpty(cont))
                {
                    dockPanel.Children.Add(CreateTextBlock(cont));
                }
            }
        }

        private TextBlock CreateHyperLink(string url, string text)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = text,
                Margin = new System.Windows.Thickness(0, 0, 3, 0),
                FontSize = 13.5
            };
            var uri = new Uri(url);
            var hyperLink = new Hyperlink(new Run(uri.Host.Contains("t.me") ? uri.AbsolutePath.Replace("/","") : uri.Host + uri.AbsolutePath))
            {
                Cursor = Cursors.Hand,
                NavigateUri = uri,
                Foreground = converter.ConvertFrom("#FF3CE6EC") as SolidColorBrush
            };
            hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
            textBlock.Inlines.Add(hyperLink);
            return textBlock;
        }

        private TextBlock CreateTextBlock(string text)
        {
            var textBlock = new TextBlock()
            {
                Text = text,
            };
            return textBlock;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch { }
        }
    }
}
