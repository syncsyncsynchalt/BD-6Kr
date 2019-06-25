using Common.Enum;
using local.utils;
using Server_Common.Formats;

namespace local.models
{
	public class MapEventHappeningModel
	{
		private int _item_id;

		private int _count;

		private bool _dentan;

		public enumMaterialCategory Material => (enumMaterialCategory)_item_id;

		public int Count => _count;

		public bool Dentan => _dentan;

		public MapEventHappeningModel(MapHappningFmt fmt)
		{
			_item_id = fmt.Id;
			_count = fmt.Count;
			_dentan = fmt.Dentan;
		}

		public override string ToString()
		{
			string text = Utils.enumMaterialCategoryToString(Material);
			return string.Format("{0}(id:{1}) Count:{2} 電探効果:{3}", text, _item_id, _count, (!Dentan) ? "無" : "有");
		}
	}
}
