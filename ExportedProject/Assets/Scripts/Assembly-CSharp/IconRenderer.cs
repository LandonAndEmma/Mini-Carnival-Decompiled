using UnityEngine;

public class IconRenderer : MonoBehaviour
{
	public GameObject[] sourceObjs;

	public GameObject[] targetObjs;

	private int resWidth = 128;

	private int resHeight = 128;

	private int index;

	private void Start()
	{
		GameObject[] array = sourceObjs;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		RenderTextureFormat format = RenderTextureFormat.ARGB32;
		if (!SystemInfo.SupportsRenderTextureFormat(format))
		{
			format = RenderTextureFormat.Default;
		}
		RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 32, format);
		base.camera.targetTexture = renderTexture;
		int num = Mathf.Min(sourceObjs.Length, targetObjs.Length);
		for (int j = 0; j < num; j++)
		{
			sourceObjs[j].SetActive(true);
			Texture2D texture2D = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
			base.camera.Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, resWidth, resHeight), 0, 0);
			texture2D.Apply(false);
			targetObjs[j].renderer.material.mainTexture = texture2D;
			sourceObjs[j].SetActive(false);
		}
		base.camera.targetTexture = null;
		RenderTexture.active = null;
		Object.Destroy(renderTexture);
	}
}
