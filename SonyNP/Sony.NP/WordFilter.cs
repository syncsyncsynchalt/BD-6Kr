using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
    public class WordFilter
    {
        public struct FilteredComment
        {
            public bool wasChanged;

            public string comment => "";
        }

        public static event Messages.EventHandler OnCommentNotCensored;

        public static event Messages.EventHandler OnCommentCensored;

        public static event Messages.EventHandler OnCommentSanitized;

        public static event Messages.EventHandler OnWordFilterError;

        public static bool GetLastError(out ResultCode result)
        {
            result = new ResultCode();
            return false;
        }

        public static bool IsBusy()
        {
            return false;
        }

        public static bool CensorComment(string comment)
        {
            return false;
        }

        public static bool SanitizeComment(string comment)
        {
            return false;
        }

        public static FilteredComment GetResult()
        {
            return new FilteredComment();
        }

        public static bool ProcessMessage(Messages.PluginMessage msg)
        {
            return false;
        }
    }
}
