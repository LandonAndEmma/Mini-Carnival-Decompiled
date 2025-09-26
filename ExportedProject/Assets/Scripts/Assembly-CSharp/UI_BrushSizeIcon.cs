using UnityEngine;

public class UI_BrushSizeIcon : MonoBehaviour
{
	[SerializeField]
	private TUIMeshSprite[] _brushsizeIcons;

	[SerializeField]
	private UI_Pallet _pallet;

	[SerializeField]
	private UI_ColorSelMgr _colorSelMgr;

	private void Awake()
	{
		if (_pallet != null)
		{
			_pallet.ProceYesHandler += RefreshColor;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RefreshColor(Color c)
	{
		TUIMeshSprite[] brushsizeIcons = _brushsizeIcons;
		foreach (TUIMeshSprite tUIMeshSprite in brushsizeIcons)
		{
			tUIMeshSprite.color = c;
		}
		_colorSelMgr.RefreshColorSelArea(c);
		_colorSelMgr.NotifyDraw();
	}
}
