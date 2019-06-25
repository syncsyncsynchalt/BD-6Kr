using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UICommandBox : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			[Header("[Backgrounds Properties]")]
			public float backgroundsShowTime;

			public LeanTweenType backgroundsShowEase;

			[Header("[Surface Properties]")]
			public Vector3 surfaceInitPosOffs;

			public float surfaceShowMoveTime;

			public LeanTweenType surfaceShowEase;

			public float surfaceShowInterval;

			public void Dispose()
			{
				Mem.Del(ref backgroundsShowTime);
				Mem.Del(ref backgroundsShowEase);
				Mem.Del(ref surfaceInitPosOffs);
				Mem.Del(ref surfaceShowMoveTime);
				Mem.Del(ref surfaceShowEase);
				Mem.Del(ref surfaceShowInterval);
			}
		}

		[Serializable]
		private class UIBattleStartBtn : IDisposable, IUICommandSurface
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiGrow;

			[SerializeField]
			private UISprite _uiSprite;

			private UIWidget _uiWidget;

			private int _nIndex;

			private Animation _anim;

			public Transform transform => _tra;

			public UIButton button => ((Component)transform).GetComponent<UIButton>();

			public BoxCollider2D colliderBox2D => ((Component)transform).GetComponent<BoxCollider2D>();

			public bool isEnabled
			{
				get
				{
					return button.isEnabled;
				}
				set
				{
					if (value)
					{
						button.normalSprite = "select_btn_on";
						_uiGrow.transform.LTValue(0f, 1f, 1f).setEase(LeanTweenType.linear).setLoopPingPong()
							.setOnUpdate(delegate(float x)
							{
								_uiGrow.alpha = x;
							});
						button.isEnabled = true;
					}
					else
					{
						_uiGrow.transform.LTCancel();
						_uiGrow.transform.LTValue(_uiGrow.alpha, 0f, 1f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
						{
							_uiGrow.alpha = x;
						});
						button.normalSprite = "select_btn";
						button.isEnabled = false;
					}
				}
			}

			public int index
			{
				get
				{
					return _nIndex;
				}
				private set
				{
					_nIndex = value;
				}
			}

			public UIWidget widget => ((Component)transform).GetComponent<UIWidget>();

			public UIBattleStartBtn(Transform obj)
			{
			}

			public bool Init(int nIndex, Func<bool> onDecideBattleStart)
			{
				_nIndex = nIndex;
				button.onClick.Add(new EventDelegate(delegate
				{
					onDecideBattleStart();
				}));
				button.normalSprite = "select_btn";
				button.isEnabled = false;
				_uiGrow.alpha = 0f;
				_anim = ((Component)transform).GetComponent<Animation>();
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiGrow);
				Mem.Del(ref _uiSprite);
				Mem.Del(ref _uiWidget);
				Mem.Del(ref _nIndex);
				Mem.Del(ref _anim);
			}

			public LTDescr Magnify()
			{
				return transform.LTScale(Vector3.one * 1.2f, 0.2f).setEase(LeanTweenType.easeOutSine);
			}

			public LTDescr Reduction()
			{
				return transform.LTScale(Vector3.one, 0.2f).setEase(LeanTweenType.easeOutSine);
			}

			public void Show()
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_943);
				_anim.Play();
			}

			public void PlayDecide()
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_944);
				_uiGrow.transform.LTCancel();
				_uiGrow.transform.LTScale(Vector3.one * 1.5f, 0.5f);
				_uiGrow.transform.LTValue(_uiGrow.alpha, 0f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
				{
					_uiGrow.alpha = x;
				});
			}
		}

		[Serializable]
		private class Backgrounds : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiBackground;

			[SerializeField]
			private UISprite _uiBGLight;

			[SerializeField]
			private UITexture _uiGlowLight;

			[SerializeField]
			private UISprite _uiCommandLabel;

			public bool Init()
			{
				_uiBackground.height = 0;
				_uiBGLight.height = 0;
				_uiGlowLight.width = 0;
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				if (_uiBackground != null)
				{
					_uiBackground.Clear();
				}
				Mem.Del(ref _uiBackground);
				if (_uiBGLight != null)
				{
					_uiBGLight.Clear();
				}
				Mem.Del(ref _uiBGLight);
				if (_uiCommandLabel != null)
				{
					_uiCommandLabel.Clear();
				}
				Mem.Del(ref _uiCommandLabel);
				Mem.Del(ref _uiGlowLight);
			}

			public LTDescr Show(float time, LeanTweenType iType)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_941);
				_uiCommandLabel.transform.LTMoveLocalY(-125f, time).setEase(iType);
				_uiGlowLight.transform.LTValue(0f, 1f, 5f).setDelay(time).setEase(LeanTweenType.linear)
					.setOnUpdate(delegate(float x)
					{
						Rect uvRect = _uiGlowLight.uvRect;
						uvRect.x = x;
						_uiGlowLight.uvRect = uvRect;
					})
					.setLoopClamp();
				_uiGlowLight.transform.LTValue(1f, 0.3f, 1f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiGlowLight.alpha = x;
				})
					.setLoopPingPong();
				return _uiBackground.transform.LTValue(_uiBackground.width, 1150f, time).setEase(iType).setOnUpdate(delegate(float x)
				{
					int num = Convert.ToInt32(x);
					_uiBackground.height = num;
					_uiBGLight.height = num;
					_uiGlowLight.width = num;
				});
			}

			public LTDescr Hide()
			{
				return null;
			}
		}

		[SerializeField]
		private Transform _prefabCommandSurface;

		[SerializeField]
		private Backgrounds _clsBackgrounds;

		[SerializeField]
		private UIBattleStartBtn _uiBattleStartBtn;

		[SerializeField]
		private List<Vector3> _listCommandSurfacePos = new List<Vector3>();

		[SerializeField]
		[Header("[Animation Properties]")]
		private Params _strParams;

		[SerializeField]
		private Vector3 _vCommandBoxPos = Vector3.zero;

		[SerializeField]
		private Vector3 _vBattleStartBtnPos = Vector3.zero;

		private List<UICommandSurface> _listCommandSurface;

		private List<IUICommandSurface> _listICommandSurface;

		private Predicate<List<BattleCommand>> _actDecideBattleStart;

		private UIPanel _uiPanel;

		private int _nSelectIndex;

		public List<UICommandSurface> listCommandSurfaces
		{
			get
			{
				return _listCommandSurface;
			}
			private set
			{
				_listCommandSurface = value;
			}
		}

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public int selectIndex
		{
			get
			{
				return _nSelectIndex;
			}
			private set
			{
				int mx = (!_uiBattleStartBtn.isEnabled) ? (_listCommandSurface.Count - 1) : _listCommandSurface.Count;
				_nSelectIndex = Mathe.MinMax2Rev(value, 0, mx);
			}
		}

		public bool isSelectBattleStart => selectIndex == _listCommandSurface.Count;

		public UICommandSurface focusSurface => _listCommandSurface[selectIndex];

		public bool isColliderEnabled
		{
			set
			{
				_listCommandSurface.ForEach(delegate(UICommandSurface x)
				{
					x.boxCollider2D.enabled = value;
				});
				_uiBattleStartBtn.colliderBox2D.enabled = value;
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabCommandSurface);
			Mem.DelIDisposableSafe(ref _clsBackgrounds);
			Mem.DelIDisposableSafe(ref _uiBattleStartBtn);
			Mem.DelListSafe(ref _listCommandSurfacePos);
			Mem.DelIDisposableSafe(ref _strParams);
			Mem.Del(ref _vCommandBoxPos);
			Mem.Del(ref _vBattleStartBtnPos);
			Mem.DelListSafe(ref _listCommandSurface);
			Mem.DelListSafe(ref _listICommandSurface);
			Mem.Del(ref _actDecideBattleStart);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _nSelectIndex);
		}

		public bool Init(CommandPhaseModel model, Predicate<List<BattleCommand>> onDeideBattleStart)
		{
			_nSelectIndex = 0;
			base.transform.localPosition = _vCommandBoxPos;
			_actDecideBattleStart = onDeideBattleStart;
			CreateSurface(model.GetPresetCommand());
			_uiBattleStartBtn.Init(_listCommandSurface.Count, DecideStartBattle);
			_uiBattleStartBtn.transform.localPosition = _vBattleStartBtnPos;
			_uiBattleStartBtn.transform.localScaleZero();
			_listICommandSurface.Add(_uiBattleStartBtn);
			panel.alpha = 1f;
			ChkAllSurfaceSet();
			_clsBackgrounds.Init();
			return true;
		}

		private void CreateSurface(List<BattleCommand> presetList)
		{
			_listCommandSurface = new List<UICommandSurface>();
			_listICommandSurface = new List<IUICommandSurface>();
			int num = 0;
			foreach (BattleCommand preset in presetList)
			{
				_listCommandSurface.Add(UICommandSurface.Instantiate(((Component)_prefabCommandSurface).GetComponent<UICommandSurface>(), base.transform, _listCommandSurfacePos[num] + _strParams.surfaceInitPosOffs, num, preset, ChkAllSurfaceSet));
				_listICommandSurface.Add(_listCommandSurface[num]);
				num++;
			}
		}

		public void SetBattleStartButtonLayer()
		{
			_uiBattleStartBtn.transform.SetLayer(Generics.Layers.CutIn.IntLayer(), includeChildren: true);
		}

		public void FocusSurfaceMagnify()
		{
			UICommandSurface uICommandSurface = _listCommandSurface.FirstOrDefault((UICommandSurface x) => !x.isSetUnit);
			if (_uiBattleStartBtn.isEnabled)
			{
				selectIndex = _uiBattleStartBtn.index;
			}
			else if (uICommandSurface != null)
			{
				selectIndex = uICommandSurface.index;
			}
			else
			{
				selectIndex = 0;
			}
			_listICommandSurface[selectIndex].Magnify();
		}

		public void Prev()
		{
			selectIndex--;
			_listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				if (x.index == selectIndex)
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
					x.Magnify();
				}
				else
				{
					x.Reduction();
				}
			});
		}

		public void Next()
		{
			selectIndex++;
			_listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				if (x.index == selectIndex)
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
					x.Magnify();
				}
				else
				{
					x.Reduction();
				}
			});
		}

		public void RemoveCommandUnit2FocusSurface()
		{
			if (selectIndex != _uiBattleStartBtn.index)
			{
				_listCommandSurface[selectIndex].RemoveCommandUnit();
			}
		}

		public void RemoveCommandUnitAll()
		{
			_listCommandSurface.ForEach(delegate(UICommandSurface x)
			{
				x.RemoveCommandUnit();
			});
			selectIndex = 0;
			_listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				if (x.index == selectIndex)
				{
					x.Magnify();
				}
				else
				{
					x.Reduction();
				}
			});
		}

		public void AbsodedUnitIcon2FocusSurface()
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			UICommandUnitIcon focusUnitIcon = prodBattleCommandSelect.commandUnitList.focusUnitIcon;
			_listCommandSurface[selectIndex].Absorded(focusUnitIcon);
		}

		public void SetFocusUnitIcon2FocusSurface()
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			UICommandUnitIcon focusUnitIcon = prodBattleCommandSelect.commandUnitList.focusUnitIcon;
			_listCommandSurface[selectIndex].SetCommandUnit(focusUnitIcon);
		}

		public void ReductionAll()
		{
			_listICommandSurface.ForEach(delegate(IUICommandSurface x)
			{
				x.Reduction();
			});
		}

		public void ChkAllSurfaceSet()
		{
			if (listCommandSurfaces.All((UICommandSurface x) => x.isSetUnit))
			{
				_uiBattleStartBtn.isEnabled = true;
			}
			else
			{
				_uiBattleStartBtn.isEnabled = false;
			}
		}

		public IEnumerator PlayShowAnimation()
		{
			_clsBackgrounds.Show(_strParams.backgroundsShowTime, _strParams.backgroundsShowEase);
			yield return new WaitForSeconds(0.033f);
			_listCommandSurface.ForEach(delegate(UICommandSurface x)
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // x.transform.LTMoveLocal(this._listCommandSurfacePos[base._003Ccnt_003E__0], this._strParams.surfaceShowMoveTime).setOnStart(delegate
				// {
				//	KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_942);
				// }).setEase(this._strParams.surfaceShowEase)
				// 	.setDelay(this._strParams.surfaceShowInterval * (float)base._003Ccnt_003E__0);
				// base._003Ccnt_003E__0++;
			});
			yield return new WaitForSeconds(0.901f);
			_uiBattleStartBtn.Show();
		}

		public bool DecideStartBattle()
		{
			List<BattleCommand> val = (from x in _listCommandSurface
				select x.commandType).ToList();
			bool flag = Dlg.Call(ref _actDecideBattleStart, val);
			if (flag)
			{
				_uiBattleStartBtn.PlayDecide();
				_uiBattleStartBtn.button.isEnabled = false;
				_listCommandSurface.ForEach(delegate(UICommandSurface x)
				{
					x.boxCollider2D.enabled = false;
				});
			}
			return flag;
		}
	}
}
