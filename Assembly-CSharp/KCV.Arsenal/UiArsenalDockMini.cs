using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalDockMini : MonoBehaviour
	{
		public enum AnimationType
		{
			ConstStart,
			Const
		}

		private Animation _anim;

		[SerializeField]
		private ParticleSystem par;

		[SerializeField]
		private ParticleSystem fire;

		[SerializeField]
		private Animation mini4anim;

		[SerializeField]
		private ParticleSystem mini4psHit;

		private UiArsenalMiniManager miniManager1;

		private UiArsenalMiniManager miniManager2;

		private UiArsenalMiniManager miniManager3;

		private UiArsenalMiniManager miniManager4;

		private int _index;

		private bool isCreate;

		private bool _isParticle;

		private bool isCreateAnim;

		private bool isHightAnim;

		private bool isFirstHight;

		private Action _callBack;

		private void OnDestroy()
		{
			_anim = null;
			par = null;
			fire = null;
			mini4anim = null;
			mini4psHit = null;
			miniManager1 = null;
			miniManager2 = null;
			miniManager3 = null;
			miniManager4 = null;
			_callBack = null;
		}

		public void init(int num)
		{
			_index = num;
			_isParticle = false;
			isCreateAnim = false;
			isHightAnim = false;
			isFirstHight = false;
			isCreate = true;
			GameObject gameObject = base.transform.parent.parent.FindChild("ParPanel").gameObject;
			Util.FindParentToChild<ParticleSystem>(ref par, gameObject.transform, "Par");
			Util.FindParentToChild<ParticleSystem>(ref fire, gameObject.transform, "Fire");
			Util.FindParentToChild(ref miniManager1, base.transform, "Mini1");
			Util.FindParentToChild(ref miniManager2, base.transform, "Mini2");
			Util.FindParentToChild(ref miniManager3, base.transform, "Mini3");
			Util.FindParentToChild(ref miniManager4, base.transform, "Mini4");
			if ((UnityEngine.Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
			par.Stop();
			fire.Stop();
			((Component)par).SetActive(isActive: false);
			((Component)fire).SetActive(isActive: false);
			_anim.Stop();
			miniManager1.init(isDefault: false);
			miniManager2.init(isDefault: false);
			miniManager3.init(isDefault: false);
			miniManager4.init(isDefault: true);
			mini4psHit.Stop();
			for (int i = 0; i < 3; i++)
			{
				miniManager1.addSprite("mini_03_a_0" + (i + 1));
				miniManager2.addSprite("mini_01_a_0" + (i + 1));
				miniManager3.addSprite("mini_04_a_0" + (i + 1));
			}
			miniManager3.transform.localPosition = new Vector3(250f, 0f, 0f);
		}

		private void Update()
		{
			if (isCreateAnim)
			{
				if (miniManager1.GetIndex() == 1 && !_isParticle)
				{
					_isParticle = true;
					((Component)par).SetActive(isActive: true);
					par.time = 0f;
					par.Stop();
					par.Play();
				}
				else if (miniManager1.GetIndex() == 0)
				{
					_isParticle = false;
				}
			}
			if (miniManager1 != null)
			{
				miniManager1.Run();
			}
			if (miniManager2 != null)
			{
				miniManager2.Run();
			}
			if (miniManager3 != null)
			{
				miniManager3.Run();
			}
			if (miniManager4 != null)
			{
				miniManager4.Run();
			}
		}

		public void DisableParticles()
		{
			miniManager4.DisableParticles();
		}

		public void EnableParticles()
		{
			miniManager4.EnableParticles();
		}

		public void PlayIdleAnimation()
		{
			miniManager4.transform.localPosition = new Vector3(0f, 0f, 0f);
			miniManager4.gameObject.SetActive(true);
			if (UnityEngine.Random.value < 0.5f)
			{
				miniManager4.StartAnimation("DockMini4Idle", WrapMode.Loop, UnityEngine.Random.value * 8f);
			}
			else
			{
				miniManager4.StartAnimation("DockMini4Sleep", WrapMode.Loop, UnityEngine.Random.value * 8f);
			}
		}

		public void StopIdleAnimation()
		{
			_anim.Stop();
			miniManager4.stopAnimate();
			DisableParticles();
		}

		public void PlayConstStartAnimation()
		{
			_anim.Play("DockMini1Start");
			miniManager1.StartAnimation("DockMini1StartLoop", WrapMode.Loop, 0f);
			miniManager2.StartAnimation("DockMiniRunLoop", WrapMode.Loop, 0f);
			miniManager4.StartAnimation("DockMini4Enter", WrapMode.Clamp, 0f);
			miniManager4.DisableParticles();
		}

		public void StopConstAnimation()
		{
			_anim.Stop();
			_anim.Play("DockMiniEmpty");
			miniManager1.stopAnimate();
			miniManager2.stopAnimate();
			miniManager1.transform.localPosition = new Vector3(250f, 0f, 0f);
			miniManager2.transform.localPosition = new Vector3(250f, 0f, 0f);
			init(_index);
		}

		public void PlayConstCompAnimation()
		{
			isCreateAnim = false;
			miniManager1.init(isDefault: false);
			miniManager2.init(isDefault: false);
			for (int i = 0; i < 2; i++)
			{
				miniManager1.addSprite("mini_03_c_0" + (i + 1));
				miniManager2.addSprite("mini_01_c_0" + (i + 1));
			}
			_anim.Stop();
			_anim.Play("DockMiniComp");
			miniManager1.StartAnimation("DockMini1CmpLoop", WrapMode.Loop, 0f);
			miniManager2.StartAnimation("DockMini2CmpLoop", WrapMode.Loop, 0f);
			miniManager4.StartAnimation("DockMini4Jump", WrapMode.Loop, 0f);
			miniManager4.DisableParticles();
		}

		public void PlayHalfwayHightAnimation()
		{
			isCreateAnim = false;
			miniManager3.init(isDefault: false);
			for (int i = 0; i < 3; i++)
			{
				miniManager3.addSprite("mini_04_a_0" + (i + 1));
			}
			_anim.Stop();
			_anim.Play("DockMiniEnd");
			miniManager1.StartAnimation("DockMini1StartLoop", WrapMode.Loop, 0f);
			miniManager2.StartAnimation("DockMiniRunLoop", WrapMode.Loop, 0f);
			miniManager3.StartAnimation("DockMiniRunLoop", WrapMode.Loop, 0f);
			miniManager4.StartAnimation("DockMini4Bask", WrapMode.Clamp, 0f);
			miniManager4.DisableParticles();
		}

		public void PlayFirstHighAnimation()
		{
			isCreateAnim = false;
			isFirstHight = true;
			_anim.Stop();
			_anim.Play("DockMiniFirstHight");
			miniManager3.StartAnimation("DockMiniRunLoop", WrapMode.Loop, 0f);
			miniManager4.DisableParticles();
			miniManager4.stopAnimate();
			miniManager4.transform.localPosition = new Vector3(400f, 0f, 0f);
		}

		public void CompStartMini1()
		{
			miniManager1.stopAnimate();
			miniManager1.init(isDefault: false);
			for (int i = 0; i < 2; i++)
			{
				miniManager1.addSprite("mini_03_b_0" + (i + 1));
			}
			miniManager1.StartAnimation("DockMini1CreateLoop", WrapMode.Loop, 0f);
			isCreateAnim = true;
			miniManager4.StartAnimation("DockMini4Work", WrapMode.Loop, 0f);
			mini4psHit.Play();
		}

		public void CompStartMini2()
		{
			_anim.Stop();
			_anim.Play("DockMiniRightRun");
			miniManager2.stopAnimate();
			miniManager2.StartAnimation("DockMiniRunLoop", WrapMode.Loop, 0f);
		}

		public void CompRightRun()
		{
			_anim.Stop();
			_anim.Play("DockMiniLeftRun");
		}

		public void CompLeftRun()
		{
			_anim.Stop();
			_anim.Play("DockMiniRightRun");
		}

		public void CompEndAnimate()
		{
			_anim.Stop();
			miniManager3.stopAnimate();
			miniManager3.init(isDefault: false);
			for (int i = 0; i < 2; i++)
			{
				miniManager3.addSprite("mini_04_b_0" + (i + 1));
			}
			_anim.Play("DockMiniHight");
			((Component)fire).SetActive(isActive: true);
			fire.time = 0f;
			fire.Play();
			miniManager3.StartAnimation("DockMiniRunLoop", WrapMode.Loop, 0f);
		}

		public void CompHightAnimate()
		{
			_anim.Stop();
			fire.Stop();
			((Component)fire).SetActive(isActive: false);
			TaskMainArsenalManager.dockMamager[_index].endSpeedUpAnimate();
		}

		public void PlayEndHightAnimate()
		{
			_anim.Stop();
			miniManager3.stopAnimate();
			miniManager3.init(isDefault: false);
			for (int i = 0; i < 2; i++)
			{
				miniManager3.addSprite("mini_04_c_0" + (i + 1));
			}
			miniManager3.StartAnimation("DockMini3CmpLoop", WrapMode.Loop, 0f);
		}

		public void CompCmpAnimate()
		{
			_anim.Stop();
			_anim.Play("DockMiniComp");
		}

		public void CompCmpHighAnimate()
		{
			_anim.Stop();
			_anim.Play("DockMiniCompHigh");
			miniManager3.StartAnimation("DockMiniRunLoop", WrapMode.Loop, 0f);
		}
	}
}
