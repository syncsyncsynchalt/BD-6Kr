using System;
using UnityEngine;
using UnityEngine.UI;

namespace LT.Tweening
{
	public class LTDescr
	{
		public bool toggle;

		public bool useEstimatedTime;

		public bool useFrames;

		public bool useManualTime;

		public bool hasInitiliazed;

		public bool hasPhysics;

		public bool onCompleteOnRepeat;

		public bool onCompleteOnStart;

		public float passed;

		public float delay;

		public float time;

		public float lastVal;

		private uint _id;

		public int loopCount;

		public uint counter;

		public float direction;

		public float directionLast;

		public float overshoot;

		public float period;

		public bool destroyOnComplete;

		public Transform trans;

		public LTRect ltRect;

		public Vector3 from;

		public Vector3 to;

		public Vector3 diff;

		public Vector3 point;

		public Vector3 axis;

		public Quaternion origRotation;

		public LTBezierPath path;

		public LTSpline spline;

		public TweenAction type;

		public LeanTweenType tweenType;

		public AnimationCurve animationCurve;

		public LeanTweenType loopType;

		public bool hasUpdateCallback;

		public Action<float> onUpdateFloat;

		public Action<float, float> onUpdateFloatRatio;

		public Action<float, object> onUpdateFloatObject;

		public Action<Vector2> onUpdateVector2;

		public Action<Vector3> onUpdateVector3;

		public Action<Vector3, object> onUpdateVector3Object;

		public Action<Color> onUpdateColor;

		public Action onComplete;

		public Action<object> onCompleteObject;

		public object onCompleteParam;

		public object onUpdateParam;

		public Action onStart;

		public RectTransform rectTransform;

		public Text uiText;

		public Image uiImage;

		public Sprite[] sprites;

		private static uint global_counter;

		public int uniqueId => (int)(_id | (counter << 16));

		public int id => uniqueId;

		public override string ToString()
		{
			return ((!(trans != null)) ? "gameObject:null" : ("gameObject:" + trans.gameObject)) + " toggle:" + toggle + " passed:" + passed + " time:" + time + " delay:" + delay + " direction:" + direction + " from:" + from + " to:" + to + " type:" + type + " ease:" + tweenType + " useEstimatedTime:" + useEstimatedTime + " id:" + id + " hasInitiliazed:" + hasInitiliazed;
		}

		[Obsolete("Use 'LeanTween.cancel( id )' instead")]
		public LTDescr cancel(GameObject gameObject)
		{
			if (gameObject == trans.gameObject)
			{
				LeanTween.removeTween((int)_id, uniqueId);
			}
			return this;
		}

		public void reset()
		{
			toggle = true;
			trans = null;
			passed = (delay = (lastVal = 0f));
			hasUpdateCallback = (useEstimatedTime = (useFrames = (hasInitiliazed = (onCompleteOnRepeat = (destroyOnComplete = (onCompleteOnStart = (useManualTime = false)))))));
			animationCurve = null;
			tweenType = LeanTweenType.linear;
			loopType = LeanTweenType.once;
			loopCount = 0;
			direction = (directionLast = (overshoot = 1f));
			period = 0.3f;
			point = Vector3.zero;
			cleanup();
			global_counter++;
			if (global_counter > 32768)
			{
				global_counter = 0u;
			}
		}

		public void cleanup()
		{
			onUpdateFloat = null;
			onUpdateFloatRatio = null;
			onUpdateVector2 = null;
			onUpdateVector3 = null;
			onUpdateFloatObject = null;
			onUpdateVector3Object = null;
			onUpdateColor = null;
			onComplete = null;
			onCompleteObject = null;
			onCompleteParam = null;
			onStart = null;
			rectTransform = null;
			uiText = null;
			uiImage = null;
			sprites = null;
		}

