using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LT.Tweening
{
	public class LeanTween : MonoBehaviour
	{
		public static bool throwErrors = true;

		public static float tau = (float)Math.PI * 2f;

		private static LTDescr[] tweens;

		private static int[] tweensFinished;

		private static LTDescr tween;

		private static int tweenMaxSearch = -1;

		private static int maxTweens = 400;

		private static int frameRendered = -1;

		private static GameObject _tweenEmpty;

		private static float dtEstimated = -1f;

		public static float dtManual;

		private static float dt;

		private static float dtActual;

		private static int i;

		private static int j;

		private static int finishedCnt;

		private static AnimationCurve punch = new AnimationCurve(new Keyframe[9]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.112586f, 0.9976035f),
			new Keyframe(0.3120486f, -0.1720615f),
			new Keyframe(0.4316337f, 0.07030682f),
			new Keyframe(0.5524869f, -0.03141804f),
			new Keyframe(0.6549395f, 0.003909959f),
			new Keyframe(0.770987f, -0.009817753f),
			new Keyframe(0.8838775f, 0.001939224f),
			new Keyframe(1f, 0f)
		});

		private static AnimationCurve shake = new AnimationCurve(new Keyframe[4]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 1f),
			new Keyframe(0.75f, -1f),
			new Keyframe(1f, 0f)
		});

		private static Transform trans;

		private static float timeTotal;

		private static TweenAction tweenAction;

		private static float ratioPassed;

		private static float from;

		private static float val;

		private static bool isTweenFinished;

		private static int maxTweenReached;

		private static Vector3 newVect;

		private static GameObject target;

		private static GameObject customTarget;

		public static int startSearch = 0;

		public static LTDescr d;

		private static Action<LTEvent>[] eventListeners;

		private static GameObject[] goListeners;

		private static int eventsMaxSearch = 0;

		public static int EVENTS_MAX = 10;

		public static int LISTENERS_MAX = 10;

		private static int INIT_LISTENERS_MAX = LISTENERS_MAX;

		public static int maxSearch => tweenMaxSearch;

		public static int tweensRunning
		{
			get
			{
				int num = 0;
				for (int i = 0; i <= tweenMaxSearch; i++)
				{
					if (tweens[i].toggle)
					{
						num++;
					}
				}
				return num;
			}
		}

		public static GameObject tweenEmpty
		{
			get
			{
				init(maxTweens);
				return _tweenEmpty;
			}
		}

		public static void init()
		{
			init(maxTweens);
		}

		public static void init(int maxSimultaneousTweens)
		{
			if (tweens == null)
			{
				maxTweens = maxSimultaneousTweens;
				tweens = new LTDescr[maxTweens];
				tweensFinished = new int[maxTweens];
				_tweenEmpty = new GameObject();
				_tweenEmpty.name = "~LeanTween";
				_tweenEmpty.AddComponent(typeof(LeanTween));
				_tweenEmpty.isStatic = true;
				_tweenEmpty.hideFlags = HideFlags.HideAndDontSave;
				UnityEngine.Object.DontDestroyOnLoad(_tweenEmpty);
				for (int i = 0; i < maxTweens; i++)
				{
					tweens[i] = new LTDescr();
				}
			}
		}

		public static void reset()
		{
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				tweens[i].toggle = false;
			}
			tweens = null;
			UnityEngine.Object.Destroy(_tweenEmpty);
		}

		public void Update()
		{
			update();
		}

		public void OnLevelWasLoaded(int lvl)
		{
			LTGUI.reset();
		}

		public static void update()
		{
			//IL_336c: Unknown result type (might be due to invalid IL or missing references)
			//IL_338f: Expected O, but got Unknown
			//IL_33da: Unknown result type (might be due to invalid IL or missing references)
			//IL_33fd: Expected O, but got Unknown
			if (frameRendered == Time.frameCount)
			{
				return;
			}
			init();
			if (dtEstimated < 0f)
			{
				dtEstimated = 0f;
			}
			else
			{
				dtEstimated = Time.unscaledDeltaTime;
			}
			dtActual = Time.deltaTime;
			maxTweenReached = 0;
			finishedCnt = 0;
			for (int i = 0; i <= tweenMaxSearch && i < maxTweens; i++)
			{
				if (!tweens[i].toggle)
				{
					continue;
				}
				maxTweenReached = i;
				tween = tweens[i];
				trans = tween.trans;
				timeTotal = tween.time;
				tweenAction = tween.type;
				if (tween.useEstimatedTime)
				{
					dt = dtEstimated;
				}
				else if (tween.useFrames)
				{
					dt = 1f;
				}
				else if (tween.useManualTime)
				{
					dt = dtManual;
				}
				else if (tween.direction == 0f)
				{
					dt = 0f;
				}
				else
				{
					dt = dtActual;
				}
				if (trans == null)
				{
					removeTween(i);
					continue;
				}
				isTweenFinished = false;
				if (tween.delay <= 0f)
				{
					if (tween.passed + dt > tween.time && tween.direction > 0f)
					{
						isTweenFinished = true;
						tween.passed = tween.time;
					}
					else if (tween.direction < 0f && tween.passed - dt < 0f)
					{
						isTweenFinished = true;
						tween.passed = Mathf.Epsilon;
					}
				}
				if (!tween.hasInitiliazed && (((double)tween.passed == 0.0 && (double)tween.delay == 0.0) || (double)tween.passed > 0.0))
				{
					tween.init();
				}
				if (tween.delay <= 0f)
				{
					if (timeTotal <= 0f)
					{
						ratioPassed = 1f;
					}
					else
					{
						ratioPassed = tween.passed / timeTotal;
					}
					if (ratioPassed > 1f)
					{
						ratioPassed = 1f;
					}
					else if (ratioPassed < 0f)
					{
						ratioPassed = 0f;
					}
					if (tweenAction >= TweenAction.MOVE_X && tweenAction < TweenAction.MOVE)
					{
						if (tween.animationCurve != null)
						{
							val = tweenOnCurve(tween, ratioPassed);
						}
						else
						{
							switch (tween.tweenType)
							{
							case LeanTweenType.linear:
								val = tween.from.x + tween.diff.x * ratioPassed;
								break;
							case LeanTweenType.easeOutQuad:
								val = easeOutQuadOpt(tween.from.x, tween.diff.x, ratioPassed);
								break;
							case LeanTweenType.easeInQuad:
								val = easeInQuadOpt(tween.from.x, tween.diff.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutQuad:
								val = easeInOutQuadOpt(tween.from.x, tween.diff.x, ratioPassed);
								break;
							case LeanTweenType.easeInCubic:
								val = easeInCubic(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeOutCubic:
								val = easeOutCubic(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutCubic:
								val = easeInOutCubic(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInQuart:
								val = easeInQuart(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeOutQuart:
								val = easeOutQuart(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutQuart:
								val = easeInOutQuart(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInQuint:
								val = easeInQuint(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeOutQuint:
								val = easeOutQuint(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutQuint:
								val = easeInOutQuint(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInSine:
								val = easeInSine(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeOutSine:
								val = easeOutSine(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutSine:
								val = easeInOutSine(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInExpo:
								val = easeInExpo(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeOutExpo:
								val = easeOutExpo(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutExpo:
								val = easeInOutExpo(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInCirc:
								val = easeInCirc(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeOutCirc:
								val = easeOutCirc(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutCirc:
								val = easeInOutCirc(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInBounce:
								val = easeInBounce(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeOutBounce:
								val = easeOutBounce(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInOutBounce:
								val = easeInOutBounce(tween.from.x, tween.to.x, ratioPassed);
								break;
							case LeanTweenType.easeInBack:
								val = easeInBack(tween.from.x, tween.to.x, ratioPassed, tween.overshoot);
								break;
							case LeanTweenType.easeOutBack:
								val = easeOutBack(tween.from.x, tween.to.x, ratioPassed, tween.overshoot);
								break;
							case LeanTweenType.easeInOutBack:
								val = easeInOutBack(tween.from.x, tween.to.x, ratioPassed, tween.overshoot);
								break;
							case LeanTweenType.easeInElastic:
								val = easeInElastic(tween.from.x, tween.to.x, ratioPassed, tween.overshoot, tween.period);
								break;
							case LeanTweenType.easeOutElastic:
								val = easeOutElastic(tween.from.x, tween.to.x, ratioPassed, tween.overshoot, tween.period);
								break;
							case LeanTweenType.easeInOutElastic:
								val = easeInOutElastic(tween.from.x, tween.to.x, ratioPassed, tween.overshoot, tween.period);
								break;
							case LeanTweenType.easeShake:
							case LeanTweenType.punch:
								if (tween.tweenType == LeanTweenType.punch)
								{
									tween.animationCurve = punch;
								}
								else if (tween.tweenType == LeanTweenType.easeShake)
								{
									tween.animationCurve = shake;
								}
								tween.to.x = tween.from.x + tween.to.x;
								tween.diff.x = tween.to.x - tween.from.x;
								val = tweenOnCurve(tween, ratioPassed);
								break;
							case LeanTweenType.easeSpring:
								val = spring(tween.from.x, tween.to.x, ratioPassed);
								break;
							default:
								val = tween.from.x + tween.diff.x * ratioPassed;
								break;
							}
						}
						if (tweenAction == TweenAction.MOVE_X)
						{
							Transform transform = trans;
							float x = val;
							Vector3 position = trans.position;
							float y = position.y;
							Vector3 position2 = trans.position;
							transform.position = new Vector3(x, y, position2.z);
						}
						else if (tweenAction == TweenAction.MOVE_Y)
						{
							Transform transform2 = trans;
							Vector3 position3 = trans.position;
							float x2 = position3.x;
							float y2 = val;
							Vector3 position4 = trans.position;
							transform2.position = new Vector3(x2, y2, position4.z);
						}
						else if (tweenAction == TweenAction.MOVE_Z)
						{
							Transform transform3 = trans;
							Vector3 position5 = trans.position;
							float x3 = position5.x;
							Vector3 position6 = trans.position;
							transform3.position = new Vector3(x3, position6.y, val);
						}
						if (tweenAction == TweenAction.MOVE_LOCAL_X)
						{
							Transform transform4 = trans;
							float x4 = val;
							Vector3 localPosition = trans.localPosition;
							float y3 = localPosition.y;
							Vector3 localPosition2 = trans.localPosition;
							transform4.localPosition = new Vector3(x4, y3, localPosition2.z);
						}
						else if (tweenAction == TweenAction.MOVE_LOCAL_Y)
						{
							Transform transform5 = trans;
							Vector3 localPosition3 = trans.localPosition;
							float x5 = localPosition3.x;
							float y4 = val;
							Vector3 localPosition4 = trans.localPosition;
							transform5.localPosition = new Vector3(x5, y4, localPosition4.z);
						}
						else if (tweenAction == TweenAction.MOVE_LOCAL_Z)
						{
							Transform transform6 = trans;
							Vector3 localPosition5 = trans.localPosition;
							float x6 = localPosition5.x;
							Vector3 localPosition6 = trans.localPosition;
							transform6.localPosition = new Vector3(x6, localPosition6.y, val);
						}
						else if (tweenAction == TweenAction.MOVE_CURVED)
						{
							if (tween.path.orientToPath)
							{
								if (tween.path.orientToPath2d)
								{
									tween.path.place2d(trans, val);
								}
								else
								{
									tween.path.place(trans, val);
								}
							}
							else
							{
								trans.position = tween.path.point(val);
							}
						}
						else if (tweenAction == TweenAction.MOVE_CURVED_LOCAL)
						{
							if (tween.path.orientToPath)
							{
								if (tween.path.orientToPath2d)
								{
									tween.path.placeLocal2d(trans, val);
								}
								else
								{
									tween.path.placeLocal(trans, val);
								}
							}
							else
							{
								trans.localPosition = tween.path.point(val);
							}
						}
						else if (tweenAction == TweenAction.MOVE_SPLINE)
						{
							if (tween.spline.orientToPath)
							{
								if (tween.spline.orientToPath2d)
								{
									tween.spline.place2d(trans, val);
								}
								else
								{
									tween.spline.place(trans, val);
								}
							}
							else
							{
								trans.position = tween.spline.point(val);
							}
						}
						else if (tweenAction == TweenAction.MOVE_SPLINE_LOCAL)
						{
							if (tween.spline.orientToPath)
							{
								if (tween.spline.orientToPath2d)
								{
									tween.spline.placeLocal2d(trans, val);
								}
								else
								{
									tween.spline.placeLocal(trans, val);
								}
							}
							else
							{
								trans.localPosition = tween.spline.point(val);
							}
						}
						else if (tweenAction == TweenAction.SCALE_X)
						{
							Transform transform7 = trans;
							float x7 = val;
							Vector3 localScale = trans.localScale;
							float y5 = localScale.y;
							Vector3 localScale2 = trans.localScale;
							transform7.localScale = new Vector3(x7, y5, localScale2.z);
						}
						else if (tweenAction == TweenAction.SCALE_Y)
						{
							Transform transform8 = trans;
							Vector3 localScale3 = trans.localScale;
							float x8 = localScale3.x;
							float y6 = val;
							Vector3 localScale4 = trans.localScale;
							transform8.localScale = new Vector3(x8, y6, localScale4.z);
						}
						else if (tweenAction == TweenAction.SCALE_Z)
						{
							Transform transform9 = trans;
							Vector3 localScale5 = trans.localScale;
							float x9 = localScale5.x;
							Vector3 localScale6 = trans.localScale;
							transform9.localScale = new Vector3(x9, localScale6.y, val);
						}
						else if (tweenAction == TweenAction.ROTATE_X)
						{
							Transform transform10 = trans;
							float x10 = val;
							Vector3 eulerAngles = trans.eulerAngles;
							float y7 = eulerAngles.y;
							Vector3 eulerAngles2 = trans.eulerAngles;
							transform10.eulerAngles = new Vector3(x10, y7, eulerAngles2.z);
						}
						else if (tweenAction == TweenAction.ROTATE_Y)
						{
							Transform transform11 = trans;
							Vector3 eulerAngles3 = trans.eulerAngles;
							float x11 = eulerAngles3.x;
							float y8 = val;
							Vector3 eulerAngles4 = trans.eulerAngles;
							transform11.eulerAngles = new Vector3(x11, y8, eulerAngles4.z);
						}
						else if (tweenAction == TweenAction.ROTATE_Z)
						{
							Transform transform12 = trans;
							Vector3 eulerAngles5 = trans.eulerAngles;
							float x12 = eulerAngles5.x;
							Vector3 eulerAngles6 = trans.eulerAngles;
							transform12.eulerAngles = new Vector3(x12, eulerAngles6.y, val);
						}
						else if (tweenAction == TweenAction.ROTATE_AROUND)
						{
							Vector3 localPosition7 = trans.localPosition;
							Vector3 vector = trans.TransformPoint(tween.point);
							trans.RotateAround(vector, tween.axis, 0f - val);
							Vector3 b = localPosition7 - trans.localPosition;
							trans.localPosition = localPosition7 - b;
							trans.rotation = tween.origRotation;
							vector = trans.TransformPoint(tween.point);
							trans.RotateAround(vector, tween.axis, val);
						}
						else if (tweenAction == TweenAction.ROTATE_AROUND_LOCAL)
						{
							Vector3 localPosition8 = trans.localPosition;
							trans.RotateAround(trans.TransformPoint(tween.point), trans.TransformDirection(tween.axis), 0f - val);
							Vector3 b2 = localPosition8 - trans.localPosition;
							trans.localPosition = localPosition8 - b2;
							trans.localRotation = tween.origRotation;
							Vector3 vector2 = trans.TransformPoint(tween.point);
							trans.RotateAround(vector2, trans.TransformDirection(tween.axis), val);
						}
						else if (tweenAction == TweenAction.ALPHA)
						{
							SpriteRenderer component = trans.gameObject.GetComponent<SpriteRenderer>();
							if ((UnityEngine.Object)component != null)
							{
								SpriteRenderer spriteRenderer = component;
								Color color = component.color;
								float r = color.r;
								Color color2 = component.color;
								float g = color2.g;
								Color color3 = component.color;
								spriteRenderer.color = new Color(r, g, color3.b, val);
							}
							else
							{
								if ((UnityEngine.Object)trans.gameObject.GetComponent<Renderer>() != null)
								{
									Material[] materials = trans.gameObject.GetComponent<Renderer>().materials;
									foreach (Material material in materials)
									{
										if (material.HasProperty("_Color"))
										{
											Material material2 = material;
											Color color4 = material.color;
											float r2 = color4.r;
											Color color5 = material.color;
											float g2 = color5.g;
											Color color6 = material.color;
											material2.color = new Color(r2, g2, color6.b, val);
										}
										else if (material.HasProperty("_TintColor"))
										{
											Color color7 = material.GetColor("_TintColor");
											material.SetColor("_TintColor", new Color(color7.r, color7.g, color7.b, val));
										}
									}
								}
								if (trans.childCount > 0)
								{
									foreach (Transform tran in trans)
									{
										if ((UnityEngine.Object)tran.gameObject.GetComponent<Renderer>() != null)
										{
											Material[] materials2 = tran.gameObject.GetComponent<Renderer>().materials;
											foreach (Material material3 in materials2)
											{
												Material material4 = material3;
												Color color8 = material3.color;
												float r3 = color8.r;
												Color color9 = material3.color;
												float g3 = color9.g;
												Color color10 = material3.color;
												material4.color = new Color(r3, g3, color10.b, val);
											}
										}
									}
								}
							}
						}
						else if (tweenAction == TweenAction.ALPHA_VERTEX)
						{
							Mesh mesh = ((Component)trans).GetComponent<MeshFilter>().mesh;
							Vector3[] vertices = mesh.vertices;
							Color32[] array = new Color32[vertices.Length];
							Color32 color11 = mesh.colors32[0];
							color11 = new Color((int)color11.r, (int)color11.g, (int)color11.b, val);
							for (int l = 0; l < vertices.Length; l++)
							{
								array[l] = color11;
							}
							mesh.colors32 = array;
						}
						else if (tweenAction == TweenAction.COLOR || tweenAction == TweenAction.CALLBACK_COLOR)
						{
							Color color12 = tweenColor(tween, val);
							SpriteRenderer component2 = trans.gameObject.GetComponent<SpriteRenderer>();
							if ((UnityEngine.Object)component2 != null)
							{
								component2.color = color12;
							}
							else if (tweenAction == TweenAction.COLOR)
							{
								if ((UnityEngine.Object)trans.gameObject.GetComponent<Renderer>() != null)
								{
									Material[] materials3 = trans.gameObject.GetComponent<Renderer>().materials;
									foreach (Material material5 in materials3)
									{
										material5.color = color12;
									}
								}
								if (trans.childCount > 0)
								{
									foreach (Transform tran2 in trans)
									{
										if ((UnityEngine.Object)tran2.gameObject.GetComponent<Renderer>() != null)
										{
											Material[] materials4 = tran2.gameObject.GetComponent<Renderer>().materials;
											foreach (Material material6 in materials4)
											{
												material6.color = color12;
											}
										}
									}
								}
							}
							if (dt != 0f && tween.onUpdateColor != null)
							{
								tween.onUpdateColor(color12);
							}
						}
						else if (tweenAction == TweenAction.CANVAS_ALPHA)
						{
							Color color13 = tween.uiImage.color;
							color13.a = val;
							tween.uiImage.color = color13;
						}
						else if (tweenAction == TweenAction.CANVAS_COLOR)
						{
							Color color14 = tweenColor(tween, val);
                            tween.uiImage.color = color14;
							if (dt != 0f && tween.onUpdateColor != null)
							{
								tween.onUpdateColor(color14);
							}
						}
						else if (tweenAction == TweenAction.TEXT_ALPHA)
						{
							textAlphaRecursive(trans, val);
						}
						else if (tweenAction == TweenAction.TEXT_COLOR)
						{
							Color color15 = tweenColor(tween, val);
							tween.uiText.color = color15;
							if (dt != 0f && tween.onUpdateColor != null)
							{
								tween.onUpdateColor(color15);
							}
							if (trans.childCount > 0)
							{
								foreach (Transform tran3 in trans)
								{
									Text component3 = tran3.gameObject.GetComponent<Text>();
									if ((UnityEngine.Object)component3 != null)
									{
										component3.color = color15;
									}
								}
							}
						}
						else if (tweenAction == TweenAction.CANVAS_ROTATEAROUND)
						{
							RectTransform rectTransform = tween.rectTransform;
							Vector3 localPosition9 = rectTransform.localPosition;
							((Transform)rectTransform).RotateAround(rectTransform.TransformPoint(tween.point), tween.axis, 0f - val);
							Vector3 b3 = localPosition9 - rectTransform.localPosition;
							rectTransform.localPosition = localPosition9 - b3;
							rectTransform.rotation = tween.origRotation;
							((Transform)rectTransform).RotateAround(rectTransform.TransformPoint(tween.point), tween.axis, val);
						}
						else if (tweenAction == TweenAction.CANVAS_ROTATEAROUND_LOCAL)
						{
							RectTransform rectTransform2 = tween.rectTransform;
							Vector3 localPosition10 = rectTransform2.localPosition;
							((Transform)rectTransform2).RotateAround(rectTransform2.TransformPoint(tween.point), rectTransform2.TransformDirection(tween.axis), 0f - val);
							Vector3 b4 = localPosition10 - rectTransform2.localPosition;
							rectTransform2.localPosition = localPosition10 - b4;
							rectTransform2.rotation = tween.origRotation;
							((Transform)rectTransform2).RotateAround(rectTransform2.TransformPoint(tween.point), rectTransform2.TransformDirection(tween.axis), val);
						}
						else if (tweenAction == TweenAction.CANVAS_PLAYSPRITE)
						{
							int num = (int)Mathf.Round(val);
							tween.uiImage.sprite = tween.sprites[num];
						}
						else if (tweenAction == TweenAction.CANVAS_MOVE_X)
						{
							Vector3 anchoredPosition3D = tween.rectTransform.anchoredPosition3D;
							tween.rectTransform.anchoredPosition3D = new Vector3(val, anchoredPosition3D.y, anchoredPosition3D.z);
						}
						else if (tweenAction == TweenAction.CANVAS_MOVE_Y)
						{
							Vector3 anchoredPosition3D2 = tween.rectTransform.anchoredPosition3D;
							tween.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D2.x, val, anchoredPosition3D2.z);
						}
						else if (tweenAction == TweenAction.CANVAS_MOVE_Z)
						{
							Vector3 anchoredPosition3D3 = tween.rectTransform.anchoredPosition3D;
							tween.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D3.x, anchoredPosition3D3.y, val);
						}
					}
					else if (tweenAction >= TweenAction.MOVE)
					{
						if (tween.animationCurve != null)
						{
							newVect = tweenOnCurveVector(tween, ratioPassed);
						}
						else if (tween.tweenType == LeanTweenType.linear)
						{
							newVect = new Vector3(tween.from.x + tween.diff.x * ratioPassed, tween.from.y + tween.diff.y * ratioPassed, tween.from.z + tween.diff.z * ratioPassed);
						}
						else if (tween.tweenType >= LeanTweenType.linear)
						{
							switch (tween.tweenType)
							{
							case LeanTweenType.easeOutQuad:
								newVect = new Vector3(easeOutQuadOpt(tween.from.x, tween.diff.x, ratioPassed), easeOutQuadOpt(tween.from.y, tween.diff.y, ratioPassed), easeOutQuadOpt(tween.from.z, tween.diff.z, ratioPassed));
								break;
							case LeanTweenType.easeInQuad:
								newVect = new Vector3(easeInQuadOpt(tween.from.x, tween.diff.x, ratioPassed), easeInQuadOpt(tween.from.y, tween.diff.y, ratioPassed), easeInQuadOpt(tween.from.z, tween.diff.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutQuad:
								newVect = new Vector3(easeInOutQuadOpt(tween.from.x, tween.diff.x, ratioPassed), easeInOutQuadOpt(tween.from.y, tween.diff.y, ratioPassed), easeInOutQuadOpt(tween.from.z, tween.diff.z, ratioPassed));
								break;
							case LeanTweenType.easeInCubic:
								newVect = new Vector3(easeInCubic(tween.from.x, tween.to.x, ratioPassed), easeInCubic(tween.from.y, tween.to.y, ratioPassed), easeInCubic(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutCubic:
								newVect = new Vector3(easeOutCubic(tween.from.x, tween.to.x, ratioPassed), easeOutCubic(tween.from.y, tween.to.y, ratioPassed), easeOutCubic(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutCubic:
								newVect = new Vector3(easeInOutCubic(tween.from.x, tween.to.x, ratioPassed), easeInOutCubic(tween.from.y, tween.to.y, ratioPassed), easeInOutCubic(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInQuart:
								newVect = new Vector3(easeInQuart(tween.from.x, tween.to.x, ratioPassed), easeInQuart(tween.from.y, tween.to.y, ratioPassed), easeInQuart(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutQuart:
								newVect = new Vector3(easeOutQuart(tween.from.x, tween.to.x, ratioPassed), easeOutQuart(tween.from.y, tween.to.y, ratioPassed), easeOutQuart(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutQuart:
								newVect = new Vector3(easeInOutQuart(tween.from.x, tween.to.x, ratioPassed), easeInOutQuart(tween.from.y, tween.to.y, ratioPassed), easeInOutQuart(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInQuint:
								newVect = new Vector3(easeInQuint(tween.from.x, tween.to.x, ratioPassed), easeInQuint(tween.from.y, tween.to.y, ratioPassed), easeInQuint(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutQuint:
								newVect = new Vector3(easeOutQuint(tween.from.x, tween.to.x, ratioPassed), easeOutQuint(tween.from.y, tween.to.y, ratioPassed), easeOutQuint(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutQuint:
								newVect = new Vector3(easeInOutQuint(tween.from.x, tween.to.x, ratioPassed), easeInOutQuint(tween.from.y, tween.to.y, ratioPassed), easeInOutQuint(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInSine:
								newVect = new Vector3(easeInSine(tween.from.x, tween.to.x, ratioPassed), easeInSine(tween.from.y, tween.to.y, ratioPassed), easeInSine(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutSine:
								newVect = new Vector3(easeOutSine(tween.from.x, tween.to.x, ratioPassed), easeOutSine(tween.from.y, tween.to.y, ratioPassed), easeOutSine(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutSine:
								newVect = new Vector3(easeInOutSine(tween.from.x, tween.to.x, ratioPassed), easeInOutSine(tween.from.y, tween.to.y, ratioPassed), easeInOutSine(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInExpo:
								newVect = new Vector3(easeInExpo(tween.from.x, tween.to.x, ratioPassed), easeInExpo(tween.from.y, tween.to.y, ratioPassed), easeInExpo(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutExpo:
								newVect = new Vector3(easeOutExpo(tween.from.x, tween.to.x, ratioPassed), easeOutExpo(tween.from.y, tween.to.y, ratioPassed), easeOutExpo(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutExpo:
								newVect = new Vector3(easeInOutExpo(tween.from.x, tween.to.x, ratioPassed), easeInOutExpo(tween.from.y, tween.to.y, ratioPassed), easeInOutExpo(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInCirc:
								newVect = new Vector3(easeInCirc(tween.from.x, tween.to.x, ratioPassed), easeInCirc(tween.from.y, tween.to.y, ratioPassed), easeInCirc(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutCirc:
								newVect = new Vector3(easeOutCirc(tween.from.x, tween.to.x, ratioPassed), easeOutCirc(tween.from.y, tween.to.y, ratioPassed), easeOutCirc(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutCirc:
								newVect = new Vector3(easeInOutCirc(tween.from.x, tween.to.x, ratioPassed), easeInOutCirc(tween.from.y, tween.to.y, ratioPassed), easeInOutCirc(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInBounce:
								newVect = new Vector3(easeInBounce(tween.from.x, tween.to.x, ratioPassed), easeInBounce(tween.from.y, tween.to.y, ratioPassed), easeInBounce(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutBounce:
								newVect = new Vector3(easeOutBounce(tween.from.x, tween.to.x, ratioPassed), easeOutBounce(tween.from.y, tween.to.y, ratioPassed), easeOutBounce(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInOutBounce:
								newVect = new Vector3(easeInOutBounce(tween.from.x, tween.to.x, ratioPassed), easeInOutBounce(tween.from.y, tween.to.y, ratioPassed), easeInOutBounce(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeInBack:
								newVect = new Vector3(easeInBack(tween.from.x, tween.to.x, ratioPassed), easeInBack(tween.from.y, tween.to.y, ratioPassed), easeInBack(tween.from.z, tween.to.z, ratioPassed));
								break;
							case LeanTweenType.easeOutBack:
								newVect = new Vector3(easeOutBack(tween.from.x, tween.to.x, ratioPassed, tween.overshoot), easeOutBack(tween.from.y, tween.to.y, ratioPassed, tween.overshoot), easeOutBack(tween.from.z, tween.to.z, ratioPassed, tween.overshoot));
								break;
							case LeanTweenType.easeInOutBack:
								newVect = new Vector3(easeInOutBack(tween.from.x, tween.to.x, ratioPassed, tween.overshoot), easeInOutBack(tween.from.y, tween.to.y, ratioPassed, tween.overshoot), easeInOutBack(tween.from.z, tween.to.z, ratioPassed, tween.overshoot));
								break;
							case LeanTweenType.easeInElastic:
								newVect = new Vector3(easeInElastic(tween.from.x, tween.to.x, ratioPassed, tween.overshoot, tween.period), easeInElastic(tween.from.y, tween.to.y, ratioPassed, tween.overshoot, tween.period), easeInElastic(tween.from.z, tween.to.z, ratioPassed, tween.overshoot, tween.period));
								break;
							case LeanTweenType.easeOutElastic:
								newVect = new Vector3(easeOutElastic(tween.from.x, tween.to.x, ratioPassed, tween.overshoot, tween.period), easeOutElastic(tween.from.y, tween.to.y, ratioPassed, tween.overshoot, tween.period), easeOutElastic(tween.from.z, tween.to.z, ratioPassed, tween.overshoot, tween.period));
								break;
							case LeanTweenType.easeInOutElastic:
								newVect = new Vector3(easeInOutElastic(tween.from.x, tween.to.x, ratioPassed, tween.overshoot, tween.period), easeInOutElastic(tween.from.y, tween.to.y, ratioPassed, tween.overshoot, tween.period), easeInOutElastic(tween.from.z, tween.to.z, ratioPassed, tween.overshoot, tween.period));
								break;
							case LeanTweenType.easeShake:
							case LeanTweenType.punch:
								if (tween.tweenType == LeanTweenType.punch)
								{
									tween.animationCurve = punch;
								}
								else if (tween.tweenType == LeanTweenType.easeShake)
								{
									tween.animationCurve = shake;
								}
								tween.to = tween.from + tween.to;
								tween.diff = tween.to - tween.from;
								if (tweenAction == TweenAction.ROTATE || tweenAction == TweenAction.ROTATE_LOCAL)
								{
									tween.to = new Vector3(closestRot(tween.from.x, tween.to.x), closestRot(tween.from.y, tween.to.y), closestRot(tween.from.z, tween.to.z));
								}
								newVect = tweenOnCurveVector(tween, ratioPassed);
								break;
							case LeanTweenType.easeSpring:
								newVect = new Vector3(spring(tween.from.x, tween.to.x, ratioPassed), spring(tween.from.y, tween.to.y, ratioPassed), spring(tween.from.z, tween.to.z, ratioPassed));
								break;
							}
						}
						else
						{
							newVect = new Vector3(tween.from.x + tween.diff.x * ratioPassed, tween.from.y + tween.diff.y * ratioPassed, tween.from.z + tween.diff.z * ratioPassed);
						}
						if (tweenAction == TweenAction.MOVE)
						{
							trans.position = newVect;
						}
						else if (tweenAction == TweenAction.MOVE_LOCAL)
						{
							trans.localPosition = newVect;
						}
						else if (tweenAction == TweenAction.ROTATE)
						{
							trans.eulerAngles = newVect;
						}
						else if (tweenAction == TweenAction.ROTATE_LOCAL)
						{
							trans.localEulerAngles = newVect;
						}
						else if (tweenAction == TweenAction.SCALE)
						{
							trans.localScale = newVect;
						}
						else if (tweenAction == TweenAction.GUI_MOVE)
						{
							tween.ltRect.rect = new Rect(newVect.x, newVect.y, tween.ltRect.rect.width, tween.ltRect.rect.height);
						}
						else if (tweenAction == TweenAction.GUI_MOVE_MARGIN)
						{
							tween.ltRect.margin = new Vector2(newVect.x, newVect.y);
						}
						else if (tweenAction == TweenAction.GUI_SCALE)
						{
							tween.ltRect.rect = new Rect(tween.ltRect.rect.x, tween.ltRect.rect.y, newVect.x, newVect.y);
						}
						else if (tweenAction == TweenAction.GUI_ALPHA)
						{
							tween.ltRect.alpha = newVect.x;
						}
						else if (tweenAction == TweenAction.GUI_ROTATE)
						{
							tween.ltRect.rotation = newVect.x;
						}
						else if (tweenAction == TweenAction.CANVAS_MOVE)
						{
							tween.rectTransform.anchoredPosition3D = newVect;
						}
						else if (tweenAction == TweenAction.CANVAS_SCALE)
						{
							tween.rectTransform.localScale = newVect;
						}
					}
					if (dt != 0f && tween.hasUpdateCallback)
					{
						if (tween.onUpdateFloat != null)
						{
							tween.onUpdateFloat(val);
						}
						if (tween.onUpdateFloatRatio != null)
						{
							tween.onUpdateFloatRatio(val, ratioPassed);
						}
						else if (tween.onUpdateFloatObject != null)
						{
							tween.onUpdateFloatObject(val, tween.onUpdateParam);
						}
						else if (tween.onUpdateVector3Object != null)
						{
							tween.onUpdateVector3Object(newVect, tween.onUpdateParam);
						}
						else if (tween.onUpdateVector3 != null)
						{
							tween.onUpdateVector3(newVect);
						}
						else if (tween.onUpdateVector2 != null)
						{
							tween.onUpdateVector2(new Vector2(newVect.x, newVect.y));
						}
					}
				}
				if (isTweenFinished)
				{
					if (tween.loopType == LeanTweenType.once || tween.loopCount == 1)
					{
						tweensFinished[finishedCnt] = i;
						finishedCnt++;
						if (tweenAction == TweenAction.GUI_ROTATE)
						{
							tween.ltRect.rotateFinished = true;
						}
						if (tweenAction == TweenAction.DELAYED_SOUND)
						{
							AudioSource.PlayClipAtPoint((AudioClip) tween.onCompleteParam, tween.to, tween.from.x);
						}
						continue;
					}
					if ((tween.loopCount < 0 && tween.type == TweenAction.CALLBACK) || tween.onCompleteOnRepeat)
					{
						if (tweenAction == TweenAction.DELAYED_SOUND)
						{
							AudioSource.PlayClipAtPoint((AudioClip) tween.onCompleteParam, tween.to, tween.from.x);
						}
						if (tween.onComplete != null)
						{
							tween.onComplete();
						}
						else if (tween.onCompleteObject != null)
						{
							tween.onCompleteObject(tween.onCompleteParam);
						}
					}
					if (tween.loopCount >= 1)
					{
						tween.loopCount--;
					}
					if (tween.loopType == LeanTweenType.pingPong)
					{
						tween.direction = 0f - tween.direction;
					}
					else
					{
						tween.passed = Mathf.Epsilon;
					}
				}
				else if (tween.delay <= 0f)
				{
					tween.passed += dt * tween.direction;
				}
				else
				{
					tween.delay -= dt;
					if (tween.delay < 0f)
					{
						tween.passed = 0f;
						tween.delay = 0f;
					}
				}
			}
			tweenMaxSearch = maxTweenReached;
			frameRendered = Time.frameCount;
			for (int num2 = 0; num2 < finishedCnt; num2++)
			{
				LeanTween.j = tweensFinished[num2];
				tween = tweens[LeanTween.j];
				if (tween.onComplete != null)
				{
					Action onComplete = tween.onComplete;
					removeTween(LeanTween.j);
					onComplete();
				}
				else if (tween.onCompleteObject != null)
				{
					Action<object> onCompleteObject = tween.onCompleteObject;
					object onCompleteParam = tween.onCompleteParam;
					removeTween(LeanTween.j);
					onCompleteObject(onCompleteParam);
				}
				else
				{
					removeTween(LeanTween.j);
				}
			}
		}

		private static void textAlphaRecursive(Transform trans, float val)
		{
			Text component = trans.gameObject.GetComponent<Text>();
			if ((UnityEngine.Object)component != null)
			{
				Color color = component.color;
				color.a = val;
				component.color = color;
			}
			if (trans.childCount > 0)
			{
				foreach (Transform tran in trans)
				{
					textAlphaRecursive(tran, val);
				}
			}
		}

		private static Color tweenColor(LTDescr tween, float val)
		{
			Vector3 vector = tween.point - tween.axis;
			float num = tween.to.y - tween.from.y;
			return new Color(tween.axis.x + vector.x * val, tween.axis.y + vector.y * val, tween.axis.z + vector.z * val, tween.from.y + num * val);
		}

		public static void removeTween(int i, int uniqueId)
		{
			if (tweens[i].uniqueId == uniqueId)
			{
				removeTween(i);
			}
		}

		public static void removeTween(int i)
		{
			if (!tweens[i].toggle)
			{
				return;
			}
			tweens[i].toggle = false;
			if (tweens[i].destroyOnComplete)
			{
				if (tweens[i].ltRect != null)
				{
					LTGUI.destroy(tweens[i].ltRect.id);
				}
				else if (tweens[i].trans.gameObject != _tweenEmpty)
				{
					UnityEngine.Object.Destroy(tweens[i].trans.gameObject);
				}
			}
			tweens[i].cleanup();
			startSearch = i;
			if (i + 1 >= tweenMaxSearch)
			{
				startSearch = 0;
			}
		}

		public static Vector3[] add(Vector3[] a, Vector3 b)
		{
			Vector3[] array = new Vector3[a.Length];
			for (i = 0; i < a.Length; i++)
			{
				array[i] = a[i] + b;
			}
			return array;
		}

		public static float closestRot(float from, float to)
		{
			float num = 0f - (360f - to);
			float num2 = 360f + to;
			float num3 = Mathf.Abs(to - from);
			float num4 = Mathf.Abs(num - from);
			float num5 = Mathf.Abs(num2 - from);
			if (num3 < num4 && num3 < num5)
			{
				return to;
			}
			if (num4 < num5)
			{
				return num;
			}
			return num2;
		}

		public static void cancelAll()
		{
			cancelAll(callComplete: false);
		}

		public static void cancelAll(bool callComplete)
		{
			init();
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				if (tweens[i].trans != null)
				{
					if (callComplete && tweens[i].onComplete != null)
					{
						tweens[i].onComplete();
					}
					removeTween(i);
				}
			}
		}

		public static void cancel(GameObject gameObject)
		{
			cancel(gameObject, callOnComplete: false);
		}

		public static void cancel(GameObject gameObject, bool callOnComplete)
		{
			init();
			Transform transform = gameObject.transform;
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				if (tweens[i].toggle && tweens[i].trans == transform)
				{
					if (callOnComplete && tweens[i].onComplete != null)
					{
						tweens[i].onComplete();
					}
					removeTween(i);
				}
			}
		}

		public static void cancel(GameObject gameObject, int uniqueId)
		{
			if (uniqueId >= 0)
			{
				init();
				int num = uniqueId & 0xFFFF;
				int num2 = uniqueId >> 16;
				if (tweens[num].trans == null || (tweens[num].trans.gameObject == gameObject && tweens[num].counter == num2))
				{
					removeTween(num);
				}
			}
		}

		public static void cancel(LTRect ltRect, int uniqueId)
		{
			if (uniqueId >= 0)
			{
				init();
				int num = uniqueId & 0xFFFF;
				int num2 = uniqueId >> 16;
				if (tweens[num].ltRect == ltRect && tweens[num].counter == num2)
				{
					removeTween(num);
				}
			}
		}

		public static void cancel(int uniqueId)
		{
			cancel(uniqueId, callOnComplete: false);
		}

		public static void cancel(int uniqueId, bool callOnComplete)
		{
			if (uniqueId < 0)
			{
				return;
			}
			init();
			int num = uniqueId & 0xFFFF;
			int num2 = uniqueId >> 16;
			if (tweens[num].counter == num2)
			{
				if (callOnComplete && tweens[num].onComplete != null)
				{
					tweens[num].onComplete();
				}
				removeTween(num);
			}
		}

		public static LTDescr descr(int uniqueId)
		{
			int num = uniqueId & 0xFFFF;
			int num2 = uniqueId >> 16;
			if (tweens[num] != null && tweens[num].uniqueId == uniqueId && tweens[num].counter == num2)
			{
				return tweens[num];
			}
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				if (tweens[i].uniqueId == uniqueId && tweens[i].counter == num2)
				{
					return tweens[i];
				}
			}
			return null;
		}

		public static LTDescr description(int uniqueId)
		{
			return descr(uniqueId);
		}

		public static LTDescr[] descriptions(GameObject gameObject = null)
		{
			if (gameObject == null)
			{
				return null;
			}
			List<LTDescr> list = new List<LTDescr>();
			Transform transform = gameObject.transform;
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				if (tweens[i].toggle && tweens[i].trans == transform)
				{
					list.Add(tweens[i]);
				}
			}
			return list.ToArray();
		}

		[Obsolete("Use 'pause( id )' instead")]
		public static void pause(GameObject gameObject, int uniqueId)
		{
			pause(uniqueId);
		}

		public static void pause(int uniqueId)
		{
			int num = uniqueId & 0xFFFF;
			int num2 = uniqueId >> 16;
			if (tweens[num].counter == num2)
			{
				tweens[num].pause();
			}
		}

		public static void pause(GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				if (tweens[i].trans == transform)
				{
					tweens[i].pause();
				}
			}
		}

		public static void pauseAll()
		{
			init();
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				tweens[i].pause();
			}
		}

		public static void resumeAll()
		{
			init();
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				tweens[i].resume();
			}
		}

		[Obsolete("Use 'resume( id )' instead")]
		public static void resume(GameObject gameObject, int uniqueId)
		{
			resume(uniqueId);
		}

		public static void resume(int uniqueId)
		{
			int num = uniqueId & 0xFFFF;
			int num2 = uniqueId >> 16;
			if (tweens[num].counter == num2)
			{
				tweens[num].resume();
			}
		}

		public static void resume(GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				if (tweens[i].trans == transform)
				{
					tweens[i].resume();
				}
			}
		}

		public static bool isTweening(GameObject gameObject = null)
		{
			if (gameObject == null)
			{
				for (int i = 0; i <= tweenMaxSearch; i++)
				{
					if (tweens[i].toggle)
					{
						return true;
					}
				}
				return false;
			}
			Transform transform = gameObject.transform;
			for (int j = 0; j <= tweenMaxSearch; j++)
			{
				if (tweens[j].toggle && tweens[j].trans == transform)
				{
					return true;
				}
			}
			return false;
		}

		public static bool isTweening(int uniqueId)
		{
			int num = uniqueId & 0xFFFF;
			int num2 = uniqueId >> 16;
			if (num < 0 || num >= maxTweens)
			{
				return false;
			}
			if (tweens[num].counter == num2 && tweens[num].toggle)
			{
				return true;
			}
			return false;
		}

		public static bool isTweening(LTRect ltRect)
		{
			for (int i = 0; i <= tweenMaxSearch; i++)
			{
				if (tweens[i].toggle && tweens[i].ltRect == ltRect)
				{
					return true;
				}
			}
			return false;
		}

		public static void drawBezierPath(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float arrowSize = 0f, Transform arrowTransform = null)
		{
			Vector3 vector = a;
			Vector3 a2 = -a + 3f * (b - c) + d;
			Vector3 b2 = 3f * (a + c) - 6f * b;
			Vector3 b3 = 3f * (b - a);
			if (arrowSize > 0f)
			{
				Vector3 position = arrowTransform.position;
				Quaternion rotation = arrowTransform.rotation;
				float num = 0f;
				for (float num2 = 1f; num2 <= 120f; num2 += 1f)
				{
					float num3 = num2 / 120f;
					Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
					Gizmos.DrawLine(vector, vector2);
					num += (vector2 - vector).magnitude;
					if (num > 1f)
					{
						num -= 1f;
						arrowTransform.position = vector2;
						arrowTransform.LookAt(vector, Vector3.forward);
						Vector3 a3 = arrowTransform.TransformDirection(Vector3.right);
						Vector3 normalized = (vector - vector2).normalized;
						Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
						a3 = arrowTransform.TransformDirection(-Vector3.right);
						Gizmos.DrawLine(vector2, vector2 + (a3 + normalized) * arrowSize);
					}
					vector = vector2;
				}
				arrowTransform.position = position;
				arrowTransform.rotation = rotation;
			}
			else
			{
				for (float num4 = 1f; num4 <= 30f; num4 += 1f)
				{
					float num3 = num4 / 30f;
					Vector3 vector2 = ((a2 * num3 + b2) * num3 + b3) * num3 + a;
					Gizmos.DrawLine(vector, vector2);
					vector = vector2;
				}
			}
		}

		public static object logError(string error)
		{
			if (throwErrors)
			{
				Debug.LogError(error);
			}
			else
			{
				Debug.Log(error);
			}
			return null;
		}

		public static LTDescr options(LTDescr seed)
		{
			Debug.LogError("error this function is no longer used");
			return null;
		}

		public static LTDescr options()
		{
			init();
			bool flag = false;
			j = 0;
			i = startSearch;
			while (j < maxTweens)
			{
				if (i >= maxTweens - 1)
				{
					i = 0;
				}
				if (!tweens[i].toggle)
				{
					if (i + 1 > tweenMaxSearch)
					{
						tweenMaxSearch = i + 1;
					}
					startSearch = i + 1;
					flag = true;
					break;
				}
				j++;
				if (j >= maxTweens)
				{
					return logError("LeanTween - You have run out of available spaces for tweening. To avoid this error increase the number of spaces to available for tweening when you initialize the LeanTween class ex: LeanTween.init( " + maxTweens * 2 + " );") as LTDescr;
				}
				i++;
			}
			if (!flag)
			{
				logError("no available tween found!");
			}
			tweens[i].reset();
			tweens[i].setId((uint)i);
			return tweens[i];
		}

		private static LTDescr pushNewTween(GameObject gameObject, Vector3 to, float time, TweenAction tweenAction, LTDescr tween)
		{
			init(maxTweens);
			if (gameObject == null || tween == null)
			{
				return null;
			}
			tween.trans = gameObject.transform;
			tween.to = to;
			tween.time = time;
			tween.type = tweenAction;
			return tween;
		}

		public static LTDescr play(RectTransform rectTransform, Sprite[] sprites)
		{
			float num = 0.25f;
			float time = num * (float)sprites.Length;
			return pushNewTween(rectTransform.gameObject, new Vector3((float)sprites.Length - 1f, 0f, 0f), time, TweenAction.CANVAS_PLAYSPRITE, options().setSprites(sprites).setRepeat(-1));
		}

		public static LTDescr alpha(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ALPHA, options());
		}

		public static LTDescr alpha(LTRect ltRect, float to, float time)
		{
			ltRect.alphaEnabled = true;
			return pushNewTween(tweenEmpty, new Vector3(to, 0f, 0f), time, TweenAction.GUI_ALPHA, options().setRect(ltRect));
		}

		public static LTDescr textAlpha(RectTransform rectTransform, float to, float time)
		{
			return pushNewTween(rectTransform.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.TEXT_ALPHA, options());
		}

		public static LTDescr alphaVertex(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ALPHA_VERTEX, options());
		}

		public static LTDescr color(GameObject gameObject, Color to, float time)
		{
			return pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.COLOR, options().setPoint(new Vector3(to.r, to.g, to.b)));
		}

		public static LTDescr textColor(RectTransform rectTransform, Color to, float time)
		{
			return pushNewTween(rectTransform.gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.TEXT_COLOR, options().setPoint(new Vector3(to.r, to.g, to.b)));
		}

		public static LTDescr delayedCall(float delayTime, Action callback)
		{
			return pushNewTween(tweenEmpty, Vector3.zero, delayTime, TweenAction.CALLBACK, options().setOnComplete(callback));
		}

		public static LTDescr delayedCall(float delayTime, Action<object> callback)
		{
			return pushNewTween(tweenEmpty, Vector3.zero, delayTime, TweenAction.CALLBACK, options().setOnComplete(callback));
		}

		public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action callback)
		{
			return pushNewTween(gameObject, Vector3.zero, delayTime, TweenAction.CALLBACK, options().setOnComplete(callback));
		}

		public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action<object> callback)
		{
			return pushNewTween(gameObject, Vector3.zero, delayTime, TweenAction.CALLBACK, options().setOnComplete(callback));
		}

		public static LTDescr destroyAfter(LTRect rect, float delayTime)
		{
			return pushNewTween(tweenEmpty, Vector3.zero, delayTime, TweenAction.CALLBACK, options().setRect(rect).setDestroyOnComplete(doesDestroy: true));
		}

		public static LTDescr move(GameObject gameObject, Vector3 to, float time)
		{
			return pushNewTween(gameObject, to, time, TweenAction.MOVE, options());
		}

		public static LTDescr move(GameObject gameObject, Vector2 to, float time)
		{
			float x = to.x;
			float y = to.y;
			Vector3 position = gameObject.transform.position;
			return pushNewTween(gameObject, new Vector3(x, y, position.z), time, TweenAction.MOVE, options());
		}

		public static LTDescr move(GameObject gameObject, Vector3[] to, float time)
		{
			d = options();
			if (d.path == null)
			{
				d.path = new LTBezierPath(to);
			}
			else
			{
				d.path.setPoints(to);
			}
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED, d);
		}

		public static LTDescr move(GameObject gameObject, LTBezierPath to, float time)
		{
			d = options();
			d.path = to;
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED, d);
		}

		public static LTDescr move(GameObject gameObject, LTSpline to, float time)
		{
			d = options();
			d.spline = to;
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE, d);
		}

		public static LTDescr moveSpline(GameObject gameObject, Vector3[] to, float time)
		{
			d = options();
			d.spline = new LTSpline(to);
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE, d);
		}

		public static LTDescr moveSplineLocal(GameObject gameObject, Vector3[] to, float time)
		{
			d = options();
			d.spline = new LTSpline(to);
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE_LOCAL, d);
		}

		public static LTDescr move(LTRect ltRect, Vector2 to, float time)
		{
			return pushNewTween(tweenEmpty, to, time, TweenAction.GUI_MOVE, options().setRect(ltRect));
		}

		public static LTDescr moveMargin(LTRect ltRect, Vector2 to, float time)
		{
			return pushNewTween(tweenEmpty, to, time, TweenAction.GUI_MOVE_MARGIN, options().setRect(ltRect));
		}

		public static LTDescr moveX(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_X, options());
		}

		public static LTDescr moveY(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_Y, options());
		}

		public static LTDescr moveZ(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_Z, options());
		}

		public static LTDescr moveLocal(GameObject gameObject, Vector3 to, float time)
		{
			return pushNewTween(gameObject, to, time, TweenAction.MOVE_LOCAL, options());
		}

		public static LTDescr moveLocal(GameObject gameObject, Vector3[] to, float time)
		{
			d = options();
			if (d.path == null)
			{
				d.path = new LTBezierPath(to);
			}
			else
			{
				d.path.setPoints(to);
			}
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED_LOCAL, d);
		}

		public static LTDescr moveLocalX(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_X, options());
		}

		public static LTDescr moveLocalY(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_Y, options());
		}

		public static LTDescr moveLocalZ(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_Z, options());
		}

		public static LTDescr moveLocal(GameObject gameObject, LTBezierPath to, float time)
		{
			d = options();
			d.path = to;
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED_LOCAL, d);
		}

		public static LTDescr moveLocal(GameObject gameObject, LTSpline to, float time)
		{
			d = options();
			d.spline = to;
			return pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE_LOCAL, d);
		}

		public static LTDescr rotate(GameObject gameObject, Vector3 to, float time)
		{
			return pushNewTween(gameObject, to, time, TweenAction.ROTATE, options());
		}

		public static LTDescr rotate(LTRect ltRect, float to, float time)
		{
			return pushNewTween(tweenEmpty, new Vector3(to, 0f, 0f), time, TweenAction.GUI_ROTATE, options().setRect(ltRect));
		}

		public static LTDescr rotateLocal(GameObject gameObject, Vector3 to, float time)
		{
			return pushNewTween(gameObject, to, time, TweenAction.ROTATE_LOCAL, options());
		}

		public static LTDescr rotateX(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_X, options());
		}

		public static LTDescr rotateY(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_Y, options());
		}

		public static LTDescr rotateZ(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_Z, options());
		}

		public static LTDescr rotateAround(GameObject gameObject, Vector3 axis, float add, float time)
		{
			return pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, TweenAction.ROTATE_AROUND, options().setAxis(axis));
		}

		public static LTDescr rotateAroundLocal(GameObject gameObject, Vector3 axis, float add, float time)
		{
			return pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, TweenAction.ROTATE_AROUND_LOCAL, options().setAxis(axis));
		}

		public static LTDescr scale(GameObject gameObject, Vector3 to, float time)
		{
			return pushNewTween(gameObject, to, time, TweenAction.SCALE, options());
		}

		public static LTDescr scale(LTRect ltRect, Vector2 to, float time)
		{
			return pushNewTween(tweenEmpty, to, time, TweenAction.GUI_SCALE, options().setRect(ltRect));
		}

		public static LTDescr scaleX(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_X, options());
		}

		public static LTDescr scaleY(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_Y, options());
		}

		public static LTDescr scaleZ(GameObject gameObject, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_Z, options());
		}

		public static LTDescr value(GameObject gameObject, float from, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, options().setFrom(new Vector3(from, 0f, 0f)));
		}

		public static LTDescr value(GameObject gameObject, Vector2 from, Vector2 to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, TweenAction.VALUE3, options().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)));
		}

		public static LTDescr value(GameObject gameObject, Vector3 from, Vector3 to, float time)
		{
			return pushNewTween(gameObject, to, time, TweenAction.VALUE3, options().setFrom(from));
		}

		public static LTDescr value(GameObject gameObject, Color from, Color to, float time)
		{
			return pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CALLBACK_COLOR, options().setPoint(new Vector3(to.r, to.g, to.b)).setFromColor(from).setHasInitialized(has: false));
		}

		public static LTDescr value(GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, options().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
		{
			return pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CALLBACK_COLOR, options().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f))
				.setHasInitialized(has: false)
				.setOnUpdateColor(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, TweenAction.VALUE3, options().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)).setOnUpdateVector2(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
		{
			return pushNewTween(gameObject, to, time, TweenAction.VALUE3, options().setTo(to).setFrom(from).setOnUpdateVector3(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
		{
			return pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, options().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdateObject(callOnUpdate));
		}

		public static LTDescr delayedSound(AudioClip audio, Vector3 pos, float volume)
		{
			return pushNewTween(tweenEmpty, pos, 0f, TweenAction.DELAYED_SOUND, options().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
		}

		public static LTDescr delayedSound(GameObject gameObject, AudioClip audio, Vector3 pos, float volume)
		{
			return pushNewTween(gameObject, pos, 0f, TweenAction.DELAYED_SOUND, options().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
		}

		public static LTDescr move(RectTransform rectTrans, Vector3 to, float time)
		{
			return pushNewTween(rectTrans.gameObject, to, time, TweenAction.CANVAS_MOVE, options().setRect(rectTrans));
		}

		public static LTDescr moveX(RectTransform rectTrans, float to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_X, options().setRect(rectTrans));
		}

		public static LTDescr moveY(RectTransform rectTrans, float to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_Y, options().setRect(rectTrans));
		}

		public static LTDescr moveZ(RectTransform rectTrans, float to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_Z, options().setRect(rectTrans));
		}

		public static LTDescr rotate(RectTransform rectTrans, float to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND, options().setRect(rectTrans).setAxis(Vector3.forward));
		}

		public static LTDescr rotateAround(RectTransform rectTrans, Vector3 axis, float to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND, options().setRect(rectTrans).setAxis(axis));
		}

		public static LTDescr rotateAroundLocal(RectTransform rectTrans, Vector3 axis, float to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND_LOCAL, options().setRect(rectTrans).setAxis(axis));
		}

		public static LTDescr scale(RectTransform rectTrans, Vector3 to, float time)
		{
			return pushNewTween(rectTrans.gameObject, to, time, TweenAction.CANVAS_SCALE, options().setRect(rectTrans));
		}

		public static LTDescr alpha(RectTransform rectTrans, float to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ALPHA, options().setRect(rectTrans));
		}

		public static LTDescr color(RectTransform rectTrans, Color to, float time)
		{
			return pushNewTween(rectTrans.gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CANVAS_COLOR, options().setRect(rectTrans).setPoint(new Vector3(to.r, to.g, to.b)));
		}

		private static float tweenOnCurve(LTDescr tweenDescr, float ratioPassed)
		{
			return tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.animationCurve.Evaluate(ratioPassed);
		}

		private static Vector3 tweenOnCurveVector(LTDescr tweenDescr, float ratioPassed)
		{
			return new Vector3(tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.animationCurve.Evaluate(ratioPassed), tweenDescr.from.y + tweenDescr.diff.y * tweenDescr.animationCurve.Evaluate(ratioPassed), tweenDescr.from.z + tweenDescr.diff.z * tweenDescr.animationCurve.Evaluate(ratioPassed));
		}

		private static float easeOutQuadOpt(float start, float diff, float ratioPassed)
		{
			return (0f - diff) * ratioPassed * (ratioPassed - 2f) + start;
		}

		private static float easeInQuadOpt(float start, float diff, float ratioPassed)
		{
			return diff * ratioPassed * ratioPassed + start;
		}

		private static float easeInOutQuadOpt(float start, float diff, float ratioPassed)
		{
			ratioPassed /= 0.5f;
			if (ratioPassed < 1f)
			{
				return diff / 2f * ratioPassed * ratioPassed + start;
			}
			ratioPassed -= 1f;
			return (0f - diff) / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
		}

		private static float linear(float start, float end, float val)
		{
			return Mathf.Lerp(start, end, val);
		}

		private static float clerp(float start, float end, float val)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Mathf.Abs((num2 - num) / 2f);
			if (end - start < 0f - num3)
			{
				float num4 = (num2 - start + end) * val;
				return start + num4;
			}
			if (end - start > num3)
			{
				float num4 = (0f - (num2 - end + start)) * val;
				return start + num4;
			}
			return start + (end - start) * val;
		}

		private static float spring(float start, float end, float val)
		{
			val = Mathf.Clamp01(val);
			val = (Mathf.Sin(val * (float)Math.PI * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
			return start + (end - start) * val;
		}

		private static float easeInQuad(float start, float end, float val)
		{
			end -= start;
			return end * val * val + start;
		}

		private static float easeOutQuad(float start, float end, float val)
		{
			end -= start;
			return (0f - end) * val * (val - 2f) + start;
		}

		private static float easeInOutQuad(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val + start;
			}
			val -= 1f;
			return (0f - end) / 2f * (val * (val - 2f) - 1f) + start;
		}

		private static float easeInCubic(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val + start;
		}

		private static float easeOutCubic(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val + 1f) + start;
		}

		private static float easeInOutCubic(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val + 2f) + start;
		}

		private static float easeInQuart(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val + start;
		}

		private static float easeOutQuart(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return (0f - end) * (val * val * val * val - 1f) + start;
		}

		private static float easeInOutQuart(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val + start;
			}
			val -= 2f;
			return (0f - end) / 2f * (val * val * val * val - 2f) + start;
		}

		private static float easeInQuint(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val * val + start;
		}

		private static float easeOutQuint(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val * val * val + 1f) + start;
		}

		private static float easeInOutQuint(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val * val * val + 2f) + start;
		}

		private static float easeInSine(float start, float end, float val)
		{
			end -= start;
			return (0f - end) * Mathf.Cos(val / 1f * ((float)Math.PI / 2f)) + end + start;
		}

		private static float easeOutSine(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Sin(val / 1f * ((float)Math.PI / 2f)) + start;
		}

		private static float easeInOutSine(float start, float end, float val)
		{
			end -= start;
			return (0f - end) / 2f * (Mathf.Cos((float)Math.PI * val / 1f) - 1f) + start;
		}

		private static float easeInExpo(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
		}

		private static float easeOutExpo(float start, float end, float val)
		{
			end -= start;
			return end * (0f - Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
		}

		private static float easeInOutExpo(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
			}
			val -= 1f;
			return end / 2f * (0f - Mathf.Pow(2f, -10f * val) + 2f) + start;
		}

		private static float easeInCirc(float start, float end, float val)
		{
			end -= start;
			return (0f - end) * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}

		private static float easeOutCirc(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * Mathf.Sqrt(1f - val * val) + start;
		}

		private static float easeInOutCirc(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return (0f - end) / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
			}
			val -= 2f;
			return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
		}

		private static float easeInBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			return end - easeOutBounce(0f, end, num - val) + start;
		}

		private static float easeOutBounce(float start, float end, float val)
		{
			val /= 1f;
			end -= start;
			if (val < 0.363636374f)
			{
				return end * (7.5625f * val * val) + start;
			}
			if (val < 0.727272749f)
			{
				val -= 0.545454562f;
				return end * (7.5625f * val * val + 0.75f) + start;
			}
			if ((double)val < 0.90909090909090906)
			{
				val -= 0.8181818f;
				return end * (7.5625f * val * val + 0.9375f) + start;
			}
			val -= 21f / 22f;
			return end * (7.5625f * val * val + 63f / 64f) + start;
		}

		private static float easeInOutBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			if (val < num / 2f)
			{
				return easeInBounce(0f, end, val * 2f) * 0.5f + start;
			}
			return easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
		}

		private static float easeInBack(float start, float end, float val, float overshoot = 1f)
		{
			end -= start;
			val /= 1f;
			float num = 1.70158f * overshoot;
			return end * val * val * ((num + 1f) * val - num) + start;
		}

		private static float easeOutBack(float start, float end, float val, float overshoot = 1f)
		{
			float num = 1.70158f * overshoot;
			end -= start;
			val = val / 1f - 1f;
			return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
		}

		private static float easeInOutBack(float start, float end, float val, float overshoot = 1f)
		{
			float num = 1.70158f * overshoot;
			end -= start;
			val /= 0.5f;
			if (val < 1f)
			{
				num *= 1.525f * overshoot;
				return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
			}
			val -= 2f;
			num *= 1.525f * overshoot;
			return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
		}

		private static float easeInElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
		{
			end -= start;
			float num = 0f;
			if (val == 0f)
			{
				return start;
			}
			if (val == 1f)
			{
				return start + end;
			}
			float num2;
			if (num == 0f || num < Mathf.Abs(end))
			{
				num = end;
				num2 = period / 4f;
			}
			else
			{
				num2 = period / ((float)Math.PI * 2f) * Mathf.Asin(end / num);
			}
			if (overshoot > 1f && val > 0.6f)
			{
				overshoot = 1f + (1f - val) / 0.4f * (overshoot - 1f);
			}
			val -= 1f;
			return start - num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * ((float)Math.PI * 2f) / period) * overshoot;
		}

		private static float easeOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
		{
			end -= start;
			float num = 0f;
			if (val == 0f)
			{
				return start;
			}
			if (val == 1f)
			{
				return start + end;
			}
			float num2;
			if (num == 0f || num < Mathf.Abs(end))
			{
				num = end;
				num2 = period / 4f;
			}
			else
			{
				num2 = period / ((float)Math.PI * 2f) * Mathf.Asin(end / num);
			}
			if (overshoot > 1f && val < 0.4f)
			{
				overshoot = 1f + val / 0.4f * (overshoot - 1f);
			}
			return start + end + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * ((float)Math.PI * 2f) / period) * overshoot;
		}

		private static float easeInOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
		{
			end -= start;
			float num = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= 0.5f;
			if (val == 2f)
			{
				return start + end;
			}
			float num2;
			if (num == 0f || num < Mathf.Abs(end))
			{
				num = end;
				num2 = period / 4f;
			}
			else
			{
				num2 = period / ((float)Math.PI * 2f) * Mathf.Asin(end / num);
			}
			if (overshoot > 1f)
			{
				if (val < 0.2f)
				{
					overshoot = 1f + val / 0.2f * (overshoot - 1f);
				}
				else if (val > 0.8f)
				{
					overshoot = 1f + (1f - val) / 0.2f * (overshoot - 1f);
				}
			}
			if (val < 1f)
			{
				val -= 1f;
				return start - 0.5f * (num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * ((float)Math.PI * 2f) / period)) * overshoot;
			}
			val -= 1f;
			return end + start + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * ((float)Math.PI * 2f) / period) * 0.5f * overshoot;
		}

		public static void addListener(int eventId, Action<LTEvent> callback)
		{
			addListener(tweenEmpty, eventId, callback);
		}

		public static void addListener(GameObject caller, int eventId, Action<LTEvent> callback)
		{
			if (eventListeners == null)
			{
				INIT_LISTENERS_MAX = LISTENERS_MAX;
				eventListeners = new Action<LTEvent>[EVENTS_MAX * LISTENERS_MAX];
				goListeners = new GameObject[EVENTS_MAX * LISTENERS_MAX];
			}
			for (i = 0; i < INIT_LISTENERS_MAX; i++)
			{
				int num = eventId * INIT_LISTENERS_MAX + i;
				if (goListeners[num] == null || eventListeners[num] == null)
				{
					eventListeners[num] = callback;
					goListeners[num] = caller;
					if (i >= eventsMaxSearch)
					{
						eventsMaxSearch = i + 1;
					}
					return;
				}
				if (goListeners[num] == caller && object.Equals(eventListeners[num], callback))
				{
					return;
				}
			}
			Debug.LogError("You ran out of areas to add listeners, consider increasing INIT_LISTENERS_MAX, ex: LeanTween.INIT_LISTENERS_MAX = " + INIT_LISTENERS_MAX * 2);
		}

		public static bool removeListener(int eventId, Action<LTEvent> callback)
		{
			return removeListener(tweenEmpty, eventId, callback);
		}

		public static bool removeListener(GameObject caller, int eventId, Action<LTEvent> callback)
		{
			for (i = 0; i < eventsMaxSearch; i++)
			{
				int num = eventId * INIT_LISTENERS_MAX + i;
				if (goListeners[num] == caller && object.Equals(eventListeners[num], callback))
				{
					eventListeners[num] = null;
					goListeners[num] = null;
					return true;
				}
			}
			return false;
		}

		public static void dispatchEvent(int eventId)
		{
			dispatchEvent(eventId, null);
		}

		public static void dispatchEvent(int eventId, object data)
		{
			for (int i = 0; i < eventsMaxSearch; i++)
			{
				int num = eventId * INIT_LISTENERS_MAX + i;
				if (eventListeners[num] != null)
				{
					if ((bool)goListeners[num])
					{
						eventListeners[num](new LTEvent(eventId, data));
					}
					else
					{
						eventListeners[num] = null;
					}
				}
			}
		}
	}
}
