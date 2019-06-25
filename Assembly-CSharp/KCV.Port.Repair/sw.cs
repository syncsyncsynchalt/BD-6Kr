using KCV.PopupString;
using KCV.Utils;
using System.Collections;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class sw : MonoBehaviour
	{
		private UISprite sw_ball;

		private UISprite sw_base;

		private Animation _Ani;

		private board3 bd3;

		private dialog dia;

		private bool sw_enable = true;

		private repair rep;

		private GameObject _zzz;

		public bool sw_stat;

		public bool _fairy_onoff;

		private void Awake()
		{
			sw_ball = GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>();
			sw_base = GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>();
			_Ani = GameObject.Find("sw_mini").GetComponent<Animation>();
			_zzz = GameObject.Find("SleepPar");
		}

		private void Start()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			_init_repair();
		}

		public void _init_repair()
		{
			sw_enable = true;
			sw_stat = false;
			_fairy_onoff = false;
			sw_ball = GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>();
			sw_base = GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>();
			_Ani = GameObject.Find("sw_mini").GetComponent<Animation>();
			_zzz = GameObject.Find("SleepPar");
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
		}

		private void OnDestroy()
		{
			Mem.Del(ref sw_ball);
			Mem.Del(ref sw_base);
			Mem.Del(ref _Ani);
			Mem.Del(ref bd3);
			Mem.Del(ref dia);
			Mem.Del(ref rep);
			Mem.Del(ref _zzz);
		}

		public void OnClick()
		{
			if (rep.now_repairkit() < 1)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughHighSpeedRepairKit));
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			}
			else
			{
				togggleSW();
			}
		}

		public IEnumerator switch_ONOFF()
		{
			_fairy_onoff = true;
			togggleSW();
			yield return new WaitForSeconds(1f);
			togggleSW();
			yield return new WaitForSeconds(0.3f);
			_fairy_onoff = false;
		}

		public void set_sw_stat(bool val)
		{
			sw_enable = val;
		}

		public void togggleSW()
		{
			togggleSW(value: false);
		}

		public void togggleSW(bool value)
		{
			if (_zzz == null)
			{
				_zzz = GameObject.Find("SleepPar");
			}
			if (!sw_enable)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				return;
			}
			if (!sw_stat)
			{
				_Ani.Play("mini_up");
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_on_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_on";
				_zzz.GetComponent<ParticleSystem>().Stop();
				_zzz.transform.localScale = Vector3.zero;
				sw_stat = true;
			}
			else
			{
				_Ani.Play("mini_down");
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_off_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_off";
				_zzz.GetComponent<ParticleSystem>().Play();
				_zzz.transform.localScale = Vector3.one;
				sw_stat = false;
			}
			dia = GameObject.Find("dialog").GetComponent<dialog>();
			dia.UpdateSW(sw_stat);
			dia.SetSpeed(sw_stat);
			SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
		}

		public bool getSW()
		{
			return sw_stat;
		}

		public void setSW(bool stat)
		{
			setSW(stat, isSlow: false);
		}

		public void setSW(bool stat, bool isSlow)
		{
			if (!sw_enable || stat == sw_stat)
			{
				return;
			}
			if (stat)
			{
				_Ani.Play("mini_up");
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_on_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_on";
				_zzz.GetComponent<ParticleSystem>().Stop();
				_zzz.SetActive(false);
				sw_stat = true;
			}
			else
			{
				if (isSlow)
				{
					_Ani.Play("mini_down");
				}
				else
				{
					_Ani.Play("mini_down");
				}
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_off_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_off";
				_zzz.GetComponent<ParticleSystem>().Play();
				_zzz.SetActive(true);
				sw_stat = false;
			}
			dia = GameObject.Find("dialog").GetComponent<dialog>();
			dia.UpdateSW(sw_stat);
			dia.SetSpeed(sw_stat);
		}
	}
}
