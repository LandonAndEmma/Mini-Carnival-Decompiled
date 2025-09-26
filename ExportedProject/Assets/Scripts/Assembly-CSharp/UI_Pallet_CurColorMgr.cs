using UnityEngine;

public class UI_Pallet_CurColorMgr : MonoBehaviour
{
	[SerializeField]
	private TUIMeshSprite _curColor;

	[SerializeField]
	private Texture2D _tex;

	[SerializeField]
	private int _width;

	[SerializeField]
	private int _height;

	[SerializeField]
	private Color _color;

	private Color[] pixels;

	public Color CurSelColor
	{
		get
		{
			return _color;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RefreshColor(Color c)
	{
		if (_tex == null)
		{
			_tex = new Texture2D(_width, _height);
			pixels = new Color[_width * _height];
		}
		_color = c;
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				pixels[i * _width + j] = c;
			}
		}
		_tex.SetPixels(pixels);
		_tex.Apply();
		_curColor.CustomizeTexture = _tex;
		_curColor.CustomizeRect = new Rect(0f, 0f, _width, _height);
	}
}
