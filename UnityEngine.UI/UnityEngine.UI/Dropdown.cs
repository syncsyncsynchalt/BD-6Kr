using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI.CoroutineTween;

namespace UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/Dropdown", 35)]
public class Dropdown : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler, ICancelHandler
{
	protected internal class DropdownItem : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, ICancelHandler
	{
		[SerializeField]
		private Text m_Text;

		[SerializeField]
		private Image m_Image;

		[SerializeField]
		private RectTransform m_RectTransform;

		[SerializeField]
		private Toggle m_Toggle;

		public Text text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text = value;
			}
		}

		public Image image
		{
			get
			{
				return m_Image;
			}
			set
			{
				m_Image = value;
			}
		}

		public RectTransform rectTransform
		{
			get
			{
				return m_RectTransform;
			}
			set
			{
				m_RectTransform = value;
			}
		}

		public Toggle toggle
		{
			get
			{
				return m_Toggle;
			}
			set
			{
				m_Toggle = value;
			}
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject);
		}

		public virtual void OnCancel(BaseEventData eventData)
		{
			Dropdown componentInParent = GetComponentInParent<Dropdown>();
			if ((bool)componentInParent)
			{
				componentInParent.Hide();
			}
		}
	}

	[Serializable]
	public class OptionData
	{
		[SerializeField]
		private string m_Text;

		[SerializeField]
		private Sprite m_Image;

		public string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text = value;
			}
		}

		public Sprite image
		{
			get
			{
				return m_Image;
			}
			set
			{
				m_Image = value;
			}
		}

		public OptionData()
		{
		}

		public OptionData(string text)
		{
			this.text = text;
		}

		public OptionData(Sprite image)
		{
			this.image = image;
		}

		public OptionData(string text, Sprite image)
		{
			this.text = text;
			this.image = image;
		}
	}

	[Serializable]
	public class OptionDataList
	{
		[SerializeField]
		private List<OptionData> m_Options;

		public List<OptionData> options
		{
			get
			{
				return m_Options;
			}
			set
			{
				m_Options = value;
			}
		}

		public OptionDataList()
		{
			options = new List<OptionData>();
		}
	}

	[Serializable]
	public class DropdownEvent : UnityEvent<int>
	{
	}

	[SerializeField]
	private RectTransform m_Template;

	[SerializeField]
	private Text m_CaptionText;

	[SerializeField]
	private Image m_CaptionImage;

	[Space]
	[SerializeField]
	private Text m_ItemText;

	[SerializeField]
	private Image m_ItemImage;

	[SerializeField]
	[Space]
	private int m_Value;

	[SerializeField]
	[Space]
	private OptionDataList m_Options = new OptionDataList();

	[SerializeField]
	[Space]
	private DropdownEvent m_OnValueChanged = new DropdownEvent();

	private GameObject m_Dropdown;

	private GameObject m_Blocker;

	private List<DropdownItem> m_Items = new List<DropdownItem>();

	private TweenRunner<FloatTween> m_AlphaTweenRunner;

	private bool validTemplate;

	public RectTransform template
	{
		get
		{
			return m_Template;
		}
		set
		{
			m_Template = value;
			Refresh();
		}
	}

	public Text captionText
	{
		get
		{
			return m_CaptionText;
		}
		set
		{
			m_CaptionText = value;
			Refresh();
		}
	}

	public Image captionImage
	{
		get
		{
			return m_CaptionImage;
		}
		set
		{
			m_CaptionImage = value;
			Refresh();
		}
	}

	public Text itemText
	{
		get
		{
			return m_ItemText;
		}
		set
		{
			m_ItemText = value;
			Refresh();
		}
	}

	public Image itemImage
	{
		get
		{
			return m_ItemImage;
		}
		set
		{
			m_ItemImage = value;
			Refresh();
		}
	}

	public List<OptionData> options
	{
		get
		{
			return m_Options.options;
		}
		set
		{
			m_Options.options = value;
			Refresh();
		}
	}

	public DropdownEvent onValueChanged
	{
		get
		{
			return m_OnValueChanged;
		}
		set
		{
			m_OnValueChanged = value;
		}
	}

	public int value
	{
		get
		{
			return m_Value;
		}
		set
		{
			if (!Application.isPlaying || (value != m_Value && options.Count != 0))
			{
				m_Value = Mathf.Clamp(value, 0, options.Count - 1);
				Refresh();
				m_OnValueChanged.Invoke(m_Value);
			}
		}
	}

	protected Dropdown()
	{
	}

	protected override void Awake()
	{
		m_AlphaTweenRunner = new TweenRunner<FloatTween>();
		m_AlphaTweenRunner.Init(this);
		if ((bool)m_CaptionImage)
		{
			m_CaptionImage.enabled = m_CaptionImage.sprite != null;
		}
		if ((bool)m_Template)
		{
			m_Template.gameObject.SetActive(value: false);
		}
	}

	private void Refresh()
	{
		if (options.Count == 0)
		{
			return;
		}
		OptionData optionData = options[Mathf.Clamp(m_Value, 0, options.Count - 1)];
		if ((bool)m_CaptionText)
		{
			if (optionData != null && optionData.text != null)
			{
				m_CaptionText.text = optionData.text;
			}
			else
			{
				m_CaptionText.text = string.Empty;
			}
		}
		if ((bool)m_CaptionImage)
		{
			if (optionData != null)
			{
				m_CaptionImage.sprite = optionData.image;
			}
			else
			{
				m_CaptionImage.sprite = null;
			}
			m_CaptionImage.enabled = m_CaptionImage.sprite != null;
		}
	}

	private void SetupTemplate()
	{
		validTemplate = false;
		if (!m_Template)
		{
			Debug.LogError("The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", this);
			return;
		}
		GameObject gameObject = m_Template.gameObject;
		gameObject.SetActive(value: true);
		Toggle componentInChildren = m_Template.GetComponentInChildren<Toggle>();
		validTemplate = true;
		if (!componentInChildren || componentInChildren.transform == template)
		{
			validTemplate = false;
			Debug.LogError("The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", template);
		}
		else if (!(componentInChildren.transform.parent is RectTransform))
		{
			validTemplate = false;
			Debug.LogError("The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", template);
		}
		else if (itemText != null && !itemText.transform.IsChildOf(componentInChildren.transform))
		{
			validTemplate = false;
			Debug.LogError("The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", template);
		}
		else if (itemImage != null && !itemImage.transform.IsChildOf(componentInChildren.transform))
		{
			validTemplate = false;
			Debug.LogError("The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", template);
		}
		if (!validTemplate)
		{
			gameObject.SetActive(value: false);
			return;
		}
		DropdownItem dropdownItem = componentInChildren.gameObject.AddComponent<DropdownItem>();
		dropdownItem.text = m_ItemText;
		dropdownItem.image = m_ItemImage;
		dropdownItem.toggle = componentInChildren;
		dropdownItem.rectTransform = (RectTransform)componentInChildren.transform;
		Canvas orAddComponent = GetOrAddComponent<Canvas>(gameObject);
		orAddComponent.overrideSorting = true;
		orAddComponent.sortingOrder = 30000;
		GetOrAddComponent<GraphicRaycaster>(gameObject);
		GetOrAddComponent<CanvasGroup>(gameObject);
		gameObject.SetActive(value: false);
		validTemplate = true;
	}

	private static T GetOrAddComponent<T>(GameObject go) where T : Component
	{
		T val = go.GetComponent<T>();
		if (!val)
		{
			val = go.AddComponent<T>();
		}
		return val;
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		Show();
	}

	public virtual void OnSubmit(BaseEventData eventData)
	{
		Show();
	}

	public virtual void OnCancel(BaseEventData eventData)
	{
		Hide();
	}

	public void Show()
	{
		if (!IsActive() || !IsInteractable() || m_Dropdown != null)
		{
			return;
		}
		if (!validTemplate)
		{
			SetupTemplate();
			if (!validTemplate)
			{
				return;
			}
		}
		List<Canvas> list = ListPool<Canvas>.Get();
		base.gameObject.GetComponentsInParent(includeInactive: false, list);
		if (list.Count == 0)
		{
			return;
		}
		Canvas canvas = list[0];
		ListPool<Canvas>.Release(list);
		m_Template.gameObject.SetActive(value: true);
		m_Dropdown = CreateDropdownList(m_Template.gameObject);
		m_Dropdown.name = "Dropdown List";
		m_Dropdown.SetActive(value: true);
		RectTransform rectTransform = m_Dropdown.transform as RectTransform;
		rectTransform.SetParent(m_Template.transform.parent, worldPositionStays: false);
		DropdownItem componentInChildren = m_Dropdown.GetComponentInChildren<DropdownItem>();
		GameObject gameObject = componentInChildren.rectTransform.parent.gameObject;
		RectTransform rectTransform2 = gameObject.transform as RectTransform;
		componentInChildren.rectTransform.gameObject.SetActive(value: true);
		Rect rect = rectTransform2.rect;
		Rect rect2 = componentInChildren.rectTransform.rect;
		Vector2 vector = rect2.min - rect.min + (Vector2)componentInChildren.rectTransform.localPosition;
		Vector2 vector2 = rect2.max - rect.max + (Vector2)componentInChildren.rectTransform.localPosition;
		Vector2 size = rect2.size;
		m_Items.Clear();
		Toggle toggle = null;
		for (int i = 0; i < options.Count; i++)
		{
			OptionData data = options[i];
			DropdownItem item = AddItem(data, value == i, componentInChildren, m_Items);
			if (!(item == null))
			{
				item.toggle.isOn = value == i;
				item.toggle.onValueChanged.AddListener(delegate
				{
					OnSelectItem(item.toggle);
				});
				if (item.toggle.isOn)
				{
					item.toggle.Select();
				}
				if (toggle != null)
				{
					Navigation navigation = toggle.navigation;
					Navigation navigation2 = item.toggle.navigation;
					navigation.mode = Navigation.Mode.Explicit;
					navigation2.mode = Navigation.Mode.Explicit;
					navigation.selectOnDown = item.toggle;
					navigation.selectOnRight = item.toggle;
					navigation2.selectOnLeft = toggle;
					navigation2.selectOnUp = toggle;
					toggle.navigation = navigation;
					item.toggle.navigation = navigation2;
				}
				toggle = item.toggle;
			}
		}
		Vector2 sizeDelta = rectTransform2.sizeDelta;
		sizeDelta.y = size.y * (float)m_Items.Count + vector.y - vector2.y;
		rectTransform2.sizeDelta = sizeDelta;
		float num = rectTransform.rect.height - rectTransform2.rect.height;
		if (num > 0f)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - num);
		}
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		bool flag = false;
		RectTransform rectTransform3 = canvas.transform as RectTransform;
		for (int num2 = 0; num2 < 4; num2++)
		{
			Vector3 point = rectTransform3.InverseTransformPoint(array[num2]);
			if (!rectTransform3.rect.Contains(point))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			RectTransformUtility.FlipLayoutOnAxis(rectTransform, 0, keepPositioning: false, recursive: false);
			RectTransformUtility.FlipLayoutOnAxis(rectTransform, 1, keepPositioning: false, recursive: false);
		}
		for (int num3 = 0; num3 < m_Items.Count; num3++)
		{
			RectTransform rectTransform4 = m_Items[num3].rectTransform;
			rectTransform4.anchorMin = new Vector2(rectTransform4.anchorMin.x, 0f);
			rectTransform4.anchorMax = new Vector2(rectTransform4.anchorMax.x, 0f);
			rectTransform4.anchoredPosition = new Vector2(rectTransform4.anchoredPosition.x, vector.y + size.y * (float)(m_Items.Count - 1 - num3) + size.y * rectTransform4.pivot.y);
			rectTransform4.sizeDelta = new Vector2(rectTransform4.sizeDelta.x, size.y);
		}
		AlphaFadeList(0.15f, 0f, 1f);
		m_Template.gameObject.SetActive(value: false);
		componentInChildren.gameObject.SetActive(value: false);
		m_Blocker = CreateBlocker(canvas);
	}

	protected virtual GameObject CreateBlocker(Canvas rootCanvas)
	{
		GameObject gameObject = new GameObject("Blocker");
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.SetParent(rootCanvas.transform, worldPositionStays: false);
		rectTransform.anchorMin = Vector3.zero;
		rectTransform.anchorMax = Vector3.one;
		rectTransform.sizeDelta = Vector2.zero;
		Canvas canvas = gameObject.AddComponent<Canvas>();
		canvas.overrideSorting = true;
		Canvas component = m_Dropdown.GetComponent<Canvas>();
		canvas.sortingLayerID = component.sortingLayerID;
		canvas.sortingOrder = component.sortingOrder - 1;
		gameObject.AddComponent<GraphicRaycaster>();
		Image image = gameObject.AddComponent<Image>();
		image.color = Color.clear;
		Button button = gameObject.AddComponent<Button>();
		button.onClick.AddListener(Hide);
		return gameObject;
	}

	protected virtual void DestroyBlocker(GameObject blocker)
	{
		Object.Destroy(blocker);
	}

	protected virtual GameObject CreateDropdownList(GameObject template)
	{
		return Object.Instantiate(template);
	}

	protected virtual void DestroyDropdownList(GameObject dropdownList)
	{
		Object.Destroy(dropdownList);
	}

	protected virtual DropdownItem CreateItem(DropdownItem itemTemplate)
	{
		return Object.Instantiate(itemTemplate);
	}

	protected virtual void DestroyItem(DropdownItem item)
	{
	}

	private DropdownItem AddItem(OptionData data, bool selected, DropdownItem itemTemplate, List<DropdownItem> items)
	{
		DropdownItem dropdownItem = CreateItem(itemTemplate);
		dropdownItem.rectTransform.SetParent(itemTemplate.rectTransform.parent, worldPositionStays: false);
		dropdownItem.gameObject.SetActive(value: true);
		dropdownItem.gameObject.name = "Item " + items.Count + ((data.text == null) ? string.Empty : (": " + data.text));
		if (dropdownItem.toggle != null)
		{
			dropdownItem.toggle.isOn = false;
		}
		if ((bool)dropdownItem.text)
		{
			dropdownItem.text.text = data.text;
		}
		if ((bool)dropdownItem.image)
		{
			dropdownItem.image.sprite = data.image;
			dropdownItem.image.enabled = dropdownItem.image.sprite != null;
		}
		items.Add(dropdownItem);
		return dropdownItem;
	}

	private void AlphaFadeList(float duration, float alpha)
	{
		CanvasGroup component = m_Dropdown.GetComponent<CanvasGroup>();
		AlphaFadeList(duration, component.alpha, alpha);
	}

	private void AlphaFadeList(float duration, float start, float end)
	{
		if (!end.Equals(start))
		{
			FloatTween info = new FloatTween
			{
				duration = duration,
				startValue = start,
				targetValue = end
			};
			info.AddOnChangedCallback(SetAlpha);
			info.ignoreTimeScale = true;
			m_AlphaTweenRunner.StartTween(info);
		}
	}

	private void SetAlpha(float alpha)
	{
		if ((bool)m_Dropdown)
		{
			CanvasGroup component = m_Dropdown.GetComponent<CanvasGroup>();
			component.alpha = alpha;
		}
	}

	public void Hide()
	{
		if (m_Dropdown != null)
		{
			AlphaFadeList(0.15f, 0f);
			StartCoroutine(DelayedDestroyDropdownList(0.15f));
		}
		if (m_Blocker != null)
		{
			DestroyBlocker(m_Blocker);
		}
		m_Blocker = null;
		Select();
	}

	private IEnumerator DelayedDestroyDropdownList(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < m_Items.Count; i++)
		{
			if (m_Items[i] != null)
			{
				DestroyItem(m_Items[i]);
			}
			m_Items.Clear();
		}
		if (m_Dropdown != null)
		{
			DestroyDropdownList(m_Dropdown);
		}
		m_Dropdown = null;
	}

	private void OnSelectItem(Toggle toggle)
	{
		if (!toggle.isOn)
		{
			toggle.isOn = true;
		}
		int num = -1;
		Transform transform = toggle.transform;
		Transform parent = transform.parent;
		for (int i = 0; i < parent.childCount; i++)
		{
			if (parent.GetChild(i) == transform)
			{
				num = i - 1;
				break;
			}
		}
		if (num >= 0)
		{
			value = num;
			Hide();
		}
	}
}
