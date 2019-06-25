using Common.Enum;
using KCV.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeTender : MonoBehaviour
	{
		public enum TenderState
		{
			None,
			Select,
			Maimiya,
			Irako,
			Twin
		}

		public enum BtnType
		{
			OFF,
			NORMAL,
			ON
		}

		private readonly Vector3 SWEETS_FOCUS_SCALE = new Vector3(1f, 1f, 1f);

		private readonly Vector3 SWEETS_UNFOCUS_SCALE = new Vector3(0.7f, 0.7f, 0.7f);

		private readonly Vector3 SWEETS_POSITION_CENTER = new Vector3(0f, -140f, 0f);

		private readonly Vector3 SWEETS_POSITION_LEFT = new Vector3(-265f, -40f, 0f);

		private readonly Vector3 SWEETS_POSITION_RIGHT = new Vector3(265f, -40f, 0f);

		[SerializeField]
		private GameObject _tenderPanel;

		[SerializeField]
		private GameObject _mamiyaPanel;

		[SerializeField]
		private GameObject _twinPanel;

		[SerializeField]
		private GameObject _animatePanel;

		[SerializeField]
		private UISprite _allBtn;

		[SerializeField]
		private UISprite _mamiyaBtn;

		[SerializeField]
		private UISprite _irakoBtn;

		[SerializeField]
		private UITexture _allC1;

		[SerializeField]
		private UITexture _allC2;

		[SerializeField]
		private UITexture _mainBg;

		[SerializeField]
		private UITexture _otherBg;

		[SerializeField]
		private UITexture _ship;

		[SerializeField]
		private UISprite _item;

		[SerializeField]
		private UISprite _btnYes;

		[SerializeField]
		private UISprite _btnNo;

		[SerializeField]
		private UILabel _labelFrom;

		[SerializeField]
		private UILabel _labelTo;

		[SerializeField]
		private UISprite _tBtnYes;

		[SerializeField]
		private UISprite _tBtnNo;

		[SerializeField]
		private UILabel _labelFrom1;

		[SerializeField]
		private UILabel _labelTo1;

		[SerializeField]
		private UILabel _labelFrom2;

		[SerializeField]
		private UILabel _labelTo2;

		[SerializeField]
		private UITexture _ship1;

		[SerializeField]
		private UITexture _ship2;

		[SerializeField]
		private ParticleSystem _parSystem;

		[SerializeField]
		private ParticleSystem _parSystem2;

		[SerializeField]
		private Animation _animation;

		[SerializeField]
		private UILabel _topLabel;

		private bool IsMoveMamiya;

		private bool IsMoveIrako;

		private bool IsMoveAll;

		public int setIndex;

		public int setIndex2;

		public bool isAnimation;

		public bool _GuideOff;

		public TenderState State;

		public Dictionary<SweetsType, bool> tenderDic;

		public void init()
		{
			if (_tenderPanel == null)
			{
				_tenderPanel = base.transform.FindChild("TenderDialog").gameObject;
			}
			if (_mamiyaPanel == null)
			{
				_mamiyaPanel = base.transform.FindChild("MamiyaDialog").gameObject;
			}
			if (_twinPanel == null)
			{
				_twinPanel = base.transform.FindChild("TwinDialog").gameObject;
			}
			if (_animatePanel == null)
			{
				_animatePanel = base.transform.FindChild("AnimatePanel").gameObject;
			}
			Util.FindParentToChild(ref _allBtn, _tenderPanel.transform, "AllBtn");
			Util.FindParentToChild(ref _mamiyaBtn, _tenderPanel.transform, "MamiyaBtn");
			Util.FindParentToChild(ref _irakoBtn, _tenderPanel.transform, "IrakoBtn");
			Util.FindParentToChild(ref _allC1, _tenderPanel.transform, "All1");
			Util.FindParentToChild(ref _allC2, _tenderPanel.transform, "All2");
			Util.FindParentToChild(ref _mainBg, base.transform, "Bg");
			Util.FindParentToChild(ref _otherBg, base.transform, "UseBg");
			_ship = ((Component)_mamiyaPanel.transform.FindChild("Ship")).GetComponent<UITexture>();
			_topLabel = ((Component)_mamiyaPanel.transform.FindChild("Label_Mamiya")).GetComponent<UILabel>();
			_item = ((Component)_mamiyaPanel.transform.FindChild("Item")).GetComponent<UISprite>();
			_btnYes = ((Component)_mamiyaPanel.transform.FindChild("YesBtn")).GetComponent<UISprite>();
			_btnNo = ((Component)_mamiyaPanel.transform.FindChild("NoBtn")).GetComponent<UISprite>();
			_labelFrom = ((Component)_mamiyaPanel.transform.FindChild("LabelFrom")).GetComponent<UILabel>();
			_labelTo = ((Component)_mamiyaPanel.transform.FindChild("LabelTo")).GetComponent<UILabel>();
			_tBtnYes = ((Component)_twinPanel.transform.FindChild("YesBtn")).GetComponent<UISprite>();
			_tBtnNo = ((Component)_twinPanel.transform.FindChild("NoBtn")).GetComponent<UISprite>();
			_labelFrom1 = ((Component)_twinPanel.transform.FindChild("Frame1_1/LabelFrom")).GetComponent<UILabel>();
			_labelTo1 = ((Component)_twinPanel.transform.FindChild("Frame1_2/LabelTo")).GetComponent<UILabel>();
			_labelFrom2 = ((Component)_twinPanel.transform.FindChild("Frame2_1/LabelFrom")).GetComponent<UILabel>();
			_labelTo2 = ((Component)_twinPanel.transform.FindChild("Frame2_2/LabelTo")).GetComponent<UILabel>();
			_ship1 = ((Component)_animatePanel.transform.FindChild("Ship/Ship1")).GetComponent<UITexture>();
			_ship2 = ((Component)_animatePanel.transform.FindChild("Ship/Ship2")).GetComponent<UITexture>();
			_animation = base.gameObject.GetComponent<Animation>();
			_animation.Stop();
			_parSystem = ((Component)_animatePanel.transform.FindChild("Circle/Par")).GetComponent<ParticleSystem>();
			_parSystem2 = ((Component)_animatePanel.transform.FindChild("Par2")).GetComponent<ParticleSystem>();
			_parSystem.Stop();
			_parSystem2.Stop();
			setButtonMessage(_allBtn.gameObject, "AllBtnEL");
			setButtonMessage(_irakoBtn.gameObject, "IrakoBtnEL");
			setButtonMessage(_mamiyaBtn.gameObject, "MamiyaBtnEL");
			setButtonMessage(_mainBg.gameObject, "MainBackEL");
			setButtonMessage(_otherBg.gameObject, "OtherBackEL");
			setButtonMessage(_btnYes.gameObject, "BtnYesEL");
			setButtonMessage(_tBtnYes.gameObject, "BtnYesEL");
			setButtonMessage(_btnNo.gameObject, "BtnNoEL");
			setButtonMessage(_tBtnNo.gameObject, "BtnNoEL");
			State = TenderState.None;
			setIndex = 0;
			setIndex2 = 0;
			isAnimation = false;
			_GuideOff = false;
			((Component)base.transform).GetComponent<UIPanel>().alpha = 0f;
		}

		private void setButtonMessage(GameObject obj, string name)
		{
			UIButtonMessage component = obj.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = name;
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _tenderPanel);
			Mem.Del(ref _mamiyaPanel);
			Mem.Del(ref _twinPanel);
			Mem.Del(ref _animatePanel);
			Mem.Del(ref _allBtn);
			Mem.Del(ref _mamiyaBtn);
			Mem.Del(ref _irakoBtn);
			Mem.Del(ref _allC1);
			Mem.Del(ref _allC2);
			Mem.Del(ref _mainBg);
			Mem.Del(ref _otherBg);
			Mem.Del(ref _ship);
			Mem.Del(ref _item);
			Mem.Del(ref _btnYes);
			Mem.Del(ref _btnNo);
			Mem.Del(ref _labelFrom);
			Mem.Del(ref _labelTo);
			Mem.Del(ref _tBtnYes);
			Mem.Del(ref _tBtnNo);
			Mem.Del(ref _labelFrom1);
			Mem.Del(ref _labelTo1);
			Mem.Del(ref _labelFrom2);
			Mem.Del(ref _labelTo2);
			Mem.Del(ref _ship1);
			Mem.Del(ref _ship2);
			Mem.Del(ref _parSystem);
			Mem.Del(ref _parSystem2);
			Mem.Del(ref _animation);
			Mem.Del(ref _topLabel);
			Mem.Del(ref State);
			Mem.DelDictionarySafe(ref tenderDic);
		}

		public void SetMainDialog()
		{
			if (tenderDic[SweetsType.Mamiya])
			{
				_mamiyaBtn.spriteName = ((setIndex != 0) ? "btn_mamiya" : "btn_mamiya_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(_mamiyaBtn.gameObject, (setIndex == 0) ? true : false);
			}
			else
			{
				_mamiyaBtn.spriteName = "btn_mamiya_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(_mamiyaBtn.gameObject, value: false);
			}
			if (tenderDic[SweetsType.Both])
			{
				_allBtn.spriteName = ((setIndex != 1) ? "btn_m+i" : "btn_m+i_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(_allBtn.gameObject, (setIndex == 1) ? true : false);
			}
			else
			{
				_allBtn.spriteName = "btn_m+i_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(_allBtn.gameObject, value: false);
			}
			if (tenderDic[SweetsType.Irako])
			{
				_irakoBtn.spriteName = ((setIndex != 2) ? "btn_irako" : "btn_irako_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(_allBtn.gameObject, (setIndex == 2) ? true : false);
			}
			else
			{
				_irakoBtn.spriteName = "btn_irako_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(_irakoBtn.gameObject, value: false);
			}
			if (setIndex == 0)
			{
				TaskOrganizeTop.KeyController.IsRun = false;
				moveAnimate(_mamiyaBtn, 0f, -140f, isSet: true);
				moveAnimate(_irakoBtn, -265f, -40f, isSet: false);
				moveAnimate(_allBtn, 265f, -40f, isSet: false);
				UISelectedObject.SelectedOneButtonZoomUpDown(_mamiyaBtn.gameObject, value: true);
			}
			else if (setIndex == 1)
			{
				TaskOrganizeTop.KeyController.IsRun = false;
				moveAnimate(_allBtn, 0f, -140f, isSet: true);
				moveAnimate(_mamiyaBtn, -265f, -40f, isSet: false);
				moveAnimate(_irakoBtn, 265f, -40f, isSet: false);
				UISelectedObject.SelectedOneButtonZoomUpDown(_allBtn.gameObject, value: true);
			}
			else if (setIndex == 2)
			{
				TaskOrganizeTop.KeyController.IsRun = false;
				moveAnimate(_irakoBtn, 0f, -140f, isSet: true);
				moveAnimate(_allBtn, -265f, -40f, isSet: false);
				moveAnimate(_mamiyaBtn, 265f, -40f, isSet: false);
				UISelectedObject.SelectedOneButtonZoomUpDown(_irakoBtn.gameObject, value: true);
			}
			SetMainDialogAnimation();
		}

		public void SetMainDialogAnimation()
		{
			IsMoveMamiya = true;
			IsMoveIrako = true;
			IsMoveAll = true;
			if (tenderDic[SweetsType.Mamiya] && setIndex == 0)
			{
				IsMoveMamiya = false;
				setAlphaAnimation();
			}
			if (tenderDic[SweetsType.Both] && setIndex == 1)
			{
				IsMoveAll = false;
				setAlphaAnimation();
			}
			if (tenderDic[SweetsType.Irako] && setIndex == 2)
			{
				IsMoveIrako = false;
				setAlphaAnimation();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
		}

		public void CheckMainDialog()
		{
			if (tenderDic[SweetsType.Both])
			{
				setIndex = 1;
			}
			else if (tenderDic[SweetsType.Mamiya])
			{
				setIndex = 0;
			}
			else
			{
				setIndex = 2;
			}
			if (tenderDic[SweetsType.Mamiya])
			{
				_mamiyaBtn.spriteName = ((setIndex != 0) ? "btn_mamiya" : "btn_mamiya_on");
				bool value = (setIndex == 0) ? true : false;
				UISelectedObject.SelectedOneButtonZoomUpDown(_mamiyaBtn.gameObject, value);
			}
			else
			{
				_mamiyaBtn.spriteName = "btn_mamiya_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(_mamiyaBtn.gameObject, value: false);
			}
			if (tenderDic[SweetsType.Both])
			{
				_allBtn.spriteName = ((setIndex != 1) ? "btn_m+i" : "btn_m+i_on");
				bool value2 = (setIndex == 1) ? true : false;
				UISelectedObject.SelectedOneButtonZoomUpDown(_allBtn.gameObject, value2);
			}
			else
			{
				_allBtn.spriteName = "btn_m+i_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(_allBtn.gameObject, value: false);
			}
			if (tenderDic[SweetsType.Irako])
			{
				_irakoBtn.spriteName = ((setIndex != 2) ? "btn_irako" : "btn_irako_on");
				bool value3 = (setIndex == 2) ? true : false;
				UISelectedObject.SelectedOneButtonZoomUpDown(_irakoBtn.gameObject, value3);
			}
			else
			{
				_irakoBtn.spriteName = "btn_irako_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(_irakoBtn.gameObject, value: false);
			}
			if (setIndex == 0)
			{
				MoveToMamiya();
			}
			else if (setIndex == 1)
			{
				MoveToMamiyaAndIrako();
			}
			else if (setIndex == 2)
			{
				MoveToIrako();
			}
		}

		private void MoveToMamiya()
		{
			MoveToStage(_irakoBtn, _mamiyaBtn, _allBtn);
			StopShipAnimate(_allC1.transform, -38f, 0f);
			_allC1.alpha = 1f;
			_allC2.alpha = 0f;
		}

		private void MoveToMamiyaAndIrako()
		{
			MoveToStage(_mamiyaBtn, _allBtn, _irakoBtn);
			StopShipAnimate(_allC1.transform, -38f, 0f);
			StopShipAnimate(_allC2.transform, 75f, 6f);
			_allC1.alpha = 1f;
			_allC2.alpha = 1f;
		}

		private void MoveToIrako()
		{
			MoveToStage(_allBtn, _irakoBtn, _mamiyaBtn);
			StopShipAnimate(_allC2.transform, -38f, 0f);
			_allC1.alpha = 0f;
			_allC2.alpha = 1f;
		}

		private void MoveToStage(UISprite left, UISprite center, UISprite right)
		{
			setButtonAlpha();
			left.transform.localPosition = SWEETS_POSITION_LEFT;
			center.transform.localPosition = SWEETS_POSITION_CENTER;
			right.transform.localPosition = SWEETS_POSITION_RIGHT;
			left.transform.localScale = SWEETS_UNFOCUS_SCALE;
			center.transform.localScale = SWEETS_FOCUS_SCALE;
			right.transform.localScale = SWEETS_UNFOCUS_SCALE;
			UISelectedObject.SelectedOneButtonZoomUpDown(left.gameObject, value: false);
			UISelectedObject.SelectedOneButtonZoomUpDown(center.gameObject, value: true);
		}

		private void setButtonAlpha()
		{
			_mamiyaBtn.alpha = ((!tenderDic[SweetsType.Mamiya]) ? 0.6f : 1f);
			_irakoBtn.alpha = ((!tenderDic[SweetsType.Irako]) ? 0.6f : 1f);
			_allBtn.alpha = ((!tenderDic[SweetsType.Both]) ? 0.6f : 1f);
		}

		private void setUseDialog()
		{
			if (State == TenderState.Maimiya)
			{
				_topLabel.text = "給糧艦「間宮」を使用しますか？";
				_item.spriteName = "icon_mamiya";
				_ship.mainTexture = (Resources.Load("Textures/Organize/sozai/popup2/img_mamiya") as Texture2D);
				_ship.width = 256;
				_ship.height = 512;
				_labelFrom.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount();
				_labelTo.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount() - 1;
			}
			else if (State == TenderState.Irako)
			{
				_topLabel.text = "給糧艦「伊良湖」を使用しますか？";
				_item.spriteName = "icon_irako";
				_ship.mainTexture = (Resources.Load("Textures/Organize/sozai/popup2/img_irako") as Texture2D);
				_ship.width = 256;
				_ship.height = 512;
				_labelFrom.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount();
				_labelTo.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount() - 1;
			}
		}

		private void setTwinUseDialog()
		{
			_labelFrom1.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount();
			_labelTo1.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount() - 1;
			_labelFrom2.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount();
			_labelTo2.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount() - 1;
		}

		public void updateSubBtn()
		{
			if (setIndex2 == 1)
			{
				_btnYes.spriteName = "btn_yes";
				_btnNo.spriteName = "btn_no_on";
				UISelectedObject.SelectedOneButtonZoomUpDown(_btnYes.gameObject, value: false);
				UISelectedObject.SelectedOneButtonZoomUpDown(_btnNo.gameObject, value: true);
			}
			else
			{
				_btnYes.spriteName = "btn_yes_on";
				_btnNo.spriteName = "btn_no";
				UISelectedObject.SelectedOneButtonZoomUpDown(_btnYes.gameObject, value: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(_btnNo.gameObject, value: false);
			}
		}

		public void updateTwinBtn()
		{
			if (setIndex2 == 1)
			{
				_tBtnYes.spriteName = "btn_yes";
				_tBtnNo.spriteName = "btn_no_on";
				UISelectedObject.SelectedOneButtonZoomUpDown(_tBtnYes.gameObject, value: false);
				UISelectedObject.SelectedOneButtonZoomUpDown(_tBtnNo.gameObject, value: true);
			}
			else
			{
				_tBtnYes.spriteName = "btn_yes_on";
				_tBtnNo.spriteName = "btn_no";
				UISelectedObject.SelectedOneButtonZoomUpDown(_tBtnYes.gameObject, value: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(_tBtnNo.gameObject, value: false);
			}
		}

		public void ShowSelectTender()
		{
			((Component)base.transform).GetComponent<UIPanel>().alpha = 1f;
			State = TenderState.Select;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			((Component)base.transform).GetComponent<UIPanel>().widgetsAreStatic = false;
			tenderDic = OrganizeTaskManager.Instance.GetLogicManager().GetAvailableSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID());
			_tenderPanel.transform.localPosition = new Vector3(0f, 0f);
			_mainBg.transform.localPosition = new Vector3(0f, 0f);
			_mainBg.gameObject.SafeGetTweenAlpha(0f, 0.6f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _mainBg.gameObject, string.Empty);
			_tenderPanel.gameObject.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _tenderPanel.gameObject, string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Open(_tenderPanel.gameObject, 0f, 0f, 1f, 1f);
			CheckMainDialog();
		}

		public void Hide()
		{
			_mainBg.gameObject.SafeGetTweenAlpha(0.6f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "compHideAnimation");
			_tenderPanel.gameObject.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _tenderPanel.gameObject, string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Close(_tenderPanel.gameObject, 1f, 1f, 0f, 0f);
			State = TenderState.None;
		}

		private void compHideAnimation()
		{
			if (!isAnimation)
			{
				((Component)base.transform).GetComponent<UIPanel>().alpha = 0f;
			}
		}

		public void ShowUseDialog()
		{
			switch (setIndex)
			{
			case 0:
				ShowOther(TenderState.Maimiya);
				break;
			case 1:
				ShowTwinOther();
				break;
			case 2:
				ShowOther(TenderState.Irako);
				break;
			}
		}

		public void ShowOther(TenderState state)
		{
			_mamiyaPanel.transform.localPosition = Vector3.zero;
			_otherBg.transform.localPosition = Vector3.zero;
			_mamiyaPanel.gameObject.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _mamiyaPanel.gameObject, string.Empty);
			_otherBg.gameObject.SafeGetTweenAlpha(0f, 0.6f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _otherBg.gameObject, string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Open(_mamiyaPanel.gameObject, 0f, 0f, 1f, 1f);
			setIndex2 = 0;
			State = state;
			if (State == TenderState.Maimiya)
			{
				int num = Random.Range(0, 2);
				int voiceNum = -1;
				switch (num)
				{
				case 0:
					voiceNum = 11;
					break;
				case 1:
					voiceNum = 12;
					break;
				}
				ShipUtils.PlayPortVoice(voiceNum);
				setUseDialog();
			}
			else if (State == TenderState.Irako)
			{
				State = TenderState.Irako;
				int num2 = Random.Range(0, 2);
				int voiceNum2 = -1;
				switch (num2)
				{
				case 0:
					voiceNum2 = 13;
					break;
				case 1:
					voiceNum2 = 14;
					break;
				}
				ShipUtils.PlayPortVoice(voiceNum2);
				setUseDialog();
			}
			updateSubBtn();
		}

		public void HideOther()
		{
			_mamiyaPanel.gameObject.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _mamiyaPanel.gameObject, string.Empty);
			_otherBg.gameObject.SafeGetTweenAlpha(0.6f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _otherBg.gameObject, string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Close(_mamiyaPanel.gameObject, 1f, 1f, 0f, 0f);
			State = TenderState.Select;
		}

		public void ShowTwinOther()
		{
			_twinPanel.transform.localPosition = Vector3.zero;
			_otherBg.transform.localPosition = Vector3.zero;
			_twinPanel.gameObject.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _twinPanel.gameObject, string.Empty);
			_otherBg.gameObject.SafeGetTweenAlpha(0f, 0.6f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _otherBg.gameObject, string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Open(_twinPanel.gameObject, 0f, 0f, 1f, 1f);
			int num = Random.Range(0, 2);
			int voiceNum = -1;
			switch (num)
			{
			case 0:
				voiceNum = 15;
				break;
			case 1:
				voiceNum = 16;
				break;
			}
			ShipUtils.PlayPortVoice(voiceNum);
			setIndex2 = 0;
			State = TenderState.Twin;
			setTwinUseDialog();
			updateTwinBtn();
		}

		public void HideTwinOther()
		{
			_twinPanel.gameObject.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _twinPanel.gameObject, string.Empty);
			_otherBg.gameObject.SafeGetTweenAlpha(0.6f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _otherBg.gameObject, string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Close(_twinPanel.gameObject, 1f, 1f, 0f, 0f);
			State = TenderState.Select;
		}

		private IEnumerator _HeadButtonEnable(float time = 4.5f)
		{
			yield return new WaitForSeconds(time);
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
			}
		}

		public void PlayAnimation()
		{
			isAnimation = true;
			State = TenderState.None;
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
			}
			StartCoroutine(_HeadButtonEnable());
			if (setIndex == 0)
			{
				int num = Random.Range(0, 2);
				int voiceNum = -1;
				switch (num)
				{
				case 0:
					voiceNum = 21;
					break;
				case 1:
					voiceNum = 22;
					break;
				}
				ShipUtils.PlayPortVoice(voiceNum);
				OrganizeTaskManager.Instance.GetLogicManager().UseSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), SweetsType.Mamiya);
			}
			else if (setIndex == 1)
			{
				int num2 = Random.Range(0, 2);
				int voiceNum2 = -1;
				switch (num2)
				{
				case 0:
					voiceNum2 = 25;
					break;
				case 1:
					voiceNum2 = 26;
					break;
				}
				ShipUtils.PlayPortVoice(voiceNum2);
				OrganizeTaskManager.Instance.GetLogicManager().UseSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), SweetsType.Both);
			}
			else if (setIndex == 2)
			{
				int num3 = Random.Range(0, 2);
				int voiceNum3 = -1;
				switch (num3)
				{
				case 0:
					voiceNum3 = 23;
					break;
				case 1:
					voiceNum3 = 24;
					break;
				}
				ShipUtils.PlayPortVoice(voiceNum3);
				OrganizeTaskManager.Instance.GetLogicManager().UseSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), SweetsType.Irako);
			}
			SetAnimationPanel();
			_animation.Play();
			_parSystem.Play();
		}

		public void SetAnimationPanel()
		{
			if (setIndex == 0)
			{
				_ship1.transform.localPosition = Vector3.zero;
				_ship1.alpha = 1f;
				_ship2.alpha = 0f;
			}
			else if (setIndex == 1)
			{
				_ship1.transform.localPosition = new Vector3(-180f, 0f);
				_ship2.transform.localPosition = new Vector3(215f, 0f);
				_ship1.alpha = 1f;
				_ship2.alpha = 1f;
			}
			else if (setIndex == 2)
			{
				_ship2.transform.localPosition = Vector3.zero;
				_ship1.alpha = 0f;
				_ship2.alpha = 1f;
			}
		}

		public void CompDialogClose()
		{
			HideOther();
			HideTwinOther();
			Hide();
		}

		public void CompUpdateBanner()
		{
			OrganizeTaskManager.Instance.GetTopTask().UpdateChangeFatigue();
		}

		public void CompParticle()
		{
			((Component)_parSystem2).gameObject.SetActive(true);
			_parSystem2.Play();
		}

		public void EndAnimation()
		{
			isAnimation = false;
			_animatePanel.transform.localPosition = new Vector3(-1065f, 620f);
			OrganizeTaskManager.Instance.GetTopTask().isControl = true;
			OrganizeTaskManager.Instance.GetTopTask().UpdateDeckSwitchManager();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			((Component)base.transform).GetComponent<UIPanel>().alpha = 0f;
		}

		public void AllUpAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 2f);
			hashtable.Add("y", 20f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "AllDownAnimate");
			hashtable.Add("oncompletetarget", base.gameObject);
			_allC1.transform.gameObject.MoveTo(hashtable);
			_allC2.transform.gameObject.MoveTo(hashtable);
		}

		public void AllDownAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 2f);
			hashtable.Add("y", -10f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "AllUpAnimate");
			hashtable.Add("oncompletetarget", base.gameObject);
			_allC1.transform.gameObject.MoveTo(hashtable);
			_allC2.transform.gameObject.MoveTo(hashtable);
		}

		public void StopAllAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.05f);
			hashtable.Add("x", -38f);
			hashtable.Add("y", 0f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			_allC1.transform.gameObject.MoveTo(hashtable);
			Hashtable hashtable2 = new Hashtable();
			hashtable2.Add("time", 0.05f);
			hashtable2.Add("x", 76f);
			hashtable2.Add("y", 0f);
			hashtable2.Add("easeType", iTween.EaseType.linear);
			hashtable2.Add("isLocal", true);
			_allC2.transform.gameObject.MoveTo(hashtable2);
		}

		public void AlphaInAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 2f);
			hashtable.Add("y", 20f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "AllDownAnimate");
			hashtable.Add("oncompletetarget", base.gameObject);
			_allC1.transform.gameObject.MoveTo(hashtable);
			_allC2.transform.gameObject.MoveTo(hashtable);
		}

		public void AlphaAnimate(Transform trans, float from, float to)
		{
			trans.SafeGetTweenAlpha(from, to, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
		}

		public void moveAnimate(UISprite sprite, float _toX, float to_y, bool isSet)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.3f);
			hashtable.Add("x", _toX);
			hashtable.Add("y", to_y);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", 0.1f);
			hashtable.Add("oncomplete", "compMoveAnimation");
			hashtable.Add("oncompletetarget", base.gameObject);
			sprite.transform.MoveTo(hashtable);
			Hashtable hashtable2 = new Hashtable();
			hashtable2.Add("time", 0.3f);
			if (isSet)
			{
				hashtable2.Add("scale", new Vector3(1f, 1f, 1f));
				AlphaAnimate(sprite.transform, 0.6f, 1f);
			}
			else
			{
				hashtable2.Add("scale", new Vector3(0.7f, 0.7f, 0.7f));
			}
			hashtable2.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable2.Add("isLocal", true);
			sprite.transform.ScaleTo(hashtable2);
		}

		public void compMoveAnimation()
		{
			TaskOrganizeTop.KeyController.IsRun = true;
		}

		private void setAlphaAnimation()
		{
			_allC1.alpha = 0f;
			_allC2.alpha = 0f;
			if (setIndex == 0)
			{
				StopShipAnimate(_allC1.transform, 0f, 0f);
				AlphaAnimate(_allC1.transform, 0f, 1f);
				AlphaAnimate(_allC2.transform, 0f, 0f);
			}
			else if (setIndex == 1)
			{
				StopShipAnimate(_allC1.transform, -38f, 0f);
				StopShipAnimate(_allC2.transform, 75f, 6f);
				AlphaAnimate(_allC1.transform, 0f, 1f);
				AlphaAnimate(_allC2.transform, 0f, 1f);
			}
			else if (setIndex == 2)
			{
				StopShipAnimate(_allC2.transform, 0f, 0f);
				AlphaAnimate(_allC2.transform, 0f, 1f);
				AlphaAnimate(_allC1.transform, 0f, 0f);
			}
		}

		public void StopShipAnimate(Transform trans, float x, float y)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.02f);
			hashtable.Add("x", x);
			hashtable.Add("y", y);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			trans.MoveTo(hashtable);
		}

		public void AllBtnEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && setIndex == 1)
			{
				ShowTwinOther();
			}
		}

		public void IrakoBtnEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && setIndex == 2)
			{
				ShowOther(TenderState.Irako);
			}
		}

		public void MamiyaBtnEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && setIndex == 0)
			{
				ShowOther(TenderState.Maimiya);
			}
		}

		public void MainBackEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && TaskOrganizeTop.controlState != "system")
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				Hide();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
			}
		}

		public void OtherBackEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				if (State == TenderState.Maimiya || State == TenderState.Irako)
				{
					HideOther();
				}
				else if (State == TenderState.Twin)
				{
					HideTwinOther();
				}
			}
		}

		public void BtnYesEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled())
			{
				_GuideOff = true;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				PlayAnimation();
				StartCoroutine(GuideResume());
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
				OrganizeTaskManager.Instance.GetTopTask().isControl = false;
				SoundUtils.PlaySE(SEFIleInfos.SE_004);
			}
		}

		private IEnumerator GuideResume()
		{
			yield return new WaitForSeconds(6f);
			_GuideOff = false;
		}

		public void BtnNoEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled())
			{
				OtherBackEL(null);
			}
		}
	}
}
