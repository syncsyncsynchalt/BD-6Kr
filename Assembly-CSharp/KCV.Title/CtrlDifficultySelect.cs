using Common.Enum;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlDifficultySelect : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabUIDifficultBtn;

		[SerializeField]
		private UIWritingBrush _prefabUIWritingBrush;

		[SerializeField]
		private UIGrid _uiGridBtns;

		[SerializeField]
		private UIGrid _uiGridDifficulty;

		[SerializeField]
		private List<float> _listArrowLength;

		[SerializeField]
		private UISprite _uiArrow;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UILabel _uiDescription;

		[SerializeField]
		private UIInvisibleCollider _uiInvisibleCollider;

		private UIPanel _uiPanel;

		private bool _isInputPossible;

		private int _nIndex;

		private Action _actOnCancel;

		private Action<DifficultKind> _actOnDecideDifficulty;

		private List<UIDifficultyBtn> _listDifficultyBtn;

		private KeyControl _clsInput;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static CtrlDifficultySelect Instantiate(CtrlDifficultySelect prefab, Transform parent, KeyControl input, HashSet<DifficultKind> difficulty, Action<DifficultKind> onDecideDifficulty, Action onCancel)
		{
			CtrlDifficultySelect ctrlDifficultySelect = UnityEngine.Object.Instantiate(prefab);
			ctrlDifficultySelect.transform.parent = parent;
			ctrlDifficultySelect.transform.localScaleOne();
			ctrlDifficultySelect.transform.localPositionZero();
			ctrlDifficultySelect.Init(input, difficulty, onDecideDifficulty, onCancel);
			return ctrlDifficultySelect;
		}

		private bool Init(KeyControl input, HashSet<DifficultKind> difficulty, Action<DifficultKind> onDecideDifficulty, Action onCancel)
		{
			panel.alpha = 0f;
			_actOnDecideDifficulty = onDecideDifficulty;
			_actOnCancel = onCancel;
			_nIndex = 0;
			_isInputPossible = false;
			_clsInput = input;
			CreateDifficultyBtns(GetEnabledDifficulty(difficulty));
			SetArrowLength(_listDifficultyBtn.Count);
			SetDifficultyLabelSpace(_listDifficultyBtn.Count);
			_listDifficultyBtn[0].isFocus = true;
			_uiInvisibleCollider.SetOnTouch(OnCancel);
			_uiInvisibleCollider.button.enabled = false;
			Show().setOnComplete((Action)delegate
			{
				_listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
				{
					x.toggle.enabled = true;
				});
				_uiInvisibleCollider.button.enabled = true;
				_isInputPossible = true;
			});
			return true;
		}

		private void CreateDifficultyBtns(List<DifficultKind> list)
		{
			_listDifficultyBtn = new List<UIDifficultyBtn>();
			int cnt = 0;
			list.ForEach(delegate(DifficultKind x)
			{
				_listDifficultyBtn.Add(UIDifficultyBtn.Instantiate(((Component)_prefabUIDifficultBtn).GetComponent<UIDifficultyBtn>(), _uiGridBtns.transform, x, cnt));
				_listDifficultyBtn[cnt].name = $"DifficultyBtn{(int)x}";
				_listDifficultyBtn[cnt].isFocus = false;
				_listDifficultyBtn[cnt].toggle.group = 1;
				_listDifficultyBtn[cnt].toggle.enabled = false;
				_listDifficultyBtn[cnt].toggle.onDecide = delegate
				{
					OnDecideDifficulty(_listDifficultyBtn[_nIndex].difficultKind);
				};
				_listDifficultyBtn[cnt].toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", cnt);
				if (_listDifficultyBtn[cnt].index == 0)
				{
					_listDifficultyBtn[cnt].toggle.startsActive = true;
				}
				cnt++;
			});
			_uiGridBtns.Reposition();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabUIDifficultBtn);
			Mem.Del(ref _uiGridBtns);
			Mem.Del(ref _uiGridDifficulty);
			Mem.DelListSafe(ref _listArrowLength);
			Mem.Del(ref _uiArrow);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiDescription);
			Mem.Del(ref _uiInvisibleCollider);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _isInputPossible);
			Mem.Del(ref _actOnCancel);
			Mem.Del(ref _actOnDecideDifficulty);
			Mem.DelListSafe(ref _listDifficultyBtn);
			Mem.Del(ref _clsInput);
		}

		public bool Run()
		{
			if (!_isInputPossible)
			{
				return true;
			}
			if (_clsInput == null)
			{
				return true;
			}
			if (_clsInput.GetDown(KeyControl.KeyName.RIGHT))
			{
				PreparaNext(isFoward: true);
			}
			else if (_clsInput.GetDown(KeyControl.KeyName.LEFT))
			{
				PreparaNext(isFoward: false);
			}
			else if (_clsInput.GetDown(KeyControl.KeyName.MARU))
			{
				OnDecideDifficulty(_listDifficultyBtn[_nIndex].difficultKind);
			}
			else if (_clsInput.GetDown(KeyControl.KeyName.BATU))
			{
				OnCancel();
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			int nIndex = _nIndex;
			_nIndex = Mathe.NextElement(_nIndex, 0, _listDifficultyBtn.Count - 1, isFoward);
			if (nIndex != _nIndex)
			{
				ChangeFocus(_nIndex, isPlaySE: true);
			}
		}

		private void SetArrowLength(int nDiffCnt)
		{
			_uiArrow.width = 436 + (nDiffCnt - 3) * 130;
		}

		private void SetDifficultyLabelSpace(int nDiffCnt)
		{
			_uiGridDifficulty.cellWidth = 380 + (nDiffCnt - 3) * 130;
			_uiGridDifficulty.Reposition();
		}

		private void ChangeFocus(int nIndex, bool isPlaySE)
		{
			if (isPlaySE)
			{
				TitleUtils.PlayDifficultyVoice(_listDifficultyBtn[_nIndex].difficultKind, isDecide: false, null);
			}
			_listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
			{
				x.isFocus = ((x.index == nIndex) ? true : false);
			});
		}

		private List<DifficultKind> GetEnabledDifficulty(HashSet<DifficultKind> difficulty)
		{
			List<DifficultKind> list = new List<DifficultKind>();
			if (difficulty.Contains(DifficultKind.TEI))
			{
				list.Add(DifficultKind.TEI);
			}
			if (difficulty.Contains(DifficultKind.HEI))
			{
				list.Add(DifficultKind.HEI);
			}
			if (difficulty.Contains(DifficultKind.OTU))
			{
				list.Add(DifficultKind.OTU);
			}
			if (difficulty.Contains(DifficultKind.KOU))
			{
				list.Add(DifficultKind.KOU);
			}
			if (difficulty.Contains(DifficultKind.SHI))
			{
				list.Add(DifficultKind.SHI);
			}
			return list;
		}

		private LTDescr Show()
		{
			TitleUtils.PlayOpenDifficultyVoice();
			return panel.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			_isInputPossible = false;
			_listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
			{
				x.toggle.enabled = false;
			});
			_uiInvisibleCollider.button.enabled = false;
			return panel.transform.LTValue(1f, 0f, 0.35f).setEase(LeanTweenType.easeInBack).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private IEnumerator PlayDecideAnimation(UniRx.IObserver<bool> observer)
		{
			yield return null;
			_listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
			{
				// _003CPlayDecideAnimation_003Ec__Iterator1A0 _003CPlayDecideAnimation_003Ec__Iterator1A = this;
				if (x.index == this._nIndex)
				{
					UIWritingBrush uIWritingBrush = UIWritingBrush.Instantiate(this._prefabUIWritingBrush.GetComponent<UIWritingBrush>(), this.transform);
					uIWritingBrush.transform.position = x.wrightBrushAnchor.position;
					uIWritingBrush.Play(delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
						{
                            observer.OnNext(value: true);
                            observer.OnCompleted();
						});
					});
					x.ShowDifficultyRedLabel();
				}
				else
				{
					x.transform.LTValue(1f, 0.5f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float y)
					{
						x.widget.alpha = y;
					});
				}
			});
		}

		private void OnCancel()
		{
			Hide().setOnComplete((Action)delegate
			{
				Dlg.Call(ref _actOnCancel);
			});
		}

		private void OnActive(int nIndex)
		{
			if (_nIndex != nIndex)
			{
				_nIndex = nIndex;
				ChangeFocus(_nIndex, isPlaySE: true);
			}
		}

		private void OnDecideDifficulty(DifficultKind iKind)
		{
			_listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
			{
				x.toggle.enabled = false;
			});
			_isInputPossible = false;
			_uiInvisibleCollider.button.enabled = false;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			TitleUtils.PlayDifficultyVoice(iKind, isDecide: true, delegate
			{
				Dlg.Call(ref _actOnDecideDifficulty, iKind);
			});
			Observable.FromCoroutine((UniRx.IObserver<bool> observer) => PlayDecideAnimation(observer)).Subscribe(delegate
			{
			}).AddTo(base.gameObject);
		}
	}
}
