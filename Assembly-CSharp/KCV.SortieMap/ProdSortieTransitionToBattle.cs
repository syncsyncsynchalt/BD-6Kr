using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	[ExecuteInEditMode]
	public class ProdSortieTransitionToBattle : BaseAnimation
	{
		public enum AnimationName
		{
			ProdCloudOutToBattle,
			ProdSortieTransitionToBattleFadeIn
		}

		[SerializeField]
		private UIWidget _uiBattleStart;

		[SerializeField]
		private List<UISprite> _listLabels;

		[SerializeField]
		private List<UITexture> _listClouds;

		[SerializeField]
		private UISprite _uiLine;

		[SerializeField]
		private float _fLabelWidth = 22f;

		private UIPanel _uiPanel;

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdSortieTransitionToBattle Instantiate(ProdSortieTransitionToBattle prefab, Transform parent)
		{
			ProdSortieTransitionToBattle prodSortieTransitionToBattle = UnityEngine.Object.Instantiate(prefab);
			prodSortieTransitionToBattle.transform.parent = parent;
			prodSortieTransitionToBattle.transform.localPositionZero();
			prodSortieTransitionToBattle.transform.localScaleZero();
			prodSortieTransitionToBattle.Setup();
			return prodSortieTransitionToBattle;
		}

		private void LateUpdate()
		{
			if (base.animation.isPlaying)
			{
				_uiLine.width = (int)_fLabelWidth;
			}
		}

		protected override void OnDestroy()
		{
			Mem.Del(ref _uiBattleStart);
			if (_listLabels != null)
			{
				_listLabels.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe(ref _listLabels);
			Mem.DelListSafe(ref _listClouds);
			Mem.Del(ref _uiLine);
			Mem.Del(ref _fLabelWidth);
		}

		private bool Setup()
		{
			panel.depth = 20;
			_uiBattleStart.alpha = 0.01f;
			if (_listClouds == null)
			{
				_listClouds = new List<UITexture>();
				_listClouds.Add(((Component)base.transform.FindChild("CloudBack")).GetComponent<UITexture>());
				_listClouds.Add(((Component)base.transform.FindChild("CloudFront")).GetComponent<UITexture>());
			}
			panel.widgetsAreStatic = true;
			return true;
		}

		public ProdSortieTransitionToBattle QuickFadeInInit()
		{
			panel.alpha = 1f;
			_uiLine.alpha = 0f;
			_uiBattleStart.alpha = 0f;
			_uiBattleStart.transform.localScaleZero();
			_listClouds[0].transform.localScaleOne();
			_listClouds[0].alpha = 1f;
			_listClouds[0].uvRect = new Rect(1f, 0f, 1f, 1f);
			_listClouds[1].transform.localScale = Vector3.one * 1.25f;
			_listClouds[1].alpha = 1f;
			_listClouds[1].uvRect = new Rect(0.5f, 0f, 1f, 1f);
			base.transform.localScaleOne();
			return this;
		}

		public override void Play(Action callback)
		{
			Play(AnimationName.ProdCloudOutToBattle, callback);
		}

		public void Play(AnimationName iName, Action onFinished)
		{
			panel.widgetsAreStatic = false;
			base.transform.localScaleOne();
			base.Play(iName, onFinished);
		}
	}
}
