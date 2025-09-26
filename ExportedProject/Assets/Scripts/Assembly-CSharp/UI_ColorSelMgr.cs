using UnityEngine;

public class UI_ColorSelMgr : MonoBehaviour
{
	[SerializeField]
	private TUIMeshSprite _curUseColor;

	[SerializeField]
	private TUIMeshSprite[] _selColors;

	private int _curIndex;

	private Color _preColor = new Color(1f, 1f, 1f);

	private Color _curColor = new Color(64f, 191f, 191f);

	public bool _bTest;

	private void Start()
	{
		_selColors[0].color = COMA_PaintBase.Instance.curPaint;
		for (int i = 0; i < COMA_PaintBase.Instance.bakPaint.Length; i++)
		{
			_selColors[i + 1].color = COMA_PaintBase.Instance.bakPaint[i];
		}
		_curUseColor.color = COMA_PaintBase.Instance.curPaint;
		_curColor = _curUseColor.color;
	}

	private void Update()
	{
	}

	public Color GetCurrentColor()
	{
		return _selColors[0].color;
	}

	public void NotifyDraw()
	{
		if (NeedUpdateColorSelArea())
		{
			UpdateColorSelArea();
		}
		_preColor = _curColor;
	}

	public void RefreshColorSelArea(Color c)
	{
		_curColor = c;
		_selColors[0].color = _curColor;
	}

	private bool NeedUpdateColorSelArea()
	{
		bool result = true;
		for (int num = _selColors.Length - 2; num >= 0; num--)
		{
			if (_selColors[num + 1].color == _curColor)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private void UpdateColorSelArea()
	{
		for (int num = _selColors.Length - 2; num >= 0; num--)
		{
			_selColors[num + 1].color = _selColors[num].color;
		}
		COMA_PaintBase.Instance.curPaint = _selColors[0].color;
		for (int i = 0; i < COMA_PaintBase.Instance.bakPaint.Length; i++)
		{
			COMA_PaintBase.Instance.bakPaint[i] = _selColors[i + 1].color;
		}
		COMA_Pref.Instance.Save(true);
	}

	public void HandleEventButton_Color2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Color2-CommandClick");
			_curUseColor.color = _selColors[2].color;
			_selColors[0].color = _selColors[2].color;
		}
	}

	public void HandleEventButton_Color3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Color3-CommandClick");
			_curUseColor.color = _selColors[4].color;
			_selColors[0].color = _selColors[4].color;
		}
	}

	public void HandleEventButton_Color4(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Color4-CommandClick");
			_curUseColor.color = _selColors[6].color;
			_selColors[0].color = _selColors[6].color;
		}
	}

	public void HandleEventButton_Color5(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Color5-CommandClick");
			_curUseColor.color = _selColors[1].color;
			_selColors[0].color = _selColors[1].color;
		}
	}

	public void HandleEventButton_Color6(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Color6-CommandClick");
			_curUseColor.color = _selColors[3].color;
			_selColors[0].color = _selColors[3].color;
		}
	}

	public void HandleEventButton_Color7(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Color7-CommandClick");
			_curUseColor.color = _selColors[5].color;
			_selColors[0].color = _selColors[5].color;
		}
	}
}
