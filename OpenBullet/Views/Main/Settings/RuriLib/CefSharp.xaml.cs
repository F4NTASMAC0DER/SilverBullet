using System.Windows.Controls;

namespace OpenBullet.Views.Main.Settings.RL
{
    /// <summary>
    /// Interaction logic for CefSharp.xaml
    /// </summary>
    public partial class CefSharp : Page
    {
        public CefSharp()
        {
            InitializeComponent();
            DataContext = SB.Settings.RLSettings.CefSharp;
        }
    }
}
