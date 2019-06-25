using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodeModernzationTargetShip : MonoBehaviour
	{
		public enum ActionType
		{
			OnTouch
		}

		public delegate void UIRemodeModernzationTargetShipAction(ActionType actionType, UIRemodeModernzationTargetShip calledObject);

		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private Transform mTransformLEVEL;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UISprite mSprite_Karyoku;

		[SerializeField]
		private UISprite mSprite_Raisou;

		[SerializeField]
		private UISprite mSprite_Soukou;

		[SerializeField]
		private UISprite mSprite_Taikuu;

		[SerializeField]
		private UISprite mSprite_Luck;

		[SerializeField]
		private UISprite mSprite_Add;

		[SerializeField]
		private UIButton mButton_Action;

		private ShipModel mShipModel;

		private UIRemodeModernzationTargetShipAction mUIRemodeModernzationTargetShipAction;

		private void Start()
		{
			InitializeButtonColor();
		}

		public void SetOnUIRemodeModernzationTargetShipActionListener(UIRemodeModernzationTargetShipAction actionCallBAck)
		{
			mUIRemodeModernzationTargetShipAction = actionCallBAck;
		}

		public ShipModel GetSlotInShip()
		{
			return mShipModel;
		}

		public void Initialize(ShipModel shipModel)
		{
			mShipModel = shipModel;
			if (mShipModel != null)
			{
				SetShip(mShipModel);
			}
			else
			{
				UnSet();
			}
		}

		public void OnTouchItem()
		{
			OnAction(ActionType.OnTouch, this);
		}

		public void Hover()
		{
			mButton_Action.SetState(UIButtonColor.State.Hover, immediate: true);
			UISelectedObject.SelectedOneObjectBlink(mButton_Action.gameObject, value: true);
		}

		public void RemoveHover()
		{
			mButton_Action.SetState(UIButtonColor.State.Normal, immediate: true);
			UISelectedObject.SelectedOneObjectBlink(mButton_Action.gameObject, value: false);
		}

		public void UnSet()
		{
			Texture releaseRequestTexture = mCommonShipBanner.GetUITexture().mainTexture;
			mCommonShipBanner.GetUITexture().mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref releaseRequestTexture);
			mShipModel = null;
			mCommonShipBanner.SetActive(isActive: false);
			mTransformLEVEL.SetActive(isActive: false);
			mSprite_Karyoku.alpha = 0f;
			mSprite_Raisou.alpha = 0f;
			mSprite_Soukou.alpha = 0f;
			mSprite_Taikuu.alpha = 0f;
			mSprite_Luck.alpha = 0f;
			mLabel_Level.alpha = 0f;
			mSprite_Add.alpha = 1f;
			mLabel_Name.text = string.Empty;
		}

		private void SetShip(ShipModel shipModel)
		{
			Texture releaseRequestTexture = mCommonShipBanner.GetUITexture().mainTexture;
			mCommonShipBanner.GetUITexture().mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref releaseRequestTexture);
			mShipModel = shipModel;
			mCommonShipBanner.SetActive(isActive: true);
			mTransformLEVEL.SetActive(isActive: true);
			mSprite_Karyoku.alpha = 1f;
			mSprite_Raisou.alpha = 1f;
			mSprite_Soukou.alpha = 1f;
			mSprite_Taikuu.alpha = 1f;
			mSprite_Luck.alpha = 1f;
			mLabel_Level.alpha = 1f;
			mSprite_Add.alpha = 0f;
			if (0 < shipModel.PowUpKaryoku)
			{
				mSprite_Karyoku.spriteName = "icon_1_on";
			}
			else
			{
				mSprite_Karyoku.spriteName = "icon_1";
			}
			if (0 < shipModel.PowUpRaisou)
			{
				mSprite_Raisou.spriteName = "icon_2_on";
			}
			else
			{
				mSprite_Raisou.spriteName = "icon_2";
			}
			if (0 < shipModel.PowUpSoukou)
			{
				mSprite_Soukou.spriteName = "icon_3_on";
			}
			else
			{
				mSprite_Soukou.spriteName = "icon_3";
			}
			if (0 < shipModel.PowUpTaikuu)
			{
				mSprite_Taikuu.spriteName = "icon_4_on";
			}
			else
			{
				mSprite_Taikuu.spriteName = "icon_4";
			}
			if (0 < shipModel.PowUpLucky)
			{
				mSprite_Luck.spriteName = "icon_5_on";
			}
			else
			{
				mSprite_Luck.spriteName = "icon_5";
			}
			mCommonShipBanner.SetShipData(shipModel);
			mCommonShipBanner.StopParticle();
			mLabel_Level.text = shipModel.Level.ToString();
			mLabel_Name.text = shipModel.Name;
		}

		private void OnAction(ActionType actionType, UIRemodeModernzationTargetShip calledObject)
		{
			if (base.enabled && mUIRemodeModernzationTargetShipAction != null)
			{
				mUIRemodeModernzationTargetShipAction(actionType, calledObject);
			}
		}

		private void InitializeButtonColor()
		{
			mButton_Action.hover = Util.CursolColor;
			mButton_Action.defaultColor = Color.white;
			mButton_Action.pressed = Color.white;
			mButton_Action.disabledColor = Color.white;
		}

		public void SetEnabled(bool enabled)
		{
			base.enabled = enabled;
			mButton_Action.enabled = enabled;
		}

		internal void Release()
		{
			mUIRemodeModernzationTargetShipAction = null;
			mCommonShipBanner = null;
			mTransformLEVEL = null;
			mLabel_Level = null;
			mLabel_Name = null;
			mSprite_Karyoku.RemoveFromPanel();
			mSprite_Karyoku = null;
			mSprite_Raisou.RemoveFromPanel();
			mSprite_Raisou = null;
			mSprite_Soukou.RemoveFromPanel();
			mSprite_Soukou = null;
			mSprite_Taikuu.RemoveFromPanel();
			mSprite_Taikuu = null;
			mSprite_Luck.RemoveFromPanel();
			mSprite_Luck = null;
			mSprite_Add.RemoveFromPanel();
			mSprite_Add = null;
			mButton_Action.Release();
			mButton_Action = null;
			mShipModel = null;
		}

		internal void Refresh()
		{
			Initialize(mShipModel);
		}

		internal CommonShipBanner GetBanner()
		{
			return mCommonShipBanner;
		}
	}
}
