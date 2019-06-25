using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapcellincentive : Mst_mapincentiveBase
	{
		private int _mapcell_id;

		private int _event_id;

		private int _success_level;

		private Dictionary<int, int> _req_items;

		private static string _tableName = "mst_mapcellincentive";

		public int Mapcell_id
		{
			get
			{
				return _mapcell_id;
			}
			private set
			{
				_mapcell_id = value;
			}
		}

		public int Event_id
		{
			get
			{
				return _event_id;
			}
			private set
			{
				_event_id = value;
			}
		}

		public int Success_level
		{
			get
			{
				return _success_level;
			}
			private set
			{
				_success_level = value;
			}
		}

		public Dictionary<int, int> Req_items
		{
			get
			{
				return _req_items;
			}
			private set
			{
				_req_items = value;
			}
		}

		public static string tableName => _tableName;

		protected override void setProperty(XElement element)
		{
			char c = ',';
			base.setProperty(element);
			Mapcell_id = int.Parse(element.Element("Mapcell_id").Value);
			Event_id = int.Parse(element.Element("Event_id").Value);
			Success_level = int.Parse(element.Element("Success_level").Value);
			if (element.Element("Req_item") != null)
			{
				Req_items = new Dictionary<int, int>();
				string[] array = element.Element("Req_item").Value.Split(c);
				string[] array2 = element.Element("Req_item_point").Value.Split(c);
				for (int i = 0; i < array.Length; i++)
				{
					int key = int.Parse(array[i]);
					int value = int.Parse(array2[i]);
					Req_items.Add(key, value);
				}
			}
		}

		protected override void setIncentiveItem(XElement element)
		{
			base.setIncentiveItem(element);
		}
	}
}
