using KCV.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBufferUnifiedFire : BaseProdBuffer
	{
		private Action _actOnPlayLookAnim;

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _actOnPlayLookAnim);
		}

		public bool Init(Action onPlayLookAnim)
		{
			_actOnPlayLookAnim = onPlayLookAnim;
			return true;
		}

		protected override IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			base.animation.Play(AnimationList.ProdBufferUnifiedFire.ToString());
			SoundUtils.PlaySE(SEFIleInfos.SE_926a);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            // yield return new WaitForSeconds(base.animation.get_Item(AnimationList.ProdBufferUnifiedFire.ToString()).length);
            yield return new WaitForSeconds(0);


            observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private void PlayLookAnimation()
		{
			Dlg.Call(ref _actOnPlayLookAnim);
		}
	}
}
