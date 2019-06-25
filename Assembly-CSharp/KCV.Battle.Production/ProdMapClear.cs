using KCV.Battle.Utils;
using KCV.SortieBattle;
using KCV.Utils;
using local.models;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdMapClear : BaseAnimation
	{
		[SerializeField]
		private ParticleSystem _uiParticle;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private Animation _anime;

		private int _debugIndex;

		private bool _debugDamage;

		private bool _isControl;

		private KeyControl _keyControl;

		private ShipModel_BattleAll _ship;

		private void _init()
		{
			_debugIndex = 0;
			_debugDamage = false;
			_isControl = false;
			_isFinished = false;
			Util.FindParentToChild<ParticleSystem>(ref _uiParticle, base.transform, "Particle");
			Util.FindParentToChild(ref _uiShip, base.transform, "ShipObj/Ship");
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = GetComponent<Animation>();
			}
			((Component)_uiParticle).SetActive(isActive: false);
			_anime.Stop();
		}

		private new void OnDestroy()
		{
			Mem.Del(ref _uiParticle);
			Mem.Del(ref _uiShip);
			Mem.Del(ref _anime);
			_keyControl = null;
			_ship = null;
		}

		public bool Run()
		{
			if (_isControl && _keyControl != null && _keyControl.keyState[1].down)
			{
				onAnimationComp();
			}
			return _isFinished;
		}

		private void setShipTexture()
		{
			if (_ship != null)
			{
				GameObject gameObject = base.transform.FindChild("ShipObj").gameObject;
				ShipModel_BattleResult model = (ShipModel_BattleResult)_ship;
				float lovScaleMagnification = SortieBattleUtils.GetLovScaleMagnification(model);
				_uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(_ship.GetGraphicsMstId(), isDamaged: false);
				_uiShip.MakePixelPerfect();
				_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_ship.GetGraphicsMstId()).GetShipDisplayCenter(damaged: false));
				_uiShip.transform.localScale = new Vector3(lovScaleMagnification, lovScaleMagnification, 1f);
				float num = (lovScaleMagnification - 1f) * 120f;
				gameObject.transform.localPosition = new Vector3(1f, 140f - num, 1f);
			}
		}

		private void debugShipTexture()
		{
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(_debugIndex))
			{
				GameObject gameObject = base.transform.FindChild("ShipObj").gameObject;
				float num = 1.5f;
				_uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(_debugIndex, _debugDamage);
				_uiShip.MakePixelPerfect();
				_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_debugIndex).GetShipDisplayCenter(_debugDamage));
				_uiShip.transform.localScale = new Vector3(num, num, 1f);
				float num2 = (num - 1f) * 120f;
				gameObject.transform.localPosition = new Vector3(1f, 140f - num2, 1f);
			}
		}

		public override void Play(Action callback)
		{
			_actCallback = callback;
			if (_ship == null)
			{
				onAnimationComp();
			}
			_animAnimation.Stop();
			_animAnimation.Play("MapClear");
			((Component)_uiParticle).SetActive(isActive: true);
			_uiParticle.Stop();
			_uiParticle.Play();
		}

		private void startControl()
		{
			_isControl = true;
		}

		private void _playAnimationSE(int num)
		{
			switch (num)
			{
			case 0:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.ClearAA);
				break;
			case 1:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_053);
				break;
			}
		}

		private void onAnimationComp()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
			_isFinished = true;
		}

		public static ProdMapClear Instantiate(ProdMapClear prefab, Transform parent, KeyControl keyControl, ShipModel_BattleAll ship, int nPanelDepth)
		{
			ProdMapClear prodMapClear = UnityEngine.Object.Instantiate(prefab);
			prodMapClear.transform.parent = parent;
			prodMapClear.transform.localScale = Vector3.one;
			prodMapClear.transform.localPosition = Vector3.zero;
			prodMapClear._init();
			prodMapClear._keyControl = keyControl;
			prodMapClear._ship = ship;
			prodMapClear.setShipTexture();
			return prodMapClear;
		}
	}
}
