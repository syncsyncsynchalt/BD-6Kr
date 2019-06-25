using Common.Enum;
using KCV.Battle.Utils;
using KCV.BattleCut;
using KCV.Utils;
using local.models;
using local.models.battle;
using local.utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdWinRankJudge : MonoBehaviour
	{
		[Serializable]
		private class UIBackground : IDisposable
		{
			[SerializeField]
			private UIPanel _uiBackground;

			[SerializeField]
			private UITexture _uiWhite;

			public void Dispose()
			{
				Mem.Del(ref _uiBackground);
				Mem.Del(ref _uiWhite);
			}

			public bool Init()
			{
				_uiBackground.alpha = 0f;
				_uiBackground.widgetsAreStatic = true;
				return true;
			}

			public void Show(float fTime, Action onFinished)
			{
				_uiBackground.widgetsAreStatic = false;
				_uiBackground.transform.LTValue(_uiBackground.alpha, 1f, fTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiBackground.alpha = x;
				})
					.setOnComplete((Action)delegate
					{
						_uiBackground.widgetsAreStatic = true;
						Dlg.Call(ref onFinished);
					});
			}

			public void Hide(float fTime, Action onFinished)
			{
				_uiBackground.widgetsAreStatic = false;
				_uiBackground.transform.LTValue(_uiBackground.alpha, 0f, fTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiBackground.alpha = x;
				})
					.setOnComplete((Action)delegate
					{
						_uiBackground.widgetsAreStatic = true;
						Dlg.Call(ref onFinished);
					});
			}
		}

		[SerializeField]
		private UIBackground _uiBackground;

		[SerializeField]
		private UIPanel _uiForeground;

		[SerializeField]
		private List<BtlCut_UICircleHPGauge> _listHPGauges;

		[SerializeField]
		private UILabel _uiJudgeLabel;

		[SerializeField]
		private UITexture _uiCongratulation;

		[SerializeField]
		private Transform _traRankBase;

		[SerializeField]
		private UITexture _uiRankTex;

		private BattleWinRankKinds _iWinRank;

		private BattleResultModel _clsResult;

		private int friendMaxHP;

		private int friendLeftHP;

		private int EnemyMaxHP;

		private int EnemyLeftHP;

		private int _nFriendFleetStartHP;

		private int _nFriendFleetEndHP;

		private bool _isBattleCut;

		private BattleWinRankKinds winRank
		{
			get
			{
				return _iWinRank;
			}
			set
			{
				_iWinRank = value;
				_uiRankTex.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/BtlCut_Judge/rate_" + _iWinRank.ToString());
				_uiRankTex.localSize = new Vector2(90f, 102f);
				((Component)_traRankBase).GetComponent<UITexture>().mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/BtlCut_Judge/rate_" + _iWinRank.ToString() + "_bg");
				((Component)_traRankBase).GetComponent<UITexture>().localSize = new Vector2(194f, 194f);
			}
		}

		private bool isPerfect => _nFriendFleetStartHP == _nFriendFleetEndHP;

		public static ProdWinRankJudge Instantiate(ProdWinRankJudge prefab, Transform parent, BattleResultModel model, bool isBattleCut)
		{
			ProdWinRankJudge prodWinRankJudge = UnityEngine.Object.Instantiate(prefab);
			prodWinRankJudge.transform.parent = parent;
			prodWinRankJudge.transform.localPosition = Vector3.right * 2000f;
			prodWinRankJudge.transform.localScaleOne();
			prodWinRankJudge.Init(model, isBattleCut);
			return prodWinRankJudge;
		}

		private void OnDestroy()
		{
			Mem.DelIDisposableSafe(ref _uiBackground);
			Mem.Del(ref _uiForeground);
			Mem.DelListSafe(ref _listHPGauges);
			Mem.Del(ref _uiJudgeLabel);
			Mem.Del(ref _uiCongratulation);
			Mem.Del(ref _traRankBase);
			Mem.Del(ref _uiRankTex);
			Mem.Del(ref _iWinRank);
			Mem.Del(ref _clsResult);
			Mem.Del(ref friendMaxHP);
			Mem.Del(ref friendLeftHP);
			Mem.Del(ref EnemyMaxHP);
			Mem.Del(ref EnemyLeftHP);
			Mem.Del(ref _nFriendFleetStartHP);
			Mem.Del(ref _nFriendFleetEndHP);
		}

		private bool Init(BattleResultModel model, bool isBattleCut)
		{
			_clsResult = model;
			_isBattleCut = isBattleCut;
			winRank = model.WinRank;
			_nFriendFleetStartHP = -1;
			_nFriendFleetEndHP = -1;
			_uiCongratulation.alpha = 0f;
			InitHPGauge(_listHPGauges[0], new List<ShipModel_BattleResult>(_clsResult.Ships_f));
			InitHPGauge(_listHPGauges[1], new List<ShipModel_BattleResult>(_clsResult.Ships_e));
			_listHPGauges.ForEach(delegate(BtlCut_UICircleHPGauge x)
			{
				x.panel.alpha = 0f;
				x.transform.localScaleOne();
			});
			_uiJudgeLabel.text = ((_clsResult.WinRank != BattleWinRankKinds.S) ? BattleDefines.RESULT_WINRUNK_JUDGE_TEXT[(int)_iWinRank] : BattleDefines.RESULT_WINRUNK_JUDGE_TEXT[(int)(_iWinRank + (isPerfect ? 1 : 0))]);
			_uiBackground.Init();
			return true;
		}

		private void InitHPGauge(BtlCut_UICircleHPGauge gauge, List<ShipModel_BattleResult> ships)
		{
			int num = (from x in ships
				where x != null
				select x.HpStart).Sum();
			int num2 = (from x in ships
				where x != null
				select x.HpEnd).Sum();
			if (ships[0].IsFriend())
			{
				_nFriendFleetStartHP = num;
				_nFriendFleetEndHP = num2;
			}
			gauge.SetHPGauge(num, num, num2, isChangeColor: false);
		}

		public IEnumerator StartBattleJudge()
		{
			AudioSource source = SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver[5].source;
			AudioClip clip = SoundFile.LoadSE(GetFanfare());
			float waitTime = clip.length / 3f;
			if (!_isBattleCut)
			{
				_uiBackground.Show(0.5f, null);
			}
			yield return new WaitForSeconds(0.5f);
			ShowHPGauge(_listHPGauges[0]);
			ShowHPGauge(_listHPGauges[1]);
			yield return new WaitForSeconds(0.5f);
			_listHPGauges.ForEach(delegate(BtlCut_UICircleHPGauge x)
			{
				x.PlayNonColor().setDelay(0.5f);
			});
			if (_iWinRank == BattleWinRankKinds.S)
			{
				TrophyUtil.Unlock_At_BattleResultOnlySally();
				ShowCongratulation();
			}
			_uiJudgeLabel.SetActive(isActive: true);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleNightMessage);
			yield return new WaitForSeconds(0.3f);
			_traRankBase.SetActive(isActive: true);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleNightMessage);
			yield return new WaitForSeconds(0.8f);
			_uiRankTex.SetActive(isActive: true);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleNightMessage);
			source.PlayOneShot(clip);
			yield return new WaitForSeconds(waitTime);
			Hide();
			yield return new WaitForSeconds(0.5f);
		}

		private LTDescr Hide()
		{
			if (!_isBattleCut)
			{
				_uiBackground.Hide(0.5f, null);
			}
			return base.transform.LTValue(1f, 0f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiForeground.alpha = x;
				_listHPGauges[0].panel.alpha = x;
				_listHPGauges[1].panel.alpha = x;
			});
		}

		private LTDescr ShowCongratulation()
		{
			return _uiCongratulation.transform.LTValue(0f, 1f, 0.3f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiCongratulation.alpha = x;
			});
		}

		private LTDescr ShowHPGauge(BtlCut_UICircleHPGauge gauge)
		{
			gauge.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				gauge.panel.alpha = x;
			});
			return gauge.transform.LTScale(Vector3.one * 1.2f, 0.5f).setEase(LeanTweenType.linear);
		}

		private SEFIleInfos GetFanfare()
		{
			switch (_iWinRank)
			{
			case BattleWinRankKinds.S:
				return SEFIleInfos.FanfareS;
			case BattleWinRankKinds.A:
				return SEFIleInfos.FanfareA;
			case BattleWinRankKinds.B:
				return SEFIleInfos.FanfareB;
			default:
				return SEFIleInfos.FanfareE;
			}
		}
	}
}
