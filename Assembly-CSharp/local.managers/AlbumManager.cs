using local.models;
using Server_Common.Formats;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class AlbumManager : ManagerBase
	{
		private Dictionary<int, IAlbumModel> _ship;

		private Dictionary<int, IAlbumModel> _slot;

		private int _ship_last_no;

		private int _slot_last_no;

		public int ShipLastNo => _ship_last_no;

		public int SlotLastNo => _slot_last_no;

		public int ShipCount => _ship.Count;

		public int SlotCount => _slot.Count;

		public void Init()
		{
			if (_ship == null || _slot == null)
			{
				Api_get_Member api_get_Member = new Api_get_Member();
				api_get_Member.InitBookData();
				Api_Result<Dictionary<int, User_BookFmt<BookShipData>>> api_Result = api_get_Member.PictureShip();
				_ship = new Dictionary<int, IAlbumModel>();
				if (api_Result.state == Api_Result_State.Success)
				{
					foreach (KeyValuePair<int, User_BookFmt<BookShipData>> datum in api_Result.data)
					{
						if (datum.Value != null)
						{
							_ship[datum.Key] = new AlbumShipModel(datum.Value);
						}
						_ship_last_no = Math.Max(_ship_last_no, datum.Key);
					}
				}
				Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>> api_Result2 = api_get_Member.PictureSlot();
				_slot = new Dictionary<int, IAlbumModel>();
				if (api_Result2.state == Api_Result_State.Success)
				{
					foreach (KeyValuePair<int, User_BookFmt<BookSlotData>> datum2 in api_Result2.data)
					{
						if (datum2.Value != null)
						{
							_slot[datum2.Key] = new AlbumSlotModel(datum2.Value);
						}
						_slot_last_no = Math.Max(_slot_last_no, datum2.Key);
					}
				}
			}
		}

		public IAlbumModel[] GetShips(int start_no, int count)
		{
			List<IAlbumModel> list = new List<IAlbumModel>();
			for (int i = 0; i < count; i++)
			{
				list.Add(GetShip(start_no + i));
			}
			return list.ToArray();
		}

		public IAlbumModel[] GetShips()
		{
			return GetShips(1, ShipLastNo);
		}

		public IAlbumModel GetShip(int album_id)
		{
			if (!_ship.TryGetValue(album_id, out IAlbumModel value))
			{
				return new AlbumBlankModel(album_id);
			}
			return value;
		}

		public IAlbumModel[] GetSlotitems(int start_no, int count)
		{
			List<IAlbumModel> list = new List<IAlbumModel>();
			for (int i = 0; i < count; i++)
			{
				list.Add(GetSlotitem(start_no + i));
			}
			return list.ToArray();
		}

		public IAlbumModel[] GetSlotitems()
		{
			return GetSlotitems(1, SlotLastNo);
		}

		public IAlbumModel GetSlotitem(int album_id)
		{
			if (!_slot.TryGetValue(album_id, out IAlbumModel value))
			{
				return new AlbumBlankModel(album_id);
			}
			return value;
		}

		public override string ToString()
		{
			string arg = base.ToString();
			arg = arg + "\n艦のデ\u30fcタ列の最後のNo." + ShipLastNo;
			arg = arg + "\t装備のデ\u30fcタ列の最後のNo." + SlotLastNo;
			return arg + "\n";
		}
	}
}
