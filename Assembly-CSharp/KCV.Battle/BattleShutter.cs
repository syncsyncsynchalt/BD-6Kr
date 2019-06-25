using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleShutter : BaseShutter
	{
		[SerializeField]
		private Animation _aninShutter;

		public static BattleShutter Instantiate(BattleShutter prefab, Transform parent, int nPanelDepth)
		{
			BattleShutter battleShutter = UnityEngine.Object.Instantiate(prefab);
			battleShutter.transform.parent = parent;
			battleShutter.transform.localScale = Vector3.one;
			battleShutter.transform.localPosition = Vector3.zero;
			battleShutter._uiPanel.depth = nPanelDepth;
			return battleShutter;
		}

		public static IEnumerator Instantiate(UniRx.IObserver<BattleShutter> observer, BattleShutter prefab, Transform parent, int nPanelDepth)
		{
			BattleShutter obj = UnityEngine.Object.Instantiate(prefab);
			obj.transform.parent = parent;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			yield return null;
			observer.OnNext(obj);
			observer.OnCompleted();
		}

		public override void ReqMode(ShutterMode iMode, Action callback)
		{
			if (iMode == ShutterMode.None || _iShutterMode == iMode)
			{
				return;
			}
			_actCallback = callback;
			if (!_isTween)
			{
				if (iMode == ShutterMode.Close)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_034);
				}
				_uiTop.transform.LTMoveLocal(_vTopPos[(int)iMode], 0.25f).setEase(LeanTweenType.easeInQuad).setOnComplete((Action)delegate
				{
					OnShutterActionComplate();
				});
				_uiBtm.transform.LTMoveLocal(_vBtnPos[(int)iMode], 0.25f).setEase(LeanTweenType.easeInQuad).setOnComplete((Action)delegate
				{
				});
			}
			_iShutterMode = iMode;
		}

		protected override void OnShutterActionComplate()
		{
			_isTween = false;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
