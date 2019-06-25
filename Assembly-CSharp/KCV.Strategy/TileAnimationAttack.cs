using Common.Enum;
using KCV.Utils;
using local.models;
using local.utils;
using Server_Common.Formats;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationAttack : MonoBehaviour
	{
		private TileAnimationCharacter friendly;

		private TileAnimationCharacter enemy;

		private UISprite ship;

		private UITexture explosion;

		private TileAnimationAttackShell shell;

		private ParticleSystem partExplosion;

		private UILabel count;

		private UIWidget GetMaterial;

		private UITexture[] arrows;

		private UILabel[] downTexts;

		private bool on;

		private bool flicker;

		private float timer;

		private int cnt;

		private int tot;

		private int[] resFrom;

		private int[] resTo;

		public bool isFinished;

		private bool SkipFlag;

		private bool isGuard;

		private RadingResultData RadingData;

		[SerializeField]
		private TileAnimationHukidashi hukidashi;

		private Coroutine DelayedActionsCor;

		private Coroutine ResultPhaseCor;

		private Coroutine AttackCor;

		private Coroutine NowWaitCor;

		private bool CutMode;

		private void Awake()
		{
			base.transform.parent = GameObject.Find("/StrategyTaskManager/Map Root").transform;
			base.transform.localScale = Vector3.one;
			base.transform.localPosition = new Vector3(-459.45f, 144f, -24.8f);
			friendly = ((Component)base.transform.Find("Friendly/Container")).GetComponent<TileAnimationCharacter>();
			enemy = ((Component)base.transform.Find("Enemy/Container")).GetComponent<TileAnimationCharacter>();
			ship = ((Component)base.transform.Find("Ship/ShipInner")).GetComponent<UISprite>();
			explosion = ((Component)base.transform.Find("Explosion")).GetComponent<UITexture>();
			shell = ((Component)base.transform.Find("Shell")).GetComponent<TileAnimationAttackShell>();
			partExplosion = ((Component)base.transform.Find("ParticleExplosion")).GetComponent<ParticleSystem>();
			count = ((Component)base.transform.Find("Count")).GetComponent<UILabel>();
			GetMaterial = ((Component)base.transform.Find("GetMaterial")).GetComponent<UIWidget>();
			arrows = new UITexture[4];
			for (int i = 0; i < 4; i++)
			{
				arrows[i] = ((Component)base.transform.Find("GetMaterial/Label_GetMaterial/Grid/GetMaterial" + (i + 1) + "/Arrow")).GetComponent<UITexture>();
			}
			downTexts = new UILabel[4];
			for (int j = 0; j < 4; j++)
			{
				downTexts[j] = ((Component)base.transform.Find("GetMaterial/Label_GetMaterial/Grid/GetMaterial" + (j + 1) + "/num")).GetComponent<UILabel>();
			}
			timer = 0f;
			flicker = false;
			on = false;
			ship.alpha = 0f;
			count.alpha = 0f;
			count.text = string.Empty;
			GetMaterial.alpha = 0f;
			for (int k = 0; k < 4; k++)
			{
				arrows[k].alpha = 0f;
				downTexts[k].text = string.Empty;
			}
			isFinished = false;
			cnt = App.rand.Next(7) + 3;
			tot = cnt;
			partExplosion.Pause(true);
		}

		private void Start()
		{
			for (int i = 0; i < 4; i++)
			{
				iTween.MoveTo(arrows[i].gameObject, iTween.Hash("position", arrows[i].transform.localPosition + new Vector3(0f, -30f, 0f), "islocal", true, "time", 0.75f, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.loop));
			}
		}

		private void Update()
		{
			if (on)
			{
				for (int i = 0; i < 4; i++)
				{
					if (resFrom[i] != resTo[i])
					{
						UITexture obj = arrows[i];
						float alpha = GetMaterial.alpha;
						Vector3 localPosition = arrows[0].transform.localPosition;
						obj.alpha = Mathf.Min(alpha, (170f + localPosition.y) / 45f);
					}
				}
			}
			if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Joystick1Button1)) && DelayedActionsCor != null)
			{
				CutMode = true;
			}
		}

		public void Initialize(RadingResultData radingData, MapAreaModel m)
		{
			hukidashi.Init();
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, 0.2f, 1f);
			tweenAlpha.onFinished.Clear();
			RadingData = radingData;
			int beforeNum = radingData.BeforeNum;
			int tanker_count = radingData.BeforeNum - radingData.BreakNum;
			Debug.Log(m);
			Debug.Log(m.GetEscortDeck());
			if (radingData.FlagShipMstId != 0)
			{
				bool isDamaged = radingData.FlagShipDamageState == DamageState.Taiha || radingData.FlagShipDamageState == DamageState.Tyuuha;
				friendly.UnloadTexture();
				this.DelayActionFrame(1, delegate
				{
					friendly.SetTexture(ShipUtils.LoadTexture(m.GetEscortDeck().GetFlagShip(), isDamaged));
				});
			}
			if (radingData.AttackKind == RadingKind.AIR_ATTACK)
			{
				enemy.UnloadTexture();
				this.DelayActionFrame(1, delegate
				{
					enemy.SetTexture(ShipUtils.LoadTexture(512, 9));
				});
			}
			else
			{
				enemy.UnloadTexture();
				this.DelayActionFrame(1, delegate
				{
					enemy.SetTexture(ShipUtils.LoadTexture(530, 9));
				});
			}
			tot = beforeNum;
			cnt = tanker_count;
			resFrom = new int[4];
			resFrom[0] = m.GetResources(beforeNum)[enumMaterialCategory.Fuel];
			resFrom[1] = m.GetResources(beforeNum)[enumMaterialCategory.Bull];
			resFrom[2] = m.GetResources(beforeNum)[enumMaterialCategory.Steel];
			resFrom[3] = m.GetResources(beforeNum)[enumMaterialCategory.Bauxite];
			resTo = new int[4];
			resTo[0] = m.GetResources(tanker_count)[enumMaterialCategory.Fuel];
			resTo[1] = m.GetResources(tanker_count)[enumMaterialCategory.Bull];
			resTo[2] = m.GetResources(tanker_count)[enumMaterialCategory.Steel];
			resTo[3] = m.GetResources(tanker_count)[enumMaterialCategory.Bauxite];
			for (int i = 0; i < 4; i++)
			{
				downTexts[i].text = " ×" + Convert.ToString(resFrom[i]);
			}
		}

		public void Attack(bool guard, RadingKind type)
		{
			timer = Time.time;
			on = true;
			isFinished = false;
			count.text = "ｘ " + tot;
			isGuard = guard;
			DelayedActionsCor = StartCoroutine(DelayedActions(guard, type));
		}

		public IEnumerator DelayedActions(bool guard, RadingKind type)
		{
			yield return new WaitForSeconds(0.5f);
			yield return StartCoroutine(TankerPopUp());
			yield return StartCoroutine(EnemyPopUp(guard));
			yield return StartCoroutine(GuardPopUp(guard));
			float cur = tot;
			int died = tot - cnt;
			if (!CutMode)
			{
				if (guard)
				{
					AttackCor = StartCoroutine(BattleAnimWithGuard(type, cur, died));
					yield return AttackCor;
				}
				else
				{
					AttackCor = StartCoroutine(BattleAnimWithNone(type, cur, died));
					yield return AttackCor;
				}
			}
			AttackCor = null;
			ResultPhaseCor = StartCoroutine(ResultPhase());
			yield return ResultPhaseCor;
			CutMode = false;
		}

		public void CountMove()
		{
			count.transform.localPosition = ship.transform.parent.localPosition + new Vector3(0f, 30f, 0f);
		}

		public void ShipAlpha(float f)
		{
			ship.alpha = f;
		}

		public void CountAlpha(float f)
		{
			count.alpha = f;
		}

		public void ExplosionAlpha(float f)
		{
			explosion.alpha = f;
		}

		public void InfoAlpha(float f)
		{
			GetMaterial.alpha = f;
		}

		private IEnumerator TankerPopUp()
		{
			ship.transform.localPosition = Vector3.zero;
			float time3 = (!CutMode) ? 1f : 0.2f;
			float time2 = (!CutMode) ? 0.5f : 0.1f;
			iTween.MoveTo(ship.transform.parent.gameObject, iTween.Hash("position", new Vector3(0f, 100f, 0f), "islocal", true, "time", time3, "easeType", iTween.EaseType.easeOutQuad, "onupdate", "CountMove", "onupdatetarget", base.gameObject));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", time2, "onupdate", "ShipAlpha", "onupdatetarget", base.gameObject));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", time2, "delay", time2, "onupdate", "CountAlpha", "onupdatetarget", base.gameObject));
			if (!CutMode)
			{
				yield return new WaitForSeconds(1.5f);
			}
		}

		private IEnumerator EnemyPopUp(bool guard)
		{
			enemy.PopUp(TileAnimationCharacter.STATE.WAVE, CutMode);
			if (!CutMode)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator GuardPopUp(bool guard)
		{
			if (guard)
			{
				flicker = true;
				friendly.PopUp(TileAnimationCharacter.STATE.FLOAT, CutMode);
				if (!CutMode)
				{
					yield return new WaitForSeconds(1.5f);
				}
			}
			if (!CutMode)
			{
				yield return new WaitForSeconds(1f);
			}
		}

		private IEnumerator BattleAnimWithGuard(RadingKind type, float cur, int died)
		{
			for (int i = 0; i < 2; i++)
			{
				shell.Initialize(enemy.transform.parent.localPosition, friendly.transform.parent.localPosition, type);
				yield return MyWaitForSeconds((type != 0) ? 1f : 0.5f);
				iTween.ShakePosition(friendly.gameObject, iTween.Hash("amount", Vector3.one, "islocal", true, "time", 0.5f));
				((Component)partExplosion).transform.localPosition = friendly.transform.parent.localPosition;
				partExplosion.time = 0f;
				partExplosion.Play(true);
				SoundUtils.PlaySE(SEFIleInfos.BattleDamage);
				GameObject tmp = UnityEngine.Object.Instantiate(count.gameObject);
				tmp.transform.parent = count.transform.parent;
				tmp.transform.localScale = Vector3.one;
				tmp.transform.localPosition = count.transform.localPosition;
				iTween.MoveTo(tmp, iTween.Hash("position", tmp.transform.localPosition + 50f * Vector3.up, "islocal", true, "time", 0.4f, "easeType", iTween.EaseType.linear));
				iTween.RotateTo(tmp, iTween.Hash("x", 90, "islocal", true, "time", 0.4f, "easeType", iTween.EaseType.linear));
				UnityEngine.Object.Destroy(tmp, 0.4f);
				cur -= (float)died / 2f;
				count.text = "ｘ " + Mathf.RoundToInt(cur);
				if (RadingData.RadingDamage.Any((RadingDamageData x) => x.Damage))
				{
					bool isGekichin = RadingData.RadingDamage.Any((RadingDamageData x) => x.DamageState == DamagedStates.Gekichin);
					bool isYouin = RadingData.RadingDamage.Any((RadingDamageData x) => x.DamageState == DamagedStates.Youin);
					if (i == 1 && (isGekichin || isYouin))
					{
						hukidashi.Play(TileAnimationHukidashi.Type.Goutin);
						yield return new WaitForSeconds(2f);
						if (isYouin)
						{
							hukidashi.Play(TileAnimationHukidashi.Type.Damecon);
							yield return new WaitForSeconds(1f);
							TrophyUtil.Unlock_At_Rading();
						}
					}
					else
					{
						hukidashi.Play(TileAnimationHukidashi.Type.Damage);
					}
				}
				yield return MyWaitForSeconds(1.2f);
				partExplosion.Stop(true);
				partExplosion.Clear(true);
				SoundUtils.PlaySE(SEFIleInfos.BattleDamage);
				shell.Initialize(friendly.transform.parent.localPosition, enemy.transform.parent.localPosition, (RadingKind)0);
				yield return MyWaitForSeconds(0.5f);
				iTween.ShakePosition(enemy.gameObject, iTween.Hash("amount", Vector3.one, "islocal", true, "time", 0.5f));
				((Component)partExplosion).transform.localPosition = enemy.transform.parent.localPosition;
				partExplosion.time = 0f;
				partExplosion.Play(true);
				SoundUtils.PlaySE(SEFIleInfos.BattleDamage);
				yield return MyWaitForSeconds(1.2f);
				partExplosion.Stop(true);
				partExplosion.Clear(true);
			}
		}

		private Coroutine MyWaitForSeconds(float time)
		{
			if (CutMode)
			{
				return null;
			}
			NowWaitCor = StartCoroutine(WaitForSeconds(time));
			return NowWaitCor;
		}

		private IEnumerator WaitForSeconds(float time)
		{
			float totalTime = 0f;
			while (true)
			{
				totalTime += Time.deltaTime;
				if (totalTime >= time)
				{
					break;
				}
				yield return null;
			}
		}

		private IEnumerator BattleAnimWithNone(RadingKind type, float cur, int died)
		{
			for (int i = 0; i < 2; i++)
			{
				shell.Initialize(enemy.transform.parent.localPosition, ship.transform.localPosition, type);
				yield return MyWaitForSeconds((type != 0) ? 1f : 0.5f);
				iTween.ShakePosition(ship.gameObject, iTween.Hash("amount", Vector3.one, "islocal", true, "time", 0.5f));
				explosion.transform.localPosition = ship.transform.localPosition;
				SoundUtils.PlaySE(SEFIleInfos.BattleDamage);
				iTween.ValueTo(explosion.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.1f, "onupdate", "ExplosionAlpha", "onupdatetarget", base.gameObject));
				iTween.ScaleTo(explosion.gameObject, iTween.Hash("scale", 2f * Vector3.one, "islocal", true, "time", 0.5f, "easeType", iTween.EaseType.easeOutQuad));
				iTween.ValueTo(explosion.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.1f, "delay", 0.4f, "onupdate", "ExplosionAlpha", "onupdatetarget", base.gameObject));
				GameObject tmp = UnityEngine.Object.Instantiate(count.gameObject);
				tmp.transform.parent = count.transform.parent;
				tmp.transform.localScale = Vector3.one;
				tmp.transform.localPosition = count.transform.localPosition;
				iTween.MoveTo(tmp, iTween.Hash("position", tmp.transform.localPosition + 50f * Vector3.up, "islocal", true, "time", 0.4f, "easeType", iTween.EaseType.linear));
				iTween.RotateTo(tmp, iTween.Hash("x", 90, "islocal", true, "time", 0.4f, "easeType", iTween.EaseType.linear));
				UnityEngine.Object.Destroy(tmp, 0.4f);
				cur -= (float)died / 2f;
				count.text = "ｘ " + Mathf.RoundToInt(cur);
				yield return MyWaitForSeconds(1f);
			}
		}

		private IEnumerator ShowResult()
		{
			if (RadingData.BreakNum > 0)
			{
				hukidashi.Play(RadingData.BreakNum);
			}
			count.text = "ｘ " + (RadingData.BeforeNum - RadingData.BreakNum);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1f, "time", 0.5f, "onupdate", "InfoAlpha", "onupdatetarget", base.gameObject));
			if (resFrom[0] != resTo[0] || resFrom[1] != resTo[1] || resFrom[2] != resTo[2] || resFrom[3] != resTo[3])
			{
				if (!CutMode)
				{
					yield return new WaitForSeconds(0.5f);
				}
				for (int j = 0; j < 101; j++)
				{
					for (int i = 0; i < 4; i++)
					{
						if (resFrom[i] != resTo[i])
						{
							downTexts[i].text = " ×" + Convert.ToString((int)Mathf.Lerp(resFrom[i], resTo[i], (float)j / 100f));
							UILabel obj = downTexts[i];
							float g = 1f - (float)j / 200f;
							float b = 1f - (float)j / 200f;
							Color color = downTexts[i].color;
							obj.color = new Color(1f, g, b, color.a);
						}
					}
					if (j % 4 == 0)
					{
						yield return null;
					}
				}
				yield return new WaitForSeconds(1f);
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
		}

		private IEnumerator ResultPhase()
		{
			yield return StartCoroutine(ShowResult());
			if (!CutMode)
			{
				yield return new WaitForSeconds(1f);
			}
			EndAnimation();
		}

		private void EndAnimation()
		{
			flicker = false;
			on = false;
			isFinished = true;
			ship.transform.parent.localPosition = Vector3.zero;
			cnt = App.rand.Next(7) + 3;
			tot = cnt;
			ship.alpha = 0f;
			count.alpha = 0f;
			count.text = string.Empty;
			GetMaterial.alpha = 0f;
			for (int i = 0; i < 4; i++)
			{
				arrows[i].alpha = 0f;
				if (cnt == tot)
				{
					iTween.Resume(arrows[i].gameObject);
				}
				downTexts[i].color = Color.white;
				downTexts[i].text = string.Empty;
			}
			enemy.Reset();
			friendly.Reset();
			DelayedActionsCor = null;
			ResultPhaseCor = null;
		}

		private void OnDestroy()
		{
			friendly = null;
			enemy = null;
			ship = null;
			explosion = null;
			shell = null;
			partExplosion = null;
			count = null;
			GetMaterial = null;
			arrows = null;
			downTexts = null;
			DelayedActionsCor = null;
			ResultPhaseCor = null;
			hukidashi = null;
			AttackCor = null;
			NowWaitCor = null;
		}
	}
}
