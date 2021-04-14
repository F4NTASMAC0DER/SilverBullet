using System.Windows.Media;

namespace PluginFramework
{
    public interface IBlockPlugin
    {
        string Name { get; }

        /// <summary>
        /// Button background color
        /// </summary>
        LinearGradientBrush LinearGradientBrush { get; }

        bool LightForeground { get; }
    }
}
