using Common.Enum;
using Server_Common.Formats;

namespace local.models
{
	public class AlbumSlotModel : SlotitemModel_Mst, IAlbumModel
	{
		private User_BookFmt<BookSlotData> _fmt;

		public int Id => _fmt.IndexNo;

		public string Detail => _fmt.Detail.Info;

		public AlbumSlotModel(User_BookFmt<BookSlotData> fmt)
			: base(fmt.Detail.MstData)
		{
			_fmt = fmt;
		}

		public bool CanEquip(SType ship_type)
		{
			return _fmt.Detail.EnableStype.IndexOf((int)ship_type) != -1;
		}

		public override string ToString()
		{
			return ToString(detail: false);
		}

		public string ToString(bool detail)
		{
			string text = $"図鑑ID:{Id} {base.Name}(MstId:{MstId})";
			if (detail)
			{
				text += $" 図鑑背景タイプ:{base.Type2}  装備タイプ:{base.Type3}  装備アイコンタイプ:{base.Type4}\n";
				text += $"装甲:{base.Soukou} 火力:{base.Hougeki} 雷装:{base.Raigeki} 爆撃:{base.Bakugeki} 対空:{base.Taikuu} 対潜:{base.Taisen} 命中(砲):{base.HouMeityu}";
				text += $" 回避:{base.Kaihi} 索敵:{base.Sakuteki} 射程:{base.Syatei}\n";
				text += string.Format("駆逐艦:装備{0}", (!CanEquip(SType.Destroyter)) ? "不可" : "可能");
				text += string.Format(" 軽巡洋艦:装備{0}", (!CanEquip(SType.LightCruiser)) ? "不可" : "可能");
				text += string.Format(" 重巡洋艦:装備{0}", (!CanEquip(SType.HeavyCruiser)) ? "不可" : "可能");
				text += string.Format(" 戦艦:装備{0}", (!CanEquip(SType.BattleShip)) ? "不可" : "可能");
				text += string.Format(" 軽空母:装備{0}", (!CanEquip(SType.LightAircraftCarrier)) ? "不可" : "可能");
				text += string.Format(" 正規空母:装備{0}", (!CanEquip(SType.AircraftCarrier)) ? "不可" : "可能");
				text += string.Format(" 水上機母艦:装備{0}", (!CanEquip(SType.SeaplaneTender)) ? "不可" : "可能");
				text += string.Format(" 航空戦艦:装備{0}\n", (!CanEquip(SType.AviationBattleShip)) ? "不可" : "可能");
				text += Detail;
			}
			return text;
		}
	}
}
