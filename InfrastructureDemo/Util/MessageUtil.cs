using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static InfrastructureDemo.Constans.Enum.EnumConstants;

namespace InfrastructureDemo.Util
{
    public class MessageUtil
    {
        public static async Task<MessageDialogResult> ShowMessageBoxAsync(MetroWindow win, string title, string messageContent, MessageBoxType messageBoxType)
        {
            var mySettings = new MetroDialogSettings()
            {
                AnimateShow = false,
                AnimateHide = false,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            var buttonType = MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary;

            switch (messageBoxType)
            {
                case MessageBoxType.Normal:
                    mySettings.AffirmativeButtonText = "Yes";
                    mySettings.NegativeButtonText = "No";
                    mySettings.FirstAuxiliaryButtonText = "Cancel";

                    break;
                case MessageBoxType.OK:
                    mySettings.AffirmativeButtonText = "OK";
                    buttonType = MessageDialogStyle.Affirmative;

                    break;
                default:
                    mySettings.AffirmativeButtonText = "Yes";
                    mySettings.NegativeButtonText = "No";
                    mySettings.FirstAuxiliaryButtonText = "Cancel";

                    break;
            }

            return await win.ShowMessageAsync(title, messageContent, buttonType, mySettings);
        }
    }
}
