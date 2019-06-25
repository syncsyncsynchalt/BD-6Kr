using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdShellingFormationJudge : BaseAnimation
	{
		[Serializable]
		private class Formation : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiOverlay;

			[SerializeField]
			private UITexture _uiLabel;

			[SerializeField]
			private UITexture _uiGrow;

			public Formation(Transform obj)
			{
				_tra = obj;
				Util.FindParentToChild(ref _uiOverlay, _tra, "Overlay");
				Util.FindParentToChild(ref _uiLabel, _tra, "Label");
			}

			public bool Init()
			{
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiOverlay);
				if (_uiLabel.mainTexture != null)
				{
					_uiLabel = null;
				}
				Mem.Del(ref _uiLabel);
				if (_uiGrow.mainTexture != null)
				{
					_uiGrow = null;
				}
				Mem.Del(ref _uiGrow);
			}

			public void SetFormation(BattleFormationKinds1 iKind, bool isFriend)
			{
				_uiLabel.mainTexture = Resources.Load<Texture2D>($"Textures/Battle/Shelling/FormationJudge/txt_{iKind.ToString()}");
				_uiLabel.MakePixelPerfect();
				_uiGrow.mainTexture = Resources.Load<Texture2D>(string.Format("Textures/Battle/Shelling/FormationJudge/txt_{0}_{1}", iKind.ToString(), (!isFriend) ? "r" : "g"));
				_uiGrow.MakePixelPerfect();
			}
		}

		[Serializable]
		private class FormationResult : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiMainResult;

			[SerializeField]
			private UISprite _uiMainResultFlash;

			[SerializeField]
			private UISprite _uiSubResult;

			public FormationResult(Transform obj)
			{
				_tra = obj;
				Util.FindParentToChild(ref _uiMainResult, _tra, "MainLabel");
				Util.FindParentToChild(ref _uiSubResult, _tra, "SubLabel");
			}

			public bool Init()
			{
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiMainResult);
				Mem.Del(ref _uiMainResultFlash);
				Mem.Del(ref _uiSubResult);
			}

			public void SetResult(BattleFormationKinds2 iKind)
			{
				switch (iKind)
				{
				case BattleFormationKinds2.Doukou:
				{
					UISprite uiMainResult3 = _uiMainResult;
					string spriteName = "Doukou";
					_uiMainResultFlash.spriteName = spriteName;
					uiMainResult3.spriteName = spriteName;
					Transform transform5 = _uiMainResult.transform;
					Vector3 one = Vector3.zero;
					_uiMainResultFlash.transform.localPosition = one;
					transform5.localPosition = one;
					_uiMainResult.transform.localScale = Vector3.one;
					_uiMainResultFlash.transform.localScale = Vector3.one;
					_uiSubResult.transform.localScale = Vector3.zero;
					break;
				}
				case BattleFormationKinds2.Hankou:
				{
					_uiMainResult.spriteName = "Hankou";
					_uiMainResultFlash.spriteName = "Hankou";
					Transform transform3 = _uiMainResult.transform;
					Vector3 one = Vector3.zero;
					_uiMainResultFlash.transform.localPosition = one;
					transform3.localPosition = one;
					Transform transform4 = _uiMainResult.transform;
					one = Vector3.one;
					_uiMainResultFlash.transform.localScale = one;
					transform4.localScale = one;
					_uiSubResult.transform.localScale = Vector3.zero;
					break;
				}
				case BattleFormationKinds2.T_Own:
				{
					UISprite uiMainResult2 = _uiMainResult;
					string spriteName = "T";
					_uiMainResultFlash.spriteName = spriteName;
					uiMainResult2.spriteName = spriteName;
					_uiMainResult.transform.localPosition = BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS[0];
					_uiMainResultFlash.transform.localPosition = Vector3.zero;
					Transform transform2 = _uiMainResult.transform;
					Vector3 one = Vector3.one;
					_uiMainResultFlash.transform.localScale = one;
					transform2.localScale = one;
					_uiSubResult.spriteName = "fav";
					_uiSubResult.transform.localPosition = BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS[1];
					_uiSubResult.transform.localScale = Vector3.one;
					break;
				}
				case BattleFormationKinds2.T_Enemy:
				{
					UISprite uiMainResult = _uiMainResult;
					string spriteName = "T";
					_uiMainResultFlash.spriteName = spriteName;
					uiMainResult.spriteName = spriteName;
					_uiMainResult.transform.localPosition = BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS[0];
					_uiMainResultFlash.transform.localPosition = Vector3.zero;
					Transform transform = _uiMainResult.transform;
					Vector3 one = Vector3.one;
					_uiMainResultFlash.transform.localScale = one;
					transform.localScale = one;
					_uiSubResult.spriteName = "unfav";
					_uiSubResult.transform.localPosition = BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS[1];
					_uiSubResult.transform.localScale = Vector3.one;
					break;
				}
				}
			}
		}

		[SerializeField]
		private UIAtlas _uiAtlas;

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private FormationResult _clsFormationResult;

		[SerializeField]
		private List<Formation> _listFormation;

		public static ProdShellingFormationJudge Instantiate(ProdShellingFormationJudge prefab, BattleManager manager, Transform parent)
		{
			ProdShellingFormationJudge prodShellingFormationJudge = UnityEngine.Object.Instantiate(prefab);
			prodShellingFormationJudge.transform.parent = parent;
			prodShellingFormationJudge.transform.localPosition = Vector3.zero;
			prodShellingFormationJudge.transform.localScale = Vector3.zero;
			prodShellingFormationJudge.SetFormationData(manager);
			return prodShellingFormationJudge;
		}

		protected override void Awake()
		{
			base.Awake();
			this.GetComponentThis(ref _uiPanel);
			if (_uiOverlay == null)
			{
				Util.FindParentToChild(ref _uiOverlay, base.transform, "Overlay");
			}
			if (_clsFormationResult == null)
			{
				_clsFormationResult = new FormationResult(base.transform.FindChild("FormationResult"));
			}
			if (_listFormation == null)
			{
				foreach (int value in Enum.GetValues(typeof(FleetType)))
				{
					if (value != 2)
					{
						_listFormation.Add(new Formation(base.transform.FindChild($"{((FleetType)value).ToString()}Formation")));
					}
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiAtlas);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _uiOverlay);
			Mem.DelIDisposableSafe(ref _clsFormationResult);
			if (_listFormation != null)
			{
				_listFormation.ForEach(delegate(Formation x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe(ref _listFormation);
		}

		private void SetFormationData(BattleManager manager)
		{
			_clsFormationResult.SetResult(manager.CrossFormationId);
			_listFormation[0].SetFormation(manager.FormationId_f, isFriend: true);
			_listFormation[1].SetFormation(manager.FormationId_e, isFriend: false);
		}

		public override void Play(Action forceCallback, Action callback)
		{
			base.transform.localScale = Vector3.one;
			base.Play(forceCallback, callback);
		}

		private void PlaySlideSE()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_940);
		}

		private void PlayMessageSE()
		{
		}

		protected override void OnForceAnimationFinished()
		{
			base.OnForceAnimationFinished();
		}
	}
}
