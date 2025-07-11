using System.Collections.Generic;

namespace UnityEngine;

public class TextEditor
{
	public enum DblClickSnapping : byte
	{
		WORDS,
		PARAGRAPHS
	}

	private enum CharacterType
	{
		LetterLike,
		Symbol,
		Symbol2,
		WhiteSpace
	}

	private enum TextEditOp
	{
		MoveLeft,
		MoveRight,
		MoveUp,
		MoveDown,
		MoveLineStart,
		MoveLineEnd,
		MoveTextStart,
		MoveTextEnd,
		MovePageUp,
		MovePageDown,
		MoveGraphicalLineStart,
		MoveGraphicalLineEnd,
		MoveWordLeft,
		MoveWordRight,
		MoveParagraphForward,
		MoveParagraphBackward,
		MoveToStartOfNextWord,
		MoveToEndOfPreviousWord,
		SelectLeft,
		SelectRight,
		SelectUp,
		SelectDown,
		SelectTextStart,
		SelectTextEnd,
		SelectPageUp,
		SelectPageDown,
		ExpandSelectGraphicalLineStart,
		ExpandSelectGraphicalLineEnd,
		SelectGraphicalLineStart,
		SelectGraphicalLineEnd,
		SelectWordLeft,
		SelectWordRight,
		SelectToEndOfPreviousWord,
		SelectToStartOfNextWord,
		SelectParagraphBackward,
		SelectParagraphForward,
		Delete,
		Backspace,
		DeleteWordBack,
		DeleteWordForward,
		DeleteLineBack,
		Cut,
		Copy,
		Paste,
		SelectAll,
		SelectNone,
		ScrollStart,
		ScrollEnd,
		ScrollPageUp,
		ScrollPageDown
	}

	public TouchScreenKeyboard keyboardOnScreen;

	public int controlID;

	public GUIContent content = new GUIContent();

	public GUIStyle style = GUIStyle.none;

	public bool multiline;

	public bool hasHorizontalCursorPos;

	public bool isPasswordField;

	internal bool m_HasFocus;

	public Vector2 scrollOffset = Vector2.zero;

	private Rect m_Position;

	private int m_CursorIndex;

	private int m_SelectIndex;

	private bool m_RevealCursor;

	public Vector2 graphicalCursorPos;

	public Vector2 graphicalSelectCursorPos;

	private bool m_MouseDragSelectsWholeWords;

	private int m_DblClickInitPos;

	private DblClickSnapping m_DblClickSnap;

	private bool m_bJustSelected;

	private int m_iAltCursorPos = -1;

	private string oldText;

	private int oldPos;

	private int oldSelectPos;

	private static Dictionary<Event, TextEditOp> s_Keyactions;

	public Rect position
	{
		get
		{
			return m_Position;
		}
		set
		{
			if (!(m_Position == value))
			{
				m_Position = value;
				UpdateScrollOffset();
			}
		}
	}

	public int cursorIndex
	{
		get
		{
			return m_CursorIndex;
		}
		set
		{
			int num = m_CursorIndex;
			m_CursorIndex = Mathf.Clamp(value, 0, content.text.Length);
			if (m_CursorIndex != num)
			{
				m_RevealCursor = true;
			}
		}
	}

	public int selectIndex
	{
		get
		{
			return m_SelectIndex;
		}
		set
		{
			m_SelectIndex = Mathf.Clamp(value, 0, content.text.Length);
		}
	}

	public bool hasSelection => cursorIndex != selectIndex;

	public string SelectedText
	{
		get
		{
			int length = content.text.Length;
			if (cursorIndex > length)
			{
				cursorIndex = length;
			}
			if (selectIndex > length)
			{
				selectIndex = length;
			}
			if (cursorIndex == selectIndex)
			{
				return string.Empty;
			}
			if (cursorIndex < selectIndex)
			{
				return content.text.Substring(cursorIndex, selectIndex - cursorIndex);
			}
			return content.text.Substring(selectIndex, cursorIndex - selectIndex);
		}
	}

	private void ClearCursorPos()
	{
		hasHorizontalCursorPos = false;
		m_iAltCursorPos = -1;
	}

	public void OnFocus()
	{
		if (multiline)
		{
			int num = (selectIndex = 0);
			cursorIndex = num;
		}
		else
		{
			SelectAll();
		}
		m_HasFocus = true;
	}

	public void OnLostFocus()
	{
		m_HasFocus = false;
		scrollOffset = Vector2.zero;
	}

	private void GrabGraphicalCursorPos()
	{
		if (!hasHorizontalCursorPos)
		{
			graphicalCursorPos = style.GetCursorPixelPosition(position, content, cursorIndex);
			graphicalSelectCursorPos = style.GetCursorPixelPosition(position, content, selectIndex);
			hasHorizontalCursorPos = false;
		}
	}

