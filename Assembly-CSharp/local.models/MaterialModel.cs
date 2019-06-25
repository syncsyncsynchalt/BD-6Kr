using Common.Enum;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class MaterialModel
	{
		private Dictionary<enumMaterialCategory, Mem_material> _materialData;

		public int Fuel => _materialData[enumMaterialCategory.Fuel].Value;

		public int Ammo => _materialData[enumMaterialCategory.Bull].Value;

		public int Steel => _materialData[enumMaterialCategory.Steel].Value;

		public int Baux => _materialData[enumMaterialCategory.Bauxite].Value;

		public int BuildKit => _materialData[enumMaterialCategory.Build_Kit].Value;

		public int RepairKit => _materialData[enumMaterialCategory.Repair_Kit].Value;

		public int Devkit => _materialData[enumMaterialCategory.Dev_Kit].Value;

		public int Revkit => _materialData[enumMaterialCategory.Revamp_Kit].Value;

		public bool Update()
		{
			Api_Result<Dictionary<enumMaterialCategory, Mem_material>> api_Result = new Api_get_Member().Material();
			if (api_Result.state == Api_Result_State.Success)
			{
				Update(api_Result.data);
				return true;
			}
			return false;
		}

		public bool Update(Dictionary<enumMaterialCategory, Mem_material> materialData)
		{
			_materialData = materialData;
			return true;
		}

		public int GetCount(enumMaterialCategory category)
		{
			return _materialData[category].Value;
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty += $"[資材]燃/弾/鋼/ボ: {Fuel}/{Ammo}/{Steel}/{Baux}\t";
			return empty + $"高速建造材/高速修復材/開発資材/改修資材: {BuildKit}/{RepairKit}/{Devkit}/{Revkit}";
		}
	}
}
