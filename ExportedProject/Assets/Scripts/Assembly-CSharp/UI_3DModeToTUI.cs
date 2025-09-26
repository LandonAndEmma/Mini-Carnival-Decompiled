using UnityEngine;

public class UI_3DModeToTUI : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	public Vector2 TextureSize = new Vector2(256f, 256f);

	public RenderTexture rt;

	private void Awake()
	{
		_camera = base.gameObject.GetComponent<Camera>();
		if (!(_camera == null))
		{
			RenderTextureFormat format = RenderTextureFormat.ARGB32;
			if (!SystemInfo.SupportsRenderTextureFormat(format))
			{
				format = RenderTextureFormat.Default;
			}
			if (rt == null)
			{
				rt = new RenderTexture(Mathf.FloorToInt(TextureSize.x), Mathf.FloorToInt(TextureSize.y), 32, format);
			}
			_camera.targetTexture = rt;
			_camera.aspect = rt.width / rt.height;
		}
	}

	private void Update()
	{
	}
}
