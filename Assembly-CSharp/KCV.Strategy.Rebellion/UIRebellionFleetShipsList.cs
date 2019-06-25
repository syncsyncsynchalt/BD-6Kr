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
	public class UIRebellionFleetShipsList : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabRebellionShipBanner;

		[SerializeField]
		private float _fStartOffs = -850f;

		[SerializeField]
		private Vector3 _vOriginPos = new Vector3(1000f, 201f, 0f);

		private List<UIRebellionOrgaizeShipBanner> _listShipBanners;

		private List<Vector3> _listBannerPos;

		public UIPanel panel => GetComponent<UIPanel>();

		public static UIRebellionFleetShipsList Instantiate(UIRebellionFleetShipsList prefab, Transform parent)
		{
			UIRebellionFleetShipsList uIRebellionFleetShipsList = UnityEngine.Object.Instantiate(prefab);
			uIRebellionFleetShipsList.transform.parent = parent;
			uIRebellionFleetShipsList.transform.localPositionZero();
			uIRebellionFleetShipsList.transform.localScaleOne();
			return uIRebellionFleetShipsList;
		}

		private void Awake()
		{
			base.transform.localPosition = _vOriginPos;
			_listBannerPos = new List<Vector3>
			{
				new Vector3(-337f, 66f, 0f),
				new Vector3(-1f, 66f, 0f),
				new Vector3(-337f, -64f, 0f),
				new Vector3(-1f, -64f, 0f),
				new Vector3(-337f, -194f, 0f),
				new Vector3(-1f, -194f, 0f)
			};
			Observable.FromCoroutine(CreaetShipBanner).Subscribe().AddTo(base.gameObject);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabRebellionShipBanner);
			Mem.Del(ref _fStartOffs);
			Mem.Del(ref _vOriginPos);
			Mem.DelListSafe(ref _listShipBanners);
			Mem.DelListSafe(ref _listBannerPos);
		}

		public bool Init(DeckModel detailDeck)
		{
			List<ShipModel> list = new List<ShipModel>(detailDeck.GetShips(_listShipBanners.Count));
			int cnt = 0;
			list.ForEach(delegate(ShipModel x)
			{
				if (x != null)
				{
					_listShipBanners[cnt].SetShipData(x, detailDeck.GetShipIndex(x.MemId) + 1);
				}
				else
				{
					_listShipBanners[cnt].SetShipData(x, -1);
				}
				cnt++;
			});
			return false;
		}

		public void Show(Action onFinished)
		{
			panel.widgetsAreStatic = false;
			_listShipBanners.ForEach(delegate(UIRebellionOrgaizeShipBanner x)
			{
				Action onComplete = null;
				if (x.index == _listShipBanners.Count - 1)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate
						{
							Dlg.Call(ref onFinished);
							panel.widgetsAreStatic = true;
						});
					};
				}
				Vector3 to = _listBannerPos[x.index];
				to.x += _fStartOffs;
				x.transform.LTMoveLocal(to, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setDelay((float)x.index * 0.03f)
					.setOnComplete(onComplete);
			});
		}

		public void Hide(Action onFinished)
		{
			panel.widgetsAreStatic = false;
			_listShipBanners.ForEach(delegate(UIRebellionOrgaizeShipBanner x)
			{
				Action onComplete = null;
				if (x.index == _listShipBanners.Count - 1)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate
						{
							Dlg.Call(ref onFinished);
							panel.widgetsAreStatic = true;
						});
					};
				}
				x.transform.LTMoveLocal(_listBannerPos[x.index], 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setDelay((float)x.index * 0.03f)
					.setOnComplete(onComplete);
			});
		}

		private IEnumerator CreaetShipBanner()
		{
			_listShipBanners = new List<UIRebellionOrgaizeShipBanner>();
			if (_listBannerPos != null)
			{
				for (int i = 0; i < _listBannerPos.Count; i++)
				{
					_listShipBanners.Add(UIRebellionOrgaizeShipBanner.Instantiate(((Component)_prefabRebellionShipBanner).GetComponent<UIRebellionOrgaizeShipBanner>(), base.transform, i));
					_listShipBanners[i].transform.localPosition = _listBannerPos[i];
					_listShipBanners[i].transform.name = $"RebellionOrgaizeShipBanner{i}";
					yield return null;
				}
				yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
				panel.widgetsAreStatic = true;
			}
		}
	}
}
