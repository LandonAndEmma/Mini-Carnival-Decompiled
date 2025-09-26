using UnityEngine;

public class UI_ZoomSlider : TUIButton
{
	public const float SliderLen = 140f;

	public const int CommandDown = 1;

	public const int CommandUp = 2;

	public const int CommandClick = 3;

	public const int CommandMove = 4;

	public const string DownMethod = "OnDown";

	public const string UpMethod = "OnUp";

	public const string ClickMethod = "OnClick";

	public const string MoveMethod = "OnMove";

	private float PointX2Ratio(float x)
	{
		float num = x - base.transform.position.x + 70f;
		return num / 140f;
	}

	private float PointY2S(float y)
	{
		float num = y - base.transform.position.y + 70f;
		return num / 140f;
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
				PostEvent(this, 1, PointX2Ratio(input.position.x), PointY2S(input.position.y), null);
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
						PostEvent(this, 1, PointX2Ratio(input.position.x), PointY2S(input.position.y), null);
					}
					PostEvent(this, 4, PointX2Ratio(input.position.x), PointY2S(input.position.y), null);
				}
				else if (m_bPressed)
				{
					m_bPressed = false;
					Show();
					PostEvent(this, 2, PointX2Ratio(input.position.x), 0f, null);
				}
			}
			else if (input.inputType == TUIInputType.Ended)
			{
				m_bPressed = false;
				m_iFingerId = -1;
				if (PtInControl(input.position))
				{
					Show();
					PostEvent(this, 2, PointX2Ratio(input.position.x), PointY2S(input.position.y), null);
					PostEvent(this, 3, PointX2Ratio(input.position.x), PointY2S(input.position.y), null);
				}
				else
				{
					Show();
					PostEvent(this, 2, PointX2Ratio(input.position.x), 0f, null);
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
		case 4:
			text = "OnMove";
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
