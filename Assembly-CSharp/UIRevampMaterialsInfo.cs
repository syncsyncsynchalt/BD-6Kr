using local.managers;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIRevampMaterialsInfo : MonoBehaviour
{
	private readonly float MOVE_TIME = 1f;

	[SerializeField]
	private Vector3 mVector3_ShowPosition;

	[SerializeField]
	private Vector3 mVector3_HidePosition;

	[SerializeField]
	private UILabel mLabel_DevKitValue;

	[SerializeField]
	private UILabel mLabel_RevampKitValue;

	private UIPanel mPanelThis;

	private void Awake()
	{
		mPanelThis = GetComponent<UIPanel>();
		mPanelThis.alpha = 0.01f;
	}

	public void Initialize(ManagerBase manager)
	{
		UpdateInfo(manager);
	}

	public void UpdateInfo(ManagerBase manager)
	{
		mLabel_DevKitValue.text = manager.Material.Devkit.ToString();
		mLabel_RevampKitValue.text = manager.Material.Revkit.ToString();
	}

	public void Show()
	{
		base.gameObject.transform.localPosition = mVector3_HidePosition;
		mPanelThis.alpha = 1f;
		Move(mVector3_ShowPosition);
	}

	public void Hide()
	{
		Move(mVector3_HidePosition);
	}

	private void Move(Vector3 moveTo)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("position", moveTo);
		hashtable.Add("time", MOVE_TIME);
		hashtable.Add("isLocal", true);
		hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
		iTween.MoveTo(base.gameObject, hashtable);
	}

	private void OnDestroy()
	{
		mLabel_DevKitValue = null;
		mLabel_RevampKitValue = null;
		mPanelThis = null;
	}
}
