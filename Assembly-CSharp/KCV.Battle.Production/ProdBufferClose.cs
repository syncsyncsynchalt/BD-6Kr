using KCV.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBufferClose : BaseProdBuffer
	{
		protected override IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			base.animation.Play(AnimationList.ProdBufferClose.ToString());
			SoundUtils.PlaySE(SEFIleInfos.SE_926a);

            throw new NotImplementedException("�Ȃɂ���");
            // yield return new WaitForSeconds(base.animation.get_Item(AnimationList.ProdBufferClose.ToString()).length);
            yield return new WaitForSeconds(0);

            observer.OnNext(value: true);
			observer.OnCompleted();
		}
	}
}
