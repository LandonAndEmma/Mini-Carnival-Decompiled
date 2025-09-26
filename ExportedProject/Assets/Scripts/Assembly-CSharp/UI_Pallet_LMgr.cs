using UnityEngine;

public class UI_Pallet_LMgr : MonoBehaviour
{
	[SerializeField]
	private UI_Pallet_HSMgr _hsMgr;

	[SerializeField]
	private UI_Pallet_CurColorMgr _curColorMgr;

	[SerializeField]
	private TUIMeshSprite _lPic;

	[SerializeField]
	private GameObject _pointer;

	[SerializeField]
	private int _width;

	[SerializeField]
	private int _height;

	[SerializeField]
	private float _fCurL = 0.5f;

	private int nMaxL = 240;

	private Texture2D _l;

	private Color[] pixels;

	public float CurL
	{
		get
		{
			return _fCurL;
		}
		set
		{
			_fCurL = value;
		}
	}

	public void GenerateLPic()
	{
		_l = new Texture2D(_width, _height);
		if (pixels == null)
		{
			pixels = new Color[_width * _height];
		}
		float x = _hsMgr.CurHS.x;
		float y = _hsMgr.CurHS.y;
		for (int i = 0; i < _height; i++)
		{
			float l = (float)i / (float)(_height - 1);
			for (int j = 0; j < _width; j++)
			{
				Color color = UI_Utility.HSL2RGB(x, y, l);
				pixels[i * _width + j] = color;
			}
		}
		_l.SetPixels(pixels);
		_l.Apply();
		_lPic.CustomizeTexture = _l;
		_lPic.CustomizeRect = new Rect(0f, 0f, _width, _height);
	}

	private void Awake()
	{
		GenerateLPic();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void InitL(Color c)
	{
		float h = 0f;
		float s = 0f;
		float l = 0f;
		UI_Utility.RGB2HSL(c, out h, out s, out l);
		RefreshPointerPos(0f, l, GetComponent<UI_Pallet_L_Btn>());
	}

	private void RefreshPointerPos(float fX, float fY, TUIControl control)
	{
		CurL = fY;
		UI_Pallet_L_Btn uI_Pallet_L_Btn = (UI_Pallet_L_Btn)control;
		float y = (fY - 0.5f) * uI_Pallet_L_Btn.size.y;
		Vector3 localPosition = _pointer.transform.localPosition;
		localPosition.y = y;
		_pointer.transform.localPosition = localPosition;
		_curColorMgr.RefreshColor(UI_Utility.HSL2RGB(_hsMgr.CurHS.x, _hsMgr.CurHS.y, CurL));
	}

	public void HandleEventButton_L(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			RefreshPointerPos(wparam, lparam, control);
			break;
		case 4:
			RefreshPointerPos(wparam, lparam, control);
			break;
		}
	}
}
