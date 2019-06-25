using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdShortRewardGet : BaseAnimation
	{
		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UISprite _uiItemIcon;

		[SerializeField]
		private UITexture _uiOverlay;

		private Queue<IReward> _queRewards;

		public static ProdShortRewardGet Instantiate(ProdShortRewardGet prefab, Transform parent, List<IReward> rewards)
		{
			ProdShortRewardGet prodShortRewardGet = UnityEngine.Object.Instantiate(prefab);
			prodShortRewardGet.transform.parent = parent;
			prodShortRewardGet.transform.localScaleOne();
			prodShortRewardGet.transform.localPositionZero();
			return prodShortRewardGet.VirtualCtor(rewards);
		}

		private ProdShortRewardGet VirtualCtor(List<IReward> rewards)
		{
			_uiPanel.depth = 120;
			_queRewards = new Queue<IReward>(rewards);
			SetReward(_queRewards.Dequeue());
			return this;
		}

		private bool SetReward(IReward reward)
		{
			if (reward is IReward_Material)
			{
				return SetMaterial((IReward_Material)reward);
			}
			if (reward is IReward_Materials)
			{
				return SetMaterials((IReward_Materials)reward);
			}
			if (reward is IReward_Ship)
			{
				return SetShip((IReward_Ship)reward);
			}
			if (reward is IReward_Slotitem)
			{
				return SetSlotItem((IReward_Slotitem)reward);
			}
			if (reward is IReward_Useitem)
			{
				return SetUseItem((IReward_Useitem)reward);
			}
			return false;
		}

		private bool SetMaterial(IReward_Material iMaterial)
		{
			return true;
		}

		private bool SetMaterials(IReward_Materials iMaterials)
		{
			return true;
		}

		private bool SetShip(IReward_Ship iShip)
		{
			return true;
		}

		private bool SetSlotItem(IReward_Slotitem iSlotItem)
		{
			return true;
		}

		private bool SetUseItem(IReward_Useitem iUseItem)
		{
			_uiItemIcon.spriteName = $"item_{iUseItem.Id}";
			_uiItemIcon.localSize = new Vector2(128f, 128f);
			return true;
		}

		protected override void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiItemIcon);
			Mem.Del(ref _uiOverlay);
			Mem.DelQueueSafe(ref _queRewards);
			base.OnDestroy();
		}

		public override void Play(Action callback)
		{
			base.Init();
			_actCallback = callback;
			Play();
		}

		private new void Play()
		{
			SoundUtils.PlaySE(SEFIleInfos.RewardGet);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            // base.animation.get_Item("ProdShortRewardGet").time = 0f;

			base.animation.Play();
		}

		private void OnFinished()
		{
			if (_queRewards.Count == 0)
			{
				base.onAnimationFinished();
				return;
			}
			SetReward(_queRewards.Dequeue());
			Play();
		}
	}
}
