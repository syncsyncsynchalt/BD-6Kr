using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalMiniManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiMini;

		[SerializeField]
		private ParticleSystem _sleepPar;

		[SerializeField]
		private ParticleSystem _starPar;

		private Animation _anim;

		private int index;

		private float timer;

		private bool isInit;

		private bool isLoop;

		private bool idle;

		private string _animType;

		private string _animTypeNext;

		private string[] spriteNames;

		public bool isChange;

		public bool IsDefault;

		public int GetIndex()
		{
			return index;
		}

		public void init(bool isDefault)
		{
			IsDefault = isDefault;
			if (!isDefault)
			{
				_uiMini = ((Component)base.transform.FindChild("Mini")).GetComponent<UISprite>();
			}
			if (IsDefault)
			{
				Util.FindParentToChild<ParticleSystem>(ref _sleepPar, base.transform.parent.parent.parent, "ParPanel/SleepPart");
				Util.FindParentToChild<ParticleSystem>(ref _starPar, base.transform, "Working/StarPart");
			}
			_anim = GetComponent<Animation>();
			_anim.Stop();
			index = -1;
			isChange = false;
			isLoop = false;
			_animType = string.Empty;
			_animTypeNext = string.Empty;
			spriteNames = new string[10];
			for (int i = 0; i < 10; i++)
			{
				spriteNames[i] = string.Empty;
			}
			idle = false;
			timer = 0f;
			isInit = true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiMini);
			Mem.Del(ref _sleepPar);
			Mem.Del(ref _starPar);
			Mem.Del(ref _anim);
			Mem.Del(ref spriteNames);
		}

		public bool addSprite(string name)
		{
			for (int i = 0; i < 10; i++)
			{
				if (spriteNames[i] == string.Empty)
				{
					spriteNames[i] = name;
					return true;
				}
			}
			return false;
		}

		public void Run()
		{
			if (!isInit)
			{
				return;
			}
			if (IsDefault && !_starPar.isPlaying && _animType == "DockMini4Work")
			{
				_starPar.Play();
			}
			if (IsDefault && !_sleepPar.isPlaying && _animType == "DockMini4Sleep")
			{
				_sleepPar.Play();
			}
			if (!idle)
			{
				return;
			}
			_animTypeNext = _animType;
			if (_animType != "DockMini4Doze")
			{
				timer += Time.deltaTime;
				if (timer > 15f && UnityEngine.Random.value < Time.deltaTime)
				{
					timer = 0f;
					if (_animType == "DockMini4Sleep")
					{
						_animTypeNext = "DockMini4Idle";
					}
					else
					{
						_animTypeNext = ((!(UnityEngine.Random.value < 0.5f)) ? "DockMini4Sleep" : "DockMini4Doze");
					}
				}
			}
			if (!_anim.isPlaying)
			{
				_animTypeNext = ((!(UnityEngine.Random.value < 0.5f)) ? "DockMini4Sleep" : "DockMini4Idle");
			}
			if (_animTypeNext != _animType || !_anim.isPlaying)
			{
				if (_animType == "DockMini4Sleep")
				{
					_sleepPar.enableEmission = false;
				}
				if (_animTypeNext == "DockMini4Sleep")
				{
					_sleepPar.enableEmission = true;
				}
				_animType = _animTypeNext;

                throw new NotImplementedException("‚È‚É‚±‚ê");
                //_anim.get_Item(_animTypeNext).set_wrapMode((_animTypeNext == "DockMini4Doze") ? WrapMode.Clamp : WrapMode.Loop);

                _anim.Play(_animTypeNext);
			}
		}

		private void _playStarParticle()
		{
			if (IsDefault && !_starPar.isPlaying && _animType == "DockMini4Work")
			{
				_starPar.Play();
			}
		}

		public void DisableParticles()
		{
			if (IsDefault)
			{
				((Component)_sleepPar).SetActive(isActive: false);
				((Component)_starPar).SetActive(isActive: false);
				((Component)_sleepPar).transform.localPosition = new Vector3(600f, -10f);
			}
		}

		public void EnableParticles()
		{
			if (IsDefault)
			{
				((Component)_sleepPar).SetActive(isActive: true);
				((Component)_starPar).SetActive(isActive: true);
			}
		}

		public void StartAnimation(string anim, WrapMode wrap, float time)
		{
			_anim.Stop();
			_animType = anim;
			_anim.Play(anim);


            throw new NotImplementedException("‚È‚É‚±‚ê");
            // _anim.get_Item(anim).set_wrapMode(wrap);
			// _anim.get_Item(anim).set_time(time);


			if (IsDefault)
			{
				((Component)_sleepPar).SetActive(isActive: false);
				((Component)_starPar).SetActive(isActive: false);
				if (anim == "DockMini4Sleep")
				{
					((Component)_sleepPar).transform.localPosition = new Vector3(100f, -10f);
					((Component)_sleepPar).SetActive(isActive: true);
					_sleepPar.Play();
				}
				if (anim == "DockMini4Work")
				{
					((Component)_starPar).SetActive(isActive: true);
					_starPar.Play();
				}
			}
			if (anim == "DockMini4Idle" || anim == "DockMini4Doze" || anim == "DockMini4Sleep")
			{
				idle = true;
				timer = 0f;
			}
			else
			{
				idle = false;
			}
		}

		public void ChangeSprite()
		{
			if (spriteNames != null)
			{
				if (!string.IsNullOrEmpty(spriteNames[index + 1]))
				{
					index++;
					_uiMini.spriteName = spriteNames[index];
				}
				else
				{
					index = 0;
					_uiMini.spriteName = spriteNames[index];
				}
				isChange = true;
			}
		}

		public void CompAnimate()
		{
			_anim.Stop();
			if (isLoop)
			{
				_anim.Play(_animType);
			}
		}

		public void stopAnimate()
		{
			_anim.Stop();
		}

		public void CompAnimateEnter4()
		{
			_anim.Stop();
			if (isLoop)
			{
				_anim.Play(_animType);
			}
		}
	}
}
