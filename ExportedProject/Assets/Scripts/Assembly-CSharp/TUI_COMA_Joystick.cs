using UnityEngine;

public class TUI_COMA_Joystick : TUIButton
{
	public const int CommandDown = 1;

	public const int CommandMove = 2;

	public const int CommandUp = 3;

	public GameObject m_JoyStickObj;

	public float m_MinDistance;

	public float m_MaxDistance;

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
				Vector2 vector = DoMove(input.position);
				PostEvent(this, 1, vector.x, vector.y, null);
				return true;
			}
		}
		else if (input.fingerId == m_iFingerId)
		{
			if (input.inputType == TUIInputType.Ended)
			{
				m_bPressed = false;
				m_iFingerId = -1;
				if (m_JoyStickObj != null)
				{
					m_JoyStickObj.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, m_JoyStickObj.transform.position.z);
				}
				PostEvent(this, 3, 0f, 0f, null);
			}
			else if (input.inputType == TUIInputType.Moved)
			{
				Vector2 vector2 = DoMove(input.position);
				PostEvent(this, 2, vector2.x, vector2.y, null);
			}
		}
		return false;
	}

	private Vector2 DoMove(Vector2 position)
	{
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y);
		Vector2 vector2 = position - vector;
		float magnitude = vector2.magnitude;
		if (magnitude < 0.01f)
		{
			return Vector2.zero;
		}
		float value = (magnitude - m_MinDistance) / (m_MaxDistance - m_MinDistance);
		value = Mathf.Clamp(value, 0f, 1f);
		Vector2 vector3 = value * vector2 / magnitude;
		Vector2 vector4 = vector + vector3 * m_MaxDistance;
		base.Show();
		if (m_JoyStickObj != null)
		{
			m_JoyStickObj.transform.position = new Vector3(vector4.x, vector4.y, m_JoyStickObj.transform.position.z);
		}
		return vector3;
	}
}
