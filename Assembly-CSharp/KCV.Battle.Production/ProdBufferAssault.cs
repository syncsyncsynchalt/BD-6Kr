using KCV.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBufferAssault : BaseProdBuffer
	{
		private Action _actOnPlayLookAtLine2Assult;

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _actOnPlayLookAtLine2Assult);
		}

		public bool Init(Action onPlayLookAtLine2Assult)
		{
			_actOnPlayLookAtLine2Assult = onPlayLookAtLine2Assult;
			return true;
		}

		protected override IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			base.animation.Play(AnimationList.ProdBufferAssault.ToString());
			SoundUtils.PlaySE(SEFIleInfos.SE_926a);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            //yield return new WaitForSeconds(base.animation.get_Item(AnimationList.ProdBufferAssault.ToString()).length);
            yield return new WaitForSeconds(0);

            observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private void PlayLookAtLine2Assult()
		{
			Dlg.Call(ref _actOnPlayLookAtLine2Assult);
		}
	}
}
