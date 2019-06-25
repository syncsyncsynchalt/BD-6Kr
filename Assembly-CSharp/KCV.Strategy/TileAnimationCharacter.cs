using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationCharacter : MonoBehaviour
	{
		public enum STATE
		{
			NONE,
			IDLE,
			POPUP,
			WAVE,
			FLOAT
		}

		private STATE state;

		private GameObject meshGO;

		private Mesh mesh;

		private new MeshRenderer renderer;

		private Vector3[] workingVertSet;

		private GameObject[] guides;

		private static Vector3[] VERTS;

		private static Vector2[] UVS;

		private static Vector3[] NORMS;

		private float timer;

		private void Awake()
		{
			meshGO = base.transform.Find("Mesh").gameObject;
			if (meshGO == null)
			{
				Debug.Log("Warning: ./Mesh not found");
			}
			try
			{
				mesh = meshGO.GetComponent<MeshFilter>().mesh;
			}
			catch (NullReferenceException)
			{
				Debug.Log("Warning: No mesh specified for MeshFilter component of ./Mesh");
			}
			if (mesh == null)
			{
				Debug.Log("Warning: MeshFilter component not attached to ./Mesh");
			}
			renderer = meshGO.GetComponent<MeshRenderer>();
			if ((UnityEngine.Object)renderer == null)
			{
				Debug.Log("Warning: MeshRenderer not attached to ./Mesh");
			}
			guides = new GameObject[11];
			for (int i = 0; i < 11; i++)
			{
				guides[i] = base.transform.Find("Guide" + (i + 1)).gameObject;
				if (guides[i] == null)
				{
					Debug.Log("Warning: ./Guide" + (i + 1) + " not found");
					continue;
				}
				guides[i].transform.parent = base.transform;
				guides[i].transform.localPosition = Vector3.zero;
			}
			state = STATE.NONE;
			workingVertSet = new Vector3[121];
			mesh.MarkDynamic();
			VERTS = mesh.vertices;
			UVS = mesh.uv;
			NORMS = mesh.normals;
			renderer.enabled = false;
			renderer.material.renderQueue = 5000;
			timer = 0f;
		}

		private void Update()
		{
			renderer.enabled = true;
			if (state == STATE.NONE)
			{
				renderer.enabled = false;
			}
			else if (state == STATE.POPUP)
			{
				PopUpUpdate();
			}
			else if (state == STATE.WAVE)
			{
				WaveUpdate();
			}
			else if (state == STATE.FLOAT)
			{
				FloatUpdate();
			}
		}

		public void PopUp(STATE next = STATE.WAVE, bool isSkip = false)
		{
			state = STATE.POPUP;
			workingVertSet = mesh.vertices;
			float num = (!isSkip) ? 1f : 0.1f;
			float num2 = (!isSkip) ? 0.06f : 0f;
			float num3 = (!isSkip) ? 0.2f : 0f;
			float d = (!isSkip) ? 1.6f : 0.3f;
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < 11; j++)
				{
					workingVertSet[11 * i + j] = new Vector3(workingVertSet[11 * i + j].x * 0f, 0f, workingVertSet[11 * i + j].z);
				}
				guides[i].transform.localPosition = new Vector3(0.01f, 0f, 0f);
				iTween.MoveTo(guides[i], iTween.Hash("position", Vector3.right, "islocal", true, "time", num, "delay", num2 * (float)(10 - i), "easeType", iTween.EaseType.easeOutExpo));
			}
			mesh.vertices = workingVertSet;
			mesh.uv = UVS;
			mesh.normals = NORMS;
			base.gameObject.transform.localPosition = new Vector3(0f, 0f, 5f);
			iTween.MoveTo(base.gameObject, iTween.Hash("position", Vector3.zero, "islocal", true, "time", num, "delay", num3, "easeType", iTween.EaseType.easeOutQuad));
			StartCoroutine(FinishAnimation(next, d));
		}

		public void PopUpUpdate()
		{
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < 11; j++)
				{
					ref Vector3 reference = ref workingVertSet[11 * i + j];
					Vector3 localPosition = guides[i].transform.localPosition;
					reference = new Vector3(localPosition.x * (float)(5 - j), 0f, workingVertSet[11 * i + j].z);
				}
			}
			mesh.vertices = workingVertSet;
			mesh.uv = UVS;
			mesh.normals = NORMS;
			mesh.Optimize();
			mesh.RecalculateBounds();
		}

		public void Wave()
		{
			state = STATE.WAVE;
			workingVertSet = mesh.vertices;
			for (int i = 0; i < 11; i++)
			{
				guides[i].transform.localPosition = Vector3.zero;
			}
			timer = Time.time;
		}

		public void WaveUpdate()
		{
			for (int i = 0; i < 11; i++)
			{
				guides[i].transform.localPosition = Mathf.Min(Time.time - timer, 1f) * Mathf.Sin(2f * (Time.time - timer - 0.2f * (float)i)) * Vector3.right;
				for (int j = 0; j < 11; j++)
				{
					ref Vector3 reference = ref workingVertSet[11 * i + j];
					Vector3 localPosition = guides[i].transform.localPosition;
					reference = new Vector3(0.6f * localPosition.x + 5f - (float)j, 0f, workingVertSet[11 * i + j].z);
				}
			}
			mesh.vertices = workingVertSet;
			mesh.uv = UVS;
			mesh.normals = NORMS;
			mesh.Optimize();
			mesh.RecalculateBounds();
		}

		public void Float()
		{
			state = STATE.FLOAT;
			timer = Time.time;
		}

		public void FloatUpdate()
		{
			meshGO.transform.localPosition = new Vector3(0f, 0f, 0.2f * Mathf.Sin((float)Math.PI * 2f * (Time.time - timer)));
		}

		public void Reset()
		{
			state = STATE.NONE;
		}

		public void UnloadTexture()
		{
			if (renderer.material.mainTexture != null)
			{
				Resources.UnloadAsset(renderer.material.mainTexture);
			}
		}

		public void SetTexture(Texture t)
		{
			renderer.material.mainTexture = t;
			base.transform.parent.localScale = new Vector3(0.025f * (float)renderer.material.mainTexture.width, 1f, 0.025f * (float)renderer.material.mainTexture.height);
		}

		public IEnumerator FinishAnimation(STATE next = STATE.WAVE, float d = 0f)
		{
			yield return new WaitForSeconds(d);
			if (next == STATE.WAVE)
			{
				Wave();
			}
			if (next == STATE.FLOAT)
			{
				Float();
			}
		}

		private void OnDestroy()
		{
			meshGO = null;
			mesh = null;
			renderer = null;
			workingVertSet = null;
			guides = null;
		}
	}
}
