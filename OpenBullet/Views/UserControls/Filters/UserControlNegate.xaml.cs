using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OpenBullet.Views.UserControls.Filters
{
    /// <summary>
    /// Interaction logic for UserControlNegate.xaml
    /// </summary>
    public partial class UserControlNegate : UserControl
    {
        public UserControlNegate()
        {
            InitializeComponent();

        }

        public const string ControlName = "Negate";
        public event EventHandler SetFilter;

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            try
            {
            }
            catch { }
        }

        private void ChannelsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelsComboBox.Items.Count == 0) return;

            SetFilter?.Invoke(new[] {CheckBoxOnlyGrayscale.IsChecked.GetValueOrDefault().ToString(),
                ChannelsComboBox.SelectedItem.ToString() },
           new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
           {
               Source = ControlName
           });
        }

        private void CheckBoxOnlyGrayscale_Click(object sender, RoutedEventArgs e)
        {
            SetFilter?.Invoke(new[] {CheckBoxOnlyGrayscale.IsChecked.GetValueOrDefault().ToString(),
                ChannelsComboBox.SelectedItem.ToString() },
                new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
                {
                    Source = ControlName
                });
        }
    }
}
