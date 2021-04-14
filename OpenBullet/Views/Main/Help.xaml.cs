using System.Windows.Controls;

namespace OpenBullet.Views.Main
{
    /// <summary>
    /// Logica di interazione per About.xaml
    /// </summary>
    public partial class Help : Page
    {
        AboutPage aboutPage;
        ReleaseNotesPage releaseNotesPage;
        public CheckUpdatePage CheckUpdatePage;
        public Help()
        {
            InitializeComponent();
            aboutPage = new AboutPage();
            releaseNotesPage = new ReleaseNotesPage();
            CheckUpdatePage = new CheckUpdatePage();
            Main.Content = aboutPage;
            menuOptionSelected(aboutLabel);
        }

        private void repoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void docuButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void menuOptionSelected(object sender)
        {
            foreach (var child in topMenu.Children)
            {
                try
                {
                    var c = (Label)child;
                    c.Foreground = Utils.GetBrush("ForegroundMain");
                }
                catch { }
            }
         ((Label)sender).Foreground = Utils.GetBrush("ForegroundCustom");
        }

        //about 
        private void Label_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.Content = aboutPage;
            menuOptionSelected(sender);
        }

        //release notes
        private void Label_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.Content = releaseNotesPage;
            menuOptionSelected(sender);
        }

        public void checkForUpdateLabel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.Content = CheckUpdatePage;
            menuOptionSelected(checkForUpdateLabel);
        }
    }
}
