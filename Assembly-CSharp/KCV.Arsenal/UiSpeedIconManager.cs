using Common.Struct;
using KCV.Utils;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiSpeedIconManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite _highSpeedIcon;

		[SerializeField]
		private UISprite _highSpeedBar;

		[SerializeField]
		private UISprite _selectFrame;

		[SerializeField]
		private ParticleSystem _par;

		[SerializeField]
		private Animation _anim;

		[SerializeField]
		private UILabel _nowItemNum;

		[SerializeField]
		private UILabel _nextItemNum;

		private bool _isAnimate;

		private bool _isLarge;

		public bool IsHigh;

		public bool init()
		{
			Util.FindParentToChild(ref _highSpeedIcon, base.transform, "HighBase/IconPanel/HighIcon");
			Util.FindParentToChild(ref _highSpeedBar, base.transform, "HighBase/IconPanel/HighBar");
			Util.FindParentToChild(ref _selectFrame, base.transform, "HighBase/IconPanel/FrameHigh");
			Util.FindParentToChild<ParticleSystem>(ref _par, base.transform, "HighBase/MiniChara/SleepPar");
			Util.FindParentToChild(ref _nowItemNum, base.transform, "HighBase/nowItemNum");
			Util.FindParentToChild(ref _nextItemNum, base.transform, "HighBase/nextItemNum");
			if ((Object)_anim == null)
			{
				_anim = base.gameObject.GetComponent<Animation>();
			}
			IsHigh = false;
			_isAnimate = false;
			StartSleepAnimate();
			SetOff();
			SetBuildKitValue();
			UIButtonMessage component = GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "SpeedIconEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _highSpeedIcon);
			Mem.Del(ref _highSpeedBar);
			Mem.Del(ref _selectFrame);
			Mem.Del(ref _par);
			Mem.Del(ref _anim);
			Mem.Del(ref _nowItemNum);
			Mem.Del(ref _nextItemNum);
		}

		public void SetBuildKitValue()
		{
			_nowItemNum.textInt = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
			_nextItemNum.textInt = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
		}

		public void SetOff()
		{
			_highSpeedIcon.transform.localPositionX(-60f);
			_highSpeedBar.spriteName = "switch_kenzo_off";
			_highSpeedIcon.spriteName = "switch_pin_off";
			StartSleepAnimate();
			IsHigh = false;
		}

		public void SpeedIconEL(GameObject obj)
		{
			if (!GetComponent<Collider2D>().enabled)
			{
				return;
			}
			bool flag = !IsHigh;
			int buildKit = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
			MaterialInfo materialInfo = (!flag) ? ArsenalTaskManager.GetLogicManager().GetMinForCreateShip() : ArsenalTaskManager.GetLogicManager().GetMaxForCreateShip();
			int buildKit2 = materialInfo.BuildKit;
			int num = buildKit - buildKit2;
			if (num >= 0)
			{
				IsHigh = flag;
				float x = (!IsHigh) ? (-60f) : 70f;
				TweenPosition tweenPosition = TweenPosition.Begin(_highSpeedIcon.gameObject, 0.2f, new Vector3(x, -20.1f, 0f));
				tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
				_highSpeedBar.spriteName = ((!IsHigh) ? "switch_kenzo_off" : "switch_kenzo_on");
				_highSpeedIcon.spriteName = ((!IsHigh) ? "switch_pin_off" : "switch_pin_on");
				if (IsHigh)
				{
					StartUpAnimate();
				}
				else
				{
					StartSleepAnimate();
				}
				setNextItemNum(ArsenalTaskManager.GetLogicManager().Material.BuildKit - buildKit2);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void setNextItemNum(int nextNum)
		{
			_nextItemNum.textInt = nextNum;
			_nowItemNum.textInt = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
		}

		public void SetSelect(bool isSet)
		{
			_selectFrame.alpha = ((!isSet) ? 0f : 1f);
		}

		public void StartUpAnimate()
		{
			_anim.Stop();
			_anim.Play("SpeedMiniUp");
			_par.time = 0f;
			_par.Stop();
		}

		public void StartSleepAnimate()
		{
			_anim.Stop();
			_anim.Play("SpeedMiniSleepStart");
			StartSleepParticle();
		}

		public void StartSleepParticle()
		{
			_par.time = 0f;
			_par.Stop();
			_par.Play();
		}

		public void StopSleepParticle()
		{
			_par.time = 0f;
			_par.Stop();
		}

		public void CompSleepAnimetion()
		{
			_anim.Stop();
			_anim.Play("SpeedMiniSleep");
		}

		public void CompSleepStartAnimetion()
		{
			_anim.Stop();
			_anim.Play("SpeedMiniSleep");
		}
	}
}
