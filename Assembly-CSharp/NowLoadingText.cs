using UnityEngine;

public class NowLoadingText : MonoBehaviour
{
	private UIPanel Panel;

	[SerializeField]
	private UITexture textTexture;

	private void Awake()
	{
		Panel = GetComponent<UIPanel>();
	}

	private void Start()
	{
		HideText();
	}

	public void StopAnimation()
	{
		iTween.Stop(base.gameObject);
		if (Panel == null)
		{
			Panel = GetComponent<UIPanel>();
		}
		Panel.clipOffset = new Vector2(-180f, 0f);
		textTexture.color = Color.white;
	}

	public void StartAnimation()
	{
		textTexture.color = Color.gray;
		iTween.ValueTo(base.gameObject, iTween.Hash("from", -180, "to", 180, "time", 1.2f, "onupdate", "UpdateHandler", "looptype", iTween.LoopType.loop));
	}

	private void UpdateHandler(float value)
	{
		Panel.clipOffset = new Vector2(value, 0f);
	}

	public void HideText()
	{
		Panel.SetActive(isActive: false);
		textTexture.SetActive(isActive: false);
	}
}
