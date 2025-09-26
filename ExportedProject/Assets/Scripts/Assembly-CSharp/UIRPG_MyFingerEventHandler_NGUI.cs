using UnityEngine;

public class UIRPG_MyFingerEventHandler_NGUI : MonoBehaviour
{
	[SerializeField]
	private float imageWidth;

	[SerializeField]
	private float imageHeight;

	[SerializeField]
	private float zoomSpeed = 5f;

	private float zoomMax = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);

	public void FingerScale(float value)
	{
		if (value < 0f)
		{
			zoomSpeed = 0f - value;
			ZoomOut();
		}
		else
		{
			zoomSpeed = value;
			ZoomIn();
		}
	}

	public void FingerMove(Vector2 delta)
	{
		Vector3 localPosition = base.transform.localPosition;
		Vector3 localScale = base.transform.localScale;
		Vector3 localPosition2 = new Vector3(localPosition.x + delta.x, localPosition.y + delta.y, localPosition.z);
		if (imageWidth * localScale.x + localPosition2.x < (float)GetScreenWidth())
		{
			localPosition2.x = (float)GetScreenWidth() - imageWidth * localScale.x;
		}
		if (localPosition2.x > 0f)
		{
			localPosition2.x = 0f;
		}
		if (imageHeight * localScale.y + localPosition2.y < (float)GetScreenHeight())
		{
			localPosition2.y = (float)GetScreenHeight() - imageHeight * localScale.y;
		}
		if (localPosition2.y > 0f)
		{
			localPosition2.y = 0f;
		}
		base.transform.localPosition = localPosition2;
	}

	public void ZoomOut()
	{
		Vector3 localScale = base.transform.localScale;
		Vector3 localPosition = base.transform.localPosition;
		Vector3 localScale2 = new Vector3(localScale.x * (zoomMax - zoomSpeed) / zoomMax, localScale.y * (zoomMax - zoomSpeed) / zoomMax, localScale.z);
		if (!(localScale2.x * imageWidth + localPosition.x < (float)GetScreenWidth()) && !(localScale2.y * imageHeight + localPosition.y < (float)GetScreenHeight()))
		{
			base.transform.localScale = localScale2;
		}
	}

	public void ZoomIn()
	{
		Vector3 localScale = base.transform.localScale;
		Vector3 localScale2 = new Vector3(zoomMax * localScale.x / (zoomMax - zoomSpeed), zoomMax * localScale.y / (zoomMax - zoomSpeed), localScale.z);
		base.transform.localScale = localScale2;
	}

	public void Start()
	{
		Debug.Log("Screen.width" + Screen.width);
		Debug.Log("Screen.height" + Screen.height);
	}

	public int GetScreenWidth()
	{
		return (int)((float)Screen.width * GetScreenRate());
	}

	public int GetScreenHeight()
	{
		UIRoot uIRoot = Object.FindObjectOfType<UIRoot>();
		return uIRoot.activeHeight;
	}

	public float GetScreenRate()
	{
		UIRoot uIRoot = Object.FindObjectOfType<UIRoot>();
		return (float)uIRoot.activeHeight / (float)Screen.height;
	}
}