		public void init()
		{
			hasInitiliazed = true;
			if (onStart != null)
			{
				onStart();
			}
			switch (type)
			{
			case TweenAction.MOVE:
				from = trans.position;
				break;
			case TweenAction.MOVE_X:
			{
				ref Vector3 reference14 = ref from;
				Vector3 position2 = trans.position;
				reference14.x = position2.x;
				break;
			}
			case TweenAction.MOVE_Y:
			{
				ref Vector3 reference13 = ref from;
				Vector3 position = trans.position;
				reference13.x = position.y;
				break;
			}
			case TweenAction.MOVE_Z:
			{
				ref Vector3 reference16 = ref from;
				Vector3 position3 = trans.position;
				reference16.x = position3.z;
				break;
			}
			case TweenAction.MOVE_LOCAL_X:
			{
				ref Vector3 reference11 = ref from;
				Vector3 localPosition2 = trans.localPosition;
				reference11.x = localPosition2.x;
				break;
			}
			case TweenAction.MOVE_LOCAL_Y:
			{
				ref Vector3 reference10 = ref from;
				Vector3 localPosition = trans.localPosition;
				reference10.x = localPosition.y;
				break;
			}
			case TweenAction.MOVE_LOCAL_Z:
			{
				ref Vector3 reference15 = ref from;
				Vector3 localPosition3 = trans.localPosition;
				reference15.x = localPosition3.z;
				break;
			}
			case TweenAction.SCALE_X:
			{
				ref Vector3 reference12 = ref from;
				Vector3 localScale2 = trans.localScale;
				reference12.x = localScale2.x;
				break;
			}
			case TweenAction.SCALE_Y:
			{
				ref Vector3 reference9 = ref from;
				Vector3 localScale = trans.localScale;
				reference9.x = localScale.y;
				break;
			}
			case TweenAction.SCALE_Z:
			{
				ref Vector3 reference17 = ref from;
				Vector3 localScale3 = trans.localScale;
				reference17.x = localScale3.z;
				break;
			}
			case TweenAction.ALPHA:
			{
				SpriteRenderer component2 = trans.gameObject.GetComponent<SpriteRenderer>();
				if ((UnityEngine.Object)component2 != null)
				{
					ref Vector3 reference7 = ref from;
					Color color7 = component2.color;
					reference7.x = color7.a;
				}
				else if ((UnityEngine.Object)trans.gameObject.GetComponent<Renderer>() != null && trans.gameObject.GetComponent<Renderer>().material.HasProperty("_Color"))
				{
					ref Vector3 reference8 = ref from;
					Color color8 = trans.gameObject.GetComponent<Renderer>().material.color;
					reference8.x = color8.a;
				}
				else if ((UnityEngine.Object)trans.gameObject.GetComponent<Renderer>() != null && trans.gameObject.GetComponent<Renderer>().material.HasProperty("_TintColor"))
				{
					Color color9 = trans.gameObject.GetComponent<Renderer>().material.GetColor("_TintColor");
					from.x = color9.a;
				}
				else if (trans.childCount > 0)
				{
					foreach (Transform tran in trans)
					{
						if ((UnityEngine.Object)tran.gameObject.GetComponent<Renderer>() != null)
						{
							Color color10 = tran.gameObject.GetComponent<Renderer>().material.color;
							from.x = color10.a;
							break;
						}
					}
				}
				break;
			}
			case TweenAction.MOVE_LOCAL:
				from = trans.localPosition;
				break;
			case TweenAction.MOVE_CURVED:
			case TweenAction.MOVE_CURVED_LOCAL:
			case TweenAction.MOVE_SPLINE:
			case TweenAction.MOVE_SPLINE_LOCAL:
				from.x = 0f;
				break;
			case TweenAction.ROTATE:
				from = trans.eulerAngles;
				to = new Vector3(LeanTween.closestRot(from.x, to.x), LeanTween.closestRot(from.y, to.y), LeanTween.closestRot(from.z, to.z));
				break;
			case TweenAction.ROTATE_X:
			{
				ref Vector3 reference4 = ref from;
				Vector3 eulerAngles = trans.eulerAngles;
				reference4.x = eulerAngles.x;
				to.x = LeanTween.closestRot(from.x, to.x);
				break;
			}
			case TweenAction.ROTATE_Y:
			{
				ref Vector3 reference19 = ref from;
				Vector3 eulerAngles3 = trans.eulerAngles;
				reference19.x = eulerAngles3.y;
				to.x = LeanTween.closestRot(from.x, to.x);
				break;
			}
			case TweenAction.ROTATE_Z:
			{
				ref Vector3 reference18 = ref from;
				Vector3 eulerAngles2 = trans.eulerAngles;
				reference18.x = eulerAngles2.z;
				to.x = LeanTween.closestRot(from.x, to.x);
				break;
			}
			case TweenAction.ROTATE_AROUND:
				lastVal = 0f;
				from.x = 0f;
				origRotation = trans.rotation;
				break;
			case TweenAction.ROTATE_AROUND_LOCAL:
				lastVal = 0f;
				from.x = 0f;
				origRotation = trans.localRotation;
				break;
			case TweenAction.ROTATE_LOCAL:
				from = trans.localEulerAngles;
				to = new Vector3(LeanTween.closestRot(from.x, to.x), LeanTween.closestRot(from.y, to.y), LeanTween.closestRot(from.z, to.z));
				break;
			case TweenAction.SCALE:
				from = trans.localScale;
				break;
			case TweenAction.GUI_MOVE:
				from = new Vector3(ltRect.rect.x, ltRect.rect.y, 0f);
				break;
			case TweenAction.GUI_MOVE_MARGIN:
				from = new Vector2(ltRect.margin.x, ltRect.margin.y);
				break;
			case TweenAction.GUI_SCALE:
				from = new Vector3(ltRect.rect.width, ltRect.rect.height, 0f);
				break;
			case TweenAction.GUI_ALPHA:
				from.x = ltRect.alpha;
				break;
			case TweenAction.GUI_ROTATE:
				if (!ltRect.rotateEnabled)
				{
					ltRect.rotateEnabled = true;
					ltRect.resetForRotation();
				}
				from.x = ltRect.rotation;
				break;
			case TweenAction.ALPHA_VERTEX:
				from.x = (int)((Component)trans).GetComponent<MeshFilter>().mesh.colors32[0].a;
				break;
			case TweenAction.CALLBACK_COLOR:
				diff = new Vector3(1f, 0f, 0f);
				break;
			case TweenAction.COLOR:
			{
				SpriteRenderer component = trans.gameObject.GetComponent<SpriteRenderer>();
				if ((UnityEngine.Object)component != null)
				{
					Color color3 = component.color;
					setFromColor(color3);
				}
				else if ((UnityEngine.Object)trans.gameObject.GetComponent<Renderer>() != null && trans.gameObject.GetComponent<Renderer>().material.HasProperty("_Color"))
				{
					Color color4 = trans.gameObject.GetComponent<Renderer>().material.color;
					setFromColor(color4);
				}
				else if ((UnityEngine.Object)trans.gameObject.GetComponent<Renderer>() != null && trans.gameObject.GetComponent<Renderer>().material.HasProperty("_TintColor"))
				{
					Color color5 = trans.gameObject.GetComponent<Renderer>().material.GetColor("_TintColor");
					setFromColor(color5);
				}
				else if (trans.childCount > 0)
				{
					foreach (Transform tran2 in trans)
					{
						if ((UnityEngine.Object)tran2.gameObject.GetComponent<Renderer>() != null)
						{
							Color color6 = tran2.gameObject.GetComponent<Renderer>().material.color;
							setFromColor(color6);
							break;
						}
					}
				}
				break;
			}
			case TweenAction.CANVAS_ALPHA:
				uiImage = trans.gameObject.GetComponent<Image>();
				if ((UnityEngine.Object)uiImage != null)
				{
					ref Vector3 reference6 = ref from;
					Color color2 = uiImage.color;
					reference6.x = color2.a;
				}
				break;
			case TweenAction.CANVAS_COLOR:
				uiImage = trans.gameObject.GetComponent<Image>();
				if ((UnityEngine.Object)uiImage != null)
				{
					setFromColor(uiImage.color);
				}
				break;
			case TweenAction.TEXT_ALPHA:
				uiText = trans.gameObject.GetComponent<Text>();
				if ((UnityEngine.Object)uiText != null)
				{
					ref Vector3 reference5 = ref from;
					Color color = uiText.color;
					reference5.x = color.a;
				}
				break;
			case TweenAction.TEXT_COLOR:
				uiText = trans.gameObject.GetComponent<Text>();
				if ((UnityEngine.Object)uiText != null)
				{
					setFromColor(uiText.color);
				}
				break;
			case TweenAction.CANVAS_MOVE:
				from = rectTransform.anchoredPosition3D;
				break;
			case TweenAction.CANVAS_MOVE_X:
			{
				ref Vector3 reference3 = ref from;
				Vector3 anchoredPosition3D3 = rectTransform.anchoredPosition3D;
				reference3.x = anchoredPosition3D3.x;
				break;
			}
			case TweenAction.CANVAS_MOVE_Y:
			{
				ref Vector3 reference2 = ref from;
				Vector3 anchoredPosition3D2 = rectTransform.anchoredPosition3D;
				reference2.x = anchoredPosition3D2.y;
				break;
			}
			case TweenAction.CANVAS_MOVE_Z:
			{
				ref Vector3 reference = ref from;
				Vector3 anchoredPosition3D = rectTransform.anchoredPosition3D;
				reference.x = anchoredPosition3D.z;
				break;
			}
			case TweenAction.CANVAS_ROTATEAROUND:
			case TweenAction.CANVAS_ROTATEAROUND_LOCAL:
				lastVal = 0f;
				from.x = 0f;
				origRotation = rectTransform.rotation;
				break;
			case TweenAction.CANVAS_SCALE:
				from = rectTransform.localScale;
				break;
			case TweenAction.CANVAS_PLAYSPRITE:
				uiImage = trans.gameObject.GetComponent<Image>();
				from.x = 0f;
				break;
			}
			if (type != TweenAction.CALLBACK_COLOR && type != TweenAction.COLOR && type != TweenAction.TEXT_COLOR && type != TweenAction.CANVAS_COLOR)
			{
				diff = to - from;
			}
			if (onCompleteOnStart)
			{
				if (onComplete != null)
				{
					onComplete();
				}
				else if (onCompleteObject != null)
				{
					onCompleteObject(onCompleteParam);
				}
			}
		}

