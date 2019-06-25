using live2d;
using local.models;
using local.utils;
using Server_Models;
using System;
using System.Collections;
using UnityEngine;

public class Live2DModel : SingletonMonoBehaviour<Live2DModel>
{
	public enum MotionType
	{
		Port,
		Battle,
		Loop,
		Secret,
		Dislike1,
		Dislike2,
		Love1,
		Love2
	}

	private Camera myCamera;

	[Button("PlayDebug", "Play", new object[]
	{

	})]
	public int play;

	[Button("forceStop", "Stop", new object[]
	{

	})]
	public int stop;

	[Button("NextMotion", "NextMotion", new object[]
	{

	})]
	public int NextMotionButton;

	[Button("DebugChange", "DebugChange", new object[]
	{

	})]
	public int DebugChangeCharacter;

	[Button("setModel", "Reload", new object[]
	{

	})]
	public int Reload;

	public MotionType RandomMotionType;

	[Button("RandomMotion", "RandomMotion", new object[]
	{

	})]
	public int Random;

	[Button("PlayDebugPrevMotion", "PlayPrevMotion", new object[]
	{

	})]
	public int PrevMotion;

	public TextAsset prevMotionFile;

	public bool isDontRelease;

	public int DebugMstID;

	public int NowMstID;

	public static bool __DEBUG_MotionNAME_Draw;

	private readonly string[] motionName = new string[4]
	{
		"_port.mtn",
		"_battle.mtn",
		"_loop.mtn",
		"_secret.mtn"
	};

	[SerializeField]
	private MotionType NowMotion;

	private Live2DModelUnity live2DModelUnity;

	private Matrix4x4 live2DCanvasPos;

	private MotionQueueManager motionMgr;

	public TextAsset motionFile;

	public TextAsset mocFile;

	public Texture2D[] textureFiles;

	public Live2DMotion motion;

	private IEnumerator IEnum_MotionFinish;

	public bool isLive2DModel;

	public bool isLive2DChange;

	public bool isOneDraw;

	private bool isStop;

	private bool isOnePlay;

	private bool isDrawed;

	public Action StopAction;

	public float modelW;

	public float modelH;

	public float aspect;

	private string[] DebugMotionNameList = new string[4]
	{
		"_port.mtn",
		"_battle.mtn",
		"_loop.mtn",
		"_secret.mtn"
	};

	[SerializeField]
	private UITexture DebugTexture;

	[Button("DebugChangeMotion", "DebugChangeMotion", new object[]
	{

	})]
	public int Button33;

	public bool IsStop
	{
		get
		{
			return isStop;
		}
		private set
		{
			isStop = value;
			if (isStop && StopAction != null)
			{
				StopAction();
				StopAction = null;
			}
		}
	}

	protected override void Awake()
	{
		if (CheckInstance())
		{
			Live2D.init();
			motionMgr = new MotionQueueManager();
			IsStop = false;
			isOneDraw = false;
			isOnePlay = false;
			isLive2DModel = false;
			myCamera = GetComponent<Camera>();
		}
	}

	private void OnPostRender()
	{
		if (live2DModelUnity == null)
		{
			return;
		}
		live2DModelUnity.setMatrix(base.transform.localToWorldMatrix * live2DCanvasPos);
		if (!IsStop || isOneDraw)
		{
			motionMgr.updateParam(live2DModelUnity);
			live2DModelUnity.update();
			live2DModelUnity.draw();
			isDrawed = true;
			if (motionMgr.isFinished() || isOneDraw)
			{
				isOneDraw = false;
			}
		}
	}

	public void Play()
	{
		myCamera.enabled = true;
		FinishCoroutineStop();
		motionMgr.startMotion(motion);
		IsStop = false;
		if (live2DModelUnity != null)
		{
			motionMgr.updateParam(live2DModelUnity);
			live2DModelUnity.update();
		}
		if (!motion.isLoop())
		{
			IEnum_MotionFinish = MotionFinishedStop(motion);
			StartCoroutine(IEnum_MotionFinish);
		}
	}

	public void PlayOnce()
	{
		Play();
		isOnePlay = true;
	}

	public void Play(MotionType type, Action Onfinished)
	{
		ChangeMotion(type);
		StopAction = Onfinished;
		Play();
	}

	public void PlayOnce(MotionType type, Action Onfinished)
	{
		ChangeMotion(type);
		StopAction = Onfinished;
		Play();
		isOnePlay = true;
	}

	public void forceStop()
	{
		myCamera.enabled = false;
		motionMgr.stopAllMotions();
		IsStop = true;
		FinishCoroutineStop();
	}

	private void FinishCoroutineStop()
	{
		if (IEnum_MotionFinish != null)
		{
			StopCoroutine(IEnum_MotionFinish);
			IEnum_MotionFinish = null;
		}
	}

