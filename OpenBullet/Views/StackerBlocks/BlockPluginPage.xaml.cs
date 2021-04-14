using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using OpenBullet.Plugins;
using OpenBullet.Views.UserControls;
using PluginFramework;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
    /// <summary>
    /// Logica di interazione per BlockPluginControl.xaml
    /// </summary>
    public partial class BlockPluginPage : Page
    {
        public IBlockPlugin BlockPlugin { get; set; }
        public BlockBase Block => BlockPlugin as BlockBase;
        public ObservableCollection<UserControl> Controls { get; set; } = new ObservableCollection<UserControl>();
        private List<PropertyInfo> ValidProperties { get; set; } = new List<PropertyInfo>();

        public event EventHandler AutoSave;

        public BlockPluginPage(IBlockPlugin blockPlugin)
        {
            InitializeComponent();
            DataContext = this;

            BlockPlugin = blockPlugin;

            LostFocus += BlockPluginPage_LostFocus;
            // For each valid property, add input field
            foreach (var p in BlockPlugin.GetType().GetProperties().Where(p => Check.InputProperty(p)))
            {
                ValidProperties.Add(p);
                var inputField = Build.InputField(BlockPlugin, p);
                inputField.LostFocus += InputField_LostFocus;
                Controls.Add(inputField);
            }
        }

        private void BlockPluginPage_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AutoSave?.Invoke(sender, e);
        }

        private void InputField_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AutoSave?.Invoke(sender, e);
        }

        public void SetPropertyValues()
        {
            foreach (var property in ValidProperties)
            {
                // Retrieve and set the value
                var value = Controls
                    .Where(c => c is UserControlContainer)
                    .Select(c => c as UserControlContainer)
                    .First(c => c.PropertyName == property.Name).GetValue();

                property.SetValue(BlockPlugin, value);
            }
        }
    }
}
