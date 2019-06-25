using KCV.Battle.Production;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexBreak : MonoBehaviour
	{
		[SerializeField]
		private ExplodeChild[] _explodeChild;

		[SerializeField]
		private ParticleSystem _uiParticle;

		private void OnDestroy()
		{
			Mem.Del(ref _explodeChild);
		}

		private void init()
		{
			_explodeChild = new ExplodeChild[2];
			Util.FindParentToChild(ref _explodeChild[0], base.transform, "RHex1");
			Util.FindParentToChild(ref _explodeChild[1], base.transform, "RHex2");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticle, base.transform, "Ring");
		}

		private void LateUpdate()
		{
			for (int i = 0; i < 2; i++)
			{
				if (_explodeChild != null)
				{
					_explodeChild[i].LateRun();
				}
			}
		}

		public void Play(Action callback)
		{
			for (int i = 0; i < 2; i++)
			{
				_explodeChild[i].PlayAnimation().Subscribe(delegate
				{
					Dlg.Call(ref callback);
				}).AddTo(base.gameObject);
			}
			_uiParticle.Stop();
			_uiParticle.Play();
		}

		public static StrategyHexBreak Instantiate(StrategyHexBreak prefab, Transform fromParent)
		{
			StrategyHexBreak strategyHexBreak = UnityEngine.Object.Instantiate(prefab);
			strategyHexBreak.transform.parent = fromParent;
			strategyHexBreak.transform.localScale = Vector3.one;
			strategyHexBreak.transform.localPosition = new Vector3(0f, 0f, -1f);
			strategyHexBreak.init();
			return strategyHexBreak;
		}
	}
}
