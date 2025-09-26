using UnityEngine;

public class TUI_COMA_MoveAndScale : TUIControlImpl
{
	protected struct TouchInfo
	{
		public int fingerId;

		public Vector2 position;
	}

	public const int CommandBegin = 1;

	public const int CommandMove = 2;

	public const int CommandEnd = 3;

	public const int CommandZoomBegin = 4;

	public const int CommandZooming = 5;

	public const int CommandZoomEnd = 6;

	protected TouchInfo[] touchInfo = new TouchInfo[2];

	protected int fingerIndex;

	protected bool zoom;

	public float minX;

	public float minY;

	protected bool move;

	[SerializeField]
	private TUICamera _tuiCamera;

	public override void Reset()
	{
		base.Reset();
		touchInfo[0].fingerId = -1;
		touchInfo[1].fingerId = -1;
		fingerIndex = 0;
		zoom = false;
	}

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

	private void Start()
	{
		touchInfo[0].fingerId = -1;
		touchInfo[1].fingerId = -1;
	}

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				if (touchInfo[0].fingerId != -1 || touchInfo[1].fingerId != -1)
				{
					touchInfo[fingerIndex].fingerId = input.fingerId;
					touchInfo[fingerIndex].position = input.position;
					fingerIndex = ((fingerIndex == 0) ? 1 : 0);
					float magnitude = (touchInfo[0].position - touchInfo[1].position).magnitude;
					PostEvent(this, 4, magnitude, 0f, null);
				}
				else
				{
					touchInfo[fingerIndex].fingerId = input.fingerId;
					touchInfo[fingerIndex].position = input.position;
					fingerIndex = ((fingerIndex == 0) ? 1 : 0);
					PostEvent(this, 1, X2ScreenPixelRatio(input.position), Y2ScreenPixelRatio(input.position), null);
				}
			}
			return false;
		}
		if (input.inputType == TUIInputType.Moved)
		{
			if (!PtInControl(input.position))
			{
				return false;
			}
			int num = -1;
			if (touchInfo[0].fingerId == input.fingerId)
			{
				num = 0;
			}
			else
			{
				if (touchInfo[1].fingerId != input.fingerId)
				{
					return false;
				}
				num = 1;
			}
			if (touchInfo[0].fingerId != -1 && touchInfo[1].fingerId != -1)
			{
				touchInfo[num].position = input.position;
				float magnitude2 = (touchInfo[0].position - touchInfo[1].position).magnitude;
				PostEvent(this, 5, magnitude2, 0f, null);
			}
			else
			{
				float num2 = X2ScreenPixelRatio(input.position) - X2ScreenPixelRatio(touchInfo[num].position);
				float num3 = Y2ScreenPixelRatio(input.position) - Y2ScreenPixelRatio(touchInfo[num].position);
				if (move)
				{
					touchInfo[num].position = input.position;
					PostEvent(this, 2, num2, num3, null);
				}
				else
				{
					float num4 = ((!(num2 >= 0f)) ? (0f - num2) : num2);
					float num5 = ((!(num3 >= 0f)) ? (0f - num3) : num3);
					if (num4 > minX || num5 > minY)
					{
						touchInfo[num].position = input.position;
						move = true;
						PostEvent(this, 2, num2, num3, null);
					}
				}
			}
			return true;
		}
		if (input.inputType == TUIInputType.Ended)
		{
			int num6 = -1;
			if (touchInfo[0].fingerId == input.fingerId)
			{
				num6 = 0;
			}
			else
			{
				if (touchInfo[1].fingerId != input.fingerId)
				{
					return false;
				}
				num6 = 1;
			}
			if (touchInfo[0].fingerId != -1 && touchInfo[1].fingerId != -1)
			{
				touchInfo[num6].fingerId = -1;
				fingerIndex = num6;
				PostEvent(this, 6, 0f, 0f, null);
			}
			else
			{
				touchInfo[num6].fingerId = -1;
				touchInfo[num6].position = Vector2.zero;
				move = false;
				PostEvent(this, 3, X2ScreenPixelRatio(input.position), Y2ScreenPixelRatio(input.position), null);
			}
			return true;
		}
		return false;
	}
}
