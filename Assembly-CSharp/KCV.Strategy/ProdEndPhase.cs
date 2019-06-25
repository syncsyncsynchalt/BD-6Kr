using Common.Enum;
using KCV.Utils;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class ProdEndPhase : MonoBehaviour
	{
		[SerializeField]
		private GameObject _crearTxtObj;

		[SerializeField]
		private UITexture _gameoverTxt1;

		[SerializeField]
		private UITexture _gameoverTxt2;

		[SerializeField]
		private UILabel _historyLabel;

		[SerializeField]
		private ParticleSystem _petalPar;

		[SerializeField]
		private Animation _animation;

		private bool _isAnimation;

		private bool _isHistoryAnime;

		private bool _isClear;

		private bool _isTurnOver;

		private Action _callback;

		private KeyControl _keyController;

		private List<UILabel> _lstHistoryLabel;

		private bool _init()
		{
			if (_crearTxtObj == null)
			{
				_crearTxtObj = base.transform.FindChild("ClearText").gameObject;
			}
			Util.FindParentToChild(ref _gameoverTxt1, base.transform, "GameOver1");
			Util.FindParentToChild(ref _gameoverTxt2, _gameoverTxt1.transform, "GameOver2");
			Util.FindParentToChild(ref _historyLabel, base.transform, "HistoryLabel");
			Util.FindParentToChild<ParticleSystem>(ref _petalPar, base.transform, "Petal");
			if ((UnityEngine.Object)_animation == null)
			{
				_animation = GetComponent<Animation>();
			}
			_crearTxtObj.SetActive(false);
			_gameoverTxt1.SetActive(isActive: false);
			_animation.Stop();
			_petalPar.Stop();
			_isAnimation = false;
			_isHistoryAnime = false;
			_isClear = false;
			_isTurnOver = false;
			_callback = null;
			_lstHistoryLabel = new List<UILabel>();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _crearTxtObj);
			Mem.Del(ref _historyLabel);
			Mem.Del(ref _petalPar);
			Mem.Del(ref _animation);
			if (_lstHistoryLabel != null)
			{
				for (int i = 0; i < _lstHistoryLabel.Count; i++)
				{
					UnityEngine.Object.Destroy(_lstHistoryLabel[i].gameObject);
				}
				_lstHistoryLabel.Clear();
			}
			_lstHistoryLabel = null;
			_keyController = null;
			App.TimeScale(1f);
		}

		private void Update()
		{
			if (!_isAnimation || _keyController == null)
			{
				return;
			}
			_keyController.Update();
			if (_isHistoryAnime)
			{
				if (_keyController.GetDown(KeyControl.KeyName.MARU))
				{
					App.TimeScale(3f);
				}
				else if (_keyController.GetUp(KeyControl.KeyName.MARU))
				{
					App.TimeScale(1f);
				}
				else if (!_keyController.GetDown(KeyControl.KeyName.BATU))
				{
				}
			}
		}

		public void Play(Action callback)
		{
			_isAnimation = true;
			_callback = callback;
			_setEndTxt();
			_createHistoryLabel();
			_animation.Stop();
			_animation.Play((!_isClear) ? "EndStartGO" : "EndStart");
			if (_isClear)
			{
				SoundUtils.PlaySE(SEFIleInfos.ClearAAA, delegate
				{
					SoundUtils.PlayBGM((BGMFileInfos)211, isLoop: true);
				});
			}
		}

		private void _setEndTxt()
		{
			if (_isClear)
			{
				_crearTxtObj.SetActive(true);
			}
			else
			{
				_gameoverTxt1.SetActive(isActive: true);
			}
		}

		private void _createHistoryLabel()
		{
			if (Server_Common.Utils.IsGameOver())
			{
				if (_isTurnOver)
				{
					_gameoverTxt1.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverB1") as Texture2D);
					_gameoverTxt2.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverB2") as Texture2D);
				}
				else
				{
					_gameoverTxt1.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverA1") as Texture2D);
					_gameoverTxt2.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverA2") as Texture2D);
				}
				_gameoverTxt1.MakePixelPerfect();
				_gameoverTxt2.MakePixelPerfect();
			}
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_get_Member().HistoryList();
			for (int i = 0; i < api_Result.data.Count; i++)
			{
				User_HistoryFmt user_HistoryFmt = api_Result.data[i];
				if (user_HistoryFmt != null)
				{
					UILabel uILabel = UnityEngine.Object.Instantiate(_historyLabel);
					uILabel.transform.parent = _historyLabel.transform.parent;
					uILabel.transform.localPosition = _historyLabel.transform.localPosition;
					uILabel.transform.localScale = Vector3.one;
					uILabel.text = _setHistoryType(user_HistoryFmt);
					uILabel.MakePixelPerfect();
					_lstHistoryLabel.Add(uILabel);
					if (user_HistoryFmt.Type == HistoryType.GameOverLost)
					{
						UILabel uILabel2 = UnityEngine.Object.Instantiate(_historyLabel);
						uILabel2.transform.parent = _historyLabel.transform.parent;
						uILabel2.transform.localPosition = _historyLabel.transform.localPosition;
						uILabel2.transform.localScale = Vector3.one;
						uILabel2.text = "\u3000\u3000\u3000\u3000\u3000\u3000 敗戦";
						uILabel2.MakePixelPerfect();
						_lstHistoryLabel.Add(uILabel2);
					}
				}
			}
		}

		private string _setHistoryType(User_HistoryFmt fmt)
		{
			string result = string.Empty;
			string empty = string.Empty;
			switch (fmt.Type)
			{
			case HistoryType.MapClear1:
				result = _setHistory(string.Empty, fmt.DateString.Year, fmt.DateString.Month, fmt.DateString.Day, fmt.MapInfo.Name, fmt.MapInfo.Opetext, fmt.FlagShip.Name);
				break;
			case HistoryType.MapClear2:
				result = _setHistory("第二次", fmt.DateString.Year, fmt.DateString.Month, fmt.DateString.Day, fmt.MapInfo.Name, fmt.MapInfo.Opetext, fmt.FlagShip.Name);
				break;
			case HistoryType.MapClear3:
				result = _setHistory("第三次", fmt.DateString.Year, fmt.DateString.Month, fmt.DateString.Day, fmt.MapInfo.Name, fmt.MapInfo.Opetext, fmt.FlagShip.Name);
				break;
			case HistoryType.TankerLostHalf:
			{
				string str = _setHistoryDate(fmt);
				result = str + fmt.AreaName + " 輸送船団遭難";
				break;
			}
			case HistoryType.TankerLostAll:
			{
				string str = _setHistoryDate(fmt);
				result = str + fmt.AreaName + " 輸送船団全滅";
				break;
			}
			case HistoryType.NewAreaOpen:
			{
				string str = _setHistoryDate(fmt);
				result = str + fmt.AreaName + " 攻略開始";
				break;
			}
			case HistoryType.GameOverLost:
			{
				string str = _setHistoryDate(fmt);
				result = str + "本土沖海戦 敗北（艦隊壊滅）";
				break;
			}
			case HistoryType.GameOverTurn:
			{
				string str = _setHistoryDate(fmt);
				result = str + "終戦";
				break;
			}
			case HistoryType.GameClear:
			{
				string str = _setHistoryDate(fmt);
				result = str + "勝利";
				break;
			}
			}
			return result;
		}

		private string _setHistory(string num, string year, string Month, string day, string areaName, string MapInfo, string shipName)
		{
			return year + "年" + Month + day + "日 " + areaName + " " + num + MapInfo + " 旗艦 " + shipName;
		}

		private string _setHistoryDate(User_HistoryFmt fmt)
		{
			return fmt.DateString.Year + "年" + fmt.DateString.Month + string.Empty + fmt.DateString.Day + "日 ";
		}

		private void _setHistoryMove(Transform trans, float deray, bool last)
		{
			float num = 300f;
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = hashtable;
			Vector3 localPosition = trans.localPosition;
			hashtable2.Add("x", localPosition.x);
			hashtable.Add("y", num);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", deray);
			hashtable.Add("time", 7f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", (!last) ? string.Empty : "_onCompHistoryTxt");
			hashtable.Add("oncompletetarget", base.gameObject);
			trans.MoveTo(hashtable);
		}

		private void _compEndStart()
		{
			_compDecisionTxt();
		}

		private void _compDecisionTxt()
		{
			_animation.Stop();
			_animation.Play("EndTextMove");
		}

		private void _compEndTextMove()
		{
			if (_isClear)
			{
				_petalPar.Play();
			}
			float num = 270f + (float)_historyLabel.height + 10f;
			_isHistoryAnime = true;
			if (_lstHistoryLabel.Count == 0)
			{
				_onCompHistoryTxt();
			}
			for (int i = 0; i < _lstHistoryLabel.Count; i++)
			{
				_setHistoryMove(_lstHistoryLabel[i].transform, 0.8f * (float)i, (i >= _lstHistoryLabel.Count - 1) ? true : false);
			}
		}

		private void _onCompHistoryTxt()
		{
			if (_isClear)
			{
				_petalPar.Stop();
			}
			_animation.Stop();
			_animation.Play("EndPhaseEnd");
			SoundUtils.StopFadeBGM(2f, null);
		}

		private void _onCompFadeOut()
		{
			if (_callback != null)
			{
				_callback();
			}
		}

		public static ProdEndPhase Instantiate(ProdEndPhase prefab, Transform parent, KeyControl keyController, bool clear, bool isTurnOver)
		{
			ProdEndPhase prodEndPhase = UnityEngine.Object.Instantiate(prefab);
			prodEndPhase.transform.parent = parent;
			prodEndPhase.transform.localScale = Vector3.one;
			prodEndPhase.transform.localPosition = Vector3.zero;
			prodEndPhase._init();
			prodEndPhase._keyController = keyController;
			prodEndPhase._isClear = clear;
			prodEndPhase._isTurnOver = isTurnOver;
			return prodEndPhase;
		}
	}
}
