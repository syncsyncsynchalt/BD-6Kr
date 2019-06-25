using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlStarterSelect : MonoBehaviour
	{
		public enum StarterType
		{
			Ex,
			Normal
		}

		[SerializeField]
		private List<UIButton> _listStarterBtn;

		private Action<StarterType> _actOnSelectStarter;

		private Action _actOnCancel;

		private StarterType _iSelectType;

		private List<List<Texture2D>> _listStarterTexture;

		public StarterType selectType => _iSelectType;

		public static CtrlStarterSelect Instantiate(CtrlStarterSelect prefab, Transform parent)
		{
			return UnityEngine.Object.Instantiate(prefab);
		}

		private void Awake()
		{
			_listStarterTexture = new List<List<Texture2D>>();
			_listStarterTexture.Add(new List<Texture2D>(2)
			{
				null,
				null
			});
			_listStarterTexture.Add(new List<Texture2D>(2)
			{
				null,
				null
			});
			StarterType iType = StarterType.Ex;
			_listStarterBtn.ForEach(delegate(UIButton x)
			{
				x.onClick = Util.CreateEventDelegateList(this, "OnClickStarter", iType);
				iType++;
			});
			_iSelectType = StarterType.Ex;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe(ref _listStarterBtn);
			Mem.Del(ref _actOnSelectStarter);
			Mem.Del(ref _iSelectType);
			Mem.DelListSafe(ref _listStarterTexture);
			base.transform.GetComponentsInChildren<UIWidget>().ForEach(delegate(UIWidget x)
			{
				if (x is UISprite)
				{
					((UISprite)x).Clear();
				}
				Mem.Del(ref x);
			});
		}

		public bool Init(Action<StarterType> onSelectStarter, Action onCancel)
		{
			UIStartupNavigation navigation = StartupTaskManager.GetNavigation();
			navigation.SetNavigationInStarterSelect();
			base.transform.localScaleOne();
			_actOnSelectStarter = onSelectStarter;
			_actOnCancel = onCancel;
			_iSelectType = StarterType.Ex;
			ChangeFocus(_iSelectType);
			return true;
		}

		public void PreparaNext(bool isFoward)
		{
			StarterType iSelectType = _iSelectType;
			_iSelectType = (StarterType)Mathe.NextElement((int)_iSelectType, 0, 1, isFoward);
			if (iSelectType != _iSelectType)
			{
				ChangeFocus(_iSelectType);
			}
		}

		private void ChangeFocus(StarterType iType)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			bool flag = (iType == StarterType.Ex) ? true : false;
			ChangeStarterTexture(iType);
			_listStarterBtn[(int)_iSelectType].GetComponent<UITexture>().depth = 1;
			_listStarterBtn[(int)(1 - _iSelectType)].GetComponent<UITexture>().depth = 0;
			UISelectedObject.SelectedOneBoardZoomUpDownStartup(_listStarterBtn[0].gameObject, flag);
			UISelectedObject.SelectedOneBoardZoomUpDownStartup(_listStarterBtn[1].gameObject, !flag);
		}

		private void ChangeStarterTexture(StarterType iType)
		{
			if ((iType == StarterType.Ex) ? true : false)
			{
				if (_listStarterTexture[0][1] == null)
				{
					_listStarterTexture[0][1] = Resources.Load<Texture2D>("Textures/Startup/Starter/starter1_on");
				}
				if (_listStarterTexture[1][0] == null)
				{
					_listStarterTexture[1][0] = Resources.Load<Texture2D>("Textures/Startup/Starter/starter2");
				}
				_listStarterBtn[0].GetComponent<UITexture>().mainTexture = _listStarterTexture[0][1];
				_listStarterBtn[1].GetComponent<UITexture>().mainTexture = _listStarterTexture[1][0];
			}
			else
			{
				if (_listStarterTexture[0][0] == null)
				{
					_listStarterTexture[0][0] = Resources.Load<Texture2D>("Textures/Startup/Starter/starter1");
				}
				if (_listStarterTexture[1][1] == null)
				{
					_listStarterTexture[1][1] = Resources.Load<Texture2D>("Textures/Startup/Starter/starter2_on");
				}
				_listStarterBtn[0].GetComponent<UITexture>().mainTexture = _listStarterTexture[0][0];
				_listStarterBtn[1].GetComponent<UITexture>().mainTexture = _listStarterTexture[1][1];
			}
		}

		public void OnClickStarter(StarterType iType)
		{
			if (_iSelectType == iType)
			{
				_listStarterBtn.ForEach(delegate(UIButton x)
				{
					UISelectedObject.SelectedOneBoardZoomUpDownStartup(x.gameObject, value: false);
				});
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				base.transform.localScaleZero();
				Dlg.Call(ref _actOnSelectStarter, iType);
			}
			else
			{
				_iSelectType = iType;
				ChangeFocus(iType);
			}
		}

		public void OnCancel()
		{
			Dlg.Call(ref _actOnCancel);
			base.transform.localScaleZero();
		}
	}
}
