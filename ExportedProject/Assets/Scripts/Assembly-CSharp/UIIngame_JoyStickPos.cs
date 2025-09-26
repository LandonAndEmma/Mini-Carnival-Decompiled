using UnityEngine;

public class UIIngame_JoyStickPos : TUIButton
{
	public const int CommandDown = 1;

	public const int CommandUp = 2;

	public const int CommandClick = 3;

	public const string DownMethod = "OnDown";

	public const string UpMethod = "OnUp";

	public const string ClickMethod = "OnClick";

	[SerializeField]
	private Transform _trans;

	[SerializeField]
	private Transform _leftMost;

	[SerializeField]
	private TUIButton _btn;

	public void Awake()
	{
		if (_leftMost != null)
		{
			_leftMost.localPosition = new Vector3(43f, 43f, 0f);
		}
	}

	public override bool HandleInput(TUIInput input)
	{
		if (m_bDisable)
		{
			return false;
		}
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				m_bPressed = true;
				m_iFingerId = input.fingerId;
				Show();
				Vector3 position = _trans.position;
				Debug.Log("_trans.position=" + _trans.position);
				position.x = input.position.x;
				position.y = input.position.y;
				if (_leftMost != null)
				{
					if (position.x < _leftMost.position.x)
					{
						position.x = _leftMost.position.x;
					}
					if (position.y < _leftMost.position.y)
					{
						position.y = _leftMost.position.y;
					}
				}
				_trans.position = position;
				Vector2 position2 = input.position;
				position2.x += 0.5f;
				position2.y += 0.5f;
				input.position = position2;
				_btn.HandleInput(input);
				return true;
			}
			return false;
		}
		if (input.fingerId == m_iFingerId)
		{
			if (input.inputType == TUIInputType.Moved)
			{
				if (PtInControl(input.position))
				{
					if (!m_bPressed)
					{
						m_bPressed = true;
						Show();
						PostEvent(this, 1, 0f, 0f, null);
					}
				}
				else if (m_bPressed)
				{
					m_bPressed = false;
					Show();
					PostEvent(this, 2, 0f, 0f, null);
				}
			}
			else if (input.inputType == TUIInputType.Ended)
			{
				m_bPressed = false;
				m_iFingerId = -1;
				if (PtInControl(input.position))
				{
					Show();
					PostEvent(this, 2, 0f, 0f, null);
					PostEvent(this, 3, 0f, 0f, null);
				}
				else
				{
					Show();
					PostEvent(this, 2, 0f, 0f, null);
				}
			}
			return true;
		}
		return false;
	}

	public override void PostEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		string text;
		switch (eventType)
		{
		case 2:
			text = "OnUp";
			break;
		case 1:
			text = "OnDown";
			break;
		case 3:
			text = "OnClick";
			break;
		default:
			text = null;
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			PostMessage(text, null, SendMessageOptions.DontRequireReceiver);
		}
		base.PostEvent(control, eventType, wparam, lparam, data);
	}
}
