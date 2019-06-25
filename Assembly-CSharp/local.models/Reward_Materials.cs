using System.Collections.Generic;

namespace local.models
{
	public class Reward_Materials : IReward, IReward_Materials
	{
		private List<IReward_Material> _reward;

		public IReward_Material[] Rewards => _reward.ToArray();

		public Reward_Materials(List<IReward_Material> reward)
		{
			_reward = reward;
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < _reward.Count; i++)
			{
				text += $"[{_reward[i]}] ";
			}
			return text;
		}
	}
}
