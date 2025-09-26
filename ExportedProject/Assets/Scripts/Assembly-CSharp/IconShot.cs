using System;
using System.Collections.Generic;
using UnityEngine;

public class IconShot : MonoBehaviour
{
	public class IconInfo
	{
		public GameObject _tarObj;

		public bool _bNeedDestroy;

		public int _width;

		public int _height;

		public Action<Texture2D> _renderOver;

		public IconInfo(GameObject tarObj, bool bNeedDestroy, int width, int height, Action<Texture2D> RenderOver)
		{
			_tarObj = tarObj;
			_bNeedDestroy = bNeedDestroy;
			_width = width;
			_height = height;
			_renderOver = RenderOver;
		}
	}

	private static IconShot _instance;

	private int resWidth = 1;

	private int resHeight = 1;

	public Transform tarLoc;

	private Queue<IconInfo> renderQueue = new Queue<IconInfo>();

	public static IconShot Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		resWidth = COMA_TexBase.Instance.width;
		resHeight = COMA_TexBase.Instance.height;
	}

	private void Start()
	{
		base.camera.enabled = false;
	}

	public Texture2D GetIconPic(GameObject tarObj)
	{
		return GetIconPic(tarObj, false);
	}

	public Texture2D GetIconPic(GameObject tarObj, bool bNeedDestroy)
	{
		return GetIconPic(tarObj, bNeedDestroy, resWidth, resHeight);
	}

	public Texture2D GetIconPic(GameObject tarObj, bool bNeedDestroy, int width, int height)
	{
		base.camera.enabled = true;
		if (null == tarObj)
		{
			Debug.LogError("GetIconPic-tarObj-Is NULL!");
			return null;
		}
		if (tarObj.name.StartsWith("HT08"))
		{
			Transform transform = base.transform.FindChild("HT08");
			tarObj.transform.position = transform.position;
			tarObj.transform.rotation = transform.rotation;
		}
		else if (tarObj.name.StartsWith("HT51") || tarObj.name.StartsWith("HT53"))
		{
			Transform transform2 = base.transform.FindChild("HT51");
			tarObj.transform.position = transform2.position;
			tarObj.transform.rotation = transform2.rotation;
		}
		else if (tarObj.name.StartsWith("HT52") || tarObj.name.StartsWith("HT54"))
		{
			Transform transform3 = base.transform.FindChild("HT52");
			tarObj.transform.position = transform3.position;
			tarObj.transform.rotation = transform3.rotation;
		}
		else if (tarObj.name.StartsWith("CB02") || tarObj.name.StartsWith("CB01"))
		{
			Transform transform4 = base.transform.FindChild("CB");
			tarObj.transform.position = transform4.position;
			tarObj.transform.rotation = transform4.rotation;
		}
		else if (tarObj.name.StartsWith("All") || tarObj.name.StartsWith("Body01"))
		{
			Transform transform5 = base.transform.FindChild("Body01");
			tarObj.transform.position = transform5.position;
			tarObj.transform.rotation = transform5.rotation;
		}
		else if (tarObj.name.StartsWith("Head01"))
		{
			Transform transform6 = base.transform.FindChild("Head01");
			tarObj.transform.position = transform6.position;
			tarObj.transform.rotation = transform6.rotation;
		}
		else if (tarObj.name.StartsWith("Leg01"))
		{
			Transform transform7 = base.transform.FindChild("Leg01");
			tarObj.transform.position = transform7.position;
			tarObj.transform.rotation = transform7.rotation;
		}
		else
		{
			tarObj.transform.position = tarLoc.position;
			tarObj.transform.rotation = tarLoc.rotation;
		}
		RenderTextureFormat format = RenderTextureFormat.ARGB32;
		if (!SystemInfo.SupportsRenderTextureFormat(format))
		{
			format = RenderTextureFormat.Default;
		}
		RenderTexture renderTexture = (RenderTexture.active = new RenderTexture(width, height, 32, format));
		base.camera.targetTexture = renderTexture;
		base.camera.Render();
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		texture2D.Apply(false);
		base.camera.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture);
		base.camera.enabled = false;
		if (bNeedDestroy)
		{
			UnityEngine.Object.DestroyObject(tarObj);
		}
		return texture2D;
	}

	public void ClearRenderQueue()
	{
		renderQueue.Clear();
	}

	private void Update()
	{
		if (renderQueue.Count > 0)
		{
			IconInfo iconInfo = renderQueue.Dequeue();
			Texture2D iconPic = GetIconPic(iconInfo._tarObj, iconInfo._bNeedDestroy, iconInfo._width, iconInfo._height);
			iconInfo._renderOver(iconPic);
		}
	}

	public void GetIconPic(GameObject tarObj, Action<Texture2D> RenderOver)
	{
		GetIconPic(tarObj, false, resWidth, resHeight, RenderOver);
	}

	public void GetIconPic(GameObject tarObj, bool bNeedDestroy, Action<Texture2D> RenderOver)
	{
		GetIconPic(tarObj, bNeedDestroy, resWidth, resHeight, RenderOver);
	}

	public void GetIconPic(GameObject tarObj, bool bNeedDestroy, int width, int height, Action<Texture2D> RenderOver)
	{
		renderQueue.Enqueue(new IconInfo(tarObj, bNeedDestroy, width, height, RenderOver));
	}
}