		public LTDescr setFromColor(Color col)
		{
			from = new Vector3(0f, col.a, 0f);
			diff = new Vector3(1f, 0f, 0f);
			axis = new Vector3(col.r, col.g, col.b);
			return this;
		}

		public LTDescr pause()
		{
			if (direction != 0f)
			{
				directionLast = direction;
				direction = 0f;
			}
			return this;
		}

		public LTDescr resume()
		{
			direction = directionLast;
			return this;
		}

		public LTDescr setAxis(Vector3 axis)
		{
			this.axis = axis;
			return this;
		}

		public LTDescr setDelay(float delay)
		{
			if (useEstimatedTime)
			{
				this.delay = delay;
			}
			else
			{
				this.delay = delay;
			}
			return this;
		}

		public LTDescr setEase(LeanTweenType easeType)
		{
			tweenType = easeType;
			return this;
		}

		public LTDescr setOvershoot(float overshoot)
		{
			this.overshoot = overshoot;
			return this;
		}

		public LTDescr setPeriod(float period)
		{
			this.period = period;
			return this;
		}

		public LTDescr setEase(AnimationCurve easeCurve)
		{
			animationCurve = easeCurve;
			return this;
		}

		public LTDescr setTo(Vector3 to)
		{
			if (hasInitiliazed)
			{
				this.to = to;
				diff = to - from;
			}
			else
			{
				this.to = to;
			}
			return this;
		}