	public bool HandleKeyEvent(Event e)
	{
		InitKeyActions();
		EventModifiers modifiers = e.modifiers;
		e.modifiers &= ~EventModifiers.CapsLock;
		if (s_Keyactions.ContainsKey(e))
		{
			TextEditOp operation = s_Keyactions[e];
			PerformOperation(operation);
			e.modifiers = modifiers;
			return true;
		}
		e.modifiers = modifiers;
		return false;
	}

	public bool DeleteLineBack()
	{
		if (hasSelection)
		{
			DeleteSelection();
			return true;
		}
		int num = cursorIndex;
		int num2 = num;
		while (num2-- != 0)
		{
			if (content.text[num2] == '\n')
			{
				num = num2 + 1;
				break;
			}
		}
		if (num2 == -1)
		{
			num = 0;
		}
		if (cursorIndex != num)
		{
			content.text = content.text.Remove(num, cursorIndex - num);
			int num3 = (cursorIndex = num);
			selectIndex = num3;
			return true;
		}
		return false;
	}

	public bool DeleteWordBack()
	{
		if (hasSelection)
		{
			DeleteSelection();
			return true;
		}
		int num = FindEndOfPreviousWord(cursorIndex);
		if (cursorIndex != num)
		{
			content.text = content.text.Remove(num, cursorIndex - num);
			int num2 = (cursorIndex = num);
			selectIndex = num2;
			return true;
		}
		return false;
	}

	public bool DeleteWordForward()
	{
		if (hasSelection)
		{
			DeleteSelection();
			return true;
		}
		int num = FindStartOfNextWord(cursorIndex);
		if (cursorIndex < content.text.Length)
		{
			content.text = content.text.Remove(cursorIndex, num - cursorIndex);
			return true;
		}
		return false;
	}

	public bool Delete()
	{
		if (hasSelection)
		{
			DeleteSelection();
			return true;
		}
		if (cursorIndex < content.text.Length)
		{
			content.text = content.text.Remove(cursorIndex, 1);
			return true;
		}
		return false;
	}

	public bool CanPaste()
	{
		return GUIUtility.systemCopyBuffer.Length != 0;
	}

	public bool Backspace()
	{
		if (hasSelection)
		{
			DeleteSelection();
			return true;
		}
		if (cursorIndex > 0)
		{
			content.text = content.text.Remove(cursorIndex - 1, 1);
			selectIndex = --cursorIndex;
			ClearCursorPos();
			return true;
		}
		return false;
	}

	public void SelectAll()
	{
		cursorIndex = 0;
		selectIndex = content.text.Length;
		ClearCursorPos();
	}

	public void SelectNone()
	{
		selectIndex = cursorIndex;
		ClearCursorPos();
	}

	public bool DeleteSelection()
	{
		int length = content.text.Length;
		if (cursorIndex > length)
		{
			cursorIndex = length;
		}
		if (selectIndex > length)
		{
			selectIndex = length;
		}
		if (cursorIndex == selectIndex)
		{
			return false;
		}
		if (cursorIndex < selectIndex)
		{
			content.text = content.text.Substring(0, cursorIndex) + content.text.Substring(selectIndex, content.text.Length - selectIndex);
			selectIndex = cursorIndex;
		}
		else
		{
			content.text = content.text.Substring(0, selectIndex) + content.text.Substring(cursorIndex, content.text.Length - cursorIndex);
			cursorIndex = selectIndex;
		}
		ClearCursorPos();
		return true;
	}

	public void ReplaceSelection(string replace)
	{
		DeleteSelection();
		content.text = content.text.Insert(cursorIndex, replace);
		selectIndex = (cursorIndex += replace.Length);
		ClearCursorPos();
	}

	public void Insert(char c)
	{
		ReplaceSelection(c.ToString());
	}

	public void MoveSelectionToAltCursor()
	{
		if (m_iAltCursorPos != -1)
		{
			int iAltCursorPos = m_iAltCursorPos;
			string selectedText = SelectedText;
			content.text = content.text.Insert(iAltCursorPos, selectedText);
			if (iAltCursorPos < cursorIndex)
			{
				cursorIndex += selectedText.Length;
				selectIndex += selectedText.Length;
			}
			DeleteSelection();
			int num = (cursorIndex = iAltCursorPos);
			selectIndex = num;
			ClearCursorPos();
		}
	}

	public void MoveRight()
	{
		ClearCursorPos();
		if (selectIndex == cursorIndex)
		{
			cursorIndex++;
			DetectFocusChange();
			selectIndex = cursorIndex;
		}
		else if (selectIndex > cursorIndex)
		{
			cursorIndex = selectIndex;
		}
		else
		{
			selectIndex = cursorIndex;
		}
	}

	public void MoveLeft()
	{
		if (selectIndex == cursorIndex)
		{
			cursorIndex--;
			selectIndex = cursorIndex;
		}
		else if (selectIndex > cursorIndex)
		{
			selectIndex = cursorIndex;
		}
		else
		{
			cursorIndex = selectIndex;
		}
		ClearCursorPos();
	}

