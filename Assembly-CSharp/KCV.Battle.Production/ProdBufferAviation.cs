using KCV.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBufferAviation : BaseProdBuffer
	{
        // protected override IEnumerator AnimationObserver(IObserver<bool> observer)
        protected IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
        {
            base.animation.Play(AnimationList.ProdBufferAviation.ToString());
			SoundUtils.PlaySE(SEFIleInfos.SE_926a);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            // yield return new WaitForSeconds(base.animation.get_Item(AnimationList.ProdBufferAviation.ToString()).length);
            yield return new WaitForSeconds(0);


            observer.OnNext(value: true);
			observer.OnCompleted();
		}
	}
}
