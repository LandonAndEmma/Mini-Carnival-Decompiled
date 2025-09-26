using UnityEngine;

public class UI_NewcomersWizardMask : MonoBehaviour
{
	public delegate void FullScreenBtnProcess();

	[SerializeField]
	private TUIMeshSprite _icon;

	private Texture2D _iconTexs;

	[SerializeField]
	private GameObject _handIcon;

	[SerializeField]
	private GameObject[] _lightIcons;

	[SerializeField]
	private GameObject _singleBtn;

	[SerializeField]
	private GameObject _singleBtnSel;

	[SerializeField]
	private float _fAlpha = 0.5f;

	[SerializeField]
	private TUIMeshSprite _avatarTex;

	private int width = 1136;

	private int height = 768;

	private bool bResetIconPos;

	[SerializeField]
	private UIMarket_Container _marketContainer;

	[SerializeField]
	private GameObject[] _extraTips;

	public event FullScreenBtnProcess ProceFullScreenBtn;

	public event FullScreenBtnProcess ProceSingleBtn;

	private void Awake()
	{
		if (_handIcon != null)
		{
			_handIcon.SetActive(false);
		}
		GameObject[] lightIcons = _lightIcons;
		foreach (GameObject gameObject in lightIcons)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		if (_avatarTex != null)
		{
			_avatarTex.gameObject.SetActive(false);
		}
		CreateTex();
	}

	private void Start()
	{
		if (_singleBtnSel != null)
		{
			_singleBtnSel.SetActive(false);
		}
		_icon.transform.position = new Vector3(0f, 0f, -200f);
		TUIBlock component = GetComponent<TUIBlock>();
		if (component != null)
		{
			component.size = new Vector2(1000f, 1000f);
		}
	}

	private void Update()
	{
		if (!bResetIconPos)
		{
			_icon.transform.position = new Vector3(0f, 0f, -200f);
			bResetIconPos = true;
		}
	}

	public void InitLightIcons(int[] indexs, int len)
	{
		GameObject[] array = new GameObject[_lightIcons.Length];
		for (int i = 0; i < _lightIcons.Length; i++)
		{
			array[i] = _lightIcons[i];
			_lightIcons[i] = null;
		}
		for (int j = 0; j < len; j++)
		{
			if (indexs[j] < _lightIcons.Length)
			{
				_lightIcons[indexs[j]] = array[indexs[j]];
			}
		}
	}

	private void CreateTex()
	{
		_iconTexs = null;
		_icon.CustomizeTexture = _iconTexs;
		_icon.UseCustomize = true;
		_icon.CustomizeRect = new Rect(0f, 0f, width, height);
		_icon.color = new Color(1f, 1f, 1f, _fAlpha);
	}

	public void AddNoMaskRect(Rect[] rts)
	{
		for (int i = 0; i < rts.Length; i++)
		{
			Rect rect = rts[i];
			for (int j = (int)rect.x; j < (int)rect.x + (int)rect.width; j++)
			{
				for (int k = (int)rect.y; k < (int)rect.y + (int)rect.height; k++)
				{
					_iconTexs.SetPixel(j, k, new Color(1f, 1f, 1f, 0f));
				}
			}
			_iconTexs.Apply(false);
			Debug.Log("rt.x=" + rect.x + "   rt.y=" + rect.y + "  width=" + rect.width + "  height=" + rect.height);
		}
	}

	public void ActiveExtraEffect()
	{
		if (_handIcon != null)
		{
			_handIcon.SetActive(true);
		}
		GameObject[] lightIcons = _lightIcons;
		foreach (GameObject gameObject in lightIcons)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
		if (_avatarTex != null)
		{
			_avatarTex.gameObject.SetActive(true);
			if (_marketContainer != null)
			{
				UIMarket_AvatarShopData uIMarket_AvatarShopData = (UIMarket_AvatarShopData)_marketContainer.LstBoxDatas[3];
				_avatarTex.CustomizeTexture = uIMarket_AvatarShopData.AvatarIcon;
			}
		}
	}

	public void ResetMask()
	{
		GameObject[] lightIcons = _lightIcons;
		foreach (GameObject gameObject in lightIcons)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void SingleBtnSeleced()
	{
		if (_singleBtnSel != null)
		{
			_singleBtnSel.SetActive(true);
		}
	}

	public void HandleEventButtonFullScreenBtn(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("HandleEventButtonFullScreenBtn-CommandClick");
			if (this.ProceFullScreenBtn != null)
			{
				this.ProceFullScreenBtn();
			}
		}
	}

	public void HandleEventButtonSingleBtn(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("HandleEventButtonSingleBtn-CommandClick");
			if (this.ProceSingleBtn != null)
			{
				this.ProceSingleBtn();
			}
		}
	}
}
