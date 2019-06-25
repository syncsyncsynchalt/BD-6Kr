using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class CrashReport
	{
		private static List<CrashReport> internalReports;

		private static object reportsLock = new object();

		private readonly string id;

		public readonly DateTime time;

		public readonly string text;

		public static CrashReport[] reports
		{
			get
			{
				PopulateReports();
				lock (reportsLock)
				{
					return internalReports.ToArray();
				}
			}
		}

		public static CrashReport lastReport
		{
			get
			{
				PopulateReports();
				lock (reportsLock)
				{
					if (internalReports.Count > 0)
					{
						return internalReports[internalReports.Count - 1];
					}
				}
				return null;
			}
		}

		private CrashReport(string id, DateTime time, string text)
		{
			this.id = id;
			this.time = time;
			this.text = text;
		}

		private static int Compare(CrashReport c1, CrashReport c2)
		{
			long ticks = c1.time.Ticks;
			long ticks2 = c2.time.Ticks;
			if (ticks > ticks2)
			{
				return 1;
			}
			if (ticks < ticks2)
			{
				return -1;
			}
			return 0;
		}

		private static void PopulateReports()
		{
			lock (reportsLock)
			{
				if (internalReports == null)
				{
					string[] reports = GetReports();
					internalReports = new List<CrashReport>(reports.Length);
					string[] array = reports;
					foreach (string text in array)
					{
						GetReportData(text, out double secondsSinceUnixEpoch, out string text2);
						DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(secondsSinceUnixEpoch);
						internalReports.Add(new CrashReport(text, dateTime, text2));
					}
					internalReports.Sort(Compare);
				}
			}
		}

		public static void RemoveAll()
		{
			CrashReport[] reports = CrashReport.reports;
			foreach (CrashReport crashReport in reports)
			{
				crashReport.Remove();
			}
		}

		public void Remove()
		{
			if (RemoveReport(id))
			{
				lock (reportsLock)
				{
					internalReports.Remove(this);
				}
			}
		}

		private static string[] GetReports() { throw new NotImplementedException("�Ȃɂ���"); }

		private static void GetReportData(string id, out double secondsSinceUnixEpoch, out string text) { throw new NotImplementedException("�Ȃɂ���"); }

		private static bool RemoveReport(string id) { throw new NotImplementedException("�Ȃɂ���"); }
	}
}