		public LTDescr setFrom(Vector3 from)
		{
			if ((bool)trans)
			{
				init();
			}
			this.from = from;
			diff = to - this.from;
			return this;
		}

		public LTDescr setFrom(float from)
		{
			return setFrom(new Vector3(from, 0f, 0f));
		}

		public LTDescr setDiff(Vector3 diff)
		{
			this.diff = diff;
			return this;
		}

		public LTDescr setHasInitialized(bool has)
		{
			hasInitiliazed = has;
			return this;
		}

		public LTDescr setId(uint id)
		{
			_id = id;
			counter = global_counter;
			return this;
		}

		public LTDescr setTime(float time)
		{
			this.time = time;
			return this;
		}

		public LTDescr setRepeat(int repeat)
		{
			loopCount = repeat;
			if ((repeat > 1 && loopType == LeanTweenType.once) || (repeat < 0 && loopType == LeanTweenType.once))
			{
				loopType = LeanTweenType.clamp;
			}
			if (type == TweenAction.CALLBACK || type == TweenAction.CALLBACK_COLOR)
			{
				setOnCompleteOnRepeat(isOn: true);
			}
			return this;
		}

		public LTDescr setLoopType(LeanTweenType loopType)
		{
			this.loopType = loopType;
			return this;
		}

		public LTDescr setUseEstimatedTime(bool useEstimatedTime)
		{
			this.useEstimatedTime = useEstimatedTime;
			return this;
		}

