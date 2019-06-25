using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIPentagonChart : MonoBehaviour
{
	private int mKaryoku;

	private int mRaisou;

	private int mTaiku;

	private int mKaihi;

	private int mTaikyu;

	private Action mOnCompleteListener;

	private Mesh mesh;

	private Material mat;

	[SerializeField]
	private GameObject outline;

	private Mesh outMesh;

	private Material outMat;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			TEST();
		}
	}

	public void TEST()
	{
		int min = 0;
		int max = 100;
		int karyoku = UnityEngine.Random.Range(min, max);
		int raisou = UnityEngine.Random.Range(min, max);
		int taiku = UnityEngine.Random.Range(min, max);
		int kaihi = UnityEngine.Random.Range(min, max);
		int taikyu = UnityEngine.Random.Range(min, max);
		Initialize(karyoku, raisou, taiku, kaihi, taikyu);
		SetOnCompleteListener(delegate
		{
			Debug.Log(" OnComplete :D");
		});
		Play();
	}

	public void Initialize(int karyoku, int raisou, int taiku, int kaihi, int taikyu)
	{
		MeshRenderer component = GetComponent<MeshRenderer>();
		if ((UnityEngine.Object)component == null)
		{
			Debug.Log("No MeshRenderer component");
			return;
		}
		mat = component.material;
		mat.renderQueue = 5000;
		mat.color = Color.clear;
		MeshFilter component2 = GetComponent<MeshFilter>();
		if (component2 == null)
		{
			Debug.Log("No MeshFilter component");
			return;
		}
		mesh = component2.mesh;
		mesh.Clear();
		Vector3[] array = new Vector3[6]
		{
			new Vector3(0f, 0f, 0f),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3)
		};
		for (int i = 0; i < 5; i++)
		{
			array[i + 1] = new Vector3(Mathf.Cos((float)(-i * 72) * (float)Math.PI / 180f + (float)Math.PI / 2f), Mathf.Sin((float)(-i * 72) * (float)Math.PI / 180f + (float)Math.PI / 2f), 0f);
		}
		Vector3[] array2 = new Vector3[6];
		for (int j = 0; j < 6; j++)
		{
			array2[j] = new Vector3(0f, 0f, -1f);
		}
		int[] array3 = new int[15];
		for (int k = 0; k < 5; k++)
		{
			array3[3 * k] = 0;
			array3[3 * k + 1] = k + 1;
			array3[3 * k + 2] = k + 2;
		}
		array3[14] = 1;
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.triangles = array3;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
		component = outline.GetComponent<MeshRenderer>();
		if ((UnityEngine.Object)component == null)
		{
			Debug.Log("No MeshRenderer component on sibling GameObject StatsPentagonOutline");
			return;
		}
		outMat = component.material;
		outMat.renderQueue = 5000;
		outMat.color = Color.clear;
		component2 = outline.GetComponent<MeshFilter>();
		if (component2 == null)
		{
			Debug.Log("No MeshFilter component on sibling GameObject StatsPentagonOutline");
			return;
		}
		outMesh = component2.mesh;
		outMesh.Clear();
		array = new Vector3[11]
		{
			new Vector3(0f, 0f, 0f),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3)
		};
		for (int l = 0; l < 5; l++)
		{
			array[2 * l + 1] = new Vector3(Mathf.Cos((float)(-l * 72) * (float)Math.PI / 180f + (float)Math.PI * 65f / 128f), Mathf.Sin((float)(-l * 72) * (float)Math.PI / 180f + (float)Math.PI * 65f / 128f), 0f);
			array[2 * l + 2] = new Vector3(Mathf.Cos((float)(-l * 72) * (float)Math.PI / 180f + (float)Math.PI * 63f / 128f), Mathf.Sin((float)(-l * 72) * (float)Math.PI / 180f + (float)Math.PI * 63f / 128f), 0f);
		}
		array2 = new Vector3[11];
		for (int m = 0; m < 11; m++)
		{
			array2[m] = new Vector3(0f, 0f, -1f);
		}
		array3 = new int[30];
		for (int n = 0; n < 10; n++)
		{
			array3[3 * n] = 0;
			array3[3 * n + 1] = n + 1;
			array3[3 * n + 2] = n + 2;
		}
		array3[29] = 1;
		outMesh.vertices = array;
		outMesh.normals = array2;
		outMesh.triangles = array3;
		outMesh.RecalculateNormals();
		outMesh.RecalculateBounds();
		outMesh.Optimize();
		mat.color = Color.clear;
		outMat.color = Color.clear;
		mKaryoku = karyoku;
		mRaisou = raisou;
		mTaiku = taiku;
		mKaihi = kaihi;
		mTaikyu = taikyu;
	}

	public void Play()
	{
		mat.color = new Color(0f, 0.75f, 0.75f, 0.4f);
		outMat.color = new Color(1f, 1f, 1f, 0.4f);
		Tween t = GenerateTweenChart(mKaryoku, mRaisou, mTaiku, mKaihi, mTaikyu);
		t.Play();
	}

	public void PlayHide()
	{
		Tween t = GenerateTweenChart(0, 0, 0, 0, 0);
		mat.color = new Color(0f, 0f, 0f, 0f);
		outMat.color = new Color(0f, 0f, 0f, 0f);
		t.Play();
	}

	public void SetOnCompleteListener(Action onCompleteListener)
	{
		mOnCompleteListener = onCompleteListener;
	}

	public Tween GenerateTweenChart(int karyoku, int raisou, int taiku, int kaihi, int taikyu)
	{
		float from = 0f;
		float to = 1f;
		float duration = 1f;
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		Sequence sequence = DOTween.Sequence().SetId(this);
		Tween t = DOVirtual.Float(from, to, duration, delegate(float currentPercentage)
		{
			float num = (float)karyoku * currentPercentage;
			float num2 = (float)raisou * currentPercentage;
			float num3 = (float)taiku * currentPercentage;
			float num4 = (float)kaihi * currentPercentage;
			float num5 = (float)taikyu * currentPercentage;
			float[] array = new float[6]
			{
				-1f,
				num,
				num2,
				num3,
				num4,
				num5
			};
			Vector3[] vertices = mesh.vertices;
			for (int i = 1; i < 6; i++)
			{
				vertices[i] = Mathf.Max(0.01f, array[i]) * vertices[i].normalized;
			}
			mesh.vertices = vertices;
			outMesh.vertices = CalculateOutline(mesh.vertices);
		});
		sequence.AppendInterval(0.2f);
		sequence.Append(t);
		sequence.OnComplete(OnComplete);
		return sequence;
	}

	private Vector3[] CalculateOutline(Vector3[] verts)
	{
		Vector3[] array = new Vector3[11]
		{
			Vector3.zero,
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3)
		};
		for (int i = 1; i < 6; i++)
		{
			Vector3 normalized = (verts[i] - verts[(i + 3) % 5 + 1]).normalized;
			Vector3 normalized2 = (verts[i] - verts[i % 5 + 1]).normalized;
			Vector3 a = new Vector3(0f - normalized.y, normalized.x, 0f);
			Vector3 a2 = new Vector3(normalized2.y, 0f - normalized2.x, 0f);
			float d = Mathf.Max(-4f, Mathf.Min(4f, Mathf.Sign(Vector3.Dot(verts[i], normalized + normalized2)) * 4f / Mathf.Tan(Mathf.Acos(Vector3.Dot(normalized, normalized2)) / 2f)));
			array[2 * i - 1] = verts[i] + d * normalized + 4f * a;
			array[2 * i] = verts[i] + d * normalized2 + 4f * a2;
		}
		return array;
	}

	private void OnComplete()
	{
		if (mOnCompleteListener != null)
		{
			mOnCompleteListener();
		}
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		mOnCompleteListener = null;
		mesh = null;
		mat = null;
		outline = null;
		outMesh = null;
		outMat = null;
	}
}
