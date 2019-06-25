using KCV.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBufferAvoidance : BaseProdBuffer
	{
		protected override IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			base.animation.Play(AnimationList.ProdBufferAvoidance.ToString());
			SoundUtils.PlaySE(SEFIleInfos.SE_926a);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            // yield return new WaitForSeconds(base.animation.get_Item(AnimationList.ProdBufferAvoidance.ToString()).length);
            yield return new WaitForSeconds(0);


            observer.OnNext(value: true);
			observer.OnCompleted();
		}
	}
}
