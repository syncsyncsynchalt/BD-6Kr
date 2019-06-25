using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_payitem : Model_Base
	{
		private int _id;

		private string _name;

		private string _description;

		private int _price;

		private List<PayItemEffectInfo> _items;

		private static string _tableName = "mst_payitem";

		public int Id
		{
			get
			{
				return _id;
			}
			private set
			{
				_id = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			private set
			{
				_name = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			private set
			{
				_description = value;
			}
		}

		public int Price
		{
			get
			{
				return _price;
			}
			private set
			{
				_price = value;
			}
		}

		public List<PayItemEffectInfo> Items
		{
			get
			{
				return _items;
			}
			private set
			{
				_items = value;
			}
		}

		public static string tableName => _tableName;

		public Mst_payitem()
		{
			Description = string.Empty;
			Items = new List<PayItemEffectInfo>();
		}

		public void setText(string info)
		{
			Description = info;
		}

		public int GetBuyNum()
		{
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			List<int> list = new List<int>();
			foreach (PayItemEffectInfo item2 in Items)
			{
				int item = 0;
				int num = 0;
				if (item2.Type == 1 && item2.MstId == 53)
				{
					int num2 = instance.User_basic.GetPortMaxExtendNum() - instance.User_basic.Max_chara;
					int num3 = 0;
					if (instance.User_useItem.TryGetValue(53, out Mem_useitem value))
					{
						num3 = value.Value;
					}
					if (num2 > 0)
					{
						item = num2 / 10 - num3;
					}
				}
				else if (item2.Type == 1)
				{
					Mem_useitem value2 = null;
					if (Comm_UserDatas.Instance.User_useItem.TryGetValue(item2.MstId, out value2))
					{
						num = value2.Value;
					}
					int num4 = 3000 - num;
					if (num4 > 0)
					{
						item = num4 / item2.Count;
					}
				}
				else if (item2.Type == 2)
				{
					item = int.MaxValue;
				}
				else if (item2.Type == 3)
				{
					enumMaterialCategory mstId = (enumMaterialCategory)item2.MstId;
					Dictionary<int, Mst_item_limit> mst_item_limit = Mst_DataManager.Instance.Mst_item_limit;
					int materialLimit = Mst_DataManager.Instance.Mst_item_limit[1].GetMaterialLimit(mst_item_limit, mstId);
					num = Comm_UserDatas.Instance.User_material[mstId].Value;
					int num5 = materialLimit - num;
					if (num5 > 0)
					{
						item = num5 / item2.Count;
					}
				}
				list.Add(item);
			}
			int num6 = list.Min();
			return (num6 != int.MaxValue) ? num6 : (-1);
		}

		protected override void setProperty(XElement element)
		{
			Id = int.Parse(element.Element("Id").Value);
			Name = element.Element("Name").Value;
			Price = int.Parse(element.Element("Price").Value);
			foreach (XElement item2 in element.Elements("Items"))
			{
				int[] itemData = Array.ConvertAll(item2.Value.Split(','), (string x) => int.Parse(x));
				PayItemEffectInfo item = new PayItemEffectInfo(itemData);
				Items.Add(item);
			}
		}
	}
}