	public void MoveUp()
	{
		if (selectIndex < cursorIndex)
		{
			selectIndex = cursorIndex;
		}
		else
		{
			cursorIndex = selectIndex;
		}
		GrabGraphicalCursorPos();
		graphicalCursorPos.y -= 1f;
		int num = (selectIndex = style.GetCursorStringIndex(position, content, graphicalCursorPos));
		cursorIndex = num;
		if (cursorIndex <= 0)
		{
			ClearCursorPos();
		}
	}

	public void MoveDown()
	{
		if (selectIndex > cursorIndex)
		{
			selectIndex = cursorIndex;
		}
		else
		{
			cursorIndex = selectIndex;
		}
		GrabGraphicalCursorPos();
		graphicalCursorPos.y += style.lineHeight + 5f;
		int num = (selectIndex = style.GetCursorStringIndex(position, content, graphicalCursorPos));
		cursorIndex = num;
		if (cursorIndex == content.text.Length)
		{
			ClearCursorPos();
		}
	}

	public void MoveLineStart()
	{
		int num = ((selectIndex >= cursorIndex) ? cursorIndex : selectIndex);
		int num2 = num;
		int num3;
		while (num2-- != 0)
		{
			if (content.text[num2] == '\n')
			{
				num3 = (cursorIndex = num2 + 1);
				selectIndex = num3;
				return;
			}
		}
		num3 = (cursorIndex = 0);
		selectIndex = num3;
	}

	public void MoveLineEnd()
	{
		int num = ((selectIndex <= cursorIndex) ? cursorIndex : selectIndex);
		int i = num;
		int length;
		int num2;
		for (length = content.text.Length; i < length; i++)
		{
			if (content.text[i] == '\n')
			{
				num2 = (cursorIndex = i);
				selectIndex = num2;
				return;
			}
		}
		num2 = (cursorIndex = length);
		selectIndex = num2;
	}

	public void MoveGraphicalLineStart()
	{
		int num = (selectIndex = GetGraphicalLineStart((cursorIndex >= selectIndex) ? selectIndex : cursorIndex));
		cursorIndex = num;
	}

	public void MoveGraphicalLineEnd()
	{
		int num = (selectIndex = GetGraphicalLineEnd((cursorIndex <= selectIndex) ? selectIndex : cursorIndex));
		cursorIndex = num;
	}

	public void MoveTextStart()
	{
		int num = (cursorIndex = 0);
		selectIndex = num;
	}

	public void MoveTextEnd()
	{
		int num = (cursorIndex = content.text.Length);
		selectIndex = num;
	}

	private int IndexOfEndOfLine(int startIndex)
	{
		int num = content.text.IndexOf('\n', startIndex);
		return (num == -1) ? content.text.Length : num;
	}

	public void MoveParagraphForward()
	{
		cursorIndex = ((cursorIndex <= selectIndex) ? selectIndex : cursorIndex);
		if (cursorIndex < content.text.Length)
		{
			int num = (cursorIndex = IndexOfEndOfLine(cursorIndex + 1));
			selectIndex = num;
		}
	}

	public void MoveParagraphBackward()
	{
		cursorIndex = ((cursorIndex >= selectIndex) ? selectIndex : cursorIndex);
		if (cursorIndex > 1)
		{
			int num = (cursorIndex = content.text.LastIndexOf('\n', cursorIndex - 2) + 1);
			selectIndex = num;
		}
		else
		{
			int num = (cursorIndex = 0);
			selectIndex = num;
		}
	}

	public void MoveCursorToPosition(Vector2 cursorPosition)
	{
		selectIndex = style.GetCursorStringIndex(position, content, cursorPosition + scrollOffset);
		if (!Event.current.shift)
		{
			cursorIndex = selectIndex;
		}
		DetectFocusChange();
	}

	public void MoveAltCursorToPosition(Vector2 cursorPosition)
	{
		int cursorStringIndex = style.GetCursorStringIndex(position, content, cursorPosition + scrollOffset);
		m_iAltCursorPos = Mathf.Min(content.text.Length, cursorStringIndex);
		DetectFocusChange();
	}

	public bool IsOverSelection(Vector2 cursorPosition)
	{
		int cursorStringIndex = style.GetCursorStringIndex(position, content, cursorPosition + scrollOffset);
		return cursorStringIndex < Mathf.Max(cursorIndex, selectIndex) && cursorStringIndex > Mathf.Min(cursorIndex, selectIndex);
	}

	public void SelectToPosition(Vector2 cursorPosition)
	{
		if (!m_MouseDragSelectsWholeWords)
		{
			cursorIndex = style.GetCursorStringIndex(position, content, cursorPosition + scrollOffset);
			return;
		}
		int num = style.GetCursorStringIndex(position, content, cursorPosition + scrollOffset);
		if (m_DblClickSnap == DblClickSnapping.WORDS)
		{
			if (num < m_DblClickInitPos)
			{
				cursorIndex = FindEndOfClassification(num, -1);
				selectIndex = FindEndOfClassification(m_DblClickInitPos, 1);
				return;
			}
			if (num >= content.text.Length)
			{
				num = content.text.Length - 1;
			}
			cursorIndex = FindEndOfClassification(num, 1);
			selectIndex = FindEndOfClassification(m_DblClickInitPos - 1, -1);
		}
		else if (num < m_DblClickInitPos)
		{
			if (num > 0)
			{
				cursorIndex = content.text.LastIndexOf('\n', Mathf.Max(0, num - 2)) + 1;
			}
			else
			{
				cursorIndex = 0;
			}
			selectIndex = content.text.LastIndexOf('\n', m_DblClickInitPos);
		}
		else
		{
			if (num < content.text.Length)
			{
				cursorIndex = IndexOfEndOfLine(num);
			}
			else
			{
				cursorIndex = content.text.Length;
			}
			selectIndex = content.text.LastIndexOf('\n', Mathf.Max(0, m_DblClickInitPos - 2)) + 1;
		}
	}

