using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.Util
{
    public class MessageUtil
    {
        public static async Task<MessageDialogResult> ShowMessageBoxAsync(MetroWindow win, string title, string messageContent)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                FirstAuxiliaryButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            return await win.ShowMessageAsync(title, messageContent, MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);
        }
    }
}
