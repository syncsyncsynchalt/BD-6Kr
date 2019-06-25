using Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_deckpractice", Namespace = "")]
	public class Mem_deckpractice : Model_Base
	{
		[DataMember]
		private List<bool> _practiceStatus;

		private static string _tableName = "mem_deckpractice";

		public bool this[DeckPracticeType kind]
		{
			get
			{
				int index = (int)(kind - 1);
				return _practiceStatus[index];
			}
			private set
			{
				int index = (int)(kind - 1);
				_practiceStatus[index] = value;
			}
		}

		public static string tableName => _tableName;

		public Mem_deckpractice()
		{
			_practiceStatus = new List<bool>(6);
			_practiceStatus.AddRange(Enumerable.Repeat(element: false, 6));
			_practiceStatus[0] = true;
		}

		public void StateChange(DeckPracticeType type, bool state)
		{
			if (type != DeckPracticeType.Normal)
			{
				this[type] = state;
			}
		}

		protected override void setProperty(XElement element)
		{
			List<XElement> list = element.Element("_practiceStatus").Elements().ToList();
			for (int i = 0; i < list.Count; i++)
			{
				_practiceStatus[i] = bool.Parse(list[i].Value);
			}
		}
	}
}
