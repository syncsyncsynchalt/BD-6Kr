using Common.Enum;
using LT.Tweening;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UIAreaMapFrame : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiMessageLabel;

		[SerializeField]
		private Transform _traCompass;

		[SerializeField]
		private Transform _traMessageBox;

		[SerializeField]
		private UISprite _uiInputIcon;

		private float _fShowAnimationTime = 0.7f;

		private float _fHideAnimationTime = 0.7f;

		private UIPanel _uiPanel;

		private Vector3 _vDefaultMessagePos;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		private void Awake()
		{
			_uiInputIcon.alpha = 0f;
			((Component)_traCompass).GetComponent<UIWidget>().alpha = 0f;
			((Component)_traMessageBox).GetComponent<UIWidget>().alpha = 0f;
			_vDefaultMessagePos = _uiMessageLabel.transform.localPosition;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiMessageLabel);
			Mem.Del(ref _traCompass);
			Mem.Del(ref _traMessageBox);
			Mem.Del(ref _uiInputIcon);
			Mem.Del(ref _fShowAnimationTime);
			Mem.Del(ref _fHideAnimationTime);
			Mem.Del(ref _uiPanel);
		}

		public void SetMessage(string message)
		{
			_uiMessageLabel.text = message;
			_uiInputIcon.alpha = ((!message.Equals("艦隊の針路を選択できます。\n提督、どちらの針路を選択しますか？")) ? 0f : 1f);
			_uiMessageLabel.transform.localPosition = ((!message.Equals("陣形を選択してください。")) ? _vDefaultMessagePos : new Vector3(60f, 35f, 0f));
		}

		public void SetMessage(enumMapEventType iType, enumMapWarType iWarType)
		{
			string message = string.Empty;
			if (iType == enumMapEventType.Stupid)
			{
				switch (iWarType)
				{
				case enumMapWarType.None:
					message = "気のせいだった。";
					break;
				case enumMapWarType.Normal:
					message = "敵影を見ず。";
					break;
				default:
					message = string.Empty;
					break;
				}
			}
			SetMessage(message);
		}

		public void SetMessage(MapAirReconnaissanceKind iKind)
		{
			string message = string.Empty;
			switch (iKind)
			{
			case MapAirReconnaissanceKind.Impossible:
				message = "航空偵察予定地点に到着しましたが、\n稼働偵察機がないため、偵察を中止します。";
				break;
			case MapAirReconnaissanceKind.LargePlane:
				message = "大型飛行艇による\n航空偵察を実施します。";
				break;
			case MapAirReconnaissanceKind.WarterPlane:
				message = "水上偵察機による\n航空偵察を実施します。";
				break;
			}
			SetMessage(message);
		}

		public void ClearMessage()
		{
			_uiMessageLabel.text = string.Empty;
			_uiInputIcon.alpha = 0f;
		}

		public LTDescr Show()
		{
			_traMessageBox.LTMoveLocalY(0f, _fShowAnimationTime).setEase(LeanTweenType.easeOutQuad);
			_traCompass.LTMoveLocalX(-410f, _fShowAnimationTime).setEase(LeanTweenType.easeOutQuad);
			_traCompass.LTRotate(new Vector3(0f, 0f, -45f), _fShowAnimationTime).setEase(LeanTweenType.easeOutQuad);
			UIWidget compass = ((Component)_traCompass).GetComponent<UIWidget>();
			UIWidget messageBox = ((Component)_traMessageBox).GetComponent<UIWidget>();
			return base.transform.LTValue(0f, 1f, _fShowAnimationTime).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				UIWidget uIWidget = compass;
				messageBox.alpha = x;
				uIWidget.alpha = x;
			});
		}

		public LTDescr Hide()
		{
			_traMessageBox.LTMoveLocalY(-322f, _fHideAnimationTime).setEase(LeanTweenType.easeOutQuad);
			_traCompass.LTMoveLocalX(-570f, _fHideAnimationTime).setEase(LeanTweenType.easeOutQuad);
			_traCompass.LTRotateLocal(Vector3.zero, _fHideAnimationTime).setEase(LeanTweenType.easeOutQuad);
			UIWidget compass = ((Component)_traCompass).GetComponent<UIWidget>();
			UIWidget messageBox = ((Component)_traMessageBox).GetComponent<UIWidget>();
			return _traCompass.LTValue(1f, 0f, _fHideAnimationTime).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				UIWidget uIWidget = compass;
				messageBox.alpha = x;
				uIWidget.alpha = x;
			});
		}
	}
}
