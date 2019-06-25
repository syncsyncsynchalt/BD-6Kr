using Common.Enum;
using LT.Tweening;
using System.Collections;
using UnityEngine;

namespace KCV.BattleCut
{
	public class BtlCut_FormationAnimation : MonoBehaviour
	{
		[SerializeField]
		private UISprite[] Formations;

		[SerializeField]
		private TextureFlash[] flash;

		[SerializeField]
		private Transform IconPanel;

		private Vector3 DecidePosition = new Vector3(0f, -288f, 0f);

		private Vector3[] defaultPos;

		[SerializeField]
		[Header("[Animation Properties]")]
		private float _duration = 0.3f;

		[SerializeField]
		private float _delay = 0.1f;

		[SerializeField]
		private iTween.EaseType _easeType = iTween.EaseType.easeInBack;

		public float duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
			}
		}

		public float delay
		{
			get
			{
				return _delay;
			}
			set
			{
				_delay = value;
			}
		}

		public iTween.EaseType easeType
		{
			get
			{
				return _easeType;
			}
			set
			{
				_easeType = value;
			}
		}

		private void Awake()
		{
			defaultPos = new Vector3[Formations.Length];
			int cnt = 0;
			defaultPos.ForEach(delegate(Vector3 x)
			{
				x = Formations[cnt].transform.position;
				cnt++;
			});
		}

		private void OnDestroy()
		{
			Mem.DelArySafe(ref Formations);
			Mem.DelArySafe(ref flash);
			Mem.Del(ref IconPanel);
			Mem.Del(ref DecidePosition);
			Mem.DelArySafe(ref defaultPos);
		}

		public IEnumerator StartAnimation(BattleFormationKinds1 iKind)
		{
			int nSelectNo = (int)(iKind - 1);
			Formations.ForEach(delegate(UISprite x)
			{
				iTween.Stop(x.gameObject);
			});
			yield return null;
			for (int i = 0; i < Formations.Length; i++)
			{
				if (i != nSelectNo)
				{
					Formations[i].transform.position = defaultPos[i];
					iTween.MoveBy(Formations[i].gameObject, iTween.Hash("x", 2, "easeType", easeType, "time", duration + (float)i * delay));
				}
			}
			flash[nSelectNo].flash(4, 0.05f);
			yield return new WaitForSeconds(delay * (float)Formations.Length);
			Formations[nSelectNo].transform.LTMoveLocalY(-288f, 0.5f).setEase(LeanTweenType.easeOutQuint);
			IconPanel.transform.LTMoveLocalX(190f, 0.5f).setEase(LeanTweenType.easeOutQuint);
			yield return new WaitForSeconds(0.2f);
			BtlCut_Live2D live2D = BattleCutManager.GetLive2D();
			live2D.ShowLive2D();
			live2D.ChangeMotion(Live2DModel.MotionType.Battle).Play().PlayShipVoice(13);
			yield return new WaitForSeconds(2f);
		}
	}
}
