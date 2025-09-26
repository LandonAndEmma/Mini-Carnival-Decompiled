using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_3DModeToTexture : MonoBehaviour
{
	[Serializable]
	public class SourceObjs3D
	{
		public GameObject _srcObj;

		[SerializeField]
		private bool _bEnableRender;

		private UI_3DModeToTexture _3dModeToTex;

		public UI_3DModeToTexture UI_ModeToTex
		{
			set
			{
				_3dModeToTex = value;
			}
		}

		public bool EnableRender
		{
			get
			{
				return _bEnableRender;
			}
			set
			{
				_bEnableRender = value;
				if (_bEnableRender && _3dModeToTex._dctDelayRender.ContainsKey(_srcObj))
				{
					int nIndex = _3dModeToTex._dctDelayRender[_srcObj];
					_3dModeToTex.RenderToTex(nIndex);
					_3dModeToTex._dctDelayRender.Remove(_srcObj);
				}
			}
		}

		public GameObject SrcObj
		{
			get
			{
				return _srcObj;
			}
			set
			{
				_srcObj = value;
			}
		}
	}

	public class SDelayAssign
	{
		public int _id;

		public DelayAssignment _delay;

		public SDelayAssign(int id, DelayAssignment delay)
		{
			_id = id;
			_delay = delay;
		}
	}

	public delegate void DelayAssignment(Texture2D tex);

	private static UI_3DModeToTexture _instance;

	public SourceObjs3D[] sourceObjs;

	[SerializeField]
	private Texture2D[] targetTexs;

	[SerializeField]
	private int resWidth = 1;

	[SerializeField]
	private int resHeight = 1;

	[SerializeField]
	private int maxNumPerFrame = 10;

	private int objNumber;

	private RenderTexture rt;

	private int curFrameIndex;

	private bool bRenderFinished;

	public Dictionary<GameObject, int> _dctDelayRender = new Dictionary<GameObject, int>();

	private List<SDelayAssign> _lstDelay = new List<SDelayAssign>();

	public bool bEnableTest = true;

	public bool bTest;

	public int nId;

	public static UI_3DModeToTexture Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		if (base.name == "Camera2DMgr")
		{
			_instance = this;
		}
	}

	private void OnDisable()
	{
		if (base.name == "Camera2DMgr")
		{
			_instance = null;
		}
	}

	private void Awake()
	{
		resWidth = COMA_TexBase.Instance.width;
		resHeight = COMA_TexBase.Instance.height;
		SourceObjs3D[] array = sourceObjs;
		foreach (SourceObjs3D sourceObjs3D in array)
		{
			sourceObjs3D.UI_ModeToTex = this;
		}
		objNumber = sourceObjs.Length;
		targetTexs = new Texture2D[objNumber];
		SourceObjs3D[] array2 = sourceObjs;
		foreach (SourceObjs3D sourceObjs3D2 in array2)
		{
			sourceObjs3D2.SrcObj.SetActive(false);
		}
		RenderTextureFormat format = RenderTextureFormat.ARGB32;
		if (!SystemInfo.SupportsRenderTextureFormat(format))
		{
			format = RenderTextureFormat.Default;
		}
		rt = new RenderTexture(resWidth, resHeight, 32, format);
		base.camera.targetTexture = rt;
		curFrameIndex = 0;
		bRenderFinished = false;
		RenderToTex();
	}

	private void OnDestroy()
	{
		base.camera.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(rt);
	}

	private void Update()
	{
		if (bEnableTest)
		{
			TestEnableRender();
		}
		if (bRenderFinished && _dctDelayRender.Count == 0 && _lstDelay.Count == 0)
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
			return;
		}
		RenderToTex();
		if (_lstDelay.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _lstDelay.Count; i++)
		{
			SDelayAssign sDelayAssign = _lstDelay[i];
			if (targetTexs[sDelayAssign._id] != null)
			{
				sDelayAssign._delay(targetTexs[sDelayAssign._id]);
				_lstDelay.RemoveAt(i);
				i--;
				i = ((i >= 0) ? i : 0);
			}
		}
	}

	public void RenderToTex(int nIndex)
	{
		sourceObjs[nIndex].SrcObj.SetActive(true);
		base.camera.Render();
		RenderTexture.active = rt;
		Texture2D texture2D = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
		texture2D.filterMode = FilterMode.Point;
		texture2D.ReadPixels(new Rect(0f, 0f, resWidth, resHeight), 0, 0);
		texture2D.Apply(false);
		targetTexs[nIndex] = texture2D;
		sourceObjs[nIndex].SrcObj.SetActive(false);
	}

	private void RenderToTex()
	{
		int num = Mathf.CeilToInt((float)objNumber / (float)maxNumPerFrame);
		if (curFrameIndex < num)
		{
			int num2 = Mathf.Min(objNumber - curFrameIndex * maxNumPerFrame, maxNumPerFrame);
			for (int i = 0; i < num2; i++)
			{
				int num3 = i + curFrameIndex * maxNumPerFrame;
				if (sourceObjs[num3].EnableRender)
				{
					RenderToTex(num3);
				}
				else
				{
					_dctDelayRender.Add(sourceObjs[num3].SrcObj, num3);
				}
			}
			curFrameIndex++;
			if (curFrameIndex < num)
			{
				return;
			}
		}
		bRenderFinished = true;
	}

	public Texture2D GetTexById(int nId, DelayAssignment de)
	{
		if (targetTexs[nId] == null)
		{
			SDelayAssign item = new SDelayAssign(nId, de);
			_lstDelay.Add(item);
		}
		return targetTexs[nId];
	}

	public void Set3DModelEnableRender(int nId)
	{
		if (nId >= 0 && nId < sourceObjs.Length)
		{
			sourceObjs[nId].EnableRender = true;
		}
	}

	private void TestEnableRender()
	{
		if (bTest)
		{
			Set3DModelEnableRender(nId);
			bTest = false;
		}
	}
}