	public void SelectLeft()
	{
		if (m_bJustSelected && cursorIndex > selectIndex)
		{
			int num = cursorIndex;
			cursorIndex = selectIndex;
			selectIndex = num;
		}
		m_bJustSelected = false;
		cursorIndex--;
	}

	public void SelectRight()
	{
		if (m_bJustSelected && cursorIndex < selectIndex)
		{
			int num = cursorIndex;
			cursorIndex = selectIndex;
			selectIndex = num;
		}
		m_bJustSelected = false;
		cursorIndex++;
		int length = content.text.Length;
		if (cursorIndex > length)
		{
			cursorIndex = length;
		}
	}

	public void SelectUp()
	{
		GrabGraphicalCursorPos();
		graphicalCursorPos.y -= 1f;
		cursorIndex = style.GetCursorStringIndex(position, content, graphicalCursorPos);
	}

	public void SelectDown()
	{
		GrabGraphicalCursorPos();
		graphicalCursorPos.y += style.lineHeight + 5f;
		cursorIndex = style.GetCursorStringIndex(position, content, graphicalCursorPos);
	}

	public void SelectTextEnd()
	{
		cursorIndex = content.text.Length;
	}

	public void SelectTextStart()
	{
		cursorIndex = 0;
	}

	public void MouseDragSelectsWholeWords(bool on)
	{
		m_MouseDragSelectsWholeWords = on;
		m_DblClickInitPos = cursorIndex;
	}

	public void DblClickSnap(DblClickSnapping snapping)
	{
		m_DblClickSnap = snapping;
	}

	private int GetGraphicalLineStart(int p)
	{
		Vector2 cursorPixelPosition = style.GetCursorPixelPosition(position, content, p);
		cursorPixelPosition.x = 0f;
		return style.GetCursorStringIndex(position, content, cursorPixelPosition);
	}

	private int GetGraphicalLineEnd(int p)
	{
		Vector2 cursorPixelPosition = style.GetCursorPixelPosition(position, content, p);
		cursorPixelPosition.x += 5000f;
		return style.GetCursorStringIndex(position, content, cursorPixelPosition);
	}

	private int FindNextSeperator(int startPos)
	{
		int length = content.text.Length;
		while (startPos < length && !isLetterLikeChar(content.text[startPos]))
		{
			startPos++;
		}
		while (startPos < length && isLetterLikeChar(content.text[startPos]))
		{
			startPos++;
		}
		return startPos;
	}

	private static bool isLetterLikeChar(char c)
	{
		return char.IsLetterOrDigit(c) || c == '\'';
	}

	private int FindPrevSeperator(int startPos)
	{
		startPos--;
		while (startPos > 0 && !isLetterLikeChar(content.text[startPos]))
		{
			startPos--;
		}
		while (startPos >= 0 && isLetterLikeChar(content.text[startPos]))
		{
			startPos--;
		}
		return startPos + 1;
	}

	public void MoveWordRight()
	{
		cursorIndex = ((cursorIndex <= selectIndex) ? selectIndex : cursorIndex);
		int num = (selectIndex = FindNextSeperator(cursorIndex));
		cursorIndex = num;
		ClearCursorPos();
	}

	public void MoveToStartOfNextWord()
	{
		ClearCursorPos();
		if (cursorIndex != selectIndex)
		{
			MoveRight();
			return;
		}
		int num = (selectIndex = FindStartOfNextWord(cursorIndex));
		cursorIndex = num;
	}

	public void MoveToEndOfPreviousWord()
	{
		ClearCursorPos();
		if (cursorIndex != selectIndex)
		{
			MoveLeft();
			return;
		}
		int num = (selectIndex = FindEndOfPreviousWord(cursorIndex));
		cursorIndex = num;
	}

	public void SelectToStartOfNextWord()
	{
		ClearCursorPos();
		cursorIndex = FindStartOfNextWord(cursorIndex);
	}

	public void SelectToEndOfPreviousWord()
	{
		ClearCursorPos();
		cursorIndex = FindEndOfPreviousWord(cursorIndex);
	}

	private CharacterType ClassifyChar(char c)
	{
		if (char.IsWhiteSpace(c))
		{
			return CharacterType.WhiteSpace;
		}
		if (char.IsLetterOrDigit(c) || c == '\'')
		{
			return CharacterType.LetterLike;
		}
		return CharacterType.Symbol;
	}

