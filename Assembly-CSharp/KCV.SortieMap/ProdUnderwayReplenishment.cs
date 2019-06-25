using Common.Struct;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdUnderwayReplenishment : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float showTime;

			public LeanTweenType showEaseType;

			public float hideTime;

			public LeanTweenType hideEaseType;

			public Vector3 showFleetOilerPos;

			public Vector3 hideFleetOilerPos;

			public Vector3 showTargetShipPos;

			public Vector3 hideTargetShipPos;

			public Vector3 startOilerTargetParticlePos;

			public Vector3 endOilerTargetParticlePos;

			public float oilerTargetParticleMoveTime;

			public void Dispose()
			{
				Mem.Del(ref showTime);
				Mem.Del(ref showEaseType);
				Mem.Del(ref hideTime);
				Mem.Del(ref hideEaseType);
				Mem.Del(ref showFleetOilerPos);
				Mem.Del(ref hideFleetOilerPos);
				Mem.Del(ref showTargetShipPos);
				Mem.Del(ref hideTargetShipPos);
				Mem.Del(ref startOilerTargetParticlePos);
				Mem.Del(ref endOilerTargetParticlePos);
				Mem.Del(ref oilerTargetParticleMoveTime);
			}
		}

		[SerializeField]
		private List<UITexture> _listShips;

		[SerializeField]
		private ParticleSystem _psFleetOilerMove;

		[SerializeField]
		private ParticleSystem _psOilerTargetHeal;

		[Header("[Animation Parameter]")]
		[SerializeField]
		private Params _strParams = default(Params);

		private List<System.Tuple<UITexture, ShipModel>> _listShipInfos;

		private UIPanel _uiPanel;

		private bool _isPlaying;

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public bool isPlaying
		{
			get
			{
				return _isPlaying;
			}
			private set
			{
				_isPlaying = value;
			}
		}

		public static ProdUnderwayReplenishment Instantiate(ProdUnderwayReplenishment prefab, Transform parent, MapSupplyModel model)
		{
			ProdUnderwayReplenishment prodUnderwayReplenishment = UnityEngine.Object.Instantiate(prefab);
			prodUnderwayReplenishment.transform.parent = parent;
			prodUnderwayReplenishment.transform.localScaleOne();
			prodUnderwayReplenishment.transform.localPositionZero();
			prodUnderwayReplenishment.Init(model);
			return prodUnderwayReplenishment;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe(ref _listShips);
			Mem.DelComponentSafe<ParticleSystem>(ref _psFleetOilerMove);
			Mem.DelComponentSafe<ParticleSystem>(ref _psOilerTargetHeal);
			Mem.DelIDisposableSafe(ref _strParams);
			if (_listShipInfos != null)
			{
				_listShipInfos.ForEach(delegate(System.Tuple<UITexture, ShipModel> x)
				{
					x = null;
				});
			}
			Mem.DelListSafe(ref _listShipInfos);
			Mem.Del(ref _uiPanel);
		}

		private bool Init(MapSupplyModel model)
		{
			isPlaying = false;
			_listShipInfos = new List<System.Tuple<UITexture, ShipModel>>(2);
			SetShipTexture(model.Ship, model.GivenShips);
			InitParticle();
			panel.widgetsAreStatic = true;
			return true;
		}

		private void SetShipTexture(ShipModel fleetOiler, List<ShipModel> targetShips)
		{
			_listShips[0].mainTexture = ShipUtils.LoadTexture(fleetOiler);
			_listShips[0].MakePixelPerfect();
			Point cutinSp1_InBattle = fleetOiler.Offsets.GetCutinSp1_InBattle(fleetOiler.IsDamaged());
			_listShips[0].transform.localPosition = new Vector3(cutinSp1_InBattle.x, cutinSp1_InBattle.y, 0f);
			_listShips[0].transform.parent.localPosition = _strParams.hideFleetOilerPos;
			_listShipInfos.Add(new System.Tuple<UITexture, ShipModel>(_listShips[0], fleetOiler));
			_listShips[1].mainTexture = ShipUtils.LoadTexture(targetShips[0]);
			_listShips[1].MakePixelPerfect();
			Point cutinSp1_InBattle2 = targetShips[0].Offsets.GetCutinSp1_InBattle(targetShips[0].IsDamaged());
			_listShips[1].transform.localPosition = new Vector3(cutinSp1_InBattle2.x, cutinSp1_InBattle2.y, 0f);
			_listShips[1].transform.parent.localPosition = _strParams.hideTargetShipPos;
			_listShipInfos.Add(new System.Tuple<UITexture, ShipModel>(_listShips[1], targetShips[0]));
			_listShipInfos.ForEach(delegate(System.Tuple<UITexture, ShipModel> x)
			{
				x.Item1.alpha = 0f;
			});
		}

		private void InitParticle()
		{
			((Component)_psFleetOilerMove).transform.localPosition = new Vector3(0f, 150f, 0f);
			((Component)_psFleetOilerMove).SetActive(isActive: false);
			((Component)_psOilerTargetHeal).transform.localPosition = _strParams.startOilerTargetParticlePos;
			((Component)_psOilerTargetHeal).SetActive(isActive: false);
		}

		public UniRx.IObservable<bool> Play()
		{
			return Observable.FromCoroutine((UniRx.IObserver<bool> observer) => AnimationObserver(observer));
		}

		private IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			if (isPlaying)
			{
				observer.OnNext(value: true);
				observer.OnCompleted();
				isPlaying = false;
			}
			isPlaying = true;
			panel.widgetsAreStatic = false;
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			UISortieShip uisp = SortieMapTaskManager.GetUIMapManager().sortieShip;
			bool isWaitTimer2 = false;
			bool isWaitVoiceFinished2 = false;
			bool isWaitAnimation4 = false;
			uiamf.SetMessage("艦隊に洋上補給を行います。");
			yield return new WaitForEndOfFrame();
			uisp.PlayBalloon(delegate
			{
                throw new NotImplementedException("なにこれ");
                // base._003CisWaitAnimation_003E__4 = true;
			});
			while (!isWaitAnimation4)
			{
				yield return null;
			}
			isWaitAnimation4 = false;
			LeanTween.delayedCall(1f, (Action)delegate
			{
                throw new NotImplementedException("なにこれ");
                // base._003CisWaitTimer_003E__2 = true;
			});
			LeanTween.delayedCall(1f, (Action)delegate
			{
                throw new NotImplementedException("なにこれ");
                // ShipUtils.PlayShipVoice(this._listShipInfos[1].Item2, 26, delegate
				//{
				//	base._003CisWaitVoiceFinished_003E__3 = true;
				//});
			}).setOnStart(delegate
			{
                throw new NotImplementedException("なにこれ");
                //this._listShipInfos[1].Item1.transform.LTValue(this._listShipInfos[1].Item1.alpha, 1f, this._strParams.showTime).setEase(this._strParams.showEaseType).setOnUpdate(delegate(float x)
				//{
				//	this._listShipInfos[1].Item1.alpha = x;
				//})
				//	.setOnComplete((Action)delegate
				//	{
				//		base._003CisWaitAnimation_003E__4 = true;
				//	});
				//this._listShipInfos[1].Item1.transform.parent.LTMoveLocalX(this._strParams.showTargetShipPos.x, this._strParams.showTime).setEase(this._strParams.hideEaseType);
			});
			while (!isWaitTimer2 || !isWaitVoiceFinished2 || !isWaitAnimation4)
			{
				yield return null;
			}
			isWaitTimer2 = false;
			isWaitVoiceFinished2 = false;
			isWaitAnimation4 = false;
			LeanTween.delayedCall(1f, (Action)delegate
			{
                throw new NotImplementedException("なにこれ");
                // base._003CisWaitTimer_003E__2 = true;
			});
			LeanTween.delayedCall(0.5f, (Action)delegate
			{
                throw new NotImplementedException("なにこれ");
                // ShipUtils.PlayShipVoice(this._listShipInfos[0].Item2, 26, delegate
				//{
				//	base._003CisWaitVoiceFinished_003E__3 = true;
				//});
			}).setOnStart(delegate
			{
                throw new NotImplementedException("なにこれ");
                //this._listShipInfos[0].Item1.transform.LTValue(this._listShipInfos[0].Item1.alpha, 1f, this._strParams.showTime).setEase(this._strParams.showEaseType).setOnUpdate(delegate(float x)
				//{
				//	this._listShipInfos[0].Item1.alpha = x;
				//})
				//	.setOnComplete((Action)delegate
				//	{
				//		base._003CisWaitAnimation_003E__4 = true;
				//	});
				//this._listShipInfos[0].Item1.transform.parent.LTMoveLocalX(this._strParams.showFleetOilerPos.x, this._strParams.showTime).setEase(this._strParams.hideEaseType).setOnStart(delegate
				//{
				//	((Component)this._psFleetOilerMove).SetActive(isActive: true);
				//	this._psFleetOilerMove.Play();
				//});
			});
			while (!isWaitTimer2 || !isWaitVoiceFinished2 || !isWaitAnimation4)
			{
				yield return null;
			}
			isWaitAnimation4 = false;
			((Component)_psOilerTargetHeal).transform.LTMoveLocal(_strParams.endOilerTargetParticlePos, _strParams.oilerTargetParticleMoveTime).setDelay(0.5f).setOnStart(delegate
			{
                throw new NotImplementedException("なにこれ");
                // ((Component)this._psOilerTargetHeal).SetActive(isActive: true);
				// this._psOilerTargetHeal.Play();
			})
				.setOnComplete((Action)delegate
				{
                    throw new NotImplementedException("なにこれ");
                    // base._003CisWaitAnimation_003E__4 = true;
				});
			while (!isWaitAnimation4)
			{
				yield return null;
			}
			yield return new WaitForSeconds(0.5f);
			Hide(_listShipInfos[0], _strParams.hideFleetOilerPos);
			Hide(_listShipInfos[1], _strParams.hideTargetShipPos);
			yield return new WaitForSeconds(0.5f);
			observer.OnNext(value: true);
			observer.OnCompleted();
			uiamf.ClearMessage();
			panel.widgetsAreStatic = true;
			isPlaying = false;
		}

		private LTDescr Hide(System.Tuple<UITexture, ShipModel> target, Vector3 vHidePos)
		{
			target.Item1.transform.LTValue(target.Item1.alpha, 0f, _strParams.hideTime).setEase(_strParams.hideEaseType).setOnUpdate(delegate(float x)
			{
				target.Item1.alpha = x;
			});
			return target.Item1.transform.parent.LTMoveLocalX(vHidePos.x, _strParams.hideTime).setEase(_strParams.hideEaseType);
		}
	}
}
