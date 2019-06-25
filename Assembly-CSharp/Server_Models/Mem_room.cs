using Common.Enum;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_room", Namespace = "")]
	public class Mem_room : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _furniture1;

		[DataMember]
		private int _furniture2;

		[DataMember]
		private int _furniture3;

		[DataMember]
		private int _furniture4;

		[DataMember]
		private int _furniture5;

		[DataMember]
		private int _furniture6;

		[DataMember]
		private int _bgm_id;

		[DataMember]
		private int _jukebox_bgm_id;

		private static string _tableName = "mem_room";

		public int Rid
		{
			get
			{
				return _rid;
			}
			private set
			{
				_rid = value;
			}
		}

		public int Furniture1
		{
			get
			{
				return _furniture1;
			}
			private set
			{
				_furniture1 = value;
			}
		}

		public int Furniture2
		{
			get
			{
				return _furniture2;
			}
			private set
			{
				_furniture2 = value;
			}
		}

		public int Furniture3
		{
			get
			{
				return _furniture3;
			}
			private set
			{
				_furniture3 = value;
			}
		}

		public int Furniture4
		{
			get
			{
				return _furniture4;
			}
			private set
			{
				_furniture4 = value;
			}
		}

		public int Furniture5
		{
			get
			{
				return _furniture5;
			}
			private set
			{
				_furniture5 = value;
			}
		}

		public int Furniture6
		{
			get
			{
				return _furniture6;
			}
			private set
			{
				_furniture6 = value;
			}
		}

		public int Bgm_id
		{
			get
			{
				return _bgm_id;
			}
			private set
			{
				_bgm_id = value;
			}
		}

		public static string tableName => _tableName;

		public int this[FurnitureKinds kind]
		{
			get
			{
				switch (kind)
				{
				case FurnitureKinds.Floor:
					return Furniture1;
				case FurnitureKinds.Wall:
					return Furniture2;
				case FurnitureKinds.Window:
					return Furniture3;
				case FurnitureKinds.Hangings:
					return Furniture4;
				case FurnitureKinds.Chest:
					return Furniture5;
				default:
					return Furniture6;
				}
			}
			private set
			{
				switch (kind)
				{
				case FurnitureKinds.Floor:
					Furniture1 = value;
					break;
				case FurnitureKinds.Wall:
					Furniture2 = value;
					break;
				case FurnitureKinds.Window:
					Furniture3 = value;
					break;
				case FurnitureKinds.Hangings:
					Furniture4 = value;
					break;
				case FurnitureKinds.Chest:
					Furniture5 = value;
					break;
				case FurnitureKinds.Desk:
					Furniture6 = value;
					break;
				}
			}
		}

		public Mem_room()
		{
			Furniture1 = 1;
			Furniture2 = 38;
			Furniture3 = 72;
			Furniture4 = 102;
			Furniture5 = 133;
			Furniture6 = 164;
			Bgm_id = 101;
			_jukebox_bgm_id = 101;
		}

		public Mem_room(int deck_rid)
			: this()
		{
			Rid = deck_rid;
		}

		public List<Mst_furniture> getFurnitureDatas()
		{
			List<Mst_furniture> list = new List<Mst_furniture>();
			list.Add(Mst_DataManager.Instance.Mst_furniture[Furniture1]);
			list.Add(Mst_DataManager.Instance.Mst_furniture[Furniture2]);
			list.Add(Mst_DataManager.Instance.Mst_furniture[Furniture3]);
			list.Add(Mst_DataManager.Instance.Mst_furniture[Furniture4]);
			list.Add(Mst_DataManager.Instance.Mst_furniture[Furniture5]);
			list.Add(Mst_DataManager.Instance.Mst_furniture[Furniture6]);
			return list;
		}

		public void SetFurniture(FurnitureKinds kind, int id)
		{
			this[kind] = id;
		}

		public void SetFurniture(FurnitureKinds kind, int id, int bgmId)
		{
			this[kind] = id;
			Bgm_id = ((bgmId != 0) ? bgmId : _jukebox_bgm_id);
		}

		public void SetPortBgmFromJuke(int music_id)
		{
			Bgm_id = music_id;
			_jukebox_bgm_id = music_id;
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			Furniture1 = int.Parse(element.Element("_furniture1").Value);
			Furniture2 = int.Parse(element.Element("_furniture2").Value);
			Furniture3 = int.Parse(element.Element("_furniture3").Value);
			Furniture4 = int.Parse(element.Element("_furniture4").Value);
			Furniture5 = int.Parse(element.Element("_furniture5").Value);
			Furniture6 = int.Parse(element.Element("_furniture6").Value);
			Bgm_id = int.Parse(element.Element("_bgm_id").Value);
			_jukebox_bgm_id = int.Parse(element.Element("_jukebox_bgm_id").Value);
		}
	}
}
