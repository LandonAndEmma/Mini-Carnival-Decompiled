using UnityEngine;

public class UI_Pallet_HSMgr : MonoBehaviour
{
	[SerializeField]
	private UI_Pallet_LMgr _lMgr;

	[SerializeField]
	private UI_Pallet_CurColorMgr _curColorMgr;

	[SerializeField]
	private TUIMeshSprite _hsPic;

	[SerializeField]
	private GameObject _pointer;

	[SerializeField]
	private int _width;

	[SerializeField]
	private int _height;

	[SerializeField]
	private Vector2 _curHS;

	private int nMaxH = 239;

	private int nMaxS = 240;

	private Texture2D hs;

	private Color[] pixels;

	public Vector2 CurHS
	{
		get
		{
			return _curHS;
		}
	}

	public void GenerateHSPic()
	{
		if (pixels == null)
		{
			pixels = new Color[_width * _height];
		}
		if (!(hs == null))
		{
			return;
		}
		hs = new Texture2D(_width, _height);
		int num = _height;
		int num2 = 1;
		while (num >= 1)
		{
			for (int num3 = _width; num3 >= 1; num3--)
			{
				float h = (float)(num3 - 1) / (float)(_width - 1);
				float sl = (float)(num2 - 1) / (float)(_height - 1);
				float l = 0.5f;
				Color color = UI_Utility.HSL2RGB(h, sl, l);
				pixels[(num2 - 1) * _width + (num3 - 1)] = color;
			}
			num--;
			num2++;
		}
		hs.SetPixels(pixels);
		hs.Apply();
		_hsPic.CustomizeTexture = hs;
		_hsPic.CustomizeRect = new Rect(0f, 0f, _width, _height);
		_curColorMgr.RefreshColor(UI_Utility.HSL2RGB(0.5f, 0.5f, 0.5f));
		Debug.Log("GenerateHSPic");
	}

	private void Awake()
	{
		GenerateHSPic();
		_curHS.x = 0.5f;
		_curHS.y = 0.5f;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void InitHS(Color c)
	{
		float h = 0f;
		float s = 0f;
		float l = 0f;
		UI_Utility.RGB2HSL(c, out h, out s, out l);
		RefreshPointerPos(h, s, GetComponent<UI_Pallet_HS_Btn>());
	}

	private void RefreshPointerPos(float fH, float fS, TUIControl control)
	{
		_curHS.x = fH;
		_curHS.y = fS;
		UI_Pallet_HS_Btn uI_Pallet_HS_Btn = (UI_Pallet_HS_Btn)control;
		float x = (fH - 0.5f) * uI_Pallet_HS_Btn.size.x;
		float y = (fS - 0.5f) * uI_Pallet_HS_Btn.size.y;
		Vector3 localPosition = _pointer.transform.localPosition;
		localPosition.x = x;
		localPosition.y = y;
		_pointer.transform.localPosition = localPosition;
		_lMgr.GenerateLPic();
		_curColorMgr.RefreshColor(UI_Utility.HSL2RGB(fH, fS, _lMgr.CurL));
	}

	public void HandleEventButton_HS(TUIControl control, int eventType, float wparam, float lparam, object data)
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
