using System;
using System.Collections.Generic;
using System.Globalization;

namespace Common.Struct
{
	public struct TurnString
	{
		public string Year;

		public string Month;

		public string Day;

		public string DayOfWeek;

		private readonly DateTimeFormatInfo monthFormat;

		private readonly Dictionary<string, string> yearFormat;

		public TurnString(int elapsed_year, DateTime systemDate)
		{
			monthFormat = new CultureInfo("ja-JP").DateTimeFormat;
			monthFormat.MonthNames = new string[13]
			{
				"睦月",
				"如月",
				"弥生",
				"卯月",
				"皐月",
				"水無月",
				"文月",
				"葉月",
				"長月",
				"神無月",
				"霜月",
				"師走",
				string.Empty
			};
			yearFormat = new Dictionary<string, string>
			{
				{
					"0",
					"零"
				},
				{
					"1",
					"壱"
				},
				{
					"2",
					"弐"
				},
				{
					"3",
					"参"
				},
				{
					"4",
					"肆"
				},
				{
					"5",
					"伍"
				},
				{
					"6",
					"陸"
				},
				{
					"7",
					"質"
				},
				{
					"8",
					"捌"
				},
				{
					"9",
					"玖"
				},
				{
					"10",
					"拾"
				}
			};
			string text = elapsed_year.ToString();
			if (text.Length == 1)
			{
				text = yearFormat[text];
			}
			else if (text.Length == 2)
			{
				string empty = string.Empty;
				string text2;
				if (text[0].Equals('1'))
				{
					text2 = yearFormat["10"];
				}
				else
				{
					text2 = yearFormat[text[0].ToString()];
					text2 += yearFormat["10"];
				}
				if (!text[1].Equals('0'))
				{
					text2 += yearFormat[text[1].ToString()];
				}
				text = text2;
			}
			Year = text;
			Month = systemDate.ToString("MMMM", monthFormat);
			Day = systemDate.Day.ToString();
			DayOfWeek = systemDate.ToString("dddd");
		}
	}
}
