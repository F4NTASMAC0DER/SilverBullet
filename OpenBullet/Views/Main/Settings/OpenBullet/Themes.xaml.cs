using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace OpenBullet.Views.Main.Settings.OpenBullet
{

    /// <summary>
    /// Logica di interazione per Themes.xaml
    /// </summary>
    public partial class Themes : Page
    {
        public Themes()
        {
            InitializeComponent();
            DataContext = SB.SBSettings.Themes;
            
            // Load all the saved colors
            SetColors();
            SetColorPreviews();
            SetImagePreviews();
            SB.MainWindow.AllowsTransparency = SB.SBSettings.Themes.AllowTransparency;
        }

        public void SetColors()
        {
            SetAppColor("BackgroundMain", SB.SBSettings.Themes.BackgroundMain);
            SetAppColor("BackgroundSecondary", SB.SBSettings.Themes.BackgroundSecondary);
            SetAppColor("ForegroundMain", SB.SBSettings.Themes.ForegroundMain);
            SetAppColor("ForegroundGood", SB.SBSettings.Themes.ForegroundGood);
            SetAppColor("ForegroundBad", SB.SBSettings.Themes.ForegroundBad);
            SetAppColor("ForegroundCustom", SB.SBSettings.Themes.ForegroundCustom);
            SetAppColor("ForegroundRetry", SB.SBSettings.Themes.ForegroundRetry);
            SetAppColor("ForegroundToCheck", SB.SBSettings.Themes.ForegroundToCheck);
            SetAppColor("ForegroundMenuSelected", SB.SBSettings.Themes.ForegroundMenuSelected);
            SetAppColor(nameof(SB.SBSettings.Themes.ForegroundOcrRate), SB.SBSettings.Themes.ForegroundOcrRate);

            // This sets the background for the mainwindow (alternatively solid or image)
            SB.MainWindow.SetStyle();
        }

        private void SetColorPreviews()
        {
            BackgroundMain.SelectedColor = GetAppColor("BackgroundMain");
            BackgroundSecondary.SelectedColor = GetAppColor("BackgroundSecondary");
            ForegroundMain.SelectedColor = GetAppColor("ForegroundMain");
            ForegroundGood.SelectedColor = GetAppColor("ForegroundGood");
            ForegroundBad.SelectedColor = GetAppColor("ForegroundBad");
            ForegroundCustom.SelectedColor = GetAppColor("ForegroundCustom");
            ForegroundRetry.SelectedColor = GetAppColor("ForegroundRetry");
            ForegroundToCheck.SelectedColor = GetAppColor("ForegroundToCheck");
            ForegroundOcrRate.SelectedColor = GetAppColor("ForegroundOcrRate");
            ForegroundMenuSelected.SelectedColor = GetAppColor("ForegroundMenuSelected");
        }

        public void SetAppColor(string resourceName, string color)
        {
            App.Current.Resources[resourceName] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }

        public Color GetAppColor(string resourceName)
        {
            return ((SolidColorBrush)App.Current.Resources[resourceName]).Color;
        }

        private void SetImagePreviews()
        {
            try
            {
                backgroundImagePreview.Source = GetImageBrush(SB.SBSettings.Themes.BackgroundImage);
                backgroundLogoPreview.Source = GetImageBrush(SB.SBSettings.Themes.BackgroundLogo);
            }
            catch { }
        }

        private BitmapImage GetImageBrush(string file)
        {
            try
            {
                if (File.Exists(file))
                    return new BitmapImage(new Uri(file));
                else
                    return new BitmapImage(new Uri(@"pack://application:,,,/"
                        + Assembly.GetExecutingAssembly().GetName().Name
                        + ";component/"
                        + "Images/Themes/empty.png", UriKind.Absolute));
            }
            catch { return null; }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            SB.SBSettings.Themes.BackgroundMain = "#222";
            SB.SBSettings.Themes.BackgroundSecondary = "#111";
            SB.SBSettings.Themes.ForegroundMain = "#dcdcdc";
            SB.SBSettings.Themes.ForegroundGood = "#adff2f";
            SB.SBSettings.Themes.ForegroundBad = "#ff6347";
            SB.SBSettings.Themes.ForegroundCustom = "#ff8c00";
            SB.SBSettings.Themes.ForegroundRetry = "#ffff00";
            SB.SBSettings.Themes.ForegroundToCheck = "#7fffd4";
            SB.SBSettings.Themes.ForegroundMenuSelected = "#1e90ff";
            SB.SBSettings.Themes.ForegroundOcrRate = "#ff8cc6ff";

            SetColors();
            SetColorPreviews();
            SetImagePreviews();
        }

        private void loadBackgroundImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
       + "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            ofd.FilterIndex = 4;
            ofd.ShowDialog();
            SB.SBSettings.Themes.BackgroundImage = ofd.FileName;

            SetColors();
            SetImagePreviews();
        }

        private void loadBackgroundLogo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
       + "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            ofd.FilterIndex = 4;
            ofd.ShowDialog();
            SB.SBSettings.Themes.BackgroundLogo = ofd.FileName;

            SetColors();
            SetImagePreviews();
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
                SB.SBSettings.Themes.GetType().GetProperty(((ColorPicker)sender).Name.ToString()).SetValue(SB.SBSettings.Themes, ColorToHtml(e.NewValue.Value), null);

            SetColors();
        }

        private string ColorToHtml(Color color)
        {
            return $"#{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";
        }

        private void useImagesCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            SetColors();
        }

        private void useImagesCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetColors();
        }

        private void backgroundImageOpacityUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SB.MainWindow.SetStyle();
        }
    }
}
