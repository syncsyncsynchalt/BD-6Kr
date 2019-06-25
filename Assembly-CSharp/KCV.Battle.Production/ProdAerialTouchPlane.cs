using KCV.Battle.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialTouchPlane : MonoBehaviour
	{
		[SerializeField]
		private GameObject _uiGameObjF;

		[SerializeField]
		private GameObject _uiGameObjE;

		[SerializeField]
		private UITexture _uiAircraftF;

		[SerializeField]
		private UITexture _uiAircraftE;

		private Action _actCallback;

		private int _shipNum;

		public void Init(SlotitemModel_Battle slotModelF, SlotitemModel_Battle slotModelE)
		{
			base.gameObject.SetActive(true);
			if (_uiGameObjF == null)
			{
				_uiGameObjF = base.transform.FindChild("AircraftF").gameObject;
			}
			if (_uiGameObjE == null)
			{
				_uiGameObjE = base.transform.FindChild("AircraftE").gameObject;
			}
			if (_uiAircraftF == null)
			{
				_uiAircraftF = ((Component)_uiGameObjF.transform.FindChild("Swing/Aircraft")).GetComponent<UITexture>();
			}
			if (_uiAircraftE == null)
			{
				_uiAircraftE = ((Component)_uiGameObjE.transform.FindChild("Swing/Aircraft")).GetComponent<UITexture>();
			}
			if (slotModelF != null)
			{
				_uiGameObjF.SetActive(true);
				_uiAircraftF.flip = UIBasicSprite.Flip.Nothing;
				_uiAircraftF.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotModelF.MstId, 6);
			}
			else
			{
				_uiGameObjF.SetActive(false);
			}
			if (slotModelE != null)
			{
				_uiGameObjE.SetActive(true);
				_uiAircraftE.flip = UIBasicSprite.Flip.Horizontally;
				if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
				{
					_uiAircraftE.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotModelE.MstId, 6);
					_uiAircraftE.MakePixelPerfect();
					return;
				}
				_uiAircraftE.mainTexture = SlotItemUtils.LoadTexture(slotModelE);
				_uiAircraftE.MakePixelPerfect();
				_uiAircraftE.transform.localScale = ((slotModelE.MstId > 500) ? new Vector3(0.7f, 0.7f, 0.7f) : new Vector3(0.8f, 0.8f, 0.8f));
				SlotItemUtils.GetAircraftOffsetInfo(slotModelE.MstId);
				_uiAircraftE.flip = UIBasicSprite.Flip.Nothing;
			}
			else
			{
				_uiGameObjE.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiGameObjF);
			Mem.Del(ref _uiGameObjE);
			Mem.Del(ref _uiAircraftF);
			Mem.Del(ref _uiAircraftE);
			Mem.Del(ref _actCallback);
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		private void _onFinishedInjection()
		{
		}

		public static ProdAerialTouchPlane Instantiate(ProdAerialTouchPlane _aerial, Transform fromParent)
		{
			ProdAerialTouchPlane prodAerialTouchPlane = UnityEngine.Object.Instantiate(_aerial);
			prodAerialTouchPlane.transform.parent = fromParent;
			prodAerialTouchPlane.transform.localScale = Vector3.one;
			prodAerialTouchPlane.transform.position = Vector3.zero;
			return prodAerialTouchPlane;
		}
	}
}
