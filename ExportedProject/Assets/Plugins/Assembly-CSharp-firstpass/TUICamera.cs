using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class TUICamera : MonoBehaviour
{
	public bool lock960x640;

	public Rect m_viewRect;

	private int layer;

	private int depth;

	private float width;

	private float height;

	public void Initialize(int layer, int depth)
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		bool flag = TUI.IsRetina();
		bool flag2 = TUI.IsDoubleHD();
		base.camera.transform.localPosition = new Vector3(1f / ((!flag) ? 2f : 4f) / (float)((!flag2) ? 1 : 2), -1f / ((!flag) ? 2f : 4f) / (float)((!flag2) ? 1 : 2), 0f);
		base.camera.nearClipPlane = -428f;
		base.camera.farClipPlane = 128f;
		base.camera.orthographic = true;
		base.camera.depth = depth;
		base.camera.cullingMask = 1 << layer;
		base.camera.clearFlags = CameraClearFlags.Depth;
		this.layer = layer;
		this.depth = depth;
		width = Screen.width;
		height = Screen.height;
		m_viewRect = new Rect(0f, 0f, Screen.width, Screen.height);
		if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
		{
			m_viewRect = new Rect(0f, 0f, Screen.height, Screen.width);
		}
		if (lock960x640)
		{
			if (Screen.width >= 960 && Screen.height >= 640)
			{
				float left = ((float)Screen.width - 960f) / 2f;
				float top = ((float)Screen.height - 640f) / 2f;
				m_viewRect = new Rect(left, top, 960f, 640f);
			}
			else if (Screen.width >= 640 && Screen.height >= 960)
			{
				float left2 = ((float)Screen.width - 640f) / 2f;
				float top2 = ((float)Screen.height - 960f) / 2f;
				m_viewRect = new Rect(left2, top2, 640f, 960f);
			}
		}
		base.camera.pixelRect = m_viewRect;
		base.camera.aspect = m_viewRect.width / m_viewRect.height;
		float num = Mathf.Max(Screen.width, Screen.height);
		float num2 = Mathf.Min(Screen.width, Screen.height);
		if (num / num2 > 1.5f)
		{
			base.camera.orthographicSize = 160f;
		}
		else
		{
			base.camera.orthographicSize = 170f;
		}
	}

	private void Update()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		if (num != width || num2 != height)
		{
			Initialize(layer, depth);
		}
	}
}
