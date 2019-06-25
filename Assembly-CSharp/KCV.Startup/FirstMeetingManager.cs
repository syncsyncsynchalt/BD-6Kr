using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(UIRoot))]
	public class FirstMeetingManager : MonoBehaviour
	{
		[SerializeField]
		private TweenPosition CameraZoomTween;

		[SerializeField]
		private TweenAlpha WhiteMaskTweenAlpha;

		[SerializeField]
		private UIShipCharacter ShipCharacter;

		[SerializeField]
		private TweenAlpha BGTweenAlpha;

		private Dictionary<int, Live2DModel.MotionType> MotionList;

		private UIPanel _uiPanel;

		public Live2DModel.MotionType DebugType;

		[Button("DebugPlay", "DebugPlay", new object[]
		{

		})]
		public int __Button1;

		public int DebugMstID;

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static FirstMeetingManager Instantiate(FirstMeetingManager prefab, int nMstId)
		{
			FirstMeetingManager firstMeetingManager = UnityEngine.Object.Instantiate(prefab);
			firstMeetingManager.transform.position = Vector3.right * 50f;
			firstMeetingManager.transform.localScaleZero();
			firstMeetingManager.VirtualCtor(nMstId);
			return firstMeetingManager;
		}

		private void Awake()
		{
			panel.alpha = 0f;
		}

		private void OnDestroy()
		{
			Mem.Del(ref CameraZoomTween);
			Mem.Del(ref WhiteMaskTweenAlpha);
			Mem.Del(ref ShipCharacter);
			Mem.Del(ref _uiPanel);
			if (MotionList != null)
			{
				MotionList.Clear();
			}
			MotionList = null;
		}

		private bool VirtualCtor(int nMstId)
		{
			panel.alpha = 0f;
			ShipCharacter.ChangeCharacter(new ShipModelMst(nMstId));
			return true;
		}

		public IEnumerator Play(int MstID, Action OnFinished)
		{
			panel.alpha = 1f;
			MotionList = new Dictionary<int, Live2DModel.MotionType>();
			MotionList.Add(9, Live2DModel.MotionType.Love2);
			MotionList.Add(37, Live2DModel.MotionType.Port);
			MotionList.Add(1, Live2DModel.MotionType.Port);
			MotionList.Add(33, Live2DModel.MotionType.Port);
			MotionList.Add(96, Live2DModel.MotionType.Secret);
			MotionList.Add(43, Live2DModel.MotionType.Port);
			MotionList.Add(54, Live2DModel.MotionType.Battle);
			MotionList.Add(55, Live2DModel.MotionType.Port);
			MotionList.Add(56, Live2DModel.MotionType.Secret);
			MotionList.Add(94, Live2DModel.MotionType.Secret);
			MotionList.Add(46, Live2DModel.MotionType.Secret);
			if (MotionList.ContainsKey(MstID))
			{
				Live2DModel.MotionType motionType = MotionList[MstID];
			}
			SoundUtils.StopBGM();
			SingletonMonoBehaviour<FadeCamera>.Instance.SetTransitionRule(FadeCamera.TransitionRule.NONE);
			ShipModelMst model = new ShipModelMst(MstID);
			ShipCharacter.ChangeCharacter(model);
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			SoundUtils.PlaySE(SEFIleInfos.RewardGet2, null);
			WhiteMaskTweenAlpha.ResetToBeginning();
			WhiteMaskTweenAlpha.PlayForward();
			WhiteMaskTweenAlpha.SetOnFinished(delegate
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // SingletonMonoBehaviour<Live2DModel>.Instance.Play(base._003Cmotion_003E__0, null);

				ShipUtils.PlayShipVoice(model, 1);
			});
			yield return new WaitForSeconds(6f);
			BGTweenAlpha.ResetToBeginning();
			BGTweenAlpha.PlayForward();
			CameraZoomTween.ResetToBeginning();
			CameraZoomTween.PlayForward();
			yield return new WaitForSeconds(4.5f);
			WhiteMaskTweenAlpha.onFinished.Clear();
			WhiteMaskTweenAlpha.PlayReverse();
			yield return new WaitForSeconds(WhiteMaskTweenAlpha.duration + 2f);
			TweenColor.Begin(WhiteMaskTweenAlpha.gameObject, 1f, Color.black);
			yield return new WaitForSeconds(1.5f);
			OnFinished?.Invoke();
		}

		private void DebugPlay()
		{
			base.StopAllCoroutines();
			StartCoroutine(Play(DebugMstID, null));
		}
	}
}