	public IEnumerator MotionFinishedStop(Live2DMotion motion)
	{
		float MotionSec = motion.getDurationMSec() / 1000;
		yield return new WaitForSeconds(MotionSec);
		if (!isOnePlay && NowMotion != MotionType.Loop)
		{
			ChangeMotion(MotionType.Loop);
			Play();
		}
		else
		{
			isOnePlay = false;
			forceStop();
		}
	}

	public void ChangeMotion(MotionType type)
	{
		string motionPath = getMotionPath(type);
		TextAsset x = ResourceManager.LoadResourceOrAssetBundle(motionPath) as TextAsset;
		if (!(x == null))
		{
			motionFile = x;
			ChangeMotion(type, motionFile);
		}
	}

	private void ChangeMotion(MotionType type, TextAsset motionFile)
	{
		motion = Live2DMotion.loadMotion(motionFile.bytes);
		bool loop = (type == MotionType.Loop) ? true : false;
		motion.setLoop(loop);
		NowMotion = type;
		prevMotionFile = motionFile;
	}

	public Texture ChangeCharacter(int MstID, bool isDamaged)
	{
		return ChangeCharacter(MstID, isDamaged, -1);
	}

	public Texture ChangeCharacter(Live2DModelUnity Live2D, ShipModel Ship)
	{
		motionMgr.stopAllMotions();
		NowMstID = Ship.MstId;
		int resourceMstId = Utils.GetResourceMstId(NowMstID);
		live2DModelUnity = Live2D;
		motionFile = (ResourceManager.LoadResourceOrAssetBundle("Live2D/" + resourceMstId + "/" + resourceMstId + "_loop.mtn") as TextAsset);
		motion = Live2DMotion.loadMotion(motionFile.bytes);
		motion.setLoop(loop: true);
		motionMgr.startMotion(motion);
		isDrawed = false;
		Play();
		isLive2DChange = ((!isLive2DModel) ? true : false);
		isLive2DModel = true;
		return myCamera.targetTexture;
	}

	public Texture ChangeCharacter(int MstID, bool isDamaged, int DeckID)
	{
		motionMgr.stopAllMotions();
		int resourceMstId = Utils.GetResourceMstId(MstID);
		string path = "Live2D/" + resourceMstId + "/" + resourceMstId + ".moc";
		TextAsset x = Resources.Load(path) as TextAsset;
		if (x != null && !isDamaged)
		{
			mocFile = x;
			for (int i = 0; i < 4; i++)
			{
				if (textureFiles[i] != null)
				{
					Resources.UnloadAsset(textureFiles[i]);
				}
				textureFiles[i] = null;
				textureFiles[i] = Resources.Load<Texture2D>("Live2D/" + resourceMstId + "/texture_0" + i);
			}
			NowMstID = MstID;
			motionFile = (ResourceManager.LoadResourceOrAssetBundle("Live2D/" + resourceMstId + "/" + resourceMstId + "_loop.mtn") as TextAsset);
			setModel(null);
			isDrawed = false;
			Play();
			isLive2DChange = ((!isLive2DModel) ? true : false);
			isLive2DModel = true;
			return myCamera.targetTexture;
		}
		int texNum = (!isDamaged) ? 9 : 10;
		isLive2DChange = (isLive2DModel ? true : false);
		isLive2DModel = false;
		if (live2DModelUnity != null)
		{
			live2DModelUnity.releaseModel();
			live2DModelUnity = null;
		}
		return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(resourceMstId, texNum);
	}

	private void setModel()
	{
		setModel(null);
	}

	private void setModel(Live2DModelUnity LModel)
	{
		if (live2DModelUnity != null)
		{
			if (isDontRelease)
			{
				live2DModelUnity = null;
			}
			else
			{
				live2DModelUnity.releaseModel();
			}
		}
		if (LModel != null)
		{
			live2DModelUnity = LModel;
		}
		else
		{
			live2DModelUnity = Live2DModelUnity.loadModel(mocFile.bytes);
		}
		for (int i = 0; i < textureFiles.Length; i++)
		{
			live2DModelUnity.setTexture(i, textureFiles[i]);
		}
		modelW = live2DModelUnity.getCanvasWidth();
		modelH = live2DModelUnity.getCanvasHeight();
		live2DCanvasPos = Matrix4x4.Ortho(0f, modelW, modelH, 0f, -50f, 50f);
		aspect = modelH / modelW;
		motion = Live2DMotion.loadMotion(motionFile.bytes);
		motion.setLoop(loop: true);
		motionMgr.startMotion(motion);
	}

	public void Enable()
	{
		myCamera.enabled = true;
		if (isLive2DModel)
		{
			Play();
		}
	}

	public void Disable()
	{
		forceStop();
		myCamera.enabled = false;
	}

