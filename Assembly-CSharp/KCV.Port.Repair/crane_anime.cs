using System.Collections;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class crane_anime : MonoBehaviour
	{
		private Animation _AM;

		private Animation _AMH;

		private Animation _AMG;

		private UISprite _Gauge;

		private GameObject[] CraneNormal = new GameObject[4];

		private GameObject[] Gauge = new GameObject[4];

		private int now_repairs;

		private bool _isHPgrow;

		private int gdock;

		private UISprite _HPG;

		private board bd1;

		private board2 bd2;

		private repair rep;

		private int _REPAIR_ITEMS_ = 99999;

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _AM);
			Mem.Del(ref _AMH);
			Mem.Del(ref _AMG);
			Mem.Del(ref _Gauge);
			Mem.Del(ref CraneNormal);
			Mem.Del(ref Gauge);
			Mem.Del(ref _HPG);
			Mem.Del(ref bd1);
			Mem.Del(ref bd2);
			Mem.Del(ref rep);
		}

		private void HpAnimeEnd()
		{
			_isHPgrow = false;
		}

		public void _init_repair()
		{
			bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			bd2 = GameObject.Find("board2_top/board2").GetComponent<board2>();
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			for (int i = 0; i < 4; i++)
			{
				bd1.set_anime(i, stat: false);
				bd1.set_HS_anime(i, stat: false);
			}
			now_repairs = 0;
			_isHPgrow = false;
		}

		public void HPgrowStop()
		{
			_isHPgrow = false;
		}

		public void start_anime(int no)
		{
			int num = no;
			string text = "board/Grid/0" + num.ToString() + "/Anime";
			_AM = GameObject.Find(text).GetComponent<Animation>();
			((Component)_AM).GetComponent<UIPanel>().enabled = true;
			Debug.Log("★ ★ ★ Start dock_anime[no]: " + bd1.get_anime(no));
			if (!bd1.get_anime(no))
			{
				_AM.Play();
				bd1.set_anime(no, stat: true);
			}
			CraneNormal[no] = GameObject.Find(text + "/crane1");
		}

		public void stop_anime(int no)
		{
			Debug.Log("★ ★ ★ Stop dock_anime[no]: " + bd1.get_anime(no));
			int num = no;
			_AM = GameObject.Find("board/Grid/0" + num.ToString() + "/Anime").GetComponent<Animation>();
			int num2 = no;
			GameObject gameObject = GameObject.Find("board/Grid/0" + num2.ToString() + "/Anime/crane1");
			_AM.Stop();
			iTween.MoveTo(gameObject, iTween.Hash("islocal", true, "x", 520f, "time", 5f));
			CraneNormal[no] = gameObject;
		}

		private IEnumerator change_chara_on_crane_co()
		{
			yield return new WaitForSeconds(0.26f);
			string path = base.gameObject.transform.parent.gameObject.name + "/Anime/crane1";
			string ANI = base.gameObject.transform.parent.gameObject.name + "/Anime";
			int no = (!(base.gameObject.transform.parent.gameObject.name != "dock")) ? (-1) : int.Parse(base.gameObject.transform.parent.gameObject.name);
			if (no != -1 && bd1.get_anime(no))
			{
				_AM = GameObject.Find(ANI).GetComponent<Animation>();
				_AM.Play();
			}
			UISprite on_crane10 = GameObject.Find(path + "/on_crane").GetComponent<UISprite>();
			if (Random.Range(0, 10) != 0)
			{
				on_crane10.spriteName = "on_crane_" + Random.Range(0, 4);
			}
			else
			{
				on_crane10.spriteName = "on_crane_" + Random.Range(4, 11);
			}
			on_crane10 = GameObject.Find(path + "/wire/item").GetComponent<UISprite>();
			on_crane10.spriteName = $"repitems{((Random.Range(0, _REPAIR_ITEMS_) != 10) ? (Random.Range(0, 2) * 4) : (10 + Random.Range(0, 8))):D2}";
			switch (Random.Range(1, 5))
			{
			case 1:
				on_crane10 = GameObject.Find(path + "/arm/part1").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_yellow_02";
				on_crane10 = GameObject.Find(path + "/body").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_yellow_01";
				break;
			case 2:
				on_crane10 = GameObject.Find(path + "/arm/part1").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_orange_02";
				on_crane10 = GameObject.Find(path + "/body").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_orange_01";
				break;
			case 3:
				on_crane10 = GameObject.Find(path + "/arm/part1").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_green_02";
				on_crane10 = GameObject.Find(path + "/body").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_green_01";
				break;
			default:
				on_crane10 = GameObject.Find(path + "/arm/part1").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_red_02";
				on_crane10 = GameObject.Find(path + "/body").GetComponent<UISprite>();
				on_crane10.spriteName = "crane_red_01";
				break;
			}
		}

		public void change_chara_on_crane()
		{
			StartCoroutine(change_chara_on_crane_co());
		}

		public void high_repair_anime(int dockno)
		{
			high_repair_anime(dockno, _low_anime: true);
		}

		public void high_repair_anime(int dockno, bool _low_anime)
		{
			int num = dockno;
			UIButton component = GameObject.Find("board/Grid/0" + num.ToString() + "/repair_now/btn_high_repair").GetComponent<UIButton>();
			component.isEnabled = false;
			if (_low_anime)
			{
				Debug.Log("Crane:" + dockno + "を退場させます。");
				_AM.Stop();
				iTween.MoveTo(CraneNormal[dockno].gameObject, iTween.Hash("islocal", true, "x", 520f, "time", 5f));
			}
			bd1.set_anime(dockno, stat: true);
			bd1.set_HS_anime(dockno, stat: true);
			int num2 = dockno;
			string text = "board/Grid/0" + num2.ToString() + "/Anime_H";
			_AMH = GameObject.Find(text).GetComponent<Animation>();
			_AMH.Play();
			int num3 = dockno;
			GameObject.Find("board/Grid/0" + num3.ToString() + "/Anime_H/crane1").GetComponent<UIPanel>().enabled = true;
			int num4 = dockno;
			text = "board/Grid/0" + num4.ToString() + "/repair_now/HP_Gauge";
			_AMG = GameObject.Find(text).GetComponent<Animation>();
			gdock = dockno;
			_isHPgrow = true;
			_HPG = GameObject.Find(text + "/panel/HP_bar_meter2").GetComponent<UISprite>();
			iTween.ValueTo(base.gameObject, iTween.Hash("from", _HPG.width, "to", 210, "time", 0.75f, "delay", 2.8f, "onupdate", "UpdateHandler"));
		}

		public void onHighAnimeDone(int dock)
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			int mode = rep.now_mode();
			rep.set_mode(-1);
			Debug.Log(dock + "番が入渠を終えた。");
			bd1.set_HS_anime(dock, stat: false);
			bd1.set_anime(dock, stat: false);
			rep.set_mode(mode);
		}

		public void onHighAnimeDone2(int dock)
		{
			int mode = rep.now_mode();
			rep.set_mode(-1);
			bd1.DockStatus(rep.NowArea(), dock);
			rep.set_mode(mode);
		}

		public void high_repair_anime2()
		{
		}

		private void UpdateHandler(float value)
		{
			_HPG.width = 210;
			SetHpGauge(_HPG, (int)value, 210);
		}

		public void SetHpGauge(UISprite gauge, int Now, int Max)
		{
			float num = 566f;
			if (gauge.parent.parent.name == "00" || gauge.parent.parent.name == "01" || gauge.parent.parent.name == "02" || gauge.parent.parent.name == "03")
			{
				int dock = int.Parse(gauge.parent.parent.name);
				int num2 = bd1.get_dock_MaxHP(dock);
				((Component)gauge.parent.transform.FindChild("text_hp")).GetComponent<UILabel>().text = Now * num2 / Max + "/" + num2;
			}
			gauge.width = gauge.width * Now / Max;
			float num3 = num * (float)(Max - Now) / (float)Max;
			if (num3 <= 55f)
			{
				float num4 = 0f;
				float num5 = Mathe.Rate(0f, 255f, 200f);
				if (num3 != 0f)
				{
					num4 = Mathe.Rate(0f, 255f, num3);
				}
				Color color2 = gauge.color = new Color(0f, num5 + num4, 0f, 1f);
			}
			else if (num3 <= 311f)
			{
				float r = Mathe.Rate(0f, 255f, num3 - 55f);
				Color color4 = gauge.color = new Color(r, 1f, 0f, 1f);
			}
			else
			{
				float num6 = Mathe.Rate(0f, 255f, num3 - 311f);
				Color color6 = gauge.color = new Color(1f, 1f - num6, 0f, 1f);
			}
		}
	}
}
