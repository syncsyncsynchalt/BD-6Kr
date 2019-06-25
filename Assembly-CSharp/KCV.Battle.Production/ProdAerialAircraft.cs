using KCV.Battle.Utils;
using local.managers;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialAircraft : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiAircraft;

		[SerializeField]
		public ParticleSystem _explosionParticle;

		[SerializeField]
		public ParticleSystem _smokeParticle;

		[SerializeField]
		private Animation _anime;

		private int _shipNum;

		private PlaneModelBase _plane;

		private Action _actCallback;

		public FleetType _fleetType;

		private int[] _baseNum;

		private Vector3[] _basePosition;

		public PlaneModelBase GetPlane()
		{
			return _plane;
		}

		public PlaneState GetPlaneState()
		{
			return _plane.State_Stage2End;
		}

		public void Init()
		{
			_basePosition = new Vector3[6]
			{
				new Vector3(101f, -481f, 0f),
				new Vector3(38f, -533f, 0f),
				new Vector3(105f, -419f, 0f),
				new Vector3(-39f, -581f, 0f),
				new Vector3(110f, -362f, 0f),
				new Vector3(-120f, -634f, 0f)
			};
			_baseNum = new int[6]
			{
				2,
				3,
				1,
				4,
				0,
				5
			};
			float num = 0.8f + 0.1f * (float)_baseNum[_shipNum];
			base.transform.localPosition = _basePosition[_shipNum];
			base.transform.localScale = new Vector3(num, num, num);
			GameObject gameObject = base.transform.FindChild("AircraftObj").gameObject;
			Util.FindParentToChild(ref _uiAircraft, gameObject.transform, "Aircraft");
			Util.FindParentToChild<ParticleSystem>(ref _explosionParticle, _uiAircraft.transform, "AircraftExplosion");
			Util.FindParentToChild<ParticleSystem>(ref _smokeParticle, _uiAircraft.transform, "AircraftSmoke");
			_uiAircraft.depth = 5 + _baseNum[_shipNum];
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = ((Component)_uiAircraft.GetComponent<Animation>()).GetComponent<Animation>();
			}
			if (_fleetType == FleetType.Friend)
			{
				_uiAircraft.flip = UIBasicSprite.Flip.Nothing;
				_uiAircraft.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(_plane.MstId, 6);
				_uiAircraft.transform.localScale = Vector3.one;
			}
			else if (_fleetType == FleetType.Enemy)
			{
				_uiAircraft.flip = UIBasicSprite.Flip.Nothing;
				if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
				{
					_uiAircraft.flip = UIBasicSprite.Flip.Horizontally;
					_uiAircraft.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(_plane.MstId, 6);
					_uiAircraft.transform.localScale = Vector3.one;
				}
				else
				{
					_uiAircraft.mainTexture = SlotItemUtils.LoadTexture(_plane);
					_uiAircraft.MakePixelPerfect();
					_uiAircraft.transform.localScale = ((_plane.MstId > 500) ? new Vector3(0.7f, 0.7f, 0.7f) : new Vector3(0.8f, 0.8f, 0.8f));
				}
			}
			LeanTweenExtesntions.LTMoveLocal(to: new Vector3(_basePosition[_shipNum].x, _basePosition[_shipNum].y + 544f, _basePosition[_shipNum].z), self: base.transform, time: 0.8f).setEase(LeanTweenType.easeOutBack).setDelay(0.4f + 0.1f * (float)_baseNum[_shipNum]);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiAircraft);
			Mem.Del(ref _explosionParticle);
			Mem.Del(ref _smokeParticle);
			Mem.Del(ref _anime);
			Mem.Del(ref _uiAircraft);
			Mem.DelAry(ref _baseNum);
			Mem.DelAry(ref _basePosition);
			Mem.Del(ref _plane);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _fleetType);
		}

		public void Injection(Action callback)
		{
			if (_plane.State_Stage2End == PlaneState.Crush)
			{
				_anime.Stop();
				_anime.Play((_fleetType != 0) ? "AircraftBurstEnemy" : "AircraftBurstFriend");
				_explosionParticle.Play();
			}
		}

		public void Stop()
		{
			_anime.Stop();
			_explosionParticle.time = 0f;
			_smokeParticle.time = 0f;
			_explosionParticle.Stop();
			_smokeParticle.Stop();
		}

		public void alphaOut()
		{
			_uiAircraft.alpha = 0f;
		}

		public void SubPlay()
		{
			_anime.Stop();
			_anime.Play("AircraftNormal");
		}

		public void EndMove(float toX, float time)
		{
			Transform transform = base.transform;
			Vector3 localPosition = base.transform.localPosition;
			float x = localPosition.x + toX;
			Vector3 localPosition2 = base.transform.localPosition;
			float y = localPosition2.y;
			Vector3 localPosition3 = base.transform.localPosition;
			transform.LTMoveLocal(new Vector3(x, y, localPosition3.z), time).setDelay(0.4f + 0.1f * (float)_baseNum[_shipNum]).setEase(LeanTweenType.easeInCubic);
		}

		private void _onFinishedInjection()
		{
		}

		private IEnumerator _delayDiscard(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (_actCallback != null)
			{
				_actCallback();
			}
			yield return new WaitForSeconds(0.001f);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public static ProdAerialAircraft Instantiate(ProdAerialAircraft _aerial, Transform fromParent, int number, int nDepth, PlaneModelBase plane, FleetType fleetType)
		{
			ProdAerialAircraft prodAerialAircraft = UnityEngine.Object.Instantiate(_aerial);
			prodAerialAircraft.transform.parent = fromParent;
			prodAerialAircraft.transform.localScale = Vector3.one;
			prodAerialAircraft._shipNum = number;
			prodAerialAircraft._plane = plane;
			prodAerialAircraft._fleetType = fleetType;
			prodAerialAircraft.Init();
			return prodAerialAircraft;
		}
	}
}