	public int FindStartOfNextWord(int p)
	{
		int length = content.text.Length;
		if (p == length)
		{
			return p;
		}
		char c = content.text[p];
		CharacterType characterType = ClassifyChar(c);
		if (characterType != CharacterType.WhiteSpace)
		{
			p++;
			while (p < length && ClassifyChar(content.text[p]) == characterType)
			{
				p++;
			}
		}
		else if (c == '\t' || c == '\n')
		{
			return p + 1;
		}
		if (p == length)
		{
			return p;
		}
		c = content.text[p];
		if (c == ' ')
		{
			while (p < length && char.IsWhiteSpace(content.text[p]))
			{
				p++;
			}
		}
		else if (c == '\t' || c == '\n')
		{
			return p;
		}
		return p;
	}

	private int FindEndOfPreviousWord(int p)
	{
		if (p == 0)
		{
			return p;
		}
		p--;
		while (p > 0 && content.text[p] == ' ')
		{
			p--;
		}
		CharacterType characterType = ClassifyChar(content.text[p]);
		if (characterType != CharacterType.WhiteSpace)
		{
			while (p > 0 && ClassifyChar(content.text[p - 1]) == characterType)
			{
				p--;
			}
		}
		return p;
	}

	public void MoveWordLeft()
	{
		cursorIndex = ((cursorIndex >= selectIndex) ? selectIndex : cursorIndex);
		cursorIndex = FindPrevSeperator(cursorIndex);
		selectIndex = cursorIndex;
	}

	public void SelectWordRight()
	{
		ClearCursorPos();
		int num = selectIndex;
		if (cursorIndex < selectIndex)
		{
			selectIndex = cursorIndex;
			MoveWordRight();
			selectIndex = num;
			cursorIndex = ((cursorIndex >= selectIndex) ? selectIndex : cursorIndex);
		}
		else
		{
			selectIndex = cursorIndex;
			MoveWordRight();
			selectIndex = num;
		}
	}

	public void SelectWordLeft()
	{
		ClearCursorPos();
		int num = selectIndex;
		if (cursorIndex > selectIndex)
		{
			selectIndex = cursorIndex;
			MoveWordLeft();
			selectIndex = num;
			cursorIndex = ((cursorIndex <= selectIndex) ? selectIndex : cursorIndex);
		}
		else
		{
			selectIndex = cursorIndex;
			MoveWordLeft();
			selectIndex = num;
		}
	}

	public void ExpandSelectGraphicalLineStart()
	{
		ClearCursorPos();
		if (cursorIndex < selectIndex)
		{
			cursorIndex = GetGraphicalLineStart(cursorIndex);
			return;
		}
		int num = cursorIndex;
		cursorIndex = GetGraphicalLineStart(selectIndex);
		selectIndex = num;
	}

	public void ExpandSelectGraphicalLineEnd()
	{
		ClearCursorPos();
		if (cursorIndex > selectIndex)
		{
			cursorIndex = GetGraphicalLineEnd(cursorIndex);
			return;
		}
		int num = cursorIndex;
		cursorIndex = GetGraphicalLineEnd(selectIndex);
		selectIndex = num;
	}

	public void SelectGraphicalLineStart()
	{
		ClearCursorPos();
		cursorIndex = GetGraphicalLineStart(cursorIndex);
	}

	public void SelectGraphicalLineEnd()
	{
		ClearCursorPos();
		cursorIndex = GetGraphicalLineEnd(cursorIndex);
	}

	public void SelectParagraphForward()
	{
		ClearCursorPos();
		bool flag = cursorIndex < selectIndex;
		if (cursorIndex < content.text.Length)
		{
			cursorIndex = IndexOfEndOfLine(cursorIndex + 1);
			if (flag && cursorIndex > selectIndex)
			{
				cursorIndex = selectIndex;
			}
		}
	}

	public void SelectParagraphBackward()
	{
		ClearCursorPos();
		bool flag = cursorIndex > selectIndex;
		if (cursorIndex > 1)
		{
			cursorIndex = content.text.LastIndexOf('\n', cursorIndex - 2) + 1;
			if (flag && cursorIndex < selectIndex)
			{
				cursorIndex = selectIndex;
			}
		}
		else
		{
			int num = (cursorIndex = 0);
			selectIndex = num;
		}
	}

	public void SelectCurrentWord()
	{
		ClearCursorPos();
		int length = content.text.Length;
		selectIndex = cursorIndex;
		if (length != 0)
		{
			if (cursorIndex >= length)
			{
				cursorIndex = length - 1;
			}
			if (selectIndex >= length)
			{
				selectIndex--;
			}
			if (cursorIndex < selectIndex)
			{
				cursorIndex = FindEndOfClassification(cursorIndex, -1);
				selectIndex = FindEndOfClassification(selectIndex, 1);
			}
			else
			{
				cursorIndex = FindEndOfClassification(cursorIndex, 1);
				selectIndex = FindEndOfClassification(selectIndex, -1);
			}
			m_bJustSelected = true;
		}
	}

