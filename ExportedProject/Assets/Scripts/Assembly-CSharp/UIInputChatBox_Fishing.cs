using UnityEngine;

public class UIInputChatBox_Fishing : UIInputChatBox
{
	protected override bool ShowKeyboard()
	{
		if (keyboard != null)
		{
			keyboard.active = false;
			keyboard = null;
		}
		if (keyboard == null)
		{
			keyboard = TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.ASCIICapable, false, false, false, true, "max 40 letters and numbers");
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

	public override Rect SetFocus()
	{
		base.InputText = string.Empty;
		GetComponent<TUIBlock>().m_bEnable = true;
		_bFocus = true;
		ShowKeyboard();
		return TouchScreenKeyboard.area;
	}

	public override void KillFocus()
	{
		Debug.Log(">>>>>>>>KillFocus Start  ");
		_bFocus = false;
		GetComponent<TUIBlock>().m_bEnable = false;
		keyboard.active = false;
		keyboard = null;
		Debug.Log(">>>>>>>>KillFocus End");
	}

	private void NotifyInputInfo()
	{
		_uiChat.NotifyInputString(inputText);
		base.InputText = string.Empty;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Type_Done);
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
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
				base.InputText = string.Empty;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Type_Cancel);
				_uiChat.CancelOrHide();
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				base.InputText = keyboard.text;
				NotifyInputInfo();
				Debug.Log("----------Keyboard Done!!! ");
			}
		}
		else
		{
			if (keyboard == null)
			{
				return;
			}
			if (!keyboard.active)
			{
				keyboard.text = string.Empty;
				base.InputText = string.Empty;
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Type_Cancel);
				_uiChat.CancelOrHide();
				return;
			}
			if (keyboard.text.Length > 40)
			{
				keyboard.text = keyboard.text.Substring(0, 40);
			}
			if (keyboard.wasCanceled)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Type_Cancel);
				_uiChat.CancelOrHide();
			}
		}
	}
}
