using Server_Models;

namespace local.models
{
	public class __FStoreItemModel__ : FurnitureModel
	{
		public __FStoreItemModel__(Mst_furniture mst, string description)
			: base(mst, description)
		{
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += string.Format("{0}", (!IsPossession()) ? string.Empty : "[所持] ");
			empty += base.ToString();
			empty += $" レア度:{base.Rarity} 必要コイン数:{base.Price}";
			return empty + string.Format("{0}", (!IsNeedWorker()) ? string.Empty : " [家具職人]");
		}
	}
}
