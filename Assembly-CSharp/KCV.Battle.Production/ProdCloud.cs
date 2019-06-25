using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdCloud : BaseAnimation
	{
		public enum AnimationList
		{
			ProdCloudIn,
			ProdCloudInNotFound,
			ProdCloudOut
		}

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private List<UITexture> _listClouds;

		public static ProdCloud Instantiate(ProdCloud prefab, Transform parent)
		{
			ProdCloud prodCloud = UnityEngine.Object.Instantiate(prefab);
			prodCloud.transform.parent = parent;
			prodCloud.transform.localPositionZero();
			prodCloud.transform.localScaleZero();
			prodCloud.Ctor();
			return prodCloud;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiPanel);
			Mem.DelListSafe(ref _listClouds);
		}

		private bool Ctor()
		{
			if (_uiPanel == null)
			{
				_uiPanel = GetComponent<UIPanel>();
			}
			if (_listClouds == null)
			{
				_listClouds.Add(((Component)base.transform.FindChild("CloudFront")).GetComponent<UITexture>());
				_listClouds.Add(((Component)base.transform.FindChild("CloudBack")).GetComponent<UITexture>());
			}
			return true;
		}

		public void SetPanelDepth(int nDepth)
		{
			_uiPanel.depth = nDepth;
		}

		public override void Play(Enum iEnum, Action forceCallback, Action callback)
		{
			base.transform.localScaleOne();
			base.Play(iEnum, forceCallback, callback);
		}

		protected override void onAnimationFinished()
		{
			base.onAnimationFinished();
		}
	}
}
