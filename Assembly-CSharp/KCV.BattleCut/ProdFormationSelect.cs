using Common.Enum;
using local.models;
using local.utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(BtlCut_FormationAnimation))]
	public class ProdFormationSelect : MonoBehaviour
	{
		[Serializable]
		private class BrightPoints
		{
			private delegate Vector3 GetFormationPos(int posNo, int spaceX, int spaceY);

			private GetFormationPos[] GetFormations;

			[SerializeField]
			private Transform _tra;

			private List<Transform> _listBrightPoint;

			[SerializeField]
			private FormationIconPos[] _clsFormationPos;

			public Transform transform => _tra;

			public BrightPoints(Transform obj)
			{
				_tra = obj;
			}

			public bool Init()
			{
				_listBrightPoint = new List<Transform>(6);
				for (int i = 1; i <= _listBrightPoint.Capacity; i++)
				{
					_listBrightPoint.Add(transform.FindChild($"BrightPoint{i}"));
				}
				for (int j = 0; j < _clsFormationPos[0].IconPos.Length; j++)
				{
					_listBrightPoint[j].localPosition = _clsFormationPos[0].IconPos[j];
				}
				return true;
			}

			public bool UnInit()
			{
				Mem.DelAry(ref GetFormations);
				Mem.Del(ref _tra);
				Mem.DelList(ref _listBrightPoint);
				Mem.DelAry(ref _clsFormationPos);
				return true;
			}

			public void ChangeFormation(BattleFormationKinds1 iKind)
			{
				int cnt = 0;
				_clsFormationPos[(int)(iKind - 1)].IconPos.ForEach(delegate(Vector3 x)
				{
					_listBrightPoint[cnt].transform.LTMoveLocal(x, 0.3f).setEase(LeanTweenType.easeOutExpo);
					cnt++;
				});
			}

			private void CalcFormationBrightPointPos()
			{
				GetFormations = new GetFormationPos[5];
				GetFormations[0] = GetTanjuPos;
				GetFormations[1] = GetHukujuPos;
				GetFormations[2] = GetRinkeiPos;
				GetFormations[3] = GetTeikeiPos;
				GetFormations[4] = GetTanouPos;
				for (int i = 0; i < 5; i++)
				{
					_clsFormationPos[i] = new FormationIconPos();
				}
				int spaceX = 82;
				int spaceY = 86;
				for (int j = 0; j < 5; j++)
				{
					for (int k = 0; k < 6; k++)
					{
						_clsFormationPos[j].IconPos[k] = GetFormations[j](k, spaceX, spaceY);
					}
				}
			}

			private Vector3 GetTanjuPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3(spaceX / 2, 0 + spaceY * posNo / 2 - spaceY / 4, 0f);
			}

			private Vector3 GetHukujuPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3(posNo % 2 * spaceX, spaceY * (posNo / 2), 0f);
			}

			private Vector3 GetRinkeiPos(int posNo, int spaceX, int spaceY)
			{
				if (posNo < 2)
				{
					return new Vector3(spaceX / 2, spaceY * posNo / 2, 0f);
				}
				if (posNo < 4)
				{
					return new Vector3(spaceX / 2 * (posNo % 3), spaceY, 0f);
				}
				if (posNo < 6)
				{
					return new Vector3(spaceX / 2, spaceY * (posNo - 1) / 2, 0f);
				}
				return Vector3.zero;
			}

			private Vector3 GetTeikeiPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3((posNo - 2) * spaceX / 2 + spaceX / 4, posNo * spaceY / 2 - spaceY / 4, 0f);
			}

			private Vector3 GetTanouPos(int posNo, int spaceX, int spaceY)
			{
				return new Vector3(spaceX * (posNo - 2) / 2 + spaceX / 4, spaceY, 0f);
			}
		}

		[Serializable]
		public class FormationIconPos
		{
			public Vector3[] IconPos;

			public FormationIconPos()
			{
				IconPos = new Vector3[6];
			}
		}

		private const int FORMATION_NUM = 5;

		[SerializeField]
		private BrightPoints _clsBrightPoints;

		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private DeckModel _clsDeckModel;

		private BtlCut_FormationAnimation _prodFormationAnim;

		private BattleFormationKinds1 _iSelectFormation;

		private Action<BattleFormationKinds1> _actCallback;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public BtlCut_FormationAnimation formationAnimation => this.GetComponentThis(ref _prodFormationAnim);

		public BattleFormationKinds1 selectFormation => _iSelectFormation;

		public static ProdFormationSelect Instantiate(ProdFormationSelect prefab, Transform parent, DeckModel model)
		{
			ProdFormationSelect prodFormationSelect = UnityEngine.Object.Instantiate(prefab);
			prodFormationSelect.transform.parent = parent;
			prodFormationSelect.transform.localScaleOne();
			prodFormationSelect.transform.localPositionZero();
			prodFormationSelect.Init(model);
			return prodFormationSelect;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe(ref _listLabelButton);
			_clsBrightPoints.UnInit();
			Mem.Del(ref _clsBrightPoints);
			Mem.Del(ref _clsDeckModel);
			Mem.Del(ref _isInputPossible);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _prodFormationAnim);
			Mem.Del(ref _iSelectFormation);
			Mem.Del(ref _actCallback);
		}

		private void Init(DeckModel model)
		{
			_clsDeckModel = model;
			_isInputPossible = false;
			_iSelectFormation = BattleFormationKinds1.TanJuu;
			panel.alpha = 0f;
			_clsBrightPoints.Init();
			HashSet<BattleFormationKinds1> hash = DeckUtil.GetSelectableFormations(_clsDeckModel);
			BattleFormationKinds1 cnt = BattleFormationKinds1.TanJuu;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.Init((int)cnt, hash.Contains(cnt) ? true : false);
				x.isFocus = false;
				x.toggle.group = 1;
				x.toggle.enabled = false;
				x.toggle.onDecide = delegate
				{
					DecideFormation((BattleFormationKinds1)x.index);
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", cnt);
				if (x.index == 1)
				{
					x.isFocus = (x.toggle.startsActive = true);
				}
				cnt++;
			});
		}

		public void Play(Action<BattleFormationKinds1> callback)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.SetNavigationInFormation();
			navigation.Show(Defines.PHASE_FADE_TIME, null);
			_actCallback = callback;
			Show().setOnComplete((Action)delegate
			{
				_isInputPossible = true;
				_listLabelButton.ForEach(delegate(UILabelButton x)
				{
					x.toggle.enabled = (x.isValid ? true : false);
				});
				ChangeFocus(_iSelectFormation);
			});
		}

		public void Run()
		{
			if (_isInputPossible)
			{
				KeyControl keyControl = BattleCutManager.GetKeyControl();
				if (keyControl.GetDown(KeyControl.KeyName.DOWN))
				{
					PreparaNext(isForward: true);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.UP))
				{
					PreparaNext(isForward: false);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					DecideFormation(_iSelectFormation);
				}
			}
		}

		private void PreparaNext(bool isForward)
		{
			BattleFormationKinds1 iSelectFormation = _iSelectFormation;
			_iSelectFormation = (BattleFormationKinds1)Mathe.NextElement((int)_iSelectFormation, 1, 5, isForward, delegate(int x)
			{
				ProdFormationSelect prodFormationSelect = this;
				return _listLabelButton.Find((UILabelButton y) => y.index == x).isValid;
			});
			if (iSelectFormation != _iSelectFormation)
			{
				ChangeFocus(_iSelectFormation);
			}
		}

		private LTDescr Show()
		{
			return panel.transform.LTValue(0f, 1f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			return panel.transform.LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private void ChangeFocus(BattleFormationKinds1 iKind)
		{
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = ((x.index == (int)iKind) ? true : false);
			});
			_clsBrightPoints.ChangeFormation(iKind);
		}

		private void OnActive(BattleFormationKinds1 iKind)
		{
			if (_iSelectFormation != iKind)
			{
				_iSelectFormation = iKind;
				ChangeFocus(_iSelectFormation);
			}
		}

		private void DecideFormation(BattleFormationKinds1 iKind)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			_isInputPossible = false;
			_listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.enabled = false;
			});
			Observable.FromCoroutine(() => formationAnimation.StartAnimation(_iSelectFormation)).Subscribe(delegate
			{
				Hide().setOnComplete((Action)delegate
				{
					Dlg.Call(_actCallback, iKind);
				});
				BattleCutManager.GetLive2D().Hide(null);
			});
		}
	}
}
