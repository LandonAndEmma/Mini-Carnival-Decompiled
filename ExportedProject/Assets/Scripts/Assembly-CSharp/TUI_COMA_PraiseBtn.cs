using UnityEngine;

public class TUI_COMA_PraiseBtn : TUIButton
{
	public const int CommandDown = 1;

	public const int CommandUp = 2;

	public const int CommandClick = 3;

	public const string DownMethod = "OnDown";

	public const string UpMethod = "OnUp";

	public const string ClickMethod = "OnClick";

	[SerializeField]
	private TUICamera _tuiCamera;

	private float X2ScreenPixelRatio(Vector2 v2)
	{
		Vector3 position = new Vector3(v2.x, v2.y, _tuiCamera.camera.nearClipPlane);
		float x = _tuiCamera.camera.WorldToScreenPoint(position).x;
		x /= (float)Screen.width;
		return Mathf.Clamp01(x);
	}

	private float Y2ScreenPixelRatio(Vector2 v2)
	{
		Vector3 position = new Vector3(v2.x, v2.y, _tuiCamera.camera.nearClipPlane);
		float y = _tuiCamera.camera.WorldToScreenPoint(position).y;
		y /= (float)Screen.height;
		return Mathf.Clamp01(y);
	}

	private new void Start()
	{
	}

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				m_bPressed = true;
				m_iFingerId = input.fingerId;
				Show();
				PostEvent(this, 1, 0f, 0f, null);
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
					PostEvent(this, 3, X2ScreenPixelRatio(input.position), Y2ScreenPixelRatio(input.position), null);
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
}
