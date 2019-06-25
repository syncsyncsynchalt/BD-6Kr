using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class _ShipDrawChk : MonoBehaviour
	{
		public AudioClip clip;

		private void Awake()
		{
			XorRandom.Init(0u);
			Rand.Init();
			Debug.Log(string.Empty + GetEnum().Current);
		}

		public IEnumerator<int> GetEnum()
		{
			yield return 100;
		}

		private void Start()
		{
			Debug.Log("file://" + Application.dataPath + "/AssetBundle/Editor/Sounds/BGM/1.unity3d");
			StartCoroutine(AssetBundleData.LoadAsync("file://" + Application.dataPath + "/AssetBundle/Editor/Sounds/BGM/1.unity3d", 0, delegate(AudioClip clip)
			{
				this.clip = clip;
			}));
		}

		private void Update()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			if (Input.GetKeyDown(KeyCode.A))
			{
				Debug.Log(string.Empty + Application.dataPath);
				string text = Application.dataPath + string.Empty;
				WWW val = new WWW("=?time=" + DateTime.Now.GetHashCode());
				try
				{
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (Input.GetKeyDown(KeyCode.B))
			{
			}
			if (!Input.GetKeyDown(KeyCode.C))
			{
			}
		}
	}
}
