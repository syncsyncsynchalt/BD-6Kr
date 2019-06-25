using Server_Common.Formats;
using Server_Models;

namespace local.models
{
	public class AlbumShipModel : ShipModelMst, IAlbumModel
	{
		private User_BookFmt<BookShipData> _fmt;

		public int Id => _fmt.IndexNo;

		public string Detail => _fmt.Detail.Info;

		public int[] MstIDs => _fmt.Ids.ToArray();

		public string ClassTypeName => _fmt.Detail.ClassName;

		public AlbumShipModel(User_BookFmt<BookShipData> fmt)
		{
			_fmt = fmt;
			_mst_data = _fmt.Detail.MstData;
		}

		public bool IsDamaged()
		{
			return IsDamaged(base.MstId);
		}

		public bool IsDamaged(int mst_id)
		{
			int num = _fmt.Ids.IndexOf(mst_id);
			if (num >= 0)
			{
				return _fmt.State[num][1] == 1;
			}
			return false;
		}

		public bool IsMarriage()
		{
			return IsMarriage(base.MstId);
		}

		public bool IsMarriage(int mst_id)
		{
			int num = _fmt.Ids.IndexOf(mst_id);
			if (num >= 0)
			{
				return _fmt.State[num][2] == 1;
			}
			return false;
		}

		public override string ToString()
		{
			return ToString(detail: false);
		}

		public string ToString(bool detail)
		{
			string str = $"図鑑ID:{Id} {base.Name}(MstId:{base.MstId}) {ClassTypeName}";
			str += ((!IsMarriage()) ? " [未婚]" : " [結婚]");
			if (detail)
			{
				str += $" {base.ShipTypeName}\n";
				int[] mstIDs = MstIDs;
				for (int i = 0; i < mstIDs.Length; i++)
				{
					str += string.Format(" - 表示する艦ID:{0}({3}) ダメ\u30fcジ絵:{1} {2}\n", mstIDs[i], (!IsDamaged(mstIDs[i])) ? "ナシ" : "アリ", (!IsMarriage(mstIDs[i])) ? "未婚" : "結婚", Mst_DataManager.Instance.Mst_ship[mstIDs[i]].Name);
				}
				str += $"火力:{Karyoku} 雷装:{Raisou} 対空:{Taiku} 回避:{Kaihi} 耐久:{Taikyu}";
				str += "\n";
				str += Detail;
			}
			return str;
		}
	}
}
