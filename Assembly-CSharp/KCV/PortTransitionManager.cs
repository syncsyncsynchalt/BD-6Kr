using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class PortTransitionManager : MonoBehaviour
	{
		[SerializeField]
		private UIPanel TransitionPanel;

		[SerializeField]
		private UITexture TransitionBG;

		[SerializeField]
		private Camera TransitionCamera;

		[Button("DebugTransition", "StartTransition", new object[]
		{

		})]
		public int button1;

		[Button("Reset", "Reset", new object[]
		{

		})]
		public int button2;

		private Action onfinished;

		private Action SettingOnFinished;

		public bool isOver;

		[NonSerialized]
		public bool isTransitionNow;

		private Vector3 defaultPosition;

		private Dictionary<Generics.Scene, string> SceneBG_FilePaths;

		private void Start()
		{
			SceneBG_FilePaths = new Dictionary<Generics.Scene, string>();
			SceneBG_FilePaths.Add(Generics.Scene.Arsenal, "Textures/Arsenal/arsenal_bg");
			SceneBG_FilePaths.Add(Generics.Scene.ImprovementArsenal, "Textures/Common/BG/arsenal2_bg");
			SceneBG_FilePaths.Add(Generics.Scene.Record, "Textures/Common/BG/CommonBG");
			SceneBG_FilePaths.Add(Generics.Scene.Organize, "Textures/Organize/organize_bg");
			SceneBG_FilePaths.Add(Generics.Scene.Remodel, "Textures/Common/BG/common3");
			SceneBG_FilePaths.Add(Generics.Scene.Duty, "Textures/Common/BG/hex_bg");
			SceneBG_FilePaths.Add(Generics.Scene.Repair, "Textures/repair/NewUI/bg/supply_set");
			SceneBG_FilePaths.Add(Generics.Scene.Supply, "Textures/Common/BG/hex_bg");
			SceneBG_FilePaths.Add(Generics.Scene.Item, "Textures/Item/item_bg2");
			SceneBG_FilePaths.Add(Generics.Scene.Album, "Textures/Album/album_bg");
			Vector3 localPosition = TransitionPanel.transform.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = TransitionPanel.transform.localPosition;
			float y = localPosition2.y;
			Vector3 localPosition3 = TransitionPanel.transform.localPosition;
			defaultPosition = new Vector3(x, y, localPosition3.z);
			this.SetActiveChildren(isActive: false);
		}

		public void StartTransition(Generics.Scene NextScene, bool isPortFramePos, Action act)
		{
			this.SetActiveChildren(isActive: true);
			setCameraDepth(NextScene);
			if (isPortFramePos)
			{
				setOnPortCirclePosition();
			}
			else
			{
				setDefaultPosition();
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.fadeOutCircleButtonLabel();
			}
			Reset();
			setBG(NextScene);
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0.1, "to", 2500, "time", 0.4f, "onupdate", "UpdateHandler", "oncomplete", "OnFinished"));
			onfinished = act;
			isOver = true;
		}

		private void setCameraDepth(Generics.Scene NextScene)
		{
			if (NextScene == Generics.Scene.Album || NextScene == Generics.Scene.Interior)
			{
				TransitionCamera.depth = 5f;
			}
			else
			{
				TransitionCamera.depth = 1.3f;
			}
		}

		private void setOnPortCirclePosition()
		{
			TransitionPanel.transform.localPosition = new Vector3(-415f, 210f, 0f);
			TransitionBG.transform.localPosition = new Vector3(415f, -210f, 0f);
		}

		private void setDefaultPosition()
		{
			TransitionPanel.transform.localPosition = defaultPosition;
			TransitionBG.transform.localPosition = -defaultPosition;
		}

		public void EndTransition(Action act, bool isLockTouchOff = true, bool isPortFrameColliderEnable = true)
		{
			if (!isOver)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
				{
					if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
					{
						SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
						SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
						SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(!isLockTouchOff);
					}
					if (act != null)
					{
						act();
						act = null;
						this.SetActiveChildren(isActive: false);
					}
					if (SingletonMonoBehaviour<UIPortFrame>.exist() && isPortFrameColliderEnable)
					{
						SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
					}
					isTransitionNow = false;
				});
				return;
			}
			TweenAlpha tweenAlpha = TweenAlpha.Begin(TransitionPanel.gameObject, 0.4f, 0f);
			tweenAlpha.onFinished.Clear();
			tweenAlpha.SetOnFinished(delegate
			{
				if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
					Debug.Log("transitionEnd");
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(!isLockTouchOff);
				}
				TransitionPanel.SetRect(0f, 0f, 0.1f, 0.1f);
				if (act != null)
				{
					act();
					act = null;
					this.SetActiveChildren(isActive: false);
				}
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
				}
				isOver = false;
				isTransitionNow = false;
			});
		}

		private void setBG(Generics.Scene NextScene)
		{
			if (SceneBG_FilePaths.ContainsKey(NextScene))
			{
				TransitionBG.mainTexture = (Resources.Load(SceneBG_FilePaths[NextScene]) as Texture);
			}
			else
			{
				TransitionBG.mainTexture = (Resources.Load("Textures/Common/BG/CommonBG") as Texture);
			}
			if (NextScene == Generics.Scene.Organize || NextScene == Generics.Scene.Arsenal || NextScene == Generics.Scene.Repair)
			{
				TransitionBG.width = 1040;
				TransitionBG.height = 589;
			}
			else
			{
				TransitionBG.width = 960;
				TransitionBG.height = 544;
			}
		}

		private void UpdateHandler(float value)
		{
			TransitionPanel.SetRect(0f, 0f, value, value);
		}

		private void OnFinished()
		{
			if (onfinished != null)
			{
				onfinished();
				setDefaultPosition();
				onfinished = null;
			}
		}

		private void Reset()
		{
			TransitionPanel.alpha = 1f;
			TransitionPanel.SetRect(0f, 0f, 0.1f, 0.1f);
		}

		private void DebugTransition()
		{
			Reset();
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0.1, "to", 2500, "time", 0.4f, "onupdate", "UpdateHandler", "oncomplete", "OnFinished"));
		}

		public void setOnFinished(Action onfinished)
		{
			SettingOnFinished = onfinished;
		}

		private void OnDestroy()
		{
			TransitionPanel = null;
			TransitionBG = null;
			TransitionCamera = null;
			onfinished = null;
			SettingOnFinished = null;
			if (SceneBG_FilePaths != null)
			{
				Mem.DelDictionarySafe(ref SceneBG_FilePaths);
			}
		}
	}
}
