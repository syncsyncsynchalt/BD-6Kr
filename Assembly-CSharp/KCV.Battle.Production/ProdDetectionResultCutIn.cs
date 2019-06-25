using KCV.Battle.Utils;
using KCV.Utils;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdDetectionResultCutIn : BaseAnimation
	{
		public enum AnimationList
		{
			DetectionLost,
			DetectionNotFound,
			DetectionSucces
		}

		[SerializeField]
		private UIAtlas _uiAtlas;

		[SerializeField]
		private Transform _uiLabel;

		[SerializeField]
		private List<UISprite> _listLabels;

		[SerializeField]
		private UITexture _uiOverlay;

		private AnimationList _iList;

		public AnimationList detectionResult => _iList;

		public static ProdDetectionResultCutIn Instantiate(ProdDetectionResultCutIn prefab, Transform parent, SakutekiModel model)
		{
			ProdDetectionResultCutIn prodDetectionResultCutIn = UnityEngine.Object.Instantiate(prefab);
			prodDetectionResultCutIn.transform.parent = parent;
			prodDetectionResultCutIn.transform.localScale = Vector3.zero;
			prodDetectionResultCutIn.transform.localPosition = Vector3.zero;
			prodDetectionResultCutIn.setDetection(prodDetectionResultCutIn.getDetectionProductionType(model));
			return prodDetectionResultCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			if (_uiLabel == null)
			{
				Util.FindParentToChild(ref _uiLabel, base.transform, "Label");
			}
			if (_uiOverlay == null)
			{
				Util.FindParentToChild(ref _uiOverlay, base.transform, "WhiteOverlay");
			}
			if (_listLabels == null)
			{
				_listLabels = new List<UISprite>();
				for (int i = 0; i < BattleDefines.DETECTION_RESULT_LABEL_POS[DetectionProductionType.Succes].Count; i++)
				{
					_listLabels.Add(((Component)_uiLabel.transform.FindChild($"Label{i + 1}")).GetComponent<UISprite>());
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiAtlas);
			Mem.Del(ref _uiLabel);
			if (_listLabels != null)
			{
				_listLabels.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe(ref _listLabels);
			Mem.Del(ref _uiOverlay);
			Mem.Del(ref _iList);
		}

		private DetectionProductionType getDetectionProductionType(SakutekiModel model)
		{
			if (model.IsSuccess_f())
			{
				if (model.HasPlane_f() && model.ExistLost_f())
				{
					return DetectionProductionType.SuccesLost;
				}
				return DetectionProductionType.Succes;
			}
			if (model.ExistLost_f())
			{
				return DetectionProductionType.Lost;
			}
			return DetectionProductionType.NotFound;
		}

		private void setDetection(DetectionProductionType iType)
		{
			int num = 0;
			string arg = string.Empty;
			switch (iType)
			{
			case DetectionProductionType.Succes:
			case DetectionProductionType.SuccesLost:
				_iList = AnimationList.DetectionSucces;
				arg = "s1";
				break;
			case DetectionProductionType.Lost:
				_iList = AnimationList.DetectionLost;
				arg = "s2";
				break;
			case DetectionProductionType.NotFound:
				_iList = AnimationList.DetectionNotFound;
				arg = "s3";
				break;
			}
			foreach (UISprite listLabel in _listLabels)
			{
				listLabel.transform.localPosition = new Vector3(BattleDefines.DETECTION_RESULT_LABEL_POS[iType][num], 0f, 0f);
				listLabel.spriteName = $"{arg}-{num + 1}";
				if (listLabel.spriteName == "s2-7")
				{
					listLabel.localSize = new Vector3(80f, 18f, 0f);
				}
				else if (listLabel.spriteName == "s1-6" || listLabel.spriteName == "s3-5")
				{
					listLabel.localSize = new Vector3(40f, 100f);
				}
				else
				{
					listLabel.localSize = new Vector3(100f, 100f, 0f);
				}
				num++;
			}
			_uiLabel.transform.localPosition = ((_iList != 0) ? Vector3.zero : (Vector3.left * 45f));
		}

		public override void Play(Action forceCallback, Action callback)
		{
			base.transform.localScale = Vector3.one;
			base.Play(forceCallback, callback);
		}

		private void playLabelSpacing()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_945);
			((Component)_uiLabel).GetComponent<Animation>().Play(_iList.ToString());
		}
	}
}
