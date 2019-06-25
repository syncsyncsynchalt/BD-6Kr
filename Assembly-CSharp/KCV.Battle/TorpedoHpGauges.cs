using UnityEngine;

namespace KCV.Battle
{
	public class TorpedoHpGauges
	{
		private BattleHPGauges FBattleHpGauges;

		private BattleHPGauges EBattleHpGauges;

		private UIPanel UiHpGaugePanel;

		public BattleHPGauges FHpGauge
		{
			get
			{
				return FBattleHpGauges;
			}
			set
			{
				FBattleHpGauges = value;
			}
		}

		public BattleHPGauges EHpGauge
		{
			get
			{
				return EBattleHpGauges;
			}
			set
			{
				EBattleHpGauges = value;
			}
		}

		public UIPanel UiPanel
		{
			get
			{
				return UiHpGaugePanel;
			}
			set
			{
				UiHpGaugePanel = value;
			}
		}

		public void Hide()
		{
			if (UiHpGaugePanel != null)
			{
				UiHpGaugePanel.alpha = 0f;
			}
		}

		public void SetDestroy()
		{
			if (FBattleHpGauges != null)
			{
				FBattleHpGauges.Dispose();
			}
			if (EBattleHpGauges != null)
			{
				EBattleHpGauges.Dispose();
			}
			Mem.Del(ref FBattleHpGauges);
			Mem.Del(ref EBattleHpGauges);
			if (UiHpGaugePanel != null)
			{
				Object.Destroy(UiHpGaugePanel.gameObject);
			}
			Mem.Del(ref UiHpGaugePanel);
		}

		public void InstancePanel(GameObject panel, GameObject parent)
		{
			if ((bool)parent.transform.FindChild("UICircleHpPanel"))
			{
				UiHpGaugePanel = ((Component)parent.transform.FindChild("UICircleHpPanel")).GetComponent<UIPanel>();
				UiHpGaugePanel.alpha = 0f;
			}
			else
			{
				UiHpGaugePanel = Util.Instantiate(panel, parent).GetComponent<UIPanel>();
				UiHpGaugePanel.alpha = 0f;
			}
		}
	}
}
