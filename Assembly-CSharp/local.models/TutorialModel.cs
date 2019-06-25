using Server_Models;
using System.Collections.Generic;

namespace local.models
{
	public class TutorialModel
	{
		private Mem_basic _basic;

		public int GetStep()
		{
			return _basic.GetTutorialStepLastNo();
		}

		public bool GetStepTutorialFlg(int step_count)
		{
			return _basic.GetTutorialState(1, step_count);
		}

		public int SetStepTutorialFlg()
		{
			int step = GetStep();
			int num = step + 1;
			_basic.AddTutorialProgress(1, num);
			return num;
		}

		public void SetStepTutorialFlg(int step_count)
		{
			_basic.AddTutorialProgress(1, step_count);
		}

		public bool GetKeyTutorialFlg(int key)
		{
			return _basic.GetTutorialState(0, key);
		}

		public List<int> GetTutorialKays()
		{
			return _basic.GetTutorialProgress(0);
		}

		public void SetKeyTutorialFlg(int key)
		{
			_basic.AddTutorialProgress(0, key);
		}

		public void __Update__(Mem_basic basic)
		{
			_basic = basic;
		}

		public override string ToString()
		{
			string text = $"Step:{GetStep()} Key:[";
			List<int> tutorialKays = GetTutorialKays();
			for (int i = 0; i < tutorialKays.Count; i++)
			{
				text += tutorialKays[i];
				if (i < tutorialKays.Count - 1)
				{
					text += ",";
				}
			}
			return text + "]";
		}
	}
}
