using UnityEngine;

public class CommonPopupDialog : MonoBehaviour
{
	protected static CommonPopupDialog instance;

	[SerializeField]
	private CommonPopupDialogMessage Message;

	public static CommonPopupDialog Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (CommonPopupDialog)Object.FindObjectOfType(typeof(CommonPopupDialog));
				if (instance == null)
				{
					return null;
				}
			}
			return instance;
		}
		set
		{
			instance = value;
		}
	}

	private void Awake()
	{
		Instance = this;
		Message.GetComponent<UIPanel>().clipping = UIDrawCall.Clipping.None;
		((Component)Message.transform.FindChild("p_Background")).GetComponent<UITexture>().mainTexture = (Resources.Load("Textures/Common/AlertMes/txtBG_white") as Texture2D);
	}

	private void OnDestroy()
	{
		Mem.Del(ref Message);
		Mem.Del(ref instance);
	}

	public void StartPopup(string mes)
	{
		Message.StartPopup(mes, 0);
	}

	public void StartPopup(string mes, int mesNo, CommonPopupDialogMessage.PlayType type)
	{
		Message.StartPopup(mes, mesNo, type);
	}
}