		public LTDescr setIgnoreTimeScale(bool useUnScaledTime)
		{
			useEstimatedTime = useUnScaledTime;
			return this;
		}

		public LTDescr setUseFrames(bool useFrames)
		{
			this.useFrames = useFrames;
			return this;
		}

		public LTDescr setUseManualTime(bool useManualTime)
		{
			this.useManualTime = useManualTime;
			return this;
		}

		public LTDescr setLoopCount(int loopCount)
		{
			this.loopCount = loopCount;
			return this;
		}

		public LTDescr setLoopOnce()
		{
			loopType = LeanTweenType.once;
			return this;
		}

		public LTDescr setLoopClamp()
		{
			loopType = LeanTweenType.clamp;
			if (loopCount == 0)
			{
				loopCount = -1;
			}
			return this;
		}

		public LTDescr setLoopClamp(int loops)
		{
			loopCount = loops;
			return this;
		}

		public LTDescr setLoopPingPong()
		{
			loopType = LeanTweenType.pingPong;
			if (loopCount == 0)
			{
				loopCount = -1;
			}
			return this;
		}

		public LTDescr setLoopPingPong(int loops)
		{
			loopType = LeanTweenType.pingPong;
			loopCount = ((loops != -1) ? (loops * 2) : loops);
			return this;
		}

		public LTDescr setOnComplete(Action onComplete)
		{
			this.onComplete = onComplete;
			return this;
		}

		public LTDescr setOnComplete(Action<object> onComplete)
		{
			onCompleteObject = onComplete;
			return this;
		}

		public LTDescr setOnComplete(Action<object> onComplete, object onCompleteParam)
		{
			onCompleteObject = onComplete;
			if (onCompleteParam != null)
			{
				this.onCompleteParam = onCompleteParam;
			}
			return this;
		}

		public LTDescr setOnCompleteParam(object onCompleteParam)
		{
			this.onCompleteParam = onCompleteParam;
			return this;
		}

		public LTDescr setOnUpdate(Action<float> onUpdate)
		{
			onUpdateFloat = onUpdate;
			hasUpdateCallback = true;
			return this;
		}

		public LTDescr setOnUpdateRatio(Action<float, float> onUpdate)
		{
			onUpdateFloatRatio = onUpdate;
			hasUpdateCallback = true;
			return this;
		}

		public LTDescr setOnUpdateObject(Action<float, object> onUpdate)
		{
			onUpdateFloatObject = onUpdate;
			hasUpdateCallback = true;
			return this;
		}

		public LTDescr setOnUpdateVector2(Action<Vector2> onUpdate)
		{
			onUpdateVector2 = onUpdate;
			hasUpdateCallback = true;
			return this;
		}

		public LTDescr setOnUpdateVector3(Action<Vector3> onUpdate)
		{
			onUpdateVector3 = onUpdate;
			hasUpdateCallback = true;
			return this;
		}

		public LTDescr setOnUpdateColor(Action<Color> onUpdate)
		{
			onUpdateColor = onUpdate;
			hasUpdateCallback = true;
			return this;
		}

		public LTDescr setOnUpdate(Action<Color> onUpdate)
		{
			onUpdateColor = onUpdate;
			hasUpdateCallback = true;
			return this;
		}

