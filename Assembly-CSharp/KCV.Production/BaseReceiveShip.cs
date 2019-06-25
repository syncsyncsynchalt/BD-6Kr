using local.models;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KCV.Production
{
	public class BaseReceiveShip : MonoBehaviour
	{
		protected enum CharType
		{
			Sigle,
			Any
		}

		[SerializeField]
		protected UITexture _uiBg;

		[SerializeField]
		protected UITexture _uiShip;

		[SerializeField]
		protected UILabel _clsShipName;

		[SerializeField]
		protected UILabel _clsSType;

		[SerializeField]
		protected Animation _getIconAnim;

		[SerializeField]
		protected UISprite _uiGear;

		[SerializeField]
		protected Animation _anim;

		[SerializeField]
		protected Animation _gearAnim;

		protected Generics.Message _clsShipMessage;

		protected AudioSource _Se;

		protected int debugIndex;

		protected IReward_Ship _clsRewardShip;

		protected Action _actCallback;

		protected bool _isFinished;

		protected bool _isInput;

		protected KeyControl _clsInput;

		protected virtual void init()
		{
			Util.FindParentToChild(ref _uiBg, base.transform, "BG");
			Util.FindParentToChild(ref _uiShip, base.transform, "ShipLayoutOffset/Ship");
			Util.FindParentToChild(ref _clsShipName, base.transform, "MessageWindow/ShipName");
			Util.FindParentToChild(ref _clsSType, base.transform, "MessageWindow/ShipType");
			Util.FindParentToChild<Animation>(ref _getIconAnim, base.transform, "MessageWindow/Get");
			Util.FindParentToChild(ref _uiGear, base.transform, "MessageWindow/NextBtn");
			if ((UnityEngine.Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
			if ((UnityEngine.Object)_gearAnim == null)
			{
				_gearAnim = _uiGear.GetComponent<Animation>();
			}
			_clsShipMessage = new Generics.Message(base.transform, "MessageWindow/ShipMessage");
			_uiShip.alpha = 0f;
			UIButtonMessage component = _uiGear.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "prodReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = _uiBg.GetComponent<UIButtonMessage>();
			component2.target = base.gameObject;
			component2.functionName = "backgroundEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			_isInput = false;
			_isFinished = false;
			debugIndex = 0;
			_uiGear.GetComponent<Collider2D>().enabled = false;
		}

		protected virtual void OnDestroy()
		{
			_uiShip = null;
			Mem.Del(ref _uiBg);
			Mem.Del(ref _uiShip);
			Mem.Del(ref _clsShipName);
			Mem.Del(ref _clsSType);
			Mem.Del(ref _getIconAnim);
			Mem.Del(ref _uiGear);
			Mem.Del(ref _anim);
			Mem.Del(ref _gearAnim);
			if (_clsShipMessage != null)
			{
				_clsShipMessage.UnInit();
			}
			Mem.Del(ref _clsShipMessage);
			Mem.Del(ref _clsRewardShip);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsInput);
			Mem.Del(ref _Se);
		}

		protected bool Run()
		{
			_clsShipMessage.Update();
			if (_isInput)
			{
			}
			return false;
		}

		protected void _setRewardShip()
		{
			_uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_clsRewardShip.Ship.GetGraphicsMstId(), 9);
			_uiShip.MakePixelPerfect();
			_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_clsRewardShip.Ship.GetGraphicsMstId()).GetShipDisplayCenter(damaged: false));
			_clsShipMessage.Init(_clsRewardShip.GreetingText, 0.08f);
			_clsShipName.text = _clsRewardShip.Ship.Name;
			_clsSType.text = _clsRewardShip.Ship.ShipTypeName;
			_clsShipName.SetActive(isActive: false);
			_clsSType.SetActive(isActive: false);
			((Component)_getIconAnim).gameObject.SetActive(false);
		}

		protected void _debugRewardShip()
		{
			Debug.Log("ShipID:" + debugIndex);
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(debugIndex))
			{
				IReward_Ship reward_Ship = new Reward_Ship(debugIndex);
				_uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(reward_Ship.Ship.GetGraphicsMstId(), 9);
				_uiShip.MakePixelPerfect();
				_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(reward_Ship.Ship.GetGraphicsMstId()).GetShipDisplayCenter(damaged: false));
				NormalizeDescription(26, 1, reward_Ship.GreetingText);
				_clsShipMessage.Init(reward_Ship.GreetingText, 0.01f);
				_clsShipName.text = reward_Ship.Ship.Name;
				_clsSType.text = reward_Ship.Ship.ShipTypeName;
				_clsShipName.SetActive(isActive: false);
				_clsSType.SetActive(isActive: false);
				((Component)_getIconAnim).gameObject.SetActive(false);
				_uiBg.mainTexture = TextureFile.LoadRareBG(reward_Ship.Ship.Rare);
				Debug.Log(reward_Ship.GreetingText);
			}
			_anim.Stop();
			_anim.Play("comp_GetShip");
		}

		private string NormalizeDescription(int maxLineInFullWidthChar, int fullWidthCharBuffer, string targetText)
		{
			int num = maxLineInFullWidthChar * 2;
			string text = "、。！？」』)";
			string text2 = targetText.Replace("\r\n", "\n");
			text2 = text2.Replace("\\n", "\n");
			text2 = text2.Replace("<br>", "\n");
			string[] array = text2.Split('\n');
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = 0;
				string text3 = array[i];
				StringBuilder stringBuilder = new StringBuilder();
				string text4 = text3;
				foreach (char c in text4)
				{
					int num3 = 0;
					switch (GetCharType(c))
					{
					case CharType.Sigle:
						num3 = 1;
						break;
					case CharType.Any:
						num3 = 2;
						break;
					}
					if (num2 + num3 <= num)
					{
						stringBuilder.Append(c);
						num2 += num3;
						continue;
					}
					string item = stringBuilder.ToString();
					list.Add(item);
					stringBuilder.Length = 0;
					stringBuilder.Append(c);
					num2 = num3;
				}
				if (0 < stringBuilder.Length)
				{
					list.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int k = 0; k < list.Count; k++)
			{
				if (k == 0)
				{
					stringBuilder2.Append(list[k]);
				}
				else if (-1 < text.IndexOf(list[k][0]))
				{
					string text5 = list[k];
					string value = text5.Substring(0, 1);
					stringBuilder2.Append(value);
					if (1 < text5.Length)
					{
						stringBuilder2.Append('\n');
						string value2 = text5.Substring(1);
						stringBuilder2.Append(value2);
					}
				}
				else
				{
					stringBuilder2.Append('\n');
					stringBuilder2.Append(list[k]);
				}
			}
			return stringBuilder2.ToString();
		}

		private CharType GetCharType(char character)
		{
			int result = -1;
			if (int.TryParse(character.ToString(), out result))
			{
				return CharType.Any;
			}
			Encoding encoding = new UTF8Encoding();
			int byteCount = encoding.GetByteCount(character.ToString());
			return (byteCount != 1) ? CharType.Any : CharType.Sigle;
		}

		protected void Discard()
		{
			_uiShip = null;
			UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		}
	}
}
