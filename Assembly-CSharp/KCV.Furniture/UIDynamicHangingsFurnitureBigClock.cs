using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicHangingsFurnitureBigClock : UIDynamicFurniture
	{
		private const int OPEN_GATE_WIDTH = 43;

		private const int OPEN_GATE_HEIGHT = 16;

		private const int CLOSE_GATE_WIDTH = 26;

		private const int CLOSE_GATE_HEIGHT = 12;

		private const int OPEN_PEGION_WIDTH = 29;

		private const int OPEN_PEGION_HEIGHT = 16;

		private const int CLOSE_PEGION_WIDTH = 26;

		private const int CLOSE_PEGION_HEIGHT = 12;

		[SerializeField]
		[Header("Pegion")]
		private Transform mPegion;

		[SerializeField]
		private Vector2 mVector2_ClosePegionLocalPosition;

		[SerializeField]
		private Vector2 mVector2_OpenPegionLocalPosition;

		[SerializeField]
		private UITexture mTexture_Pegion;

		[SerializeField]
		private Texture mTexture2d_Pegion_On;

		[SerializeField]
		private Texture mTexture2d_Pegion_Off;

		[SerializeField]
		[Header("Gate")]
		private UITexture mTexture_Gate;

		[SerializeField]
		private Texture mTexture2d_OpenGate;

		[SerializeField]
		private Texture mTexture2d_CloseGate;

		[SerializeField]
		private AudioClip mAudioClip_Pegion;

		private ShipModel mShipModel;

		private int mLastPlayMinute = -1;

		private void Update()
		{
			if (mFurnitureModel != null && (mFurnitureModel.GetDateTime().Minute == 0 || mFurnitureModel.GetDateTime().Minute % 15 == 0))
			{
				bool flag = DOTween.IsTweening(this);
				bool flag2 = mLastPlayMinute == mFurnitureModel.GetDateTime().Minute;
				if (!flag && !flag2)
				{
					mLastPlayMinute = mFurnitureModel.GetDateTime().Minute;
					Animation();
				}
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				Animation();
			}
		}

		protected override void OnAwake()
		{
			Initialize(new UIFurnitureModel(null, null));
			mPegion.gameObject.SetActive(false);
			CloseGate();
			ClosePegion();
		}

		private void ClosePegion()
		{
			OffPegion();
		}

		protected override void OnCalledActionEvent()
		{
			base.OnCalledActionEvent();
			Animation();
		}

		private void Animation()
		{
			if (!DOTween.IsTweening(this))
			{
				Sequence sequence = DOTween.Sequence().SetId(this);
				TweenCallback action = delegate
				{
					OpenGate();
					mPegion.gameObject.SetActive(true);
					mPegion.transform.localPosition = mVector2_ClosePegionLocalPosition;
					SoundUtils.PlaySE(mAudioClip_Pegion);
					OffPegion();
				};
				Tween t = mPegion.transform.DOLocalMove(mVector2_OpenPegionLocalPosition, 0.3f).SetId(this);
				Sequence sequence2 = DOTween.Sequence().SetId(this);
				TweenCallback callback = delegate
				{
					OnPegion();
				};
				TweenCallback callback2 = delegate
				{
					OffPegion();
				};
				sequence2.AppendCallback(callback);
				sequence2.AppendInterval(0.1f);
				sequence2.AppendCallback(callback2);
				sequence2.AppendInterval(0.1f);
				sequence2.AppendCallback(callback);
				sequence2.AppendInterval(0.1f);
				sequence2.AppendCallback(callback2);
				sequence2.AppendInterval(0.1f);
				Tween t2 = mPegion.transform.DOLocalMove(mVector2_ClosePegionLocalPosition, 0.15f).SetId(this);
				TweenCallback callback3 = delegate
				{
					CloseGate();
					mPegion.gameObject.SetActive(false);
				};
				sequence.OnPlay(action);
				sequence.Append(t);
				sequence.Append(sequence2);
				sequence.Append(t2);
				sequence.AppendCallback(callback3);
			}
		}

		private void OffPegion()
		{
			mTexture_Pegion.mainTexture = mTexture2d_Pegion_Off;
			mTexture_Pegion.width = 26;
			mTexture_Pegion.height = 12;
		}

		private void OnPegion()
		{
			mTexture_Pegion.mainTexture = mTexture2d_Pegion_On;
			mTexture_Pegion.width = 29;
			mTexture_Pegion.height = 16;
		}

		private void OpenGate()
		{
			mTexture_Gate.mainTexture = mTexture2d_OpenGate;
			mTexture_Gate.width = 43;
			mTexture_Gate.height = 16;
		}

		private void CloseGate()
		{
			mTexture_Gate.mainTexture = mTexture2d_CloseGate;
			mTexture_Gate.width = 26;
			mTexture_Gate.height = 12;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Pegion);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Pegion_On);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Pegion_Off);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Gate);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_OpenGate);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_CloseGate);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_Pegion);
			mPegion = null;
			mShipModel = null;
		}
	}
}
