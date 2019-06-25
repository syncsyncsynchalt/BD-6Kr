using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UIVeteransReportMVPShip : BaseUISortieBattleShip<ShipModel_BattleResult>
	{
		[SerializeField]
		private float _fShowXOffs = 100f;

		private Vector3 _vTweenTargetPos;

		public float textureAlpha
		{
			get
			{
				return _uiShipTex.alpha;
			}
			set
			{
				_uiShipTex.alpha = value;
			}
		}

		public static UIVeteransReportMVPShip Instantiate(UIVeteransReportMVPShip prefab, Transform parent, Vector3 pos, ShipModel_BattleResult model)
		{
			UIVeteransReportMVPShip uIVeteransReportMVPShip = UnityEngine.Object.Instantiate(prefab);
			uIVeteransReportMVPShip.transform.parent = parent;
			uIVeteransReportMVPShip.transform.localScaleOne();
			uIVeteransReportMVPShip.transform.localPosition = pos;
			uIVeteransReportMVPShip.SetShipTexture(model);
			return uIVeteransReportMVPShip;
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _fShowXOffs);
			base.OnUnInit();
		}

		protected override void SetShipTexture(ShipModel_BattleResult model)
		{
			if (model != null)
			{
				base.SetShipTexture(model);
				_uiShipTex.transform.localPosition = Util.Poi2Vec(model.Offsets.GetCutinSp1_InBattle(model.IsDamaged()));
				SetLovOffset(model);
			}
		}

		protected override void SetLovOffset(ShipModel_BattleResult model)
		{
			LovLevel lovLevel = SortieBattleUtils.GetLovLevel(model);
			Vector3 localScale = Vector3.one * SortieBattleUtils.GetLovScaleMagnification(lovLevel);
			float t = Mathe.Rate(0f, 1f, 1f / (float)(Enum.GetValues(typeof(LovLevel)).Length - 1) * (float)(lovLevel - 1));
			_vTweenTargetPos = Vector3.Lerp(originPos, lovMaxPos, t);
			base.transform.localScale = localScale;
			base.transform.localPositionY(_vTweenTargetPos.y);
		}

		public void Show(bool isPlayVoice, Action callback)
		{
			if (shipModel != null)
			{
				base.transform.LTMoveLocal(_vTweenTargetPos, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete((Action)delegate
				{
					if (isPlayVoice)
					{
						ShipUtils.PlayMVPVoice(shipModel);
					}
					Dlg.Call(ref callback);
				});
				base.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
				{
					panel.alpha = x;
				});
			}
		}
	}
}