	private int FindEndOfClassification(int p, int dir)
	{
		int length = content.text.Length;
		if (p >= length || p < 0)
		{
			return p;
		}
		CharacterType characterType = ClassifyChar(content.text[p]);
		do
		{
			p += dir;
			if (p < 0)
			{
				return 0;
			}
			if (p >= length)
			{
				return length;
			}
		}
		while (ClassifyChar(content.text[p]) == characterType);
		if (dir == 1)
		{
			return p;
		}
		return p + 1;
	}

	public void SelectCurrentParagraph()
	{
		ClearCursorPos();
		int length = content.text.Length;
		if (cursorIndex < length)
		{
			cursorIndex = IndexOfEndOfLine(cursorIndex) + 1;
		}
		if (selectIndex != 0)
		{
			selectIndex = content.text.LastIndexOf('\n', selectIndex - 1) + 1;
		}
	}

	public void UpdateScrollOffsetIfNeeded()
	{
		if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
		{
			UpdateScrollOffset();
		}
	}

	private void UpdateScrollOffset()
	{
		int cursorStringIndex = cursorIndex;
		graphicalCursorPos = style.GetCursorPixelPosition(new Rect(0f, 0f, position.width, position.height), content, cursorStringIndex);
		Rect rect = style.padding.Remove(position);
		Vector2 vector = new Vector2(style.CalcSize(content).x, style.CalcHeight(content, position.width));
		if (vector.x < position.width)
		{
			scrollOffset.x = 0f;
		}
		else if (m_RevealCursor)
		{
			if (graphicalCursorPos.x + 1f > scrollOffset.x + rect.width)
			{
				scrollOffset.x = graphicalCursorPos.x - rect.width;
			}
			if (graphicalCursorPos.x < scrollOffset.x + (float)style.padding.left)
			{
				scrollOffset.x = graphicalCursorPos.x - (float)style.padding.left;
			}
		}
		if (vector.y < rect.height)
		{
			scrollOffset.y = 0f;
		}
		else if (m_RevealCursor)
		{
			if (graphicalCursorPos.y + style.lineHeight > scrollOffset.y + rect.height + (float)style.padding.top)
			{
				scrollOffset.y = graphicalCursorPos.y - rect.height - (float)style.padding.top + style.lineHeight;
			}
			if (graphicalCursorPos.y < scrollOffset.y + (float)style.padding.top)
			{
				scrollOffset.y = graphicalCursorPos.y - (float)style.padding.top;
			}
		}
		if (scrollOffset.y > 0f && vector.y - scrollOffset.y < rect.height)
		{
			scrollOffset.y = vector.y - rect.height - (float)style.padding.top - (float)style.padding.bottom;
		}
		scrollOffset.y = ((!(scrollOffset.y < 0f)) ? scrollOffset.y : 0f);
		m_RevealCursor = false;
	}

	public void DrawCursor(string text)
	{
		string text2 = content.text;
		int num = cursorIndex;
		if (Input.compositionString.Length > 0)
		{
			content.text = text.Substring(0, cursorIndex) + Input.compositionString + text.Substring(selectIndex);
			num += Input.compositionString.Length;
		}
		else
		{
			content.text = text;
		}
		graphicalCursorPos = style.GetCursorPixelPosition(new Rect(0f, 0f, position.width, position.height), content, num);
		Vector2 contentOffset = style.contentOffset;
		style.contentOffset -= scrollOffset;
		style.Internal_clipOffset = scrollOffset;
		Input.compositionCursorPos = graphicalCursorPos + new Vector2(position.x, position.y + style.lineHeight) - scrollOffset;
		if (Input.compositionString.Length > 0)
		{
			style.DrawWithTextSelection(position, content, controlID, cursorIndex, cursorIndex + Input.compositionString.Length, drawSelectionAsComposition: true);
		}
		else
		{
			style.DrawWithTextSelection(position, content, controlID, cursorIndex, selectIndex);
		}
		if (m_iAltCursorPos != -1)
		{
			style.DrawCursor(position, content, controlID, m_iAltCursorPos);
		}
		style.contentOffset = contentOffset;
		style.Internal_clipOffset = Vector2.zero;
		content.text = text2;
	}

