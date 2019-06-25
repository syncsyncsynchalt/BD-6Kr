using UnityEngine;
using UnityEngine.PSVita;

public class TestMotion : MonoBehaviour
{
	private GUIText gui;

	private void Start()
	{
	}

	private void Update()
	{
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Invalid comparison between Unknown and I4
		if (!gui)
		{
			GameObject gameObject = new GameObject("Motion Info");
			gameObject.AddComponent<GUIText>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.transform.position = new Vector3(0.7f, 0.9f, 0f);
			gui = gameObject.GetComponent<GUIText>();
		}
		gui.pixelOffset = new Vector2(0f, 0f);
		Input.compass.enabled = true;
		PSVitaInput.gyroTiltCorrectionEnabled = false;
		PSVitaInput.gyroDeadbandFilterEnabled = true;
		gui.text = "\nInput";
		gui.text += "\n .deviceOrientation: ";
		GUIText gUIText = gui;
		gUIText.text = gUIText.text + "\n   " + Input.deviceOrientation;
		gui.text += "\n\nInput.acceleration";
		GUIText gUIText2 = gui;
		gUIText2.text = gUIText2.text + "\n .x,y,z: " + Input.acceleration;
		GUIText gUIText3 = gui;
		gUIText3.text = gUIText3.text + "\n .magnitude: " + Input.acceleration.magnitude;
		gui.text += "\n\nInput.gyro";
		GUIText gUIText4 = gui;
		gUIText4.text = gUIText4.text + "\n .enabled: " + Input.gyro.enabled;
		GUIText gUIText5 = gui;
		gUIText5.text = gUIText5.text + "\n .attitude: " + Input.gyro.attitude;
		GUIText gUIText6 = gui;
		gUIText6.text = gUIText6.text + "\n .gravity: " + Input.gyro.gravity;
		GUIText gUIText7 = gui;
		gUIText7.text = gUIText7.text + "\n .rotationRate: " + Input.gyro.rotationRate;
		GUIText gUIText8 = gui;
		gUIText8.text = gUIText8.text + "\n .rotationRateUnbiased: " + Input.gyro.rotationRateUnbiased;
		GUIText gUIText9 = gui;
		gUIText9.text = gUIText9.text + "\n .updateInterval: " + Input.gyro.updateInterval;
		GUIText gUIText10 = gui;
		gUIText10.text = gUIText10.text + "\n .userAcceleration: " + Input.gyro.userAcceleration;
		GUIText gUIText11 = gui;
		gUIText11.text = gUIText11.text + "\nPSVitaInput.gyroDeadbandFilterEnabled: " + PSVitaInput.gyroDeadbandFilterEnabled;
		GUIText gUIText12 = gui;
		gUIText12.text = gUIText12.text + "\nPSVitaInput.gyroTiltCorrectionEnabled: " + PSVitaInput.gyroTiltCorrectionEnabled;
		gui.text += "\n\nInput.compass";
		GUIText gUIText13 = gui;
		gUIText13.text = gUIText13.text + "\n .enabled: " + Input.compass.enabled;
		GUIText gUIText14 = gui;
		gUIText14.text = gUIText14.text + "\n .magneticHeading: " + Input.compass.magneticHeading;
		GUIText gUIText15 = gui;
		gUIText15.text = gUIText15.text + "\n .trueHeading: " + Input.compass.trueHeading;
		GUIText gUIText16 = gui;
		gUIText16.text = gUIText16.text + "\n .rawVector: " + Input.compass.rawVector;
		GUIText gUIText17 = gui;
		gUIText17.text = gUIText17.text + "\n .timestamp: " + Input.compass.timestamp;
		gui.text += "\n PSVitaInput.compassFieldStability:";
		GUIText gUIText18 = gui;
		gUIText18.text = gUIText18.text + "\n   " + PSVitaInput.compassFieldStability;
		if ((int)PSVitaInput.compassFieldStability != 2)
		{
			gui.text += "\nCompass unstable, needs calibration!";
		}
	}
}
