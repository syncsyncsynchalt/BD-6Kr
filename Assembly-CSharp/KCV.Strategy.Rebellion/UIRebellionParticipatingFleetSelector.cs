using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionParticipatingFleetSelector : MonoBehaviour
	{
		[Serializable]
		private class SortieStartBtn : IRebellionOrganizeSelectObject
		{
			[SerializeField]
			private UIButton _uiButton;

			[SerializeField]
			private UIToggle _uiToggle;

			public int index
			{
				get;
				private set;
			}

			public UIButton button
			{
				get
				{
					return _uiButton;
				}
				private set
				{
					_uiButton = value;
				}
			}

			public UIToggle toggle
			{
				get
				{
					return _uiToggle;
				}
				private set
				{
					_uiToggle = value;
				}
			}

			public DelDicideRebellionOrganizeSelectBtn delDicideRebellionOrganizeSelectBtn
			{
				get;
				private set;
			}

			public SortieStartBtn(Transform transform)
			{
				_uiButton = ((Component)transform).GetComponent<UIButton>();
			}

			public bool Init(int nIndex, System.Tuple<MonoBehaviour, string> delegateInfos, DelDicideRebellionOrganizeSelectBtn decideDelegate)
			{
				index = nIndex;
				_uiButton.onClick = Util.CreateEventDelegateList(delegateInfos.Item1, delegateInfos.Item2, this);
				_uiButton.isEnabled = false;
				_uiButton.GetComponent<BoxCollider2D>().enabled = false;
				return true;
			}

			public bool UnInit()
			{
				Mem.Del(ref _uiButton);
				Mem.Del(ref _uiToggle);
				delDicideRebellionOrganizeSelectBtn = null;
				return true;
			}

			public void Decide()
			{
				_uiButton.state = UIButtonColor.State.Normal;
				if (delDicideRebellionOrganizeSelectBtn != null)
				{
					delDicideRebellionOrganizeSelectBtn(this);
				}
			}
		}

		[SerializeField]
		private Transform _prefabParticipatingFleetInfo;

		[SerializeField]
		private SortieStartBtn _uiSortieStartBtn;

		[SerializeField]
		private float _fStartOffs = -400f;

		[SerializeField]
		private Vector3 _vOriginPos = new Vector3(-304f, 0f, 0f);

		private DelDicideRebellionOrganizeSelectBtn _delDicideRebellionOrganizeSelectBtn;

		private Action _actDecideSortieStart;

		private int _nSelectedIndex;

		private List<Vector3> _listInfosPos;

		private List<UIRebellionParticipatingFleetInfo> _listFleetInfos;

		private List<IRebellionOrganizeSelectObject> _listSelectorObjects;

		private int selectedObjectMax => (_uiSortieStartBtn.button.state != UIButtonColor.State.Disabled) ? (_listSelectorObjects.Count - 1) : (_listSelectorObjects.Count - 2);

		public int nowIndex => _nSelectedIndex;

		public bool isPossibleSortieStart => _listFleetInfos[2].isFlagShipExists;

		public bool isColliderEnabled
		{
			get
			{
				return GetComponentInChildren<BoxCollider2D>().enabled;
			}
			set
			{
				_listSelectorObjects.ForEach(delegate(IRebellionOrganizeSelectObject x)
				{
					x.button.isEnabled = ((x.button.state != UIButtonColor.State.Disabled) ? true : false);
				});
			}
		}

		public bool isSortieStartFocus => _uiSortieStartBtn.toggle.value;

		public UIPanel panel => GetComponent<UIPanel>();

		public List<UIRebellionParticipatingFleetInfo> participatingFleetInfo => _listFleetInfos;

		public List<UIRebellionParticipatingFleetInfo> participatingFleetList => new List<UIRebellionParticipatingFleetInfo>(_listFleetInfos.FindAll((UIRebellionParticipatingFleetInfo x) => x.isFlagShipExists));

		public static UIRebellionParticipatingFleetSelector Instantiate(UIRebellionParticipatingFleetSelector prefab, Transform parent)
		{
			UIRebellionParticipatingFleetSelector uIRebellionParticipatingFleetSelector = UnityEngine.Object.Instantiate(prefab);
			uIRebellionParticipatingFleetSelector.transform.parent = parent;
			uIRebellionParticipatingFleetSelector.transform.localScaleOne();
			uIRebellionParticipatingFleetSelector.transform.localPosition = uIRebellionParticipatingFleetSelector._vOriginPos;
			uIRebellionParticipatingFleetSelector.Setup();
			return uIRebellionParticipatingFleetSelector;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabParticipatingFleetInfo);
			_uiSortieStartBtn.UnInit();
			Mem.Del(ref _uiSortieStartBtn);
			Mem.Del(ref _fStartOffs);
			Mem.Del(ref _vOriginPos);
			Mem.Del(ref _delDicideRebellionOrganizeSelectBtn);
			Mem.Del(ref _actDecideSortieStart);
			Mem.Del(ref _nSelectedIndex);
			Mem.DelListSafe(ref _listInfosPos);
			Mem.DelListSafe(ref _listFleetInfos);
			Mem.DelListSafe(ref _listSelectorObjects);
		}

		private bool Setup()
		{
			_listInfosPos = new List<Vector3>();
			_listInfosPos.Add(new Vector3(96f, 202f, 0f));
			_listInfosPos.Add(new Vector3(100f, 91f, 0f));
			_listInfosPos.Add(new Vector3(96f, -20f, 0f));
			_listInfosPos.Add(new Vector3(100f, -131f, 0f));
			_listInfosPos.Add(new Vector3(-5f, -230f, 0f));
			_listFleetInfos = new List<UIRebellionParticipatingFleetInfo>();
			_listSelectorObjects = new List<IRebellionOrganizeSelectObject>();
			_nSelectedIndex = 0;
			return true;
		}

		public IEnumerator InstantiateObjects()
		{
			foreach (int iType in Enum.GetValues(typeof(RebellionFleetType)))
			{
				_listFleetInfos.Add(UIRebellionParticipatingFleetInfo.Instantiate(((Component)_prefabParticipatingFleetInfo).GetComponent<UIRebellionParticipatingFleetInfo>(), base.transform, Vector3.zero));
				_listFleetInfos[iType].transform.name = ((RebellionFleetType)iType).ToString();
				yield return null;
			}
			_listSelectorObjects.AddRange(_listFleetInfos.ToArray());
			_listSelectorObjects.Add(_uiSortieStartBtn);
			yield return null;
			int cnt = 0;
			foreach (IRebellionOrganizeSelectObject obj in _listSelectorObjects)
			{
				Vector3 startPos = _listInfosPos[cnt];
				startPos.x = _fStartOffs;
				obj.toggle.transform.localPosition = startPos;
				obj.toggle.group = 10;
				obj.toggle.startsActive = false;
				obj.toggle.value = false;
				cnt++;
			}
			_listSelectorObjects[0].toggle.startsActive = true;
			_listSelectorObjects[0].toggle.value = true;
			if (!isPossibleSortieStart)
			{
				_uiSortieStartBtn.button.isEnabled = false;
			}
		}

		public bool Init(DelDicideRebellionOrganizeSelectBtn decideDelegate, Action callback)
		{
			_delDicideRebellionOrganizeSelectBtn = decideDelegate;
			_actDecideSortieStart = callback;
			int cnt = 0;
			_listFleetInfos.ForEach(delegate(UIRebellionParticipatingFleetInfo x)
			{
				x.Init((RebellionFleetType)cnt, DecideParticipatingFleetInfo);
				cnt++;
			});
			_uiSortieStartBtn.Init(Enum.GetValues(typeof(RebellionFleetType)).Length, new System.Tuple<MonoBehaviour, string>(this, "DecideParticipatingFleetInfo"), DecideParticipatingFleetInfo);
			ChangeBtnState(0);
			return true;
		}

		public void Show(Action callback)
		{
			panel.widgetsAreStatic = false;
			_listSelectorObjects.ForEach(delegate(IRebellionOrganizeSelectObject x)
			{
				Action onComplete = null;
				if (x.index == _uiSortieStartBtn.index)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate
						{
							Dlg.Call(ref callback);
						});
					};
				}
				if (x.button.transform.LTIsTweening())
				{
					x.button.transform.LTCancel();
				}
				x.button.transform.LTMoveLocal(_listInfosPos[x.index], 0.2f).setEase(LeanTweenType.easeInSine).setDelay((float)x.index * 0.03f)
					.setOnComplete(onComplete);
			});
		}

		public void Hide(Action callback)
		{
			_listSelectorObjects.ForEach(delegate(IRebellionOrganizeSelectObject x)
			{
				Action onComplete = null;
				if (x.index == _uiSortieStartBtn.index)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate
						{
							Dlg.Call(ref callback);
							panel.widgetsAreStatic = true;
						});
					};
				}
				if (x.button.transform.LTIsTweening())
				{
					x.button.transform.LTCancel();
				}
				Vector3 to = _listInfosPos[x.index];
				to.x = _fStartOffs;
				x.button.transform.LTMoveLocal(to, 0.2f).setEase(LeanTweenType.easeInSine).setDelay((float)x.index * 0.03f)
					.setOnComplete(onComplete);
			});
		}

		public void MoveNext()
		{
			ChangeBtnState(isForward: true);
		}

		public void MovePrev()
		{
			ChangeBtnState(isForward: false);
		}

		public bool IsAlreadySetFleet(DeckModel model)
		{
			if (_listFleetInfos.FindAll((UIRebellionParticipatingFleetInfo x) => x.deckModel == model).Count == 0)
			{
				return false;
			}
			return true;
		}

		public void SetFleetInfo(RebellionFleetType iType, DeckModel model)
		{
			DebugUtils.Log(string.Format("{0} - {1}", iType, (model == null) ? "Null" : model.ToString()));
			_listFleetInfos[(int)iType].SetFleetInfo(model);
		}

		public void ChkSortieStartState()
		{
			_uiSortieStartBtn.button.isEnabled = !isPossibleSortieStart;
			_uiSortieStartBtn.button.isEnabled = isPossibleSortieStart;
		}

		private void ChangeBtnState(int nIndex)
		{
			_nSelectedIndex = nIndex;
		}

		private void ChangeBtnState(bool isForward)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			int a = _nSelectedIndex + (isForward ? 1 : (-1));
			_nSelectedIndex = Mathe.MinMax2Rev(a, 0, selectedObjectMax);
			_listSelectorObjects[_nSelectedIndex].toggle.value = true;
		}

		private void DecideParticipatingFleetInfo(IRebellionOrganizeSelectObject selectObj)
		{
			DebugUtils.Log("UIRebellionParticipatingFleetSelector", selectObj.button.name);
			ChangeBtnState(selectObj.index);
			if (selectObj.index == _uiSortieStartBtn.index)
			{
				if (_actDecideSortieStart != null)
				{
					_actDecideSortieStart();
					_uiSortieStartBtn.button.enabled = false;
				}
			}
			else if (_delDicideRebellionOrganizeSelectBtn != null)
			{
				_delDicideRebellionOrganizeSelectBtn(selectObj);
			}
		}
	}
}