	private bool PerformOperation(TextEditOp operation)
	{
		m_RevealCursor = true;
		switch (operation)
		{
		case TextEditOp.MoveLeft:
			MoveLeft();
			break;
		case TextEditOp.MoveRight:
			MoveRight();
			break;
		case TextEditOp.MoveUp:
			MoveUp();
			break;
		case TextEditOp.MoveDown:
			MoveDown();
			break;
		case TextEditOp.MoveLineStart:
			MoveLineStart();
			break;
		case TextEditOp.MoveLineEnd:
			MoveLineEnd();
			break;
		case TextEditOp.MoveWordRight:
			MoveWordRight();
			break;
		case TextEditOp.MoveToStartOfNextWord:
			MoveToStartOfNextWord();
			break;
		case TextEditOp.MoveToEndOfPreviousWord:
			MoveToEndOfPreviousWord();
			break;
		case TextEditOp.MoveWordLeft:
			MoveWordLeft();
			break;
		case TextEditOp.MoveTextStart:
			MoveTextStart();
			break;
		case TextEditOp.MoveTextEnd:
			MoveTextEnd();
			break;
		case TextEditOp.MoveParagraphForward:
			MoveParagraphForward();
			break;
		case TextEditOp.MoveParagraphBackward:
			MoveParagraphBackward();
			break;
		case TextEditOp.MoveGraphicalLineStart:
			MoveGraphicalLineStart();
			break;
		case TextEditOp.MoveGraphicalLineEnd:
			MoveGraphicalLineEnd();
			break;
		case TextEditOp.SelectLeft:
			SelectLeft();
			break;
		case TextEditOp.SelectRight:
			SelectRight();
			break;
		case TextEditOp.SelectUp:
			SelectUp();
			break;
		case TextEditOp.SelectDown:
			SelectDown();
			break;
		case TextEditOp.SelectWordRight:
			SelectWordRight();
			break;
		case TextEditOp.SelectWordLeft:
			SelectWordLeft();
			break;
		case TextEditOp.SelectToEndOfPreviousWord:
			SelectToEndOfPreviousWord();
			break;
		case TextEditOp.SelectToStartOfNextWord:
			SelectToStartOfNextWord();
			break;
		case TextEditOp.SelectTextStart:
			SelectTextStart();
			break;
		case TextEditOp.SelectTextEnd:
			SelectTextEnd();
			break;
		case TextEditOp.ExpandSelectGraphicalLineStart:
			ExpandSelectGraphicalLineStart();
			break;
		case TextEditOp.ExpandSelectGraphicalLineEnd:
			ExpandSelectGraphicalLineEnd();
			break;
		case TextEditOp.SelectParagraphForward:
			SelectParagraphForward();
			break;
		case TextEditOp.SelectParagraphBackward:
			SelectParagraphBackward();
			break;
		case TextEditOp.SelectGraphicalLineStart:
			SelectGraphicalLineStart();
			break;
		case TextEditOp.SelectGraphicalLineEnd:
			SelectGraphicalLineEnd();
			break;
		case TextEditOp.Delete:
			return Delete();
		case TextEditOp.Backspace:
			return Backspace();
		case TextEditOp.Cut:
			return Cut();
		case TextEditOp.Copy:
			Copy();
			break;
		case TextEditOp.Paste:
			return Paste();
		case TextEditOp.SelectAll:
			SelectAll();
			break;
		case TextEditOp.SelectNone:
			SelectNone();
			break;
		case TextEditOp.DeleteWordBack:
			return DeleteWordBack();
		case TextEditOp.DeleteLineBack:
			return DeleteLineBack();
		case TextEditOp.DeleteWordForward:
			return DeleteWordForward();
		default:
			Debug.Log("Unimplemented: " + operation);
			break;
		}
		return false;
	}

	public void SaveBackup()
	{
		oldText = content.text;
		oldPos = cursorIndex;
		oldSelectPos = selectIndex;
	}

	public void Undo()
	{
		content.text = oldText;
		cursorIndex = oldPos;
		selectIndex = oldSelectPos;
	}

	public bool Cut()
	{
		if (isPasswordField)
		{
			return false;
		}
		Copy();
		return DeleteSelection();
	}

	public void Copy()
	{
		if (selectIndex != cursorIndex && !isPasswordField)
		{
			string systemCopyBuffer = ((cursorIndex >= selectIndex) ? content.text.Substring(selectIndex, cursorIndex - selectIndex) : content.text.Substring(cursorIndex, selectIndex - cursorIndex));
			GUIUtility.systemCopyBuffer = systemCopyBuffer;
		}
	}

	private static string ReplaceNewlinesWithSpaces(string value)
	{
		value = value.Replace("\r\n", " ");
		value = value.Replace('\n', ' ');
		value = value.Replace('\r', ' ');
		return value;
	}

	public bool Paste()
	{
		string text = GUIUtility.systemCopyBuffer;
		if (text != string.Empty)
		{
			if (!multiline)
			{
				text = ReplaceNewlinesWithSpaces(text);
			}
			ReplaceSelection(text);
			return true;
		}
		return false;
	}

	private static void MapKey(string key, TextEditOp action)
	{
		s_Keyactions[Event.KeyboardEvent(key)] = action;
	}

