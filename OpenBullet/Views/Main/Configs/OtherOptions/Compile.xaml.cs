using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using RuriLib;
using Xceed.Wpf.Toolkit;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
    /// <summary>
    /// Interaction logic for Compile.xaml
    /// </summary>
    public partial class Compile : Page
    {
        private ConfigSettings vm = null;
        public Compile()
        {
            InitializeComponent();
            vm = SB.ConfigManager.CurrentConfig.Config.Settings;
            DataContext = vm;
            vm.Title = Path.GetFileNameWithoutExtension(SB.MainWindow.ConfigsPage.CurrentConfig.FileName);
            vm.IconPath = "Icon\\svbfile.ico";
            compilerVersion.Content = SB.CompilerVersion;

            SetColors();

        }

        private void SetColors()
        {
            MessageColor.SelectedColor = vm.MessageColor;
            AuthorColor.SelectedColor = vm.AuthorColor;
            WordlistColor.SelectedColor = vm.WordlistColor;
            BotsColor.SelectedColor = vm.BotsColor;
            CustomInputColor.SelectedColor = vm.CustomInputColor;
            CPMColor.SelectedColor = vm.CPMColor;
            ProgressColor.SelectedColor = vm.ProgressColor;
            HitsColor.SelectedColor = vm.HitsColor;
            CustomInputColor.SelectedColor = vm.CustomInputColor;
            ToCheckColor.SelectedColor = vm.ToCheckColor;
            FailsColor.SelectedColor = vm.FailsColor;
            RetriesColor.SelectedColor = vm.RetriesColor;
            OcrRateColor.SelectedColor = vm.OcrRateColor;
            ProxiesColor.SelectedColor = vm.ProxiesColor;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                vm.GetType().GetProperty(((ColorPicker)sender).Name.ToString()).SetValue(vm, e.NewValue.Value, null);
            }
        }

        private void SelectIcon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog()
                {
                    Filter = "Icon | *.ico"
                };
                if (dialog.ShowDialog() == true)
                {
                    if (Path.GetExtension(dialog.FileName) == ".ico")
                    {
                        vm.IconPath = dialog.FileName;
                    }
                }
            }
            catch { }
        }

        private void IconPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (File.Exists(textBox.Text) && Path.GetExtension(textBox.Text) == ".ico")
            {
                vm.IconPath = textBox.Text;
            }
        }

        private void Message_TextChanged(object sender, TextChangedEventArgs e)
        {
            vm.Message = (sender as TextBox).Text;
        }

        private void HitInfoFormatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            hitInfoFormatHint.Visibility = (sender as TextBox).Text.Length == 0 ?
                Visibility.Visible : Visibility.Hidden;
        }

        private void SelectLicSource_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog()
                {
                    Filter = "License Source (*.cs)|*.cs|License Source (*.txt)|*.txt"
                };
                if (dialog.ShowDialog() == true)
                {
                    if (Path.GetExtension(dialog.FileName) == ".cs")
                    {
                        vm.LicenseSource = dialog.FileName;
                    }
                }
            }
            catch { }
        }
    }
}
