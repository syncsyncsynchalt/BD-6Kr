using KCV.Battle.Utils;
using KCV.BattleCut;
using KCV.Utils;
using local.models;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdFlagshipWreck : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private Generics.Message _clsMessage1;

		[SerializeField]
		private Generics.Message _clsMessage2;

		[SerializeField]
		private UIButton _uiBackBtn;

		[SerializeField]
		private Animation _backBtnAnim;

		[SerializeField]
		private ParticleSystem _uiSmokePar;

		private int debugIndex;

		private bool _isControl;

		private bool _isPlayMsg1;

		private bool _isPlayMsg2;

		private bool _isBattleCut;

		private ShipModel_BattleAll _clsShipModel;

		private DeckModel _clsDeck;

		private KeyControl _keyControl;

		private UILabel _bbLabel;

		public static ProdFlagshipWreck Instantiate(ProdFlagshipWreck prefab, Transform parent, ShipModel_BattleAll flagShip, DeckModel deck, KeyControl input, bool isBattleCut)
		{
			ProdFlagshipWreck prodFlagshipWreck = UnityEngine.Object.Instantiate(prefab);
			prodFlagshipWreck.transform.parent = parent;
			prodFlagshipWreck.transform.localPosition = Vector3.zero;
			prodFlagshipWreck.transform.localScale = Vector3.zero;
			prodFlagshipWreck._init(flagShip, deck, input, isBattleCut);
			return prodFlagshipWreck;
		}

		private bool _init(ShipModel_BattleAll flagShip, DeckModel deck, KeyControl input, bool isBattleCut)
		{
			debugIndex = 0;
			_isControl = false;
			_isFinished = false;
			_isPlayMsg1 = false;
			_isPlayMsg2 = false;
			_isBattleCut = isBattleCut;
			_clsShipModel = flagShip;
			_clsDeck = deck;
			_keyControl = input;
			Util.FindParentToChild(ref _uiShip, base.transform, "ShipObj/Ship");
			Util.FindParentToChild(ref _uiBackBtn, base.transform, "BackIcon");
			Util.FindParentToChild<ParticleSystem>(ref _uiSmokePar, base.transform, "Smoke");
			if ((UnityEngine.Object)_backBtnAnim == null)
			{
				_backBtnAnim = _uiBackBtn.GetComponent<Animation>();
			}
			_uiBackBtn.onClick = Util.CreateEventDelegateList(this, "compFlagshipWreck", null);
			_setObject();
			Util.FindParentToChild(ref _bbLabel, base.transform, "Message1");
			_bbLabel.supportEncoding = false;
			return true;
		}

		private new void OnDestroy()
		{
			_clsMessage1.UnInit();
			_clsMessage2.UnInit();
			Mem.Del(ref _uiShip);
			Mem.Del(ref _clsMessage1);
			Mem.Del(ref _clsMessage2);
			Mem.Del(ref _uiBackBtn);
			Mem.Del(ref _backBtnAnim);
			Mem.Del(ref _uiSmokePar);
			Mem.Del(ref _clsShipModel);
			Mem.Del(ref _clsDeck);
			Mem.Del(ref _keyControl);
			Mem.Del(ref _bbLabel);
		}

		private void _setShipTexture()
		{
			_uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(_clsShipModel, isStart: false);
			_uiShip.MakePixelPerfect();
			_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_clsShipModel.GetGraphicsMstId()).GetShipDisplayCenter(damaged: true));
		}

		private void _setObject()
		{
			_setShipTexture();
			_clsMessage1 = new Generics.Message(base.transform, "Message1");
			_clsMessage1.Init($"『{_clsDeck.Name}』艦隊<br>旗艦「{_clsShipModel.Name}」が<br>大破しました。", 0.05f);
			_clsMessage2 = new Generics.Message(base.transform, "Message2");
			_clsMessage2.Init("進撃は困難です……帰投します。", 0.05f);
		}

		private void _debugObject()
		{
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(debugIndex))
			{
				ShipModelMst shipModelMst = new ShipModelMst(debugIndex);
				_uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(debugIndex, isDamaged: true);
				_uiShip.MakePixelPerfect();
				_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(debugIndex).GetShipDisplayCenter(damaged: true));
				_clsMessage1.UnInit();
				_clsMessage2.UnInit();
				_clsMessage1 = new Generics.Message(base.transform, "Message1");
				_clsMessage1.Init($"『{_clsDeck.Name}』艦隊<br>旗艦「{shipModelMst.Name}」が<br>大破しました。", 0.05f);
				_clsMessage2 = new Generics.Message(base.transform, "Message2");
				_clsMessage2.Init("進撃は困難です……帰投します。", 0.05f);
			}
		}

		public bool Run()
		{
			if (_isPlayMsg1)
			{
				_clsMessage1.Update();
				if (_clsMessage1.IsMessageEnd)
				{
					_clsMessage2.Play();
					_isPlayMsg1 = false;
					_isPlayMsg2 = true;
				}
			}
			if (_isPlayMsg2)
			{
				_clsMessage2.Update();
				if (_clsMessage2.IsMessageEnd)
				{
					if (_isBattleCut)
					{
						UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
						navigation.SetNavigationInFlagshipWreck();
						navigation.Show(0.2f, null);
					}
					else
					{
						UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
						battleNavigation.SetNavigationInFlagshipWreck();
						battleNavigation.Show(0.2f, null);
					}
					_isPlayMsg2 = false;
					_isControl = true;
					_uiBackBtn.GetComponent<UISprite>().alpha = 1f;
					_uiBackBtn.transform.localPosition = new Vector3(420f, -205f, 0f);
					_backBtnAnim.Play();
				}
			}
			if (_isFinished)
			{
				return true;
			}
			if (!_isControl)
			{
				return false;
			}
			if (_keyControl.keyState[1].down)
			{
				compFlagshipWreck();
			}
			return false;
		}

		public override void Play(Action callback)
		{
			base.transform.localScale = Vector3.one;
			base.Play(callback);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.EnemyComming);
		}

		private void DebugPlay()
		{
			_isControl = false;
			_isPlayMsg1 = false;
			_isPlayMsg2 = false;
			_debugObject();
			Play(_actCallback);
		}

		private void _messageStart()
		{
			_isPlayMsg1 = true;
			_clsMessage1.Play();
			_playFShipVoice();
		}

		private void _playParticle()
		{
			_uiSmokePar.Stop();
			_uiSmokePar.Play();
		}

		private void _playPlaneAdmission()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.IA_Failure);
		}

		private void _playFShipVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlayFlagshipWreckVoice(BattleTaskManager.GetBattleManager().Ships_f[0]);
		}

		private void BackBtnEL(GameObject obj)
		{
			compFlagshipWreck();
		}

		private void compFlagshipWreck()
		{
			if (_isControl)
			{
				_isFinished = true;
				_isControl = false;
				if (_isBattleCut)
				{
					UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
					navigation.Hide(0.2f, null);
				}
				else
				{
					UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
					battleNavigation.Hide(0.2f, null);
				}
				Dlg.Call(ref _actCallback);
			}
		}
	}
}
