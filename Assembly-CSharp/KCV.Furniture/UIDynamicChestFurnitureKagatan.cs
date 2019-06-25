using DG.Tweening;
using KCV.Scene.Port;
using System.Collections;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicChestFurnitureKagatan : UIDynamicFurniture
	{
		private enum ShipType
		{
			Kubo,
			Goei
		}

		private ShipType mCurrentShipType;

		[Header("加賀")]
		[SerializeField]
		private UITexture mTexture_Kaga;

		[Header("かが")]
		[SerializeField]
		private UITexture mTexture_KAGA;

		[SerializeField]
		[Header("妖精")]
		private UISprite mSprite_Tansuyokusei;

		[SerializeField]
		[Header("DEBUG")]
		private bool mIsMoving;

		protected override void OnAwake()
		{
			mSprite_Tansuyokusei.alpha = 0.1f;
			mTexture_KAGA.alpha = 1E-06f;
			mSprite_Tansuyokusei.spriteName = "00";
			mCurrentShipType = ShipType.Kubo;
		}

		private void OnClick()
		{
			OnCalledActionEvent();
		}

		protected override void OnCalledActionEvent()
		{
			if (!mIsMoving)
			{
				IEnumerator routine = OnCalledActionEventCoroutine();
				StartCoroutine(routine);
			}
		}

		private IEnumerator OnCalledActionEventCoroutine()
		{
			mIsMoving = true;
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.alpha = 1f;
			mSprite_Tansuyokusei.spriteName = "00";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "01";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "02";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "03";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "04";
			yield return new WaitForSeconds(2f);
			switch (mCurrentShipType)
			{
			case ShipType.Goei:
			{
				Tween KAGAToKaga2 = DOVirtual.Float(0.95f, 1f, 1.2f, delegate(float alpha)
				{
					this.mTexture_Kaga.alpha = alpha;
					this.mTexture_KAGA.alpha = 1f - alpha;
				});
				yield return KAGAToKaga2.WaitForKill();
				mCurrentShipType = ShipType.Kubo;
				break;
			}
			case ShipType.Kubo:
			{
				Tween KagaToKAGA = DOVirtual.Float(0.95f, 1f, 1.2f, delegate(float alpha)
				{
					this.mTexture_Kaga.alpha = 1f - alpha;
					this.mTexture_KAGA.alpha = alpha;
				});
				yield return KagaToKAGA.WaitForKill();
				mCurrentShipType = ShipType.Goei;
				break;
			}
			}
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "03";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "02";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "01";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.spriteName = "00";
			yield return new WaitForEndOfFrame();
			mSprite_Tansuyokusei.alpha = 0.1f;
			yield return new WaitForSeconds(10f);
			ShipType shipType = mCurrentShipType;
			if (shipType == ShipType.Goei)
			{
				Tween KAGAToKaga = DOVirtual.Float(0.95f, 1f, 1.2f, delegate(float alpha)
				{
					this.mTexture_Kaga.alpha = alpha;
					this.mTexture_KAGA.alpha = 1f - alpha;
				});
				yield return KAGAToKaga.WaitForKill();
				mCurrentShipType = ShipType.Kubo;
			}
			mIsMoving = false;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Kaga);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_KAGA);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Tansuyokusei);
		}
	}
}
