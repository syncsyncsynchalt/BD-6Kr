using KCV.Scene.Port;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRevampAkashi : MonoBehaviour
	{
		public enum CharacterType
		{
			NONE,
			Akashi,
			AkashiKai
		}

		public enum BodyType
		{
			Normal,
			Making
		}

		[SerializeField]
		private UITexture mTexture_Body;

		[SerializeField]
		private UITexture mTexture_Eye;

		[SerializeField]
		private Texture mTexture2d_Eye_00;

		[SerializeField]
		private Texture mTexture2d_Eye_01;

		[SerializeField]
		private Texture mTexture2d_Eye_02;

		[SerializeField]
		private Texture mTexture2d_Eye_03;

		[SerializeField]
		private Texture mTexture2d_Eye_04;

		private Texture mTexture2d_Body_Normal;

		private Texture mTexture2d_Body_Making;

		private Coroutine mAnimationCoroutine;

		private UIWidget mWidgetThis;

		private bool isAnimation;

		private CharacterType mCharacterType;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 0f;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Body);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Eye);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Eye_00);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Eye_01);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Eye_02);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Eye_03);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Eye_04);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Body_Normal);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Body_Making);
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
			mAnimationCoroutine = null;
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Q))
			{
				ChangeBodyTo(BodyType.Making);
			}
			else if (Input.GetKeyUp(KeyCode.W))
			{
				ChangeBodyTo(BodyType.Normal);
			}
			if (mCharacterType != 0)
			{
				RandomWait(delegate
				{
					bool flag = (UnityEngine.Random.Range(0, 2) != 1) ? true : false;
					bool flag2 = (20 > UnityEngine.Random.Range(0, 100)) ? true : false;
					if (flag)
					{
						if (flag2)
						{
							BlinkWithMove(delegate
							{
								isAnimation = false;
							});
						}
						else
						{
							Blink(delegate
							{
								isAnimation = false;
							});
						}
					}
					else if (flag2)
					{
						Blink(delegate
						{
							BlinkWithMove(delegate
							{
								isAnimation = false;
							});
						});
					}
					else
					{
						DoubleBlink(delegate
						{
							isAnimation = false;
						});
					}
				});
			}
		}

		public void Initialize(CharacterType type)
		{
			mCharacterType = type;
			switch (mCharacterType)
			{
			case CharacterType.Akashi:
				mTexture2d_Body_Normal = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashi");
				mTexture2d_Body_Making = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashi_making");
				mTexture_Body.mainTexture = mTexture2d_Body_Normal;
				mTexture_Eye.mainTexture = mTexture2d_Eye_00;
				break;
			case CharacterType.AkashiKai:
				mTexture2d_Body_Normal = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashikai");
				mTexture2d_Body_Making = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashikai_making");
				mTexture_Body.mainTexture = mTexture2d_Body_Normal;
				mTexture_Eye.mainTexture = mTexture2d_Eye_00;
				break;
			}
			mWidgetThis.alpha = 1f;
		}

		public void RandomWait(Action callBack)
		{
			if (mAnimationCoroutine == null)
			{
				mAnimationCoroutine = StartCoroutine(RandomWaitCoroutine(delegate
				{
					mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack();
					}
				}));
			}
		}

		private void Blink(Action callBack)
		{
			if (mAnimationCoroutine == null)
			{
				mAnimationCoroutine = StartCoroutine(BlinkCoroutine(delegate
				{
					mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack();
					}
				}));
			}
		}

		private void DoubleBlink(Action callBack)
		{
			if (mAnimationCoroutine == null)
			{
				mAnimationCoroutine = StartCoroutine(DoubleBlinkCoroutine(delegate
				{
					mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack();
					}
				}));
			}
		}

		private void BlinkWithMove(Action callBack)
		{
			if (mAnimationCoroutine == null)
			{
				mAnimationCoroutine = StartCoroutine(BlinkWithMoveCoroutine(delegate
				{
					mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack();
					}
				}));
			}
		}

		public void ChangeBodyTo(BodyType bodyType)
		{
			switch (bodyType)
			{
			case BodyType.Making:
				mTexture_Body.mainTexture = mTexture2d_Body_Making;
				break;
			case BodyType.Normal:
				mTexture_Body.mainTexture = mTexture2d_Body_Normal;
				break;
			}
		}

		private IEnumerator BlinkWithMoveCoroutine(Action finished)
		{
			mTexture_Eye.mainTexture = mTexture2d_Eye_00;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_01;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_02;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_03;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_04;
			yield return new WaitForSeconds(0.45f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_03;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_02;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_01;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_00;
			finished?.Invoke();
			yield return null;
		}

		private IEnumerator BlinkCoroutine(Action finished)
		{
			mTexture_Eye.mainTexture = mTexture2d_Eye_00;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_01;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_02;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_01;
			yield return new WaitForSeconds(0.05f);
			mTexture_Eye.mainTexture = mTexture2d_Eye_00;
			finished?.Invoke();
			yield return null;
		}

		private IEnumerator DoubleBlinkCoroutine(Action finished)
		{
			yield return StartCoroutine(BlinkCoroutine(delegate
			{
				this.StartCoroutine(this.BlinkCoroutine(delegate
				{
					if (finished != null)
					{
                        finished();
					}
				}));
			}));
		}

		private IEnumerator RandomWaitCoroutine(Action finished)
		{
			float waitForSeconds = UnityEngine.Random.Range(1.3f, 4f);
			yield return new WaitForSeconds(waitForSeconds);
			finished?.Invoke();
		}
	}
}
