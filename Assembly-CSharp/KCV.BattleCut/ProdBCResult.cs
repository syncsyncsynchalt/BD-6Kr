using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using local.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class ProdBCResult : BtlCut_Base
	{
		[Serializable]
		private class MVPShip
		{
			[SerializeField]
			private UIPanel _uiPanel;

			[SerializeField]
			private UITexture _uiShipTexture;

			[SerializeField]
			private UITexture _uiShipShadow;

			private ShipModel_BattleResult _clsShipModel;

			private UIPlayTween _uiPlayTween;

			[SerializeField]
			[Header("[Animation Properties]")]
			private float _fDuration = 0.2f;

			[SerializeField]
			private iTween.EaseType _iEaseType = iTween.EaseType.easeOutSine;

			public int index
			{
				get;
				private set;
			}

			public float duration
			{
				get
				{
					return _fDuration;
				}
				set
				{
					_fDuration = value;
				}
			}

			public iTween.EaseType easeType
			{
				get
				{
					return _iEaseType;
				}
				set
				{
					_iEaseType = value;
				}
			}

			public UIPanel panel => _uiPanel;

			public Transform transform => _uiPanel.transform;

			public ShipModel_BattleResult shipModel => _clsShipModel;

			private UIPlayTween playTween
			{
				get
				{
					if (_uiPlayTween == null)
					{
						_uiPlayTween = ((Component)transform).GetComponent<UIPlayTween>();
					}
					return _uiPlayTween;
				}
			}

			public MVPShip(Transform transform)
			{
			}

			public bool Init(ShipModel_BattleResult model)
			{
				_clsShipModel = model;
				panel.alpha = 0f;
				if (model == null)
				{
					return false;
				}
				index = model.Index;
				Texture2D texture2D = ShipUtils.LoadTexture(model);
				UITexture uiShipTexture = _uiShipTexture;
				Texture mainTexture = texture2D;
				_uiShipShadow.mainTexture = mainTexture;
				uiShipTexture.mainTexture = mainTexture;
				_uiShipTexture.MakePixelPerfect();
				_uiShipShadow.MakePixelPerfect();
				Vector3 vpos = Util.Poi2Vec(model.Offsets.GetShipDisplayCenter(model.IsDamaged())) + Vector3.down * 10f;
				_uiShipTexture.transform.localScale = Vector3.one * 1.1f;
				_uiShipTexture.transform.AddLocalPosition(vpos);
				_uiShipShadow.transform.AddLocalPosition(vpos);
				return true;
			}

			public bool UnInit()
			{
				_uiPanel = null;
				_uiShipTexture = null;
				_uiShipShadow = null;
				return true;
			}

			public void PlayMVPVoice(bool isPlayVoice)
			{
				if (_clsShipModel != null)
				{
					if (isPlayVoice)
					{
						ShipUtils.PlayMVPVoice(shipModel);
					}
					playTween.Play(forward: true);
					transform.ValueTo(0f, 1f, duration, _iEaseType, delegate(object x)
					{
						panel.alpha = Convert.ToSingle(x);
					}, null);
				}
			}
		}

		[Serializable]
		private class ResultFrame
		{
			[SerializeField]
			private UIPanel _uiPanel;

			[SerializeField]
			[Header("[Animation Properties]")]
			private float _fDuration = 0.7f;

			[SerializeField]
			private Vector3 _vFromPos = Vector3.left * 36.1f;

			[SerializeField]
			private Vector3 _vToPos = Vector3.zero;

			[SerializeField]
			private iTween.EaseType _iEaseType = iTween.EaseType.easeOutSine;

			public UIPanel panel => _uiPanel;

			public Transform transform => _uiPanel.transform;

			public float duration
			{
				get
				{
					return _fDuration;
				}
				set
				{
					_fDuration = value;
				}
			}

			public ResultFrame(Transform transform)
			{
			}

			public bool Init()
			{
				return true;
			}

			public bool UnInit()
			{
				return true;
			}

			public void Show()
			{
				panel.widgetsAreStatic = false;
				transform.localPosition = _vFromPos;
				transform.LocalMoveTo(_vToPos, duration, _iEaseType, null);
				transform.ValueTo(0f, 1f, duration, _iEaseType, delegate(object x)
				{
					panel.alpha = Convert.ToSingle(x);
				}, null);
			}

			public void Hide()
			{
				transform.localPosition = _vToPos;
				transform.LocalMoveTo(_vFromPos, duration, _iEaseType, null);
				transform.ValueTo(1f, 0f, duration, _iEaseType, delegate(object x)
				{
					panel.alpha = Convert.ToSingle(x);
				}, delegate
				{
					panel.widgetsAreStatic = true;
				});
			}
		}

		[Serializable]
		private class Ships
		{
			[SerializeField]
			private UIPanel _uiPanel;

			[SerializeField]
			[Header("[Animation Properties]")]
			private float _fDuration = 0.7f;

			[SerializeField]
			private Vector3 _vFromPos = new Vector3(-385.72f, 12.06f, 0f);

			[SerializeField]
			private Vector3 _vToPos = new Vector3(-315f, -12.06f, 0f);

			[SerializeField]
			private iTween.EaseType _iEaseType = iTween.EaseType.easeOutSine;

			private List<BtlCut_ResultShip> _listResultShips;

			public Transform transform => _uiPanel.transform;

			public UIPanel panel => _uiPanel;

			public float duration
			{
				get
				{
					return _fDuration;
				}
				set
				{
					_fDuration = value;
				}
			}

			public List<BtlCut_ResultShip> resultShips => _listResultShips;

			public Ships(Transform transform)
			{
			}

			public bool Init(System.Tuple<BattleResultModel, Transform> infos)
			{
				_listResultShips = new List<BtlCut_ResultShip>(infos.Item1.Ships_f.Length);
				for (int i = 0; i < _listResultShips.Capacity; i++)
				{
					if (infos.Item1.Ships_f[i] != null)
					{
						_listResultShips.Add(BtlCut_ResultShip.Instantiate(((Component)infos.Item2).GetComponent<BtlCut_ResultShip>(), transform, new Vector3(0f, 150f - (float)(i * 60), 0f), infos.Item1.Ships_f[i]));
					}
				}
				return true;
			}

			public bool UnInit()
			{
				return true;
			}

			public void Show()
			{
				_uiPanel.widgetsAreStatic = false;
				transform.localPosition = _vFromPos;
				transform.LocalMoveTo(_vToPos, duration, _iEaseType, null);
				transform.ValueTo(0f, 1f, duration, _iEaseType, delegate(object x)
				{
					panel.alpha = Convert.ToSingle(x);
				}, null);
			}

			public void Hide()
			{
				transform.localPosition = _vToPos;
				transform.LocalMoveTo(_vFromPos, duration, _iEaseType, null);
				transform.ValueTo(1f, 0f, duration, _iEaseType, delegate(object x)
				{
					panel.alpha = Convert.ToSingle(x);
				}, delegate
				{
					panel.widgetsAreStatic = true;
				});
			}
		}

		[SerializeField]
		private Transform _prefabResultShipOrigin;

		[SerializeField]
		private MVPShip _clsMVPShip;

		[SerializeField]
		private ResultFrame _clsResultFrame;

		[SerializeField]
		private Ships _clsShips;

		private bool isAnimEnd;

		private int[] startHP;

		private BattleResultModel _clsBattleResult;

		private KeyControl key;

		private Action _actCallback;

		public static ProdBCResult Instantiate(ProdBCResult prefab, Transform parent)
		{
			ProdBCResult prodBCResult = UnityEngine.Object.Instantiate(prefab);
			prodBCResult.transform.parent = parent;
			prodBCResult.transform.localScaleOne();
			prodBCResult.transform.localPositionZero();
			prodBCResult.Init();
			return prodBCResult;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabResultShipOrigin);
			if (_clsMVPShip != null)
			{
				_clsMVPShip.UnInit();
			}
			Mem.Del(ref _clsMVPShip);
			if (_clsResultFrame != null)
			{
				_clsResultFrame.UnInit();
			}
			Mem.Del(ref _clsResultFrame);
			if (_clsShips != null)
			{
				_clsShips.UnInit();
			}
			Mem.Del(ref _clsShips);
			Mem.Del(ref isAnimEnd);
			Mem.DelAry(ref startHP);
			Mem.Del(ref _clsBattleResult);
			Mem.Del(ref key);
			Mem.Del(ref _actCallback);
		}

		public override void Init()
		{
			_clsBattleResult = BattleCutManager.GetBattleManager().GetBattleResult();
			startHP = GetStartHP(_clsBattleResult);
			isAnimEnd = false;
			key = new KeyControl();
			SetResultShips();
			_clsMVPShip.Init(_clsBattleResult.MvpShip);
			_clsMVPShip.PlayMVPVoice(BattleUtils.IsPlayMVPVoice(_clsBattleResult.WinRank));
		}

		private int[] GetStartHP(BattleResultModel model)
		{
			int[] array = new int[model.Ships_f.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (model.Ships_f[i] == null)
				{
					array[i] = 0;
				}
				else
				{
					array[i] = model.Ships_f[i].HpStart;
				}
			}
			return array;
		}

		public override void Run()
		{
			if (isAnimEnd)
			{
				key.Update();
				if (key.keyState[1].down || Input.GetMouseButton(0) || Input.touchCount > 0)
				{
					SceneFinish();
				}
			}
		}

		private void SetResultShips()
		{
			_clsShips.Init(new System.Tuple<BattleResultModel, Transform>(_clsBattleResult, _prefabResultShipOrigin));
		}

		public void StartAnimation(Action callback)
		{
			TrophyUtil.Unlock_UserLevel();
			_actCallback = callback;
			Observable.Timer(TimeSpan.FromSeconds(0.800000011920929)).Subscribe(delegate
			{
				_clsResultFrame.Show();
				_clsShips.Show();
			}).AddTo(base.gameObject);
			Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(delegate
			{
				StartHPBarAnim();
				_clsShips.resultShips[0].act = delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
					{
						_clsShips.resultShips[_clsMVPShip.index].ShowMVPIcon();
						StartEXPBarAnim();
						isAnimEnd = true;
						UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
						navigation.SetNavigationInResult();
						navigation.Show(0.2f, null);
					});
				};
			}).AddTo(base.gameObject);
		}

		public void StartHPBarAnim()
		{
			(from order in _clsShips.resultShips
				where order.isActiveAndEnabled
				select order).ForEach(delegate(BtlCut_ResultShip x)
			{
				x.UpdateHP();
			});
		}

		public void StartEXPBarAnim()
		{
			(from order in _clsShips.resultShips
				where order.isActiveAndEnabled
				select order).ForEach(delegate(BtlCut_ResultShip x)
			{
				x.UpdateEXPGauge();
			});
		}

		private void SceneFinish()
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			navigation.Hide(0.2f, null);
			isAnimEnd = false;
			hideAll(delegate
			{
				Dlg.Call(ref _actCallback);
			});
		}

		private void hideAll(Action callback)
		{
			base.transform.ValueTo(1f, 0f, 0.2f, iTween.EaseType.easeOutSine, delegate(object x)
			{
				float num = Convert.ToSingle(x);
				UIPanel panel = _clsMVPShip.panel;
				float num2 = num;
				_clsShips.panel.alpha = num2;
				num2 = num2;
				_clsResultFrame.panel.alpha = num2;
				panel.alpha = num2;
			}, callback);
		}
	}
}
