using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OpenBullet.Views.CustomMessageBox
{
    /// <summary>
    /// Interaction logic for CustomMsgBoxWindow.xaml
    /// </summary>
    public partial class CustomMsgBoxWindow : IDisposable
    {
        public MessageBoxResult Result { get; set; }

        public CustomMsgBoxWindow()
        {
            InitializeComponent();
            Result = MessageBoxResult.Cancel;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        public void Dispose()
        {
            Close();
        }

        private void BtnCopyMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(Message.Text);
            }
            catch { }
        }

        private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void btnQuit_MouseEnter(object sender, MouseEventArgs e)
        {
            btnQuit.BorderBrush = btnQuit.Background = Brushes.DarkRed;
        }

        private void btnQuit_MouseLeave(object sender, MouseEventArgs e)
        {
            btnQuit.BorderBrush = btnQuit.Background = Brushes.Transparent;
        }
    }
}
