using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class UISelectedObject
	{
		private static bool ZoomInOut;

		public static void SelectedObjectZoom(GameObject[] Buttons, int Index, float Zoom_Rate_Normal, float Zoom_Rate_Zoom, float time)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.one * Zoom_Rate_Zoom);
					tweenScale.duration = time;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.one);
					tweenScale2.duration = time;
				}
			}
		}

		public static void SelectedObjectZoomUpDown(GameObject[] Buttons, int Index, float Zoom_Rate_Normal, float Zoom_Rate_Zoom, float time)
		{
			if (!ZoomInOut)
			{
				return;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.one * Zoom_Rate_Zoom);
					tweenScale.duration = time;
					tweenScale.style = UITweener.Style.PingPong;
					tweenScale.ignoreTimeScale = true;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.one);
					tweenScale2.duration = time;
					tweenScale2.style = UITweener.Style.Once;
					tweenScale2.ignoreTimeScale = true;
				}
			}
		}

		public static void SelectedObjectBlink(GameObject[] Buttons, int Index)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenColor tweenColor = TweenColor.Begin(Buttons[i].gameObject, 0.2f, Util.CursolColor);
					tweenColor.from.r = Util.CursolColor.r;
					tweenColor.from.g = Util.CursolColor.g;
					tweenColor.from.b = Util.CursolColor.b;
					ref Color to = ref tweenColor.to;
					Color white = Color.white;
					to.r = white.r * 0.8f + Util.CursolColor.r * 0.2f;
					ref Color to2 = ref tweenColor.to;
					Color white2 = Color.white;
					to2.g = white2.g * 0.8f + Util.CursolColor.g * 0.2f;
					ref Color to3 = ref tweenColor.to;
					Color white3 = Color.white;
					to3.b = white3.b * 0.8f + Util.CursolColor.b * 0.2f;
					tweenColor.duration = Util.CursolBarDurationTime;
					tweenColor.method = UITweener.Method.EaseInOut;
					tweenColor.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenColor.Begin(Buttons[i].gameObject, 0.2f, Color.white);
				}
			}
		}

		public static void SelectedObjectBlink(UIButton[] Buttons, int Index)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenColor tweenColor = TweenColor.Begin(Buttons[i].gameObject, 0.2f, Util.CursolColor);
					tweenColor.from.r = Util.CursolColor.r;
					tweenColor.from.g = Util.CursolColor.g;
					tweenColor.from.b = Util.CursolColor.b;
					ref Color to = ref tweenColor.to;
					Color white = Color.white;
					to.r = white.r * 0.8f + Util.CursolColor.r * 0.2f;
					ref Color to2 = ref tweenColor.to;
					Color white2 = Color.white;
					to2.g = white2.g * 0.8f + Util.CursolColor.g * 0.2f;
					ref Color to3 = ref tweenColor.to;
					Color white3 = Color.white;
					to3.b = white3.b * 0.8f + Util.CursolColor.b * 0.2f;
					tweenColor.duration = Util.CursolBarDurationTime;
					tweenColor.method = UITweener.Method.EaseInOut;
					tweenColor.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenColor.Begin(Buttons[i].gameObject, 0.2f, Color.white);
				}
			}
		}

		public static void SelectedOneObjectBlink(UIButton Button, bool value)
		{
			if (value)
			{
				TweenColor tweenColor = TweenColor.Begin(Button.gameObject, 0.2f, Util.CursolColor);
				tweenColor.from.r = Util.CursolColor.r;
				tweenColor.from.g = Util.CursolColor.g;
				tweenColor.from.b = Util.CursolColor.b;
				ref Color to = ref tweenColor.to;
				Color white = Color.white;
				to.r = white.r * 0.8f + Util.CursolColor.r * 0.2f;
				ref Color to2 = ref tweenColor.to;
				Color white2 = Color.white;
				to2.g = white2.g * 0.8f + Util.CursolColor.g * 0.2f;
				ref Color to3 = ref tweenColor.to;
				Color white3 = Color.white;
				to3.b = white3.b * 0.8f + Util.CursolColor.b * 0.2f;
				tweenColor.duration = Util.CursolBarDurationTime;
				tweenColor.method = UITweener.Method.EaseInOut;
				tweenColor.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenColor.Begin(Button.gameObject, 0.2f, Color.white);
			}
		}

		public static void SelectedOneObjectBlink(GameObject Button, bool value)
		{
			if (value)
			{
				TweenColor tweenColor = TweenColor.Begin(Button.gameObject, 0.2f, Util.CursolColor);
				ref Color from = ref tweenColor.from;
				Color white = Color.white;
				from.r = white.r * 0.8f + Util.CursolColor.r * 0.2f;
				ref Color from2 = ref tweenColor.from;
				Color white2 = Color.white;
				from2.g = white2.g * 0.8f + Util.CursolColor.g * 0.2f;
				ref Color from3 = ref tweenColor.from;
				Color white3 = Color.white;
				from3.b = white3.b * 0.8f + Util.CursolColor.b * 0.2f;
				tweenColor.to.r = Util.CursolColor.r;
				tweenColor.to.g = Util.CursolColor.g;
				tweenColor.to.b = Util.CursolColor.b;
				tweenColor.duration = Util.CursolBarDurationTime;
				tweenColor.method = UITweener.Method.EaseInOut;
				tweenColor.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenColor.Begin(Button.gameObject, 0f, Color.white).duration = 0f;
			}
		}

		public static void SelectedOneObjectBlinkArsenal(GameObject Button, bool value)
		{
			if (value)
			{
				TweenColor tweenColor = TweenColor.Begin(Button.gameObject, 0.2f, Util.CursolColor);
				tweenColor.from = Color.white;
				ref Color to = ref tweenColor.to;
				Color white = Color.white;
				to.r = white.r * 0f;
				ref Color to2 = ref tweenColor.to;
				Color white2 = Color.white;
				to2.g = white2.g * 0.63f;
				ref Color to3 = ref tweenColor.to;
				Color white3 = Color.white;
				to3.b = white3.b * 1f;
				tweenColor.duration = Util.CursolBarDurationTime;
				tweenColor.method = UITweener.Method.EaseInOut;
				tweenColor.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenColor.Begin(Button.gameObject, 0f, Color.white).duration = 0f;
			}
		}

		public static void StartListItemBlink(Transform target, Color currentColor)
		{
			TweenColor tweenColor = TweenColor.Begin(target.gameObject, 0.2f, Util.CursolColor);
			tweenColor.from.r = Util.CursolColor.r;
			tweenColor.from.g = Util.CursolColor.g;
			tweenColor.from.b = Util.CursolColor.b;
			tweenColor.value = currentColor;
			ref Color to = ref tweenColor.to;
			Color white = Color.white;
			to.r = white.r * 0.8f + Util.CursolColor.r * 0.2f;
			ref Color to2 = ref tweenColor.to;
			Color white2 = Color.white;
			to2.g = white2.g * 0.8f + Util.CursolColor.g * 0.2f;
			ref Color to3 = ref tweenColor.to;
			Color white3 = Color.white;
			to3.b = white3.b * 0.8f + Util.CursolColor.b * 0.2f;
			tweenColor.duration = Util.CursolBarDurationTime;
			tweenColor.method = UITweener.Method.EaseInOut;
			tweenColor.style = UITweener.Style.PingPong;
		}

		public static Color StopListItemBlink(Transform target)
		{
			Color result = Color.white;
			TweenColor component = ((Component)target).GetComponent<TweenColor>();
			if (component != null)
			{
				result = component.value;
				TweenColor.Begin(target.gameObject, 0f, Color.white);
			}
			return result;
		}

		public static void SelectedOneButtonZoomUpDown(UIButton Button, bool value)
		{
			if (ZoomInOut)
			{
				if (value)
				{
					TweenScale tweenScale = TweenScale.Begin(Button.gameObject, Util.ButtonDurationTime, Vector3.one * Util.ButtonZoomUp);
					tweenScale.from = Vector3.one;
					tweenScale.duration = Util.ButtonDurationTime;
					tweenScale.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Button.gameObject, 0f, Vector3.one);
					tweenScale2.from = Vector3.one;
					tweenScale2.to = Vector3.one;
				}
			}
		}

		public static void SelectedOneButtonZoomUpDown(GameObject Button, bool value)
		{
			if (ZoomInOut)
			{
				if (value)
				{
					TweenScale tweenScale = TweenScale.Begin(Button, Util.ButtonDurationTime, Vector3.one * Util.ButtonZoomUp);
					tweenScale.from = Vector3.one;
					tweenScale.duration = Util.ButtonDurationTime;
					tweenScale.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Button, 0f, Vector3.one);
					tweenScale2.from = Vector3.one;
					tweenScale2.to = Vector3.one;
				}
			}
		}

		public static void SelectedOneBoardZoomUpDown(GameObject Button, bool value)
		{
			if (value)
			{
				TweenScale tweenScale = TweenScale.Begin(Button, Util.ButtonDurationTime, Vector3.one * Util.ButtonZoomUp);
				tweenScale.from = Vector3.one;
				tweenScale.duration = Util.ButtonDurationTime;
				tweenScale.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenScale tweenScale2 = TweenScale.Begin(Button, 0f, Vector3.one);
				tweenScale2.from = Vector3.one;
				tweenScale2.to = Vector3.one;
			}
		}

		public static void SelectedOneBoardZoomUpDownStartup(GameObject Button, bool value)
		{
			if (value)
			{
				TweenScale tweenScale = TweenScale.Begin(Button, Util.ButtonDurationTime * 2f, Vector3.one * 1.01f);
				tweenScale.from = Vector3.one;
				tweenScale.duration = Util.ButtonDurationTime * 2f;
				tweenScale.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenScale tweenScale2 = TweenScale.Begin(Button, 0f, Vector3.one);
				tweenScale2.from = Vector3.one;
				tweenScale2.to = Vector3.one;
			}
		}

		public static void SelectedButtonsZoomUpDown(UIButton[] Buttons, int Index)
		{
			if (!ZoomInOut)
			{
				return;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i].gameObject, Util.ButtonDurationTime, Vector3.one * Util.ButtonZoomUp);
					tweenScale.from = Vector3.one;
					tweenScale.duration = Util.ButtonDurationTime;
					tweenScale.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i].gameObject, 0f, Vector3.one);
					tweenScale2.from = Vector3.one;
					tweenScale2.to = Vector3.one;
				}
			}
		}

		public static void SelectedButtonsZoomUpDown(GameObject[] Buttons, int Index)
		{
			if (!ZoomInOut)
			{
				return;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i], Util.ButtonDurationTime, Vector3.one * Util.ButtonZoomUp);
					tweenScale.from = Vector3.one;
					tweenScale.duration = Util.ButtonDurationTime;
					tweenScale.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i], 0f, Vector3.one);
					tweenScale2.from = Vector3.one;
					tweenScale2.to = Vector3.one;
				}
			}
		}

		public static void SelectDicButtonZoomUpDown<T>(Dictionary<T, UIButton> dictionary, Enum iEnum)
		{
			foreach (KeyValuePair<T, UIButton> item in dictionary)
			{
				if (item.Key.ToString() == iEnum.ToString())
				{
					TweenScale tweenScale = TweenScale.Begin(item.Value.gameObject, Util.ButtonDurationTime, Vector3.one * Util.ButtonZoomUp);
					tweenScale.from = Vector3.one;
					tweenScale.duration = Util.ButtonDurationTime;
					tweenScale.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(item.Value.gameObject, 0f, Vector3.one);
					tweenScale2.from = Vector3.one;
					tweenScale2.to = Vector3.one;
				}
			}
		}
	}
}
