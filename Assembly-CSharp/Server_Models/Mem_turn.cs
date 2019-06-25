using Common.Struct;
using Server_Controllers;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_turn", Namespace = "")]
	public class Mem_turn : Model_Base
	{
		[DataMember]
		private int _total_turn;

		[DataMember]
		private int _erc;

		[DataMember]
		private bool _reqQuestReset;

		private static string _tableName = "mem_turn";

		private readonly DateTime baseTime;

		public int Total_turn
		{
			get
			{
				return _total_turn;
			}
			private set
			{
				_total_turn = value;
			}
		}

		public int Erc
		{
			get
			{
				return _erc;
			}
			private set
			{
				_erc = value;
			}
		}

		public bool ReqQuestReset
		{
			get
			{
				return _reqQuestReset;
			}
			private set
			{
				_reqQuestReset = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_turn()
		{
			baseTime = new DateTime(1941, 12, 8);
			Total_turn = 0;
			ReqQuestReset = true;
		}

		public int GetElapsedYear()
		{
			return GetElapsedYear(GetDateTime());
		}

		public int GetElapsedYear(DateTime dt)
		{
			return dt.Year - baseTime.Year;
		}

		public TurnString GetTurnString()
		{
			DateTime systemDate = baseTime.AddDays(Total_turn);
			int elapsed_year = systemDate.Year - baseTime.Year;
			return new TurnString(elapsed_year, systemDate);
		}

		public TurnString GetTurnString(int reqTurn)
		{
			DateTime systemDate = baseTime.AddDays(reqTurn);
			int elapsed_year = systemDate.Year - baseTime.Year;
			return new TurnString(elapsed_year, systemDate);
		}

		public DateTime GetDateTime()
		{
			return baseTime.AddDays(Total_turn);
		}

		public DateTime GetDateTime(int plusYear, int month, int day)
		{
			int year = baseTime.Year + plusYear;
			return new DateTime(year, month, day);
		}

		public void AddTurn(Api_TurnOperator instance)
		{
			if (instance != null)
			{
				Total_turn++;
				ReqQuestReset = true;
			}
		}

		public void DisableQuestReset()
		{
			ReqQuestReset = false;
		}

		protected override void setProperty(XElement element)
		{
			Total_turn = int.Parse(element.Element("_total_turn").Value);
			Erc = int.Parse(element.Element("_erc").Value);
			ReqQuestReset = bool.Parse(element.Element("_reqQuestReset").Value);
		}
	}
}
