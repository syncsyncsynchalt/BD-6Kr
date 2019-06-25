using Common.Struct;
using local.utils;

namespace local.models
{
	public class Reward_PortExtend : IReward
	{
		public int ShipMaxNum
		{
			get
			{
				MemberMaxInfo memberMaxInfo = Utils.ShipCountData();
				return memberMaxInfo.MaxCount;
			}
		}

		public int SlotMaxNum
		{
			get
			{
				MemberMaxInfo memberMaxInfo = Utils.SlotitemCountData();
				return memberMaxInfo.MaxCount;
			}
		}

		public override string ToString()
		{
			return $"母港拡張報酬: 最大艦数:{ShipMaxNum} 最大装備数:{SlotMaxNum}";
		}
	}
}
