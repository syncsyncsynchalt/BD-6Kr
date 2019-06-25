using Common.Struct;
using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Mission
{
	public class UIMissionResultBonus : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Exp;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_SPoint;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private Transform mTransform_ShipFrame;

		[SerializeField]
		private Transform mTransform_ResultBonusFrame;

		[SerializeField]
		private Transform[] mTransforms_Reward;

		private MissionResultModel mMissionResultModel;

		public void Inititalize(MissionResultModel missionResultModel)
		{
			mMissionResultModel = missionResultModel;
			Transform[] array = mTransforms_Reward;
			foreach (Transform transform in array)
			{
				transform.SetActive(isActive: false);
				((Component)transform.Find("Label_Value")).GetComponent<UILabel>().text = string.Empty;
				((Component)transform.Find("Sprite_RewardIcon")).GetComponent<UISprite>().spriteName = string.Empty;
			}
			int extraItemCount = missionResultModel.ExtraItemCount;
			for (int j = 0; j < extraItemCount && j < mTransforms_Reward.Length; j++)
			{
				Transform transform2 = mTransforms_Reward[j];
				transform2.SetActive(isActive: true);
				((Component)transform2.Find("Label_Value")).GetComponent<UILabel>().text = missionResultModel.GetItemCount(j).ToString();
				((Component)transform2.Find("Sprite_RewardIcon")).GetComponent<UISprite>().spriteName = "item_" + missionResultModel.GetItemID(j).ToString();
			}
		}

		public void Play(Action onFinished)
		{
			PlayResult(onFinished);
		}

		public void PlayResult(Action onFinished)
		{
			mLabel_Exp.text = "0";
			mLabel_Fuel.text = "0";
			mLabel_Ammo.text = "0";
			mLabel_Steel.text = "0";
			mLabel_Bauxite.text = "0";
			mLabel_SPoint.text = "0";
			Sequence sequence = DOTween.Sequence().SetId(this);
			Tween t = mTransform_ShipFrame.DOLocalMove(new Vector3(265f, 150f), 0.8f);
			mTransform_ResultBonusFrame.transform.localPositionX(128f);
			Tween t2 = mTransform_ResultBonusFrame.DOLocalMove(Vector3.zero, 0.4f).SetEase(Ease.OutCirc);
			UIWidget resultBonusAlpha = ((Component)mTransform_ResultBonusFrame).GetComponent<UIWidget>();
			resultBonusAlpha.alpha = 0f;
			Tween t3 = DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
			{
				resultBonusAlpha.alpha = alpha;
			}).SetEase(Ease.InCirc);
			Tween t4 = DOVirtual.Float(0f, 1f, 0.8f, delegate(float part)
			{
				MaterialInfo materialInfo = mMissionResultModel.GetMaterialInfo();
				mLabel_Exp.text = ((int)((float)mMissionResultModel.Exp * part)).ToString();
				mLabel_Fuel.text = ((int)((float)materialInfo.Fuel * part)).ToString();
				mLabel_Ammo.text = ((int)((float)materialInfo.Ammo * part)).ToString();
				mLabel_Steel.text = ((int)((float)materialInfo.Steel * part)).ToString();
				mLabel_Bauxite.text = ((int)((float)materialInfo.Baux * part)).ToString();
				mLabel_SPoint.text = ((int)((float)mMissionResultModel.Spoint * part)).ToString();
			});
			sequence.Append(t);
			sequence.AppendInterval(0.5f);
			sequence.Append(t4);
			sequence.Join(t3);
			sequence.Join(t2);
			sequence.OnComplete(delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
			});
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Exp);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Fuel);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Steel);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Ammo);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_SPoint);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Bauxite);
			mTransform_ShipFrame = null;
			mTransform_ResultBonusFrame = null;
			mTransforms_Reward = null;
			mMissionResultModel = null;
		}
	}
}
