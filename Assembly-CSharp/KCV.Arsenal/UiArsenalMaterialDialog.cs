using KCV.Utils;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalMaterialDialog : MonoBehaviour
	{
		private const int MAX_FRAME_COUNT = 4;

		[SerializeField]
		private UITexture[] _uiSelect;

		private int moveSlotIndex;

		private int[] materialPow;

		private TweenPosition _tp;

		[SerializeField]
		public GameObject[] _uiMaterialFrame;

		public int _frameIndex;

		public void init(int number)
		{
			_uiSelect = new UITexture[4];
			_uiMaterialFrame = new GameObject[4];
			for (int i = 0; i < 4; i++)
			{
				_uiMaterialFrame[i] = base.transform.FindChild("MaterialFrame" + (i + 1)).gameObject;
				Util.FindParentToChild(ref _uiSelect[i], _uiMaterialFrame[i].transform, "Select");
				UISelectedObject.SelectedOneObjectBlinkArsenal(_uiSelect[i].gameObject, value: true);
			}
			materialPow = new int[4];
			materialPow[0] = 1000;
			materialPow[1] = 100;
			materialPow[2] = 10;
			materialPow[3] = 1;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiSelect);
			Mem.Del(ref materialPow);
			Mem.Del(ref _tp);
			Mem.Del(ref _uiMaterialFrame);
		}

		private void setButtonMsg(UIButton obj, GameObject targetObj, string functionName)
		{
			UIButtonMessage component = obj.GetComponent<UIButtonMessage>();
			component.target = targetObj;
			component.functionName = functionName;
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		public void ShowDialog(int materialIndex)
		{
			base.transform.localPosition = Vector3.zero;
			ArsenalTaskManager._clsConstruct.dialogPopUp.Open(base.gameObject, 0f, 0f, 1f, 1f);
			this.SafeGetTweenAlpha(0f, 1f, 0.125f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "compShowDialog");
			UISprite component = ((Component)base.transform.FindChild("Icon")).GetComponent<UISprite>();
			UILabel component2 = ((Component)base.transform.FindChild("Label")).GetComponent<UILabel>();
			component.spriteName = ArsenalTaskManager._clsConstruct._uiMaterialIcon[materialIndex].spriteName;
			component.MakePixelPerfect();
			if (component.spriteName != "icon_item4")
			{
				component.transform.localPosition = Vector3.left * 40f + Vector3.up * 119f;
				component2.transform.localPosition = Vector3.right * 40f + Vector3.up * 119f;
				component2.spacingX = 7;
				if (component.spriteName == "icon_item1")
				{
					component2.text = "燃料";
				}
				else if (component.spriteName == "icon_item2")
				{
					component2.text = "弾薬";
				}
				else if (component.spriteName == "icon_item3")
				{
					component2.text = "鋼材";
				}
			}
			else
			{
				component.transform.localPosition = Vector3.left * 110f + Vector3.up * 119f;
				component2.transform.localPosition = Vector3.right * 40f + Vector3.up * 119f;
				component2.spacingX = -1;
				component2.text = "ボーキサイト";
			}
			UpdateFrameSelect();
			ArsenalTaskManager._clsConstruct.UpdateDialogMaterialCount();
		}

		private void compShowDialog()
		{
		}

		public void ActiveMaterialFrame(bool isBigConstruct)
		{
			_uiMaterialFrame[0].SetActive(isBigConstruct ? true : false);
		}

		public void HidelDialog()
		{
			this.SafeGetTweenAlpha(1f, 0f, 0.125f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
			ArsenalTaskManager._clsConstruct.updateStartBtn();
		}

		public bool SetFrameIndex(bool isLeft, bool isBigConstruct)
		{
			if (isLeft)
			{
				if (_frameIndex < 3)
				{
					_frameIndex++;
					UpdateFrameSelect();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					return true;
				}
			}
			else if (_frameIndex > 0)
			{
				if (_frameIndex == 1 && !isBigConstruct)
				{
					_frameIndex = 1;
				}
				else
				{
					_frameIndex--;
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				UpdateFrameSelect();
				return true;
			}
			return false;
		}

		public int SetMaterialCount()
		{
			return materialPow[_frameIndex];
		}

		public void UpdateFrameSelect()
		{
			for (int i = 0; i < 4; i++)
			{
				if (i == _frameIndex)
				{
					_uiSelect[i].transform.localScale = Vector3.one;
				}
				else
				{
					_uiSelect[i].transform.localScale = Vector3.zero;
				}
			}
		}

		public void MoveMaterialSlot(bool isUp)
		{
			MoveMaterialSlot(isUp, isAnime: false);
		}

		public void MoveMaterialSlot(bool isUp, bool isAnime)
		{
			UIPanel component = ((Component)_uiMaterialFrame[_frameIndex].transform.FindChild("Panel")).GetComponent<UIPanel>();
			moveSlotIndex = _frameIndex;
			float d = (!isUp) ? 85f : (-85f);
			float duration = (!isAnime) ? 0.0625f : 0.01f;
			Transform component2 = ((Component)component.transform.FindChild("LabelGrp")).GetComponent<Transform>();
			TweenPosition tweenPosition = TweenPosition.Begin(component2.transform.gameObject, duration, Vector3.up * d);
			tweenPosition.animationCurve = UtilCurves.TweenEaseInOutQuad;
			tweenPosition.AddOnFinished(CompMoveMaterialSlot);
		}

		public void CompMoveMaterialSlot()
		{
			ArsenalTaskManager._clsConstruct.CompMoveMaterialSlot();
			UIPanel component = ((Component)_uiMaterialFrame[moveSlotIndex].transform.FindChild("Panel")).GetComponent<UIPanel>();
			((Component)component.transform.FindChild("LabelGrp")).GetComponent<Transform>().localPosition = Vector3.zero;
			for (int i = 0; i < 5; i++)
			{
				UILabel component2 = ((Component)component.transform.FindChild("LabelGrp/Label" + (i + 1))).GetComponent<UILabel>();
				Vector3 right = Vector3.right;
				Vector3 localPosition = component2.transform.localPosition;
				Vector3 localPosition2 = right * localPosition.x + Vector3.up * (170f - 85f * (float)i);
				component2.transform.localPosition = localPosition2;
			}
		}

		public void MoveMaterialCount(int setMaterial, int index, int nowMaterial)
		{
			UIPanel component = ((Component)_uiMaterialFrame[index].transform.FindChild("Panel")).GetComponent<UIPanel>();
			int num = setMaterial;
			for (int i = 0; i < 3 - index; i++)
			{
				num /= 10;
			}
			num %= 10;
			for (int j = 0; j < 5; j++)
			{
				UILabel component2 = ((Component)component.transform.FindChild("LabelGrp/Label" + (j + 1))).GetComponent<UILabel>();
				int num2 = num + (2 - j);
				if (num2 > 9)
				{
					num2 -= 10;
				}
				if (num2 < 0)
				{
					num2 += 10;
				}
				component2.textInt = num2;
			}
		}
	}
}
