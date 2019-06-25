using Common.Enum;
using KCV;
using KCV.Utils;
using local.managers;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdmiralRankJudge : MonoBehaviour
{
	[SerializeField]
	private UILabel lostShipLabel;

	[SerializeField]
	private UILabel TurnLabel;

	[SerializeField]
	private UISprite DiffSprite;

	[SerializeField]
	private UITexture DiffMedal;

	[SerializeField]
	private UISprite RankSprite;

	[SerializeField]
	private Transform PlusIconParent;

	[SerializeField]
	private UISprite[] PlusIcon;

	[SerializeField]
	private ParticleSystem PetalParticle;

	[SerializeField]
	private ParticleSystem MedalParticle;

	private Dictionary<OverallRank, int> RankPlusPosXDic;

	private int _turn;

	private uint _lostShip;

	private Action _callback;

	private Animation _anime;

	public uint Debug_lostShip;

	public int Debug_Turn;

	public OverallRank Debug_rank;

	public int Debug_plus;

	public DifficultKind Debug_diff;

	[Button("DebugInit", "DebugInit", new object[]
	{

	})]
	public int button;

	public void Initialize(EndingManager manager)
	{
		uint lostShipCount = manager.GetLostShipCount();
		int turn = manager.Turn;
		int decorationValue = 0;
		DifficultKind difficulty = manager.UserInfo.Difficulty;
		manager.CalculateTotalRank(out OverallRank rank, out decorationValue);
		TurnLabel.textInt = turn;
		lostShipLabel.text = lostShipCount.ToString();
		SetDiffSprite(difficulty);
		SetDiffMedal(difficulty);
		SetRankSprite(rank);
		SetPlus(decorationValue, rank);
		_turn = turn;
		_lostShip = lostShipCount;
		((Component)PetalParticle).SetActive(isActive: false);
		((Component)MedalParticle).SetActive(isActive: false);
	}

	public void DebugInit()
	{
		Initialize(new EndingManager());
		Play(null);
	}

	public void Play(Action CallBack)
	{
		_callback = CallBack;
		_anime = GetComponent<Animation>();
		_anime.Stop();
		_anime.Play();
	}

	public void StopParticle()
	{
		PetalParticle.Stop();
		((Component)PetalParticle).SetActive(isActive: false);
		MedalParticle.Stop();
		((Component)PetalParticle).SetActive(isActive: false);
	}

	private void stratParticle()
	{
		((Component)PetalParticle).SetActive(isActive: true);
		PetalParticle.Play();
		((Component)MedalParticle).SetActive(isActive: true);
		MedalParticle.Play();
	}

	private void startCountUp(int index)
	{
		if (index == 0 && _turn > 0)
		{
			base.transform.LTValue(0f, _turn, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				int textInt2 = (int)Math.Round(x);
				TurnLabel.textInt = textInt2;
			});
		}
		if (index == 1 && _lostShip != 0)
		{
			base.transform.LTValue(0f, (float)(double)_lostShip, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				int textInt = (int)Math.Round(x);
				lostShipLabel.textInt = textInt;
			})
				.setOnComplete(compTurnCountUp);
		}
	}

	private void compTurnCountUp()
	{
		TurnLabel.textInt = _turn;
	}

	private void compLostCountUp()
	{
		lostShipLabel.text = _lostShip.ToString();
	}

	private void startAnimationFinished()
	{
		if (_callback != null)
		{
			_callback();
		}
	}

	private void playSE()
	{
		SoundUtils.PlayOneShotSE(SEFIleInfos.BattleNightMessage);
	}

	private void playSEPlus(int index)
	{
		if (PlusIcon[index].isActiveAndEnabled)
		{
			SoundUtils.PlayOneShotSE(SEFIleInfos.BattleNightMessage);
		}
	}

	private void SetDiffSprite(DifficultKind diff)
	{
		DiffSprite.spriteName = "txt_diff" + (int)diff + "_gray";
	}

	private void SetDiffMedal(DifficultKind diff)
	{
		DiffMedal.mainTexture = Resources.Load<Texture2D>("Textures/Record/medals/reward_" + (int)diff);
	}

	private void SetRankSprite(OverallRank rank)
	{
		RankSprite.spriteName = "rate_" + rank.ToString();
		RankSprite.MakePixelPerfect();
	}

	private void SetPlus(int plus, OverallRank rank)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i < Mathf.Abs(plus))
			{
				PlusIcon[i].SetActive(isActive: true);
				PlusIcon[i].spriteName = ((0 >= plus) ? "rate_-" : "rate_+");
				PlusIcon[i].MakePixelPerfect();
			}
			else
			{
				PlusIcon[i].SetActive(isActive: false);
			}
		}
		RankPlusPosXDic = new Dictionary<OverallRank, int>();
		RankPlusPosXDic.Add(OverallRank.EX, 222);
		RankPlusPosXDic.Add(OverallRank.S, 177);
		RankPlusPosXDic.Add(OverallRank.A, 177);
		RankPlusPosXDic.Add(OverallRank.B, 190);
		RankPlusPosXDic.Add(OverallRank.C, 190);
		RankPlusPosXDic.Add(OverallRank.D, 190);
		RankPlusPosXDic.Add(OverallRank.E, 190);
		RankPlusPosXDic.Add(OverallRank.F, 190);
		PlusIconParent.localPositionX(RankPlusPosXDic[rank]);
	}

	private void OnDestroy()
	{
		lostShipLabel = null;
		TurnLabel = null;
		DiffSprite = null;
		DiffMedal = null;
		RankSprite = null;
		PlusIconParent = null;
		PlusIcon = null;
		PetalParticle = null;
		MedalParticle = null;
		RankPlusPosXDic.Clear();
		RankPlusPosXDic = null;
	}
}
