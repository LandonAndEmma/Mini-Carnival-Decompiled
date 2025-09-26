using System.Text.RegularExpressions;
using UnityEngine;

public class UIInputChatBox : MonoBehaviour
{
	[SerializeField]
	protected UIChatAble _uiChat;

	protected Regex myRex;

	protected TouchScreenKeyboard keyboard;

	protected string inputText;

	protected bool _bFocus;

	public string InputText
	{
		get
		{
			return inputText;
		}
		set
		{
			inputText = value;
		}
	}

	protected virtual bool ShowKeyboard()
	{
		if (keyboard != null)
		{
			Debug.Log("---------------keyboard has opened---------------------------1");
			keyboard.active = false;
			keyboard = null;
		}
		if (keyboard == null)
		{
			keyboard = TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.ASCIICapable, false, false, false, true, "max 50 letters and numbers");
			TouchScreenKeyboard.hideInput = false;
			Debug.Log("1.open keyboard");
		}
		if (keyboard == null)
		{
			return false;
		}
		if (!keyboard.active)
		{
			Debug.Log("2.active keyboard");
			keyboard.active = true;
		}
		return keyboard.active;
	}

	public virtual Rect SetFocus()
	{
		InputText = string.Empty;
		GetComponent<TUIBlock>().m_bEnable = true;
		_bFocus = true;
		ShowKeyboard();
		return TouchScreenKeyboard.area;
	}

	public virtual void KillFocus()
	{
		_bFocus = false;
		GetComponent<TUIBlock>().m_bEnable = false;
		_uiChat.NotifyInputString(inputText);
	}

	protected void Start()
	{
		myRex = new Regex("^[A-Za-z0-9]+$");
	}

	public void closedkeyboard()
	{
		if (keyboard != null)
		{
			Debug.Log("---------------keyboard has opened---------------------------2");
			keyboard.active = false;
			keyboard = null;
		}
	}

	protected void Update()
	{
		if (!_bFocus || keyboard == null)
		{
			return;
		}
		if (keyboard.done)
		{
			if (keyboard.wasCanceled || TouchScreenKeyboard.hideInput)
			{
				keyboard.active = false;
				keyboard.text = string.Empty;
				InputText = string.Empty;
				KillFocus();
				return;
			}
			bool flag = false;
			bool flag2 = false;
			InputText = keyboard.text;
			keyboard = null;
			KillFocus();
		}
		if (keyboard != null)
		{
			if (!keyboard.active)
			{
				keyboard.text = string.Empty;
				InputText = string.Empty;
				KillFocus();
			}
			if (keyboard.text.Length > 50)
			{
				keyboard.text = keyboard.text.Substring(0, 50);
			}
			if (keyboard.wasCanceled || TouchScreenKeyboard.hideInput)
			{
				keyboard.active = false;
				keyboard.text = string.Empty;
				InputText = string.Empty;
				KillFocus();
			}
		}
	}
}
