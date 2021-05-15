using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using OpenBullet.Views.CustomMessageBox;

namespace OpenBullet.Views.Main
{
    /// <summary>
    /// Interaction logic for CheckUpdatePage.xaml
    /// </summary>
    public partial class CheckUpdatePage : Page
    {
        public CheckUpdatePage()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void CheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => CheckForUpdate());
        }

        public void CheckForUpdate()
        {
            try
            {
                Dispatcher.Invoke(() => richTextBox.Document.Blocks.Clear());
                Dispatcher.Invoke(() => richTextBoxDonate.Document.Blocks.Clear());
                var result = CheckUpdate.Run<SBUpdate>();
                var releasesLatest = CheckUpdate.Run<LatestRelease>("https://api.github.com/repos/mohamm4dx/SilverBullet/releases/latest");
                if (releasesLatest.Assets != null && releasesLatest.Assets.Length > 0)
                {
                    result.DownloadUrl = releasesLatest.Assets.First().Browser_Download_Url;
                }
                result.Available = releasesLatest.Available;
                Dispatcher.Invoke(() => runUpdaterButton.IsEnabled = result.Available);
                releasesLatest.Body.Split('\n')
                    .ToList().ForEach(re =>
                    {
                        re = re.Replace("•", string.Empty)
                        .Replace("â€¢", string.Empty).Trim();
                        if (result.ReleaseNotes.Any(r => r.Note.Trim()
                        .Replace("â€¢", string.Empty)
                        .Replace("•", string.Empty) != re))
                        {
                            result.ReleaseNotes.Add(new ReleaseNotes() { Note = re });
                        }
                    });

                Dispatcher.Invoke(() => DataContext = result);
                try
                {
                    for (var i = 0; i < result.Donate.Length; i++)
                    {
                        Dispatcher.Invoke(() => AppendParagraph(richTextBoxDonate, new[] { result.Donate[i].Address }));
                    }
                }
                catch { }
                if (result.Available)
                {
                    try
                    {
                        for (var i = 0; i < result.ReleaseNotes.Count; i++)
                        {
                            Dispatcher.Invoke(() => AppendParagraph(richTextBox, new[] { result.ReleaseNotes[i].Note }));
                        }
                    }
                    catch { }
                    if (!string.IsNullOrWhiteSpace(result.Message))
                    {
                        Dispatcher.Invoke(() => CustomMsgBox.Show(result.Message));
                    }
                }
                else
                {
                    Dispatcher.Invoke(() => CustomMsgBox.Show("there are currently no updates available"));
                }
            }
            catch { }
        }

        private void AppendParagraph(RichTextBox richTextBox, string[] paragraphs)
        {
            foreach (var par in paragraphs)
            {
                var bold = new Bold(new Run("• "));
                bold.SetResourceReference(Bold.ForegroundProperty, "ForegroundMain");
                var paragraph = new Paragraph(bold);
                paragraph.SetResourceReference(Paragraph.ForegroundProperty, "ForegroundMain");
                paragraph.Inlines.Add(new Run(par));
                richTextBox.Document.Blocks.Add(paragraph);
            }
        }

        private void RunUpdater_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process updater = null;
                if ((updater = Process.GetProcessesByName("SilverBulletUpdater").FirstOrDefault()) == null)
                {
                    Process.Start("Updater\\SilverBulletUpdater.exe");
                }
                else
                {
                    if (!updater.HasExited)
                    {
                        SetForegroundWindow(updater.MainWindowHandle);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMsgBox.ShowError(ex.Message);
            }
        }
    }
}
