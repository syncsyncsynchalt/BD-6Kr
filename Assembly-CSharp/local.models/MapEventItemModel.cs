using Common.Enum;
using local.utils;
using Server_Common.Formats;
using Server_Models;

namespace local.models
{
	public class MapEventItemModel
	{
		private MapItemGetFmt.enumCategory _category;

		private int _item_id;

		private int _count;

		public int ItemID => _item_id;

		public int Count => _count;

		public enumMaterialCategory MaterialCategory => (enumMaterialCategory)(IsMaterial() ? _item_id : 0);

		public MapEventItemModel(MapItemGetFmt fmt)
		{
			if (fmt == null)
			{
				_category = MapItemGetFmt.enumCategory.None;
				_item_id = 0;
				_count = 0;
			}
			else
			{
				_category = fmt.Category;
				_item_id = fmt.Id;
				_count = fmt.GetCount;
			}
		}

		public bool IsMaterial()
		{
			return _category == MapItemGetFmt.enumCategory.Material;
		}

		public bool IsUseItem()
		{
			return _category == MapItemGetFmt.enumCategory.UseItem;
		}

		public override string ToString()
		{
			if (IsMaterial())
			{
				string arg = Utils.enumMaterialCategoryToString(MaterialCategory);
				return $"{arg}(id:{_item_id}) Count:{_count}";
			}
			if (IsUseItem())
			{
				Mst_useitem mst_useitem = Mst_DataManager.Instance.Mst_useitem[ItemID];
				return $"使用アイテム:{mst_useitem.Name}(id:{ItemID}) Count:{Count}";
			}
			return $"[Item None]";
		}
	}
}
