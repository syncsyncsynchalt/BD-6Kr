using Common.Enum;
using System.Collections.Generic;

namespace Common.Struct
{
	public struct MaterialInfo
	{
		public int Fuel;

		public int Ammo;

		public int Steel;

		public int Baux;

		public int BuildKit;

		public int RepairKit;

		public int Devkit;

		public int Revkit;

		public MaterialInfo(Dictionary<enumMaterialCategory, int> dic)
		{
			if (dic != null)
			{
				dic.TryGetValue(enumMaterialCategory.Fuel, out Fuel);
				dic.TryGetValue(enumMaterialCategory.Bull, out Ammo);
				dic.TryGetValue(enumMaterialCategory.Steel, out Steel);
				dic.TryGetValue(enumMaterialCategory.Bauxite, out Baux);
				dic.TryGetValue(enumMaterialCategory.Build_Kit, out BuildKit);
				dic.TryGetValue(enumMaterialCategory.Repair_Kit, out RepairKit);
				dic.TryGetValue(enumMaterialCategory.Dev_Kit, out Devkit);
				dic.TryGetValue(enumMaterialCategory.Revamp_Kit, out Revkit);
			}
			else
			{
				Fuel = (Ammo = (Steel = (Baux = (BuildKit = (RepairKit = (Devkit = (Revkit = 0)))))));
			}
		}

		public bool IsAllZero()
		{
			return Fuel == 0 && Ammo == 0 && Steel == 0 && Baux == 0 && BuildKit == 0 && RepairKit == 0 && Devkit == 0 && Revkit == 0;
		}

		public bool HasPositive()
		{
			return Fuel > 0 || Ammo > 0 || Steel > 0 || Baux > 0 || BuildKit > 0 || RepairKit > 0 || Devkit > 0 || Revkit > 0;
		}

		public void Set4(int fuel, int ammo, int steel, int baux)
		{
			Fuel = fuel;
			Ammo = ammo;
			Steel = steel;
			Baux = baux;
		}
	}
}
