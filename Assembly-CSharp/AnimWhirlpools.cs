using Common.Enum;
using local.models;
using System;
using UnityEngine;

public class AnimWhirlpools : MonoBehaviour
{
	[SerializeField]
	private GameObject mHasUISpriteGameObject;

	[SerializeField]
	private GameObject mHasUILabelLostCountGameObject;

	private Action mCallBack;

	public void PlayAnim(Action callBack, MapEventHappeningModel lostMaterial)
	{
		mCallBack = callBack;
		mHasUISpriteGameObject.GetComponent<UISprite>().spriteName = GetMaterialImageFileName(lostMaterial.Material);
		mHasUILabelLostCountGameObject.GetComponent<UILabel>().text = lostMaterial.Count.ToString();
		base.gameObject.GetComponent<Animation>().Play("ship_uzu");
	}

	private string GetMaterialImageFileName(enumMaterialCategory material)
	{
		switch (material)
		{
		case enumMaterialCategory.Fuel:
			return "icon-m1";
		case enumMaterialCategory.Bull:
			return "icon-m2";
		case enumMaterialCategory.Steel:
			return "icon-m3";
		case enumMaterialCategory.Bauxite:
			return "icon-m4";
		default:
			return null;
		}
	}

	public void OnPlayAnimFinished()
	{
		if (mCallBack != null)
		{
			mCallBack();
			mCallBack = null;
		}
	}
}
