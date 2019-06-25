using Common.Enum;
using KCV.SortieBattle;
using KCV.Utils;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(CircleCollider2D))]
	[RequireComponent(typeof(UISprite))]
	public class UISortieMapCell : MonoBehaviour
	{
		private bool _isActiveBranchingTarget;

		private bool _isFocus2ActiveBranching;

		private UISprite _uiCell;

		private UISprite _uiGlowCell;

		private UISprite _uiFocusCell;

		private UIToggle _uiActiveBranchingToggle;

		private CellModel _clsCellModel;

		private ProdShipRipple _prodRipple;

		private CircleCollider2D _colCircle2D;

		private Vector3 _vOriginPos;

		public CellModel cellModel => _clsCellModel;

		private int colorNo
		{
			set
			{
				_uiCell.spriteName = $"mapIcon_color_{value}";
				_uiCell.MakePixelPerfect();
				if (value == 5)
				{
					base.transform.localPositionY(_vOriginPos.y + 5f);
				}
			}
		}

		private new CircleCollider2D collider => this.GetComponentThis(ref _colCircle2D);

		public bool isPassedCell
		{
			set
			{
				if (_clsCellModel != null)
				{
					if (value)
					{
						SetPassedCellColor();
					}
					else
					{
						SetStartPassedState(_clsCellModel);
					}
				}
			}
		}

		public bool isActiveBranchingTarget
		{
			get
			{
				return _isActiveBranchingTarget;
			}
			set
			{
				if (value)
				{
					bool flag = true;
					collider.enabled = flag;
					_isActiveBranchingTarget = flag;
					SetActiveBranchingTargetCell(isMake: true);
					PlayActiveBranchingGlow();
				}
				else
				{
					bool flag = false;
					collider.enabled = flag;
					_isActiveBranchingTarget = flag;
					SetActiveBranchingTargetCell(isMake: false);
					StopActiveBranchingGlow();
				}
			}
		}

		public bool isFocus2ActiveBranching
		{
			get
			{
				return _isFocus2ActiveBranching;
			}
			set
			{
				if (value)
				{
					_isFocus2ActiveBranching = true;
					_uiActiveBranchingToggle.value = true;
					_uiFocusCell.transform.LTCancel();
					_uiFocusCell.transform.LTValue(_uiFocusCell.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						_uiFocusCell.alpha = x;
					});
					_uiFocusCell.transform.localScaleOne();
					_uiFocusCell.transform.LTScale(Vector3.one * 1.15f, 0.75f).setEase(LeanTweenType.linear).setLoopClamp();
				}
				else
				{
					_isFocus2ActiveBranching = false;
					_uiActiveBranchingToggle.value = false;
					_uiFocusCell.transform.LTCancel();
					_uiFocusCell.transform.LTValue(_uiFocusCell.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						_uiFocusCell.alpha = x;
					});
				}
			}
		}

		public UISortieMapCell Startup()
		{
			_vOriginPos = base.transform.localPosition;
			this.GetComponentThis(ref _uiCell);
			collider.offset = Vector2.zero;
			collider.radius = 30f;
			collider.enabled = false;
			return this;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiCell);
			Mem.Del(ref _uiGlowCell);
			Mem.Del(ref _uiFocusCell);
			Mem.Del(ref _uiActiveBranchingToggle);
			Mem.Del(ref _clsCellModel);
			Mem.Del(ref _prodRipple);
			Mem.Del(ref _colCircle2D);
			Mem.Del(ref _isActiveBranchingTarget);
		}

		public bool Init(CellModel cellModel)
		{
			_clsCellModel = cellModel;
			SetStartPassedState(cellModel);
			return true;
		}

		public void SetPassedDefaultColor()
		{
			enumMapEventType eventType = _clsCellModel.EventType;
			if (eventType == enumMapEventType.Stupid)
			{
				colorNo = 4;
			}
			ChkLinkCellAfterPassed();
		}

		private void SetStartPassedState(CellModel model)
		{
			bool isLinkPassed = IsAnyLinkCellPassed(model.GetLinkNo());
			switch (model.EventType)
			{
			case enumMapEventType.NOT_USE:
				_uiCell.spriteName = string.Empty;
				break;
			case enumMapEventType.None:
				SetStartCellColor(model, isLinkPassed, 1, 0);
				break;
			case enumMapEventType.ItemGet:
				SetStartCellColor(model, isLinkPassed, 2, 0);
				break;
			case enumMapEventType.Uzushio:
				SetStartCellColor(model, isLinkPassed, 3, 0);
				break;
			case enumMapEventType.War_Normal:
				SetStartCellColor(model, isLinkPassed, 4, 0);
				break;
			case enumMapEventType.War_Boss:
				SetStartCellColor(model, isLinkPassed, 5, 4);
				break;
			case enumMapEventType.Stupid:
				SetStartCellColor(model, isLinkPassed, 4, 0);
				break;
			case enumMapEventType.PortBackEo:
				SetStartCellColor(model, isLinkPassed, 8, 8);
				break;
			case enumMapEventType.AirReconnaissance:
				SetStartCellColor(model, isLinkPassed, 9, 0);
				break;
			}
		}

		private bool IsAnyLinkCellPassed(List<int> linkList)
		{
			bool result = false;
			if (linkList.Count != 0)
			{
				MapManager mapManager = SortieBattleTaskManager.GetMapManager();
				{
					foreach (int link in linkList)
					{
						if (mapManager.Cells[link].Passed)
						{
							return true;
						}
					}
					return result;
				}
			}
			return result;
		}

		private void SetStartCellColor(CellModel cellModel, bool isLinkPassed, int nPassedColor, int nNonPassedColor)
		{
			if (isLinkPassed)
			{
				colorNo = nPassedColor;
			}
			else
			{
				colorNo = ((!cellModel.Passed) ? nNonPassedColor : nPassedColor);
			}
		}

		private void SetPassedCellColor()
		{
			switch (_clsCellModel.EventType)
			{
			case enumMapEventType.NOT_USE:
				_uiCell.spriteName = string.Empty;
				break;
			case enumMapEventType.None:
				colorNo = 1;
				break;
			case enumMapEventType.ItemGet:
				colorNo = 2;
				break;
			case enumMapEventType.Uzushio:
				colorNo = 3;
				break;
			case enumMapEventType.War_Normal:
				colorNo = ((_clsCellModel.WarType != enumMapWarType.AirBattle) ? 4 : 7);
				break;
			case enumMapEventType.War_Boss:
				colorNo = 5;
				break;
			case enumMapEventType.Stupid:
				colorNo = 1;
				break;
			case enumMapEventType.PortBackEo:
				colorNo = 8;
				break;
			case enumMapEventType.AirReconnaissance:
				colorNo = 9;
				break;
			}
			ChkLinkCellAfterPassed();
		}

		private void ChkLinkCellAfterPassed()
		{
			if (_clsCellModel.GetLinkNo().Count != 0)
			{
				UIMapManager uimm = SortieMapTaskManager.GetUIMapManager();
				_clsCellModel.GetLinkNo().ForEach(delegate(int x)
				{
					uimm.cells[x].SetActive(isActive: false);
				});
				_uiCell.depth++;
			}
		}

		public void PlayMailstrom(UISortieShip sortieShip, MapEventHappeningModel eventHappeningModel, Action onFinished)
		{
			ProdShipRipple component = Util.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabProdShipRipple.gameObject, base.transform.gameObject).GetComponent<ProdShipRipple>();
			ProdMailstrom prodMailstrom = ProdMailstrom.Instantiate(((Component)SortieMapTaskManager.GetPrefabFile().prefabProdMaelstrom).GetComponent<ProdMailstrom>(), base.transform, eventHappeningModel);
			prodMailstrom.PlayMailstrom(sortieShip, component, onFinished);
		}

		public void PlayRipple(Color color)
		{
			_prodRipple = Util.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabProdShipRipple.gameObject, base.transform.gameObject).GetComponent<ProdShipRipple>();
			_prodRipple.Play(color);
			SoundUtils.PlaySE(SEFIleInfos.SE_032);
			Observable.Timer(TimeSpan.FromSeconds(1.5)).Subscribe(delegate
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_032);
			});
		}

		public void StopRipple()
		{
			_prodRipple.Stop();
		}

		private void SetActiveBranchingTargetCell(bool isMake)
		{
			if (isMake)
			{
				if (_uiFocusCell == null)
				{
					GameObject gameObject = new GameObject("ActiveBranchingFocusCell");
					_uiFocusCell = gameObject.AddComponent<UISprite>();
					_uiFocusCell.transform.parent = base.transform;
					_uiFocusCell.atlas = _uiCell.atlas;
					_uiFocusCell.alpha = 0f;
					_uiFocusCell.spriteName = "sail_mapIcon_white_30";
					_uiFocusCell.MakePixelPerfect();
					_uiFocusCell.transform.localScaleOne();
					_uiFocusCell.transform.localPosition = new Vector3(-1f, -0.5f, 0f);
				}
				if (_uiGlowCell == null)
				{
					GameObject gameObject2 = new GameObject("ActiveBranchingGlow");
					_uiGlowCell = gameObject2.AddComponent<UISprite>();
					_uiGlowCell.transform.parent = base.transform;
					_uiGlowCell.transform.localScaleOne();
					_uiGlowCell.transform.localPositionZero();
					_uiGlowCell.atlas = _uiCell.atlas;
					_uiGlowCell.alpha = 0f;
					_uiGlowCell.depth = _uiCell.depth - 2;
					_uiGlowCell.spriteName = "mapIcon_activebranching_glow";
					_uiGlowCell.MakePixelPerfect();
				}
				if (_uiActiveBranchingToggle == null)
				{
					_uiActiveBranchingToggle = this.AddComponent<UIToggle>();
					_uiActiveBranchingToggle.group = 10;
				}
			}
			else
			{
				if (_uiFocusCell != null)
				{
					_uiFocusCell.transform.LTCancel();
				}
				Mem.DelComponent(ref _uiFocusCell);
				if (_uiActiveBranchingToggle != null)
				{
					_uiActiveBranchingToggle.onActive.Clear();
					_uiActiveBranchingToggle.onDecide = null;
					UnityEngine.Object.Destroy(_uiActiveBranchingToggle);
				}
				Mem.Del(ref _uiActiveBranchingToggle);
			}
		}

		public void SetOnDecideActiveBranchingTarget(int nIndex, Action<int> onActive, Action<UISortieMapCell> onDecide)
		{
			_uiActiveBranchingToggle.onActive.Clear();
			_uiActiveBranchingToggle.onDecide = null;
			_uiActiveBranchingToggle.onActive.Add(new EventDelegate(delegate
			{
				Dlg.Call(ref onActive, nIndex);
			}));
			_uiActiveBranchingToggle.onDecide = delegate
			{
				Dlg.Call(ref onDecide, this);
			};
		}

		private void PlayActiveBranchingGlow()
		{
			_uiGlowCell.transform.LTValue(0f, 0.5f, 0.85f).setLoopPingPong().setEase(LeanTweenType.linear)
				.setOnUpdate(delegate(float x)
				{
					_uiGlowCell.alpha = x;
				});
		}

		private void StopActiveBranchingGlow()
		{
			if (_uiGlowCell != null)
			{
				_uiGlowCell.transform.LTCancel();
				_uiGlowCell.transform.LTValue(_uiGlowCell.alpha, 0f, 0.25f).setOnUpdate(delegate(float x)
				{
					_uiGlowCell.alpha = x;
				}).setEase(LeanTweenType.linear)
					.setOnComplete((Action)delegate
					{
						Mem.DelComponent(ref _uiGlowCell);
					});
			}
		}
	}
}
