using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Constants
{
    public class MessageBoxConstant
    {
        public static readonly string TITLE_WARNING = "Warning";

        public static readonly string IMPORT_FILE_CONFIRM_MESSAGE = "Importing data will restart the application. \r\n Are you sure to continue?";
        public static readonly string IMPORT_FILE_LIMIT_ONE_FILE_MESSAGE = "Allow only one file.";
        public static readonly string DELETE_CATEGORY_CONFIRM_MESSAGE = "Do you want to delete this category? [{0}]";
        public static readonly string DELETE_TYPE_CONFIRM_MESSAGE = "Do you want to delete this type? [{0}]";
        public static readonly string RESET_NOTE_APPLICATION_CONFIRM_MESSAGE = "If you restore to factory settings, the data will not be restored. \r\n Are you sure you want to continue?";
    }
}
