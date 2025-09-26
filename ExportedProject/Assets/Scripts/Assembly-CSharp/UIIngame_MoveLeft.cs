using UnityEngine;

public class UIIngame_MoveLeft : TUIButton
{
	public const int CommandDown = 1;

	public const int CommandMove = 2;

	public const int CommandUp = 3;

	[SerializeField]
	private TUIMeshSprite _moveSprite;

	public GameObject m_JoyStickObj;

	public float m_MinDistance;

	public float m_MaxDistance;

	private void Awake()
	{
		_moveSprite.gameObject.renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
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
				_moveSprite.gameObject.renderer.material.color = new Color(1f, 1f, 1f, 1f);
				m_bPressed = true;
				m_iFingerId = input.fingerId;
				PostEvent(this, 1, 0f, 0f, null);
				return true;
			}
		}
		else if (input.fingerId == m_iFingerId)
		{
			if (input.inputType == TUIInputType.Ended)
			{
				_moveSprite.gameObject.renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
				m_bPressed = false;
				m_iFingerId = -1;
				PostEvent(this, 3, 0f, 0f, null);
			}
			return true;
		}
		return false;
	}
}
