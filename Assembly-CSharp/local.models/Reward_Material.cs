using Common.Enum;
using local.utils;

namespace local.models
{
	public class Reward_Material : IReward, IReward_Material
	{
		private enumMaterialCategory _type;

		private int _count;

		public string Name => Utils.enumMaterialCategoryToString(_type);

		public enumMaterialCategory Type => _type;

		public int Count => _count;

		public Reward_Material(enumMaterialCategory type, int count)
		{
			_type = type;
			_count = count;
		}

		public override string ToString()
		{
			return $"{Name} {Count}個";
		}
	}
}
