using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class WordFilter
	{
		public struct FilteredComment
		{
			public bool wasChanged;

			private IntPtr _comment;

			public string comment => Marshal.PtrToStringAnsi(_comment);
		}

		public static event Messages.EventHandler OnCommentNotCensored;

		public static event Messages.EventHandler OnCommentCensored;

		public static event Messages.EventHandler OnCommentSanitized;

		public static event Messages.EventHandler OnWordFilterError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxWordFilterGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxWordFilterGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxWordFilterIsBusy();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxWordFilterCensorComment(string name);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxWordFilterSanitizeComment(string name);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxWordFilterGetResult(out FilteredComment result);

		public static bool IsBusy()
		{
			return PrxWordFilterIsBusy();
		}

		public static bool CensorComment(string comment)
		{
			return PrxWordFilterCensorComment(comment);
		}

		public static bool SanitizeComment(string comment)
		{
			return PrxWordFilterSanitizeComment(comment);
		}

		public static FilteredComment GetResult()
		{
			FilteredComment result = default(FilteredComment);
			PrxWordFilterGetResult(out result);
			return result;
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_WordFilterNotCensored:
				if (WordFilter.OnCommentNotCensored != null)
				{
					WordFilter.OnCommentNotCensored(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_WordFilterCensored:
				if (WordFilter.OnCommentCensored != null)
				{
					WordFilter.OnCommentCensored(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_WordFilterSanitized:
				if (WordFilter.OnCommentSanitized != null)
				{
					WordFilter.OnCommentSanitized(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_WordFilterError:
				if (WordFilter.OnWordFilterError != null)
				{
					WordFilter.OnWordFilterError(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