	private void InitKeyActions()
	{
		if (s_Keyactions == null)
		{
			s_Keyactions = new Dictionary<Event, TextEditOp>();
			MapKey("left", TextEditOp.MoveLeft);
			MapKey("right", TextEditOp.MoveRight);
			MapKey("up", TextEditOp.MoveUp);
			MapKey("down", TextEditOp.MoveDown);
			MapKey("#left", TextEditOp.SelectLeft);
			MapKey("#right", TextEditOp.SelectRight);
			MapKey("#up", TextEditOp.SelectUp);
			MapKey("#down", TextEditOp.SelectDown);
			MapKey("delete", TextEditOp.Delete);
			MapKey("backspace", TextEditOp.Backspace);
			MapKey("#backspace", TextEditOp.Backspace);
			if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer || Application.platform == RuntimePlatform.OSXEditor || (Application.platform == RuntimePlatform.WebGLPlayer && SystemInfo.operatingSystem.StartsWith("Mac")))
			{
				MapKey("^left", TextEditOp.MoveGraphicalLineStart);
				MapKey("^right", TextEditOp.MoveGraphicalLineEnd);
				MapKey("&left", TextEditOp.MoveWordLeft);
				MapKey("&right", TextEditOp.MoveWordRight);
				MapKey("&up", TextEditOp.MoveParagraphBackward);
				MapKey("&down", TextEditOp.MoveParagraphForward);
				MapKey("%left", TextEditOp.MoveGraphicalLineStart);
				MapKey("%right", TextEditOp.MoveGraphicalLineEnd);
				MapKey("%up", TextEditOp.MoveTextStart);
				MapKey("%down", TextEditOp.MoveTextEnd);
				MapKey("#home", TextEditOp.SelectTextStart);
				MapKey("#end", TextEditOp.SelectTextEnd);
				MapKey("#^left", TextEditOp.ExpandSelectGraphicalLineStart);
				MapKey("#^right", TextEditOp.ExpandSelectGraphicalLineEnd);
				MapKey("#^up", TextEditOp.SelectParagraphBackward);
				MapKey("#^down", TextEditOp.SelectParagraphForward);
				MapKey("#&left", TextEditOp.SelectWordLeft);
				MapKey("#&right", TextEditOp.SelectWordRight);
				MapKey("#&up", TextEditOp.SelectParagraphBackward);
				MapKey("#&down", TextEditOp.SelectParagraphForward);
				MapKey("#%left", TextEditOp.ExpandSelectGraphicalLineStart);
				MapKey("#%right", TextEditOp.ExpandSelectGraphicalLineEnd);
				MapKey("#%up", TextEditOp.SelectTextStart);
				MapKey("#%down", TextEditOp.SelectTextEnd);
				MapKey("%a", TextEditOp.SelectAll);
				MapKey("%x", TextEditOp.Cut);
				MapKey("%c", TextEditOp.Copy);
				MapKey("%v", TextEditOp.Paste);
				MapKey("^d", TextEditOp.Delete);
				MapKey("^h", TextEditOp.Backspace);
				MapKey("^b", TextEditOp.MoveLeft);
				MapKey("^f", TextEditOp.MoveRight);
				MapKey("^a", TextEditOp.MoveLineStart);
				MapKey("^e", TextEditOp.MoveLineEnd);
				MapKey("&delete", TextEditOp.DeleteWordForward);
				MapKey("&backspace", TextEditOp.DeleteWordBack);
				MapKey("%backspace", TextEditOp.DeleteLineBack);
			}
			else
			{
				MapKey("home", TextEditOp.MoveGraphicalLineStart);
				MapKey("end", TextEditOp.MoveGraphicalLineEnd);
				MapKey("%left", TextEditOp.MoveWordLeft);
				MapKey("%right", TextEditOp.MoveWordRight);
				MapKey("%up", TextEditOp.MoveParagraphBackward);
				MapKey("%down", TextEditOp.MoveParagraphForward);
				MapKey("^left", TextEditOp.MoveToEndOfPreviousWord);
				MapKey("^right", TextEditOp.MoveToStartOfNextWord);
				MapKey("^up", TextEditOp.MoveParagraphBackward);
				MapKey("^down", TextEditOp.MoveParagraphForward);
				MapKey("#^left", TextEditOp.SelectToEndOfPreviousWord);
				MapKey("#^right", TextEditOp.SelectToStartOfNextWord);
				MapKey("#^up", TextEditOp.SelectParagraphBackward);
				MapKey("#^down", TextEditOp.SelectParagraphForward);
				MapKey("#home", TextEditOp.SelectGraphicalLineStart);
				MapKey("#end", TextEditOp.SelectGraphicalLineEnd);
				MapKey("^delete", TextEditOp.DeleteWordForward);
				MapKey("^backspace", TextEditOp.DeleteWordBack);
				MapKey("%backspace", TextEditOp.DeleteLineBack);
				MapKey("^a", TextEditOp.SelectAll);
				MapKey("^x", TextEditOp.Cut);
				MapKey("^c", TextEditOp.Copy);
				MapKey("^v", TextEditOp.Paste);
				MapKey("#delete", TextEditOp.Cut);
				MapKey("^insert", TextEditOp.Copy);
				MapKey("#insert", TextEditOp.Paste);
			}
		}
	}

	public void DetectFocusChange()
	{
		if (m_HasFocus && controlID != GUIUtility.keyboardControl)
		{
			OnLostFocus();
		}
		if (!m_HasFocus && controlID == GUIUtility.keyboardControl)
		{
			OnFocus();
		}
	}
}