		public LTDescr setOnUpdate(Action<float, object> onUpdate, object onUpdateParam = null)
		{
			onUpdateFloatObject = onUpdate;
			hasUpdateCallback = true;
			if (onUpdateParam != null)
			{
				this.onUpdateParam = onUpdateParam;
			}
			return this;
		}

		public LTDescr setOnUpdate(Action<Vector3, object> onUpdate, object onUpdateParam = null)
		{
			onUpdateVector3Object = onUpdate;
			hasUpdateCallback = true;
			if (onUpdateParam != null)
			{
				this.onUpdateParam = onUpdateParam;
			}
			return this;
		}

		public LTDescr setOnUpdate(Action<Vector2> onUpdate, object onUpdateParam = null)
		{
			onUpdateVector2 = onUpdate;
			hasUpdateCallback = true;
			if (onUpdateParam != null)
			{
				this.onUpdateParam = onUpdateParam;
			}
			return this;
		}

		public LTDescr setOnUpdate(Action<Vector3> onUpdate, object onUpdateParam = null)
		{
			onUpdateVector3 = onUpdate;
			hasUpdateCallback = true;
			if (onUpdateParam != null)
			{
				this.onUpdateParam = onUpdateParam;
			}
			return this;
		}

		public LTDescr setOnUpdateParam(object onUpdateParam)
		{
			this.onUpdateParam = onUpdateParam;
			return this;
		}

		public LTDescr setOrientToPath(bool doesOrient)
		{
			if (type == TweenAction.MOVE_CURVED || type == TweenAction.MOVE_CURVED_LOCAL)
			{
				if (path == null)
				{
					path = new LTBezierPath();
				}
				path.orientToPath = doesOrient;
			}
			else
			{
				spline.orientToPath = doesOrient;
			}
			return this;
		}

		public LTDescr setOrientToPath2d(bool doesOrient2d)
		{
			setOrientToPath(doesOrient2d);
			if (type == TweenAction.MOVE_CURVED || type == TweenAction.MOVE_CURVED_LOCAL)
			{
				path.orientToPath2d = doesOrient2d;
			}
			else
			{
				spline.orientToPath2d = doesOrient2d;
			}
			return this;
		}

		public LTDescr setRect(LTRect rect)
		{
			ltRect = rect;
			return this;
		}

		public LTDescr setRect(Rect rect)
		{
			ltRect = new LTRect(rect);
			return this;
		}

		public LTDescr setPath(LTBezierPath path)
		{
			this.path = path;
			return this;
		}

		public LTDescr setPoint(Vector3 point)
		{
			this.point = point;
			return this;
		}

		public LTDescr setDestroyOnComplete(bool doesDestroy)
		{
			destroyOnComplete = doesDestroy;
			return this;
		}

		public LTDescr setAudio(object audio)
		{
			onCompleteParam = audio;
			return this;
		}

		public LTDescr setOnCompleteOnRepeat(bool isOn)
		{
			onCompleteOnRepeat = isOn;
			return this;
		}

		public LTDescr setOnCompleteOnStart(bool isOn)
		{
			onCompleteOnStart = isOn;
			return this;
		}

		public LTDescr setRect(RectTransform rect)
		{
			rectTransform = rect;
			return this;
		}

		public LTDescr setSprites(Sprite[] sprites)
		{
			this.sprites = sprites;
			return this;
		}

		public LTDescr setFrameRate(float frameRate)
		{
			time = (float)sprites.Length / frameRate;
			return this;
		}

		public LTDescr setOnStart(Action onStart)
		{
			this.onStart = onStart;
			return this;
		}

		public LTDescr setDirection(float direction)
		{
			if (this.direction != -1f && this.direction != 1f)
			{
				Debug.LogWarning("You have passed an incorrect direction of '" + direction + "', direction must be -1f or 1f");
				return this;
			}
			if (this.direction != direction)
			{
				if (path != null)
				{
					path = new LTBezierPath(LTUtility.reverse(path.pts));
				}
				else if (spline != null)
				{
					spline = new LTSpline(LTUtility.reverse(spline.pts));
				}
			}
			return this;
		}
	}
}
