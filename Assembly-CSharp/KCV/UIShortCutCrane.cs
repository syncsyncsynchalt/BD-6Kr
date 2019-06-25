using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class UIShortCutCrane : MonoBehaviour
	{
		private class HookYousei
		{
			public int GraphicID;

			public Vector2 LocalPos;

			public HookYousei(int GraphicID, Vector2 LocalPos)
			{
				this.GraphicID = GraphicID;
				this.LocalPos = LocalPos;
			}
		}

		private Animation anim;

		[SerializeField]
		private UISprite HookYouseiSprite;

		private List<HookYousei> HookYouseis;

		private List<int[]> YouseiGroups;

		[Button("StartAnimation", "StartAnimation", new object[]
		{

		})]
		public int button1;

		private void Awake()
		{
			anim = GetComponent<Animation>();
			HookYouseis = new List<HookYousei>();
			HookYouseis.Add(new HookYousei(0, Vector2.zero));
			HookYouseis.Add(new HookYousei(1, new Vector2(-31f, -75f)));
			HookYouseis.Add(new HookYousei(2, new Vector2(-58f, -55f)));
			HookYouseis.Add(new HookYousei(3, new Vector2(-29f, 85f)));
			HookYouseis.Add(new HookYousei(4, new Vector2(33f, 89f)));
			HookYouseis.Add(new HookYousei(5, new Vector2(-60f, -60f)));
			HookYouseis.Add(new HookYousei(6, new Vector2(-85f, -36f)));
			HookYouseis.Add(new HookYousei(7, new Vector2(-58f, -4f)));
			HookYouseis.Add(new HookYousei(8, new Vector2(-38f, 78f)));
			HookYouseis.Add(new HookYousei(9, new Vector2(-49f, -64f)));
			YouseiGroups = new List<int[]>();
			YouseiGroups.Add(new int[2]
			{
				1,
				2
			});
			YouseiGroups.Add(new int[2]
			{
				3,
				4
			});
			YouseiGroups.Add(new int[2]
			{
				5,
				6
			});
			YouseiGroups.Add(new int[2]
			{
				6,
				7
			});
			YouseiGroups.Add(new int[2]
			{
				6,
				8
			});
			YouseiGroups.Add(new int[2]
			{
				6,
				9
			});
			YouseiGroups.Add(new int[3]
			{
				0,
				1,
				8
			});
		}

		private void StartAnimationPrivate(bool isStop)
		{
			ChangeYousei();
			int num = (Random.Range(0, 3) == 0) ? 1 : 2;
			string text = "SCMenuYousei" + num;
			if (isStop)
			{
				anim.Stop();
			}
			if (!anim.isPlaying)
			{
				anim.Play(text);
			}
		}

		public void StartAnimation()
		{
			StartAnimationPrivate(isStop: true);
		}

		public void StartAnimationNoReset()
		{
			StartAnimationPrivate(isStop: false);
		}

		public void AnimStop()
		{
			base.transform.localPositionX(550f);
			anim.Stop();
		}

		private void ChangeYousei()
		{
			ShipModel flagShipModel = SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel;
			int[] array = (flagShipModel == null) ? YouseiGroups[6] : getYouseiGroup(flagShipModel.ShipType);
			int num = array[Random.Range(0, array.Length)];
			HookYouseiSprite.spriteName = "hookyousei_" + num;
			HookYouseiSprite.MakePixelPerfect();
			HookYouseiSprite.transform.localPosition = HookYouseis[num].LocalPos;
		}

		private int[] getYouseiGroup(int Stype)
		{
			int index;
			switch (Stype)
			{
			case 7:
			case 11:
			case 16:
			case 17:
			case 18:
				index = 0;
				break;
			case 8:
			case 9:
			case 10:
			case 12:
				index = 1;
				break;
			case 5:
			case 6:
				index = 2;
				break;
			case 3:
			case 4:
			case 21:
				index = 3;
				break;
			case 2:
				index = 4;
				break;
			case 13:
			case 14:
			case 20:
				index = 5;
				break;
			default:
				index = 6;
				break;
			}
			return YouseiGroups[index];
		}

		private void OnDestroy()
		{
			anim = null;
			HookYouseiSprite = null;
			if (HookYouseis != null)
			{
				HookYouseis.Clear();
			}
			if (YouseiGroups != null)
			{
				YouseiGroups.Clear();
			}
			HookYouseis = null;
			YouseiGroups = null;
		}
	}
}