	private string getMotionPath(MotionType type)
	{
		if ((int)type < motionName.Length)
		{
			return "Live2D/" + NowMstID + "/" + NowMstID + motionName[(int)type];
		}
		int num = 0;
		string result = string.Empty;
		switch (type)
		{
		case MotionType.Dislike1:
			num = Mst_DataManager.Instance.Mst_ship_resources[NowMstID].Motion1;
			result = ((num <= 1000) ? ("Live2D/" + num + "/" + num + motionName[0]) : ("Live2D/" + NowMstID + "/" + num + motionName[0]));
			break;
		case MotionType.Dislike2:
			num = Mst_DataManager.Instance.Mst_ship_resources[NowMstID].Motion2;
			result = ((num <= 1000) ? ("Live2D/" + num + "/" + num + motionName[0]) : ("Live2D/" + NowMstID + "/" + num + motionName[0]));
			break;
		case MotionType.Love1:
			num = Mst_DataManager.Instance.Mst_ship_resources[NowMstID].Motion3;
			result = ((num <= 1000) ? ("Live2D/" + num + "/" + num + motionName[0]) : ("Live2D/" + NowMstID + "/" + num + motionName[0]));
			break;
		case MotionType.Love2:
			num = Mst_DataManager.Instance.Mst_ship_resources[NowMstID].Motion4;
			result = ((num <= 1000) ? ("Live2D/" + num + "/" + num + motionName[0]) : ("Live2D/" + NowMstID + "/" + num + motionName[0]));
			break;
		}
		if (num == 0)
		{
			Debug.LogWarning("���[�V�����}�X�^�ɓo�^�����Ă��܂���");
			return "Live2D/" + NowMstID + "/" + NowMstID + motionName[0];
		}
		return result;
	}

	private void NextMotion()
	{
		int nowMotion = (int)NowMotion;
		nowMotion = (int)(NowMotion = (MotionType)Util.LoopValue(nowMotion + 1, 0f, 7f));
		ChangeMotion(NowMotion);
		isOnePlay = true;
		Play();
	}

	private void RandomMotion()
	{
		int num = 0;
		do
		{
			int num2 = UnityEngine.Random.Range(1, 500);
			motionFile = Resources.Load<TextAsset>("Live2D/" + num2 + "/" + num2 + DebugMotionNameList[(int)RandomMotionType]);
			if (motionFile != null)
			{
				break;
			}
			num++;
		}
		while (num < 200);
		prevMotionFile = motionFile;
		PlayDebug();
	}

	public Texture DebugChange()
	{
		for (int i = 1; i < 200; i++)
		{
			if (Mst_DataManager.Instance.Mst_ship.ContainsKey(NowMstID))
			{
				new ShipModelMst(NowMstID);
			}
			string path = "Live2D/" + (NowMstID + i) + "/" + (NowMstID + i) + ".moc";
			TextAsset x = Resources.Load(path) as TextAsset;
			if (x != null)
			{
				NowMstID += i;
				break;
			}
			if (i == 199)
			{
				NowMstID = 1;
			}
		}
		return ChangeCharacter(NowMstID, isDamaged: false);
	}

	private void OnDestroy()
	{
		Mem.DelAry(ref textureFiles);
		if (live2DModelUnity != null)
		{
			live2DModelUnity.releaseModel();
		}
		live2DModelUnity = null;
		motionMgr = null;
		motionFile = null;
		mocFile = null;
		textureFiles = null;
		motion = null;
	}

	public void PlayDebug()
	{
		ChangeMotion(NowMotion);
		Play();
	}

	public void PlayDebugPrevMotion()
	{
		ChangeMotion(MotionType.Port, prevMotionFile);
		isOnePlay = true;
		Play();
	}

	public void DebugChangeMotion()
	{
		string path = "Live2D/" + DebugMstID + "/" + DebugMstID + motionName[(int)RandomMotionType];
		TextAsset x = Resources.Load<TextAsset>(path);
		if (!(x == null))
		{
			motionFile = x;
			ChangeMotion(RandomMotionType, motionFile);
			isOnePlay = true;
			Play();
		}
	}

	public void DestroyCache()
	{
		live2DModelUnity = null;
	}

	public Live2DModelUnity CreateLive2DModelUnity(int MstID)
	{
		Texture2D[] array = new Texture2D[4];
		int resourceMstId = Utils.GetResourceMstId(MstID);
		string path = "Live2D/" + resourceMstId + "/" + resourceMstId + ".moc";
		TextAsset textAsset = Resources.Load(path) as TextAsset;
		Live2DModelUnity live2DModelUnity = Live2DModelUnity.loadModel(textAsset.bytes);
		for (int i = 0; i < 4; i++)
		{
			array[i] = Resources.Load<Texture2D>("Live2D/" + MstID + "/texture_0" + i);
		}
		for (int j = 0; j < textureFiles.Length; j++)
		{
			live2DModelUnity.setTexture(j, array[j]);
		}
		modelW = live2DModelUnity.getCanvasWidth();
		modelH = live2DModelUnity.getCanvasHeight();
		live2DCanvasPos = Matrix4x4.Ortho(0f, modelW, modelH, 0f, -50f, 50f);
		aspect = modelH / modelW;
		return live2DModelUnity;
	}
}
