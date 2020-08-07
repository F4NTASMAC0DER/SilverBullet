using System.Windows.Controls;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
    /// <summary>
    /// Interaction logic for PageBlockWebSocket.xaml
    /// </summary>
    public partial class PageBlockWebSocket : Page
    {
        BlockWebSocket vm;
        public PageBlockWebSocket(BlockWebSocket block)
        {
            InitializeComponent();
            DataContext = vm = block;

            customCookiesRTB.AppendText(vm.GetCustomHeaders());
        }

        private void wsCommandCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null == vm) return;
            vm.Command = (WSCommand)((ComboBox)e.OriginalSource).SelectedIndex;

            switch (vm.Command)
            {
                default:
                    wsCommandTabControl.SelectedIndex = 0;
                    break;

                case WSCommand.Connect:
                    wsCommandTabControl.SelectedIndex = 1;
                    break;

                case WSCommand.Send:
                    wsCommandTabControl.SelectedIndex = 2;
                    break;
            }
        }

        private void customCookiesRTB_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            try { vm.SetCustomCookies(customCookiesRTB.Lines()); } catch { }
        }
    }
}
