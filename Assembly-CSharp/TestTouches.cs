using UnityEngine;
using UnityEngine.PSVita;

public class TestTouches : MonoBehaviour
{
	private GUIText gui;

	private void Start()
	{
	}

	private void Update()
	{
		if (!gui)
		{
			GameObject gameObject = new GameObject("Touch Info");
			gameObject.AddComponent<GUIText>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.transform.position = new Vector3(0.1f, 0.5f, 0f);
			gui = gameObject.GetComponent<GUIText>();
			gui.pixelOffset = new Vector2(5f, 100f);
		}
		PSVitaInput.secondaryTouchIsScreenSpace = true;
		gui.text = "\n\n\n\n\n\n\n\nSimulated Mouse\n";
		GUIText gUIText = gui;
		string text = gUIText.text;
		object[] obj = new object[6]
		{
			text,
			" pos: ",
			null,
			null,
			null,
			null
		};
		Vector3 mousePosition = Input.mousePosition;
		obj[2] = mousePosition.x;
		obj[3] = ", ";
		Vector3 mousePosition2 = Input.mousePosition;
		obj[4] = mousePosition2.y;
		obj[5] = "\n";
		gUIText.text = string.Concat(obj);
		for (int i = 0; i < 3; i++)
		{
			GUIText gUIText2 = gui;
			gUIText2.text = gUIText2.text + " button: " + i;
			GUIText gUIText3 = gui;
			gUIText3.text = gUIText3.text + " held: " + Input.GetMouseButton(i);
			GUIText gUIText4 = gui;
			gUIText4.text = gUIText4.text + " up: " + Input.GetMouseButtonUp(i);
			GUIText gUIText5 = gui;
			gUIText5.text = gUIText5.text + " down: " + Input.GetMouseButtonDown(i);
			gui.text += "\n";
		}
		gui.text += "\nTouch Screen Front";
		GUIText gUIText6 = gui;
		text = gUIText6.text;
		gUIText6.text = text + "\n touchCount: " + Input.touchCount + "\n";
		Touch[] touches = Input.touches;
		for (int j = 0; j < touches.Length; j++)
		{
			Touch touch = touches[j];
			GUIText gUIText7 = gui;
			text = gUIText7.text;
			object[] obj2 = new object[5]
			{
				text,
				" pos: ",
				null,
				null,
				null
			};
			Vector2 position = touch.position;
			obj2[2] = position.x;
			obj2[3] = ", ";
			Vector2 position2 = touch.position;
			obj2[4] = position2.y;
			gUIText7.text = string.Concat(obj2);
			GUIText gUIText8 = gui;
			text = gUIText8.text;
			object[] obj3 = new object[5]
			{
				text,
				" mp: ",
				null,
				null,
				null
			};
			Vector3 mousePosition3 = Input.mousePosition;
			obj3[2] = mousePosition3.x;
			obj3[3] = ", ";
			Vector3 mousePosition4 = Input.mousePosition;
			obj3[4] = mousePosition4.y;
			gUIText8.text = string.Concat(obj3);
			GUIText gUIText9 = gui;
			gUIText9.text = gUIText9.text + " fid: " + touch.fingerId;
			GUIText gUIText10 = gui;
			gUIText10.text = gUIText10.text + " dpos: " + touch.deltaPosition;
			GUIText gUIText11 = gui;
			gUIText11.text = gUIText11.text + " dtime: " + touch.deltaTime;
			GUIText gUIText12 = gui;
			gUIText12.text = gUIText12.text + " tapcount: " + touch.tapCount;
			GUIText gUIText13 = gui;
			gUIText13.text = gUIText13.text + " phase: " + touch.phase;
			gui.text += "\n";
			if (touch.phase == TouchPhase.Began)
			{
				MonoBehaviour.print("Began");
			}
			if (touch.phase == TouchPhase.Ended)
			{
				if (touch.tapCount == 2)
				{
					MonoBehaviour.print("Ended Multitap");
				}
				else
				{
					MonoBehaviour.print("Ended");
				}
			}
		}
		gui.text += "\nRear Touch Pad";
		GUIText gUIText14 = gui;
		gUIText14.text = gUIText14.text + "\n isScreenSpace: " + PSVitaInput.secondaryTouchIsScreenSpace;
		if (!PSVitaInput.secondaryTouchIsScreenSpace)
		{
			GUIText gUIText15 = gui;
			gUIText15.text = gUIText15.text + "\n width: " + PSVitaInput.secondaryTouchWidth;
			GUIText gUIText16 = gui;
			gUIText16.text = gUIText16.text + " height: " + PSVitaInput.secondaryTouchHeight;
		}
		GUIText gUIText17 = gui;
		text = gUIText17.text;
		gUIText17.text = text + "\n touchCount: " + PSVitaInput.touchCountSecondary + "\n";
		Touch[] touchesSecondary = PSVitaInput.touchesSecondary;
		for (int k = 0; k < touchesSecondary.Length; k++)
		{
			Touch touch2 = touchesSecondary[k];
			GUIText gUIText18 = gui;
			text = gUIText18.text;
			object[] obj4 = new object[5]
			{
				text,
				" pos: ",
				null,
				null,
				null
			};
			Vector2 position3 = touch2.position;
			obj4[2] = position3.x;
			obj4[3] = ", ";
			Vector2 position4 = touch2.position;
			obj4[4] = position4.y;
			gUIText18.text = string.Concat(obj4);
			GUIText gUIText19 = gui;
			gUIText19.text = gUIText19.text + " fid: " + touch2.fingerId;
			GUIText gUIText20 = gui;
			gUIText20.text = gUIText20.text + " dpos: " + touch2.deltaPosition;
			GUIText gUIText21 = gui;
			gUIText21.text = gUIText21.text + " dtime: " + touch2.deltaTime;
			GUIText gUIText22 = gui;
			gUIText22.text = gUIText22.text + " tapcount: " + touch2.tapCount;
			GUIText gUIText23 = gui;
			gUIText23.text = gUIText23.text + " phase: " + touch2.phase;
			gui.text += "\n";
		}
	}
}
