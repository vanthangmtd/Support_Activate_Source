namespace SupportActivate.Common
{
    public static class Messages
    {
        public const string error = "Error";
        public const string warning = "Warning";
        public const string success = "Information";

        public static string AdvancedCheckPidkey = "The advanced check will lose 1 activation for online key or key volume.";

        public static string TokenApiCannotEmpty = "Token getcid cannot be empty.";
        public static string UpdateTokenGetcid = "Do you want to update Token getcid?";
        public static string CIDWrong = "The Confirmation ID is in the wrong format\nOr there is no Confirmation ID.";
        public static string NoCID = "There is no Confirmation ID.";
        public static string NoSelectCommand = "Please select the command.";

        public static string CannotDisabledService = "Cannot Disabled Service";
        public static string CannotEnabledService = "Cannot Enabled Service";

        public static string MemoryOverflow = "Memory overflow";
        public static string EnterPidkey = "Please enter the key";
        public static string StopPidKey = "The process will stop after the current key is checked.";
        public static string ReopenWithAdmin = "The application will reopen as an administrator";
        public static string DeleteAllKey = "You need to delete all key.";

        public static string DeleteSourceKey1Or2 = "You need to delete 1 of 2 data sources.";
        public static string NotSoft = "You have not sorted";
        public static string SaveSuccess = "Save Success";

        public static string MultythreadNoSupportWin7Off10 = "Do you want to turn on multithreading.\r\nWindows 7 and Office 2010 keys do not support multithreading";

        public static string SaveComplete = "Save complete";
        public static string SaveError = "Save Error\r\n.You need to recover the configuration file.";
        public static string OperationAgain = "Please perform the operation again.";

        public static string PleaseUpdateNewerVersion = "Please update to a newer version of Support Activate!\r\nDo you want to Update?";
        
        public static string AddNoteError = "Add note error.";

        public static string RecheckInforSuccess = "Recheck information key success.";
        public static string RecheckInforError = "Recheck information key error.";
        public static string DeleteKey = "Do you want to delete the product key?";
        public static string ChangeKeyToKeyBlocked = "Do you want to convert the product key to a key block?";
        public static string RecoverKey = "Do you want to recover the product key?";
        public static string RecoverKeySuccess = "Recovery Product Key Block success";
        public static string RecoverKeyError = "Recovery Product Key Block error";
    }
}
