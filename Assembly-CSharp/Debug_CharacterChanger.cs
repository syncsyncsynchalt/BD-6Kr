using Common.Struct;
using local.models;
using System.Collections;
using UnityEngine;

public class Debug_CharacterChanger : MonoBehaviour
{
	public enum GetLocation
	{
		GetBoko,
		GetFace,
		GetSlotItemCategory,
		GetShipDisplayCenter,
		GetCutinSp1_InBattle,
		GetCutin_InBattle
	}

	private UITexture texture;

	[SerializeField]
	public string _filebase;

	public int MstID = 1;

	public bool isDamaged;

	[SerializeField]
	private GetLocation LocationType;

	[Button("changeNext", "changeNext", new object[]
	{

	})]
	public int button1;

	[Button("changePrev", "changePrev", new object[]
	{

	})]
	public int button2;

	[Button("screenShot", "screenShot", new object[]
	{

	})]
	public int button3;

	private void Awake()
	{
		texture = GetComponent<UITexture>();
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.P) && !Input.GetKeyDown(KeyCode.O))
		{
			return;
		}
		int num = Input.GetKeyDown(KeyCode.P) ? 1 : (-1);
		MstID += num;
		int texNum = (!isDamaged) ? 9 : 10;
		for (int i = 0; i < 100; i++)
		{
			texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(MstID, texNum);
			if (texture.mainTexture != null)
			{
				break;
			}
			MstID += num;
			if (i == 99)
			{
				MstID = 1;
			}
		}
		Vector3 localScale = base.transform.localScale;
		texture.MakePixelPerfect();
		base.transform.localScale = localScale;
		ShipModelMst model = new ShipModelMst(MstID);
		draw(model);
		texture.Update();
	}

	private void changeNext()
	{
		int num = 1;
		MstID += num;
		int texNum = (!isDamaged) ? 9 : 10;
		for (int i = 0; i < 100; i++)
		{
			texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(MstID, texNum);
			if (texture.mainTexture != null)
			{
				break;
			}
			MstID += num;
			if (i == 99)
			{
				MstID = 1;
			}
		}
		texture.MakePixelPerfect();
		ShipModelMst model = new ShipModelMst(MstID);
		draw(model);
		texture.Update();
	}

	private void changePrev()
	{
		int num = -1;
		MstID += num;
		int texNum = (!isDamaged) ? 9 : 10;
		for (int i = 0; i < 100; i++)
		{
			texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(MstID, texNum);
			if (texture.mainTexture != null)
			{
				break;
			}
			MstID += num;
			if (i == 99)
			{
				MstID = 1;
			}
		}
		texture.MakePixelPerfect();
		ShipModelMst model = new ShipModelMst(MstID);
		draw(model);
		texture.Update();
	}

	private void draw(ShipModelMst model)
	{
		switch (LocationType)
		{
		case GetLocation.GetFace:
		{
			Transform transform11 = texture.transform;
			Point face = model.Offsets.GetFace(isDamaged);
			transform11.localPositionX(face.x);
			Transform transform12 = texture.transform;
			Point face2 = model.Offsets.GetFace(isDamaged);
			transform12.localPositionY(face2.y);
			break;
		}
		case GetLocation.GetSlotItemCategory:
		{
			Transform transform9 = texture.transform;
			Point slotItemCategory = model.Offsets.GetSlotItemCategory(isDamaged);
			transform9.localPositionX(slotItemCategory.x);
			Transform transform10 = texture.transform;
			Point slotItemCategory2 = model.Offsets.GetSlotItemCategory(isDamaged);
			transform10.localPositionY(slotItemCategory2.y);
			break;
		}
		case GetLocation.GetShipDisplayCenter:
		{
			Transform transform7 = texture.transform;
			Point shipDisplayCenter = model.Offsets.GetShipDisplayCenter(isDamaged);
			transform7.localPositionX(shipDisplayCenter.x);
			Transform transform8 = texture.transform;
			Point shipDisplayCenter2 = model.Offsets.GetShipDisplayCenter(isDamaged);
			transform8.localPositionY(shipDisplayCenter2.y);
			break;
		}
		case GetLocation.GetCutinSp1_InBattle:
		{
			Transform transform5 = texture.transform;
			Point cutinSp1_InBattle = model.Offsets.GetCutinSp1_InBattle(isDamaged);
			transform5.localPositionX(cutinSp1_InBattle.x);
			Transform transform6 = texture.transform;
			Point cutinSp1_InBattle2 = model.Offsets.GetCutinSp1_InBattle(isDamaged);
			transform6.localPositionY(cutinSp1_InBattle2.y);
			break;
		}
		case GetLocation.GetCutin_InBattle:
		{
			Transform transform3 = texture.transform;
			Point cutin_InBattle = model.Offsets.GetCutin_InBattle(isDamaged);
			transform3.localPositionX(cutin_InBattle.x);
			Transform transform4 = texture.transform;
			Point cutin_InBattle2 = model.Offsets.GetCutin_InBattle(isDamaged);
			transform4.localPositionY(cutin_InBattle2.y);
			break;
		}
		default:
		{
			Transform transform = texture.transform;
			Point boko = model.Offsets.GetBoko(isDamaged);
			transform.localPositionX(boko.x);
			Transform transform2 = texture.transform;
			Point boko2 = model.Offsets.GetBoko(isDamaged);
			transform2.localPositionY(boko2.y);
			break;
		}
		}
	}

	private void screenShot()
	{
		Debug.Log("called_screenShot");
		StartCoroutine(SSSS());
	}

	public IEnumerator SSSS()
	{
		MstID = 0;
		for (int i = 0; i < 250; i++)
		{
			changeNext();
			Debug.Log(i + "/250 damage:" + isDamaged);
			yield return new WaitForSeconds(1f);
		}
	}

	private void DoSS(int MstID)
	{
		string filename = (!isDamaged) ? string.Format(_filebase + "_N{0:0000}.png", MstID) : string.Format(_filebase + "_D{0:0000}.png", MstID);
		Application.CaptureScreenshot(filename);
	}
}
