using KCV.Utils;
using local.managers;
using local.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Ending
{
	public class UIEndingManager : MonoBehaviour
	{
		[SerializeField]
		private EndingShipView ShipView;

		[SerializeField]
		private EndingStaffRoll StaffRoll;

		private PSVitaMovie movie;

		private bool isStartEnd;

		private bool isMovieEnd;

		private Vector3 ShipViewDefaultPos;

		private Vector3 StaffRollDefaultPos;

		private bool isMovedPosition;

		public float ChangeCount;

		public float EndingTime;

		public float ChangeTime;

		public float Totaltime;

		private List<ShipModel> ShipModels;

		private bool isSideChanging;

		private bool isChangeTime;

		private bool isChangeWaiting;

		private int shipIndex;

		private Coroutine UpdateCharacterCor;

		private Coroutine UpdateSideChangeCor;

		private KeyControl key;

		[SerializeField]
		private UILabel foreverLabel;

		[Button("Play", "DebugPlay", new object[]
		{

		})]
		public int Button1;

		[SerializeField]
		private UIPanel MaskPanel;

		private bool isChangeNo;

		[Button("DebugSideChange", "DebugSideChange", new object[]
		{

		})]
		public int button1;

		private Coroutine DebugSideChangeCor;

		private bool isShipChanging => ShipView.isShipChanging;

		private bool isVoice => ShipView.isVoicePlaying;

		private IEnumerator Start()
		{
			movie = ((Component)base.transform.FindChild("Camera")).GetComponent<PSVitaMovie>();
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.1f, null);
			EndingManager manager = new EndingManager();
			yield return StartCoroutine(PlayMovie(manager));
			SingletonMonoBehaviour<Live2DModel>.Instance.isDontRelease = true;
			key = new KeyControl();
			EndingTime = 166f;
			StaffRoll.Speed = (float)StaffRoll.RollSize / EndingTime;
			ShipModels = manager.CreateShipList(15);
			ShipView.CreateLive2DCache(ShipModels);
			ChangeTime = EndingTime / (float)ShipModels.Count;
			ChangeCount = ChangeTime - 5f;
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			Play();
			App.OnlyController = null;
			isStartEnd = true;
		}

		private void Play()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayBGM(BGMFileInfos.Ending);
			StaffRoll.StartStaffRoll();
			UpdateCharacterCor = StartCoroutine(UpdateShipCharacter());
			UpdateSideChangeCor = StartCoroutine(UpdateSideChange());
			StaffRoll.enabled = true;
		}

		private void Update()
		{
			if (isStartEnd)
			{
				key.Update();
				if (key.keyState[1].press && !StaffRoll.isFinishRoll)
				{
					Time.timeScale = 10f;
				}
				else
				{
					Time.timeScale = 1f;
				}
			}
		}

		private IEnumerator PlayMovie(EndingManager manager)
		{
			Debug.Log("エンディングム\u30fcビ\u30fc");
			isMovieEnd = false;
			string MovieFilePath = (!manager.IsGoTrueEnd()) ? MovieFileInfos.EndingNormal.GetFilePath() : MovieFileInfos.EndingTrue.GetFilePath();
			movie.SetLooping(0).SetMode(0).SetOnWarningID(delegate
			{
				this.isMovieEnd = true;
			})
				.SetOnPlay(delegate
				{
					SoundUtils.StopFadeBGM(0.2f, null);
				})
				.SetOnFinished(delegate
				{
					this.isMovieEnd = true;
				})
				.Play(MovieFilePath);
			while (!isMovieEnd)
			{
				if (Input.GetKeyDown(KeyCode.Joystick1Button7))
				{
					isMovieEnd = true;
					movie.Stop();
				}
				yield return null;
			}
			yield return null;
		}

		private IEnumerator UpdateShipCharacter()
		{
			while (true)
			{
				ChangeCount += Time.deltaTime;
				Totaltime += Time.deltaTime;
				if (ChangeCount > EndingTime / (float)ShipModels.Count)
				{
					ChangeCount = 0f;
					isChangeTime = true;
				}
				bool canChange = !isSideChanging && !isShipChanging && !isChangeWaiting && (key.keyState[1].press || !isVoice);
				if (canChange && StaffRoll.isFinishRoll)
				{
					break;
				}
				if (isChangeTime && canChange && shipIndex < ShipModels.Count)
				{
					StartCoroutine(ShipView.ShipChangeCoroutine(ShipModels[shipIndex], shipIndex));
					isChangeTime = false;
					shipIndex++;
				}
				yield return new WaitForEndOfFrame();
				yield return null;
			}
			yield return StartCoroutine(ShipView.CharacterExit());
			StartCoroutine(ShipView.ChangeFinalShip(ShipModels[0], StaffRoll));
			SoundUtils.StopFadeBGM(10f, GotoInheritScene);
			TweenAlpha.Begin(foreverLabel.gameObject, 3f, 1f);
			yield return new WaitForSeconds(5f);
			TweenAlpha.Begin(MaskPanel.gameObject, 4f, 1f);
		}

		private IEnumerator UpdateSideChange()
		{
			bool isChanged = false;
			isChangeNo = false;
			while (true)
			{
				isChangeNo = (shipIndex == 3 || shipIndex == 7 || shipIndex == 11);
				if (!isChanged && isChangeNo && ChangeCount > 2f)
				{
					isChanged = true;
					StartCoroutine(SideChange());
					if (shipIndex >= 11)
					{
						break;
					}
				}
				else if (!isChangeNo)
				{
					isChanged = false;
				}
				yield return null;
			}
		}

		public IEnumerator SideChange()
		{
			while (isSideChanging || isShipChanging)
			{
				yield return null;
			}
			isSideChanging = true;
			float targetValue = (!isMovedPosition) ? 500 : (-500);
			isMovedPosition = !isMovedPosition;
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", targetValue, "time", 0.5f, "onupdate", "UpdateHandler", "ignoretimescale", false, "oncomplete", "OnSideChangeFinish"));
			ShipViewDefaultPos = ShipView.transform.localPosition;
			StaffRollDefaultPos = StaffRoll.transform.localPosition;
			ShipView.OnSideChange();
			DebugSideChangeCor = null;
		}

		private void UpdateHandler(float value)
		{
			ShipView.transform.localPositionX(ShipViewDefaultPos.x + value * 0.85f);
			StaffRoll.transform.localPositionX(StaffRollDefaultPos.x - value * 0.8f);
		}

		private void OnSideChangeFinish()
		{
			isSideChanging = false;
		}

		private void OnDestroy()
		{
			if (UpdateCharacterCor != null)
			{
				StopCoroutine(UpdateCharacterCor);
				UpdateCharacterCor = null;
			}
			if (UpdateSideChangeCor != null)
			{
				StopCoroutine(UpdateSideChangeCor);
				UpdateSideChangeCor = null;
			}
			ShipView = null;
			StaffRoll = null;
			if (ShipModels != null)
			{
				ShipModels.Clear();
			}
			UpdateCharacterCor = null;
			UpdateSideChangeCor = null;
			key = null;
			foreverLabel = null;
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.isDontRelease = false;
			}
		}

		private void DebugSideChange()
		{
			if (DebugSideChangeCor == null)
			{
				DebugSideChangeCor = StartCoroutine(SideChange());
			}
		}

		private void GotoInheritScene()
		{
			SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			Application.LoadLevel(Generics.Scene.InheritSave.ToString());
		}
	}
}
