using System;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace OpenBullet.Views.CustomMessageBox
{
    public sealed class CustomMsgBox
    {
        private const string MessageBoxTitle = "Message";

        /// <summary>
        /// Displays a message box with OK button
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        public static void Show(string message, bool isRTL = false)
        {
            using (var msg = new CustomMsgBoxWindow())
            {
                msg.Title = MessageBoxTitle;
                msg.TitleLabel.Content = MessageBoxTitle;
                msg.Message.Text = message;
                msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
                msg.BtnCancel.Visibility = Visibility.Collapsed;
                if (isRTL)
                {
                    msg.FlowDirection = FlowDirection.RightToLeft;
                }
                msg.BtnOk.Focus();
                msg.ShowDialog();
            }
        }


        /// <summary>
        ///  Displays a message box with OK button
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        public static void Show(string message, string title, bool isRTL = false)
        {
            using (var msg = new CustomMsgBoxWindow())
            {
                msg.Title = title;
                msg.TitleLabel.Content = title;
                msg.Message.Text = message;
                msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
                msg.BtnCancel.Visibility = Visibility.Collapsed;
                msg.MsgIcon.Kind = PackIconMaterialKind.InformationOutline;
                msg.MsgIcon.Foreground = Brushes.LightSteelBlue;
                if (isRTL)
                {
                    msg.FlowDirection = FlowDirection.RightToLeft;
                }
                msg.BtnOk.Focus();
                msg.ShowDialog();
            }
        }

        /// <summary>
        /// Displays an error message box
        /// </summary>
        /// <param name="errorMessage">The error error message to display</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        public static void ShowError(string errorMessage, bool isRTL = false)
        {
            try
            {
                using (var msg = new CustomMsgBoxWindow())
                {
                    msg.Title = "Error";
                    msg.TitleLabel.Content = "Error";
                    msg.Message.Text = errorMessage;
                    msg.BorderBrush = Brushes.Red;
                    msg.BtnCancel.Visibility = Visibility.Collapsed;
                    msg.MsgIcon.Kind = PackIconMaterialKind.CloseBoxOutline;
                    msg.MsgIcon.Foreground = Brushes.Orange;
                    if (isRTL)
                    {
                        msg.FlowDirection = FlowDirection.RightToLeft;
                    }
                    msg.BtnOk.Focus();
                    msg.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(errorMessage);
            }
        }

        /// <summary>
        /// Displays an error message box
        /// </summary>
        /// <param name="errorMessage">The error error message to display</param>
        /// <param name="errorTitle">The title of the error message box</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        public static void ShowError(string errorMessage, string errorTitle, bool isRTL = false)
        {
            try
            {
                using (var msg = new CustomMsgBoxWindow())
                {
                    msg.Title = errorTitle;
                    msg.TitleLabel.Content = errorTitle;
                    msg.Message.Text = errorMessage;
                    msg.BorderBrush = Brushes.Red;
                    msg.BtnCancel.Visibility = Visibility.Collapsed;
                    msg.MsgIcon.Kind = PackIconMaterialKind.CloseBoxOutline;
                    msg.MsgIcon.Foreground = Brushes.Red;
                    if (isRTL)
                    {
                        msg.FlowDirection = FlowDirection.RightToLeft;
                    }
                    msg.BtnOk.Focus();
                    msg.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(errorMessage);
            }
        }

        /// <summary>
        /// Displays a warning message box
        /// </summary>
        /// <param name="warningMessage">The warning message to display</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        public static void ShowWarning(string warningMessage, bool isRTL = false)
        {
            try
            {
                using (var msg = new CustomMsgBoxWindow())
                {
                    msg.Title = "Warning";
                    msg.TitleLabel.Content = "Warning";
                    msg.Message.Text = warningMessage;
                    msg.BorderBrush = Brushes.Orange;
                    msg.BtnCancel.Visibility = Visibility.Collapsed;
                    msg.MsgIcon.Kind = PackIconMaterialKind.AlertBoxOutline;
                    msg.MsgIcon.Foreground = Brushes.Orange;
                    if (isRTL)
                    {
                        msg.FlowDirection = FlowDirection.RightToLeft;
                    }
                    msg.BtnOk.Focus();
                    msg.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(warningMessage);
            }
        }

        /// <summary>
        /// Displays a warning message box
        /// </summary>
        /// <param name="warningMessage">The warning message to display</param>
        /// <param name="warningTitle">The title of the error message box</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        public static void ShowWarning(string warningMessage, string warningTitle, bool isRTL = false)
        {
            try
            {
                using (var msg = new CustomMsgBoxWindow())
                {
                    msg.Title = warningTitle;
                    msg.TitleLabel.Content = warningTitle;
                    msg.Message.Text = warningMessage;
                    msg.BorderBrush = Brushes.Orange;
                    msg.BtnCancel.Visibility = Visibility.Collapsed;
                    msg.MsgIcon.Kind = PackIconMaterialKind.AlertBoxOutline;
                    msg.MsgIcon.Foreground = Brushes.Orange;
                    if (isRTL)
                    {
                        msg.FlowDirection = FlowDirection.RightToLeft;
                    }
                    msg.BtnOk.Focus();
                    msg.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(warningMessage, warningTitle);
            }
        }

        /// <summary>
        /// Displays a message box with a cancel button
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        /// <returns>Message box Result OK or CANCEL</returns>
        public static MessageBoxResult ShowWithCancel(string message, bool isRTL = false)
        {
            try
            {
                using (var msg = new CustomMsgBoxWindow())
                {
                    msg.Title = MessageBoxTitle;
                    msg.TitleLabel.Content = MessageBoxTitle;
                    msg.Message.Text = message;
                    msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
                    if (isRTL)
                    {
                        msg.FlowDirection = FlowDirection.RightToLeft;
                    }
                    msg.BtnOk.Focus();
                    msg.ShowDialog();
                    return msg.Result == MessageBoxResult.OK ? MessageBoxResult.OK : MessageBoxResult.Cancel;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(message);
                return MessageBoxResult.Cancel;
            }
        }

        /// <summary>
        /// Displays a message box with a cancel button
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        /// <returns>Message box Result OK or CANCEL</returns>
        public static MessageBoxResult ShowWithCancel(string message, string title, bool isRTL = false)
        {
            try
            {
                using (var msg = new CustomMsgBoxWindow())
                {
                    msg.Title = title;
                    msg.TitleLabel.Content = title;
                    msg.Message.Text = message;
                    msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
                    if (isRTL)
                    {
                        msg.FlowDirection = FlowDirection.RightToLeft;
                    }
                    msg.BtnOk.Focus();
                    msg.ShowDialog();
                    return msg.Result == MessageBoxResult.OK ? MessageBoxResult.OK : MessageBoxResult.Cancel;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(message);
                return MessageBoxResult.Cancel;
            }
        }

        /// <summary>
        /// Displays a message box with a cancel button
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="isError">If the message is an error</param>
        /// <param name="isRTL">(Optional) If true the MessageBox FlowDirection will be RightToLeft</param>
        /// <returns>Message box Result OK or CANCEL</returns>
        public static MessageBoxResult ShowWithCancel(string message, bool isError, bool isRTL = false)
        {
            try
            {
                using (var msg = new CustomMsgBoxWindow())
                {
                    msg.Title = MessageBoxTitle;
                    msg.TitleLabel.Content = MessageBoxTitle;
                    msg.Message.Text = message;
                    //msg.TitleBar.Background = isError
                    //    ? Brushes.Red
                    //    : new SolidColorBrush(Color.FromRgb(3, 169, 244));
                    //msg.BorderBrush = isError
                    //    ? Brushes.Red
                    //    : new SolidColorBrush(Color.FromRgb(3, 169, 244));
                    if (isRTL)
                    {
                        msg.FlowDirection = FlowDirection.RightToLeft;
                    }
                    msg.BtnOk.Focus();
                    msg.ShowDialog();
                    return msg.Result == MessageBoxResult.OK ? MessageBoxResult.OK : MessageBoxResult.Cancel;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(message);
                return MessageBoxResult.Cancel;
            }
        }
    }
}
