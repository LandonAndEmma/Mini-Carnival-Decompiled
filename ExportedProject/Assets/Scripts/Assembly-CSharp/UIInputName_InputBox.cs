using System.Text.RegularExpressions;
using TUINameSpace;
using UnityEngine;

public class UIInputName_InputBox : MonoBehaviour
{
	[SerializeField]
	private UI_InputLabel label;

	[SerializeField]
	private TUINameSpace.UICursor cursor;

	protected Regex myRex;

	private TouchScreenKeyboard keyboard;

	private string inputText;

	public string InputText
	{
		get
		{
			return label.Text;
		}
		set
		{
			label.Text = value;
		}
	}

	private void OnDisable()
	{
		CloseKeyboard();
	}

	private void Start()
	{
		TouchScreenKeyboard.hideInput = true;
		ShowKeyboard();
		myRex = new Regex("^[A-Za-z0-9]+$");
		InputText = COMA_CommonOperation.Instance.defaultInput;
	}

	private void Update()
	{
		if (!cursor.Focus)
		{
			return;
		}
		if (keyboard != null)
		{
			bool flag = false;
			bool flag2 = false;
			if (keyboard.text.Length >= 0 && keyboard.text.Length <= 10)
			{
				flag = true;
				Match match = myRex.Match(keyboard.text);
				if (keyboard.text.Length == 0 || match.Success)
				{
					flag2 = true;
				}
			}
			if (flag && flag2)
			{
				inputText = keyboard.text;
				label.Text = inputText;
			}
			else
			{
				inputText = label.Text;
				keyboard.text = inputText;
			}
			if (keyboard.done)
			{
				bool flag3 = false;
				bool flag4 = false;
				if (keyboard.text.Length >= 1 && keyboard.text.Length <= 10)
				{
					flag3 = true;
					Match match2 = myRex.Match(keyboard.text);
					if (match2.Success)
					{
						flag4 = true;
					}
				}
				if (flag3 && flag4)
				{
					Debug.Log(1);
					inputText = keyboard.text;
					Debug.Log(2);
					label.Text = inputText;
					Debug.Log(3);
					cursor.Focus = false;
					Debug.Log(4);
					keyboard = null;
					Debug.Log(4);
				}
				else
				{
					keyboard = null;
					keyboard = TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.ASCIICapable, false, false, false, true, "max 10 letters and numbers");
				}
			}
			if (keyboard != null && (!keyboard.active || keyboard.wasCanceled))
			{
				keyboard.active = true;
				keyboard.text = string.Empty;
			}
		}
		cursor.RefreshPos(label.GetLineWidth());
	}

	public bool ShowKeyboard()
	{
		if (keyboard != null)
		{
			keyboard.active = false;
			keyboard = null;
		}
		if (keyboard == null)
		{
			keyboard = TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.ASCIICapable, false, false, false, true, "max 10 letters and numbers");
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
		cursor.Focus = true;
		return keyboard.active;
	}

	public bool CloseKeyboard()
	{
		if (keyboard != null)
		{
			keyboard.active = false;
		}
		TouchScreenKeyboard.hideInput = false;
		return true;
	}
}
