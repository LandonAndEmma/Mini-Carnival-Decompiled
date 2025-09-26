using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UIRPG_GemCompoundInfo : UIEntity
{
	[SerializeField]
	private List<UISprite> _spriteLst = new List<UISprite>();

	private List<int> _spriteOriDepth = new List<int>();

	[SerializeField]
	private UIRPG_GemCompoundBox _gemCombinBox;

	private int _gemLevel;

	public int GemLevel
	{
		get
		{
			return _gemLevel;
		}
		set
		{
			_gemLevel = value;
		}
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		_spriteOriDepth.Clear();
		for (int i = 0; i < _spriteLst.Count; i++)
		{
			_spriteOriDepth.Add(_spriteLst[i].depth);
		}
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	private void Start()
	{
	}

	public void OnClick()
	{
		_gemCombinBox.MGR.CurLevel = _gemLevel;
		_gemCombinBox.MGR.PopupObj.SetActive(true);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		switch ((int)msg._pExtraInfo)
		{
		case 1:
		{
			for (int j = 0; j < _spriteLst.Count; j++)
			{
				_spriteLst[j].depth += 600;
				_spriteLst[j].enabled = true;
			}
			break;
		}
		case 2:
		{
			Debug.Log(" End NG  0!!");
			for (int i = 0; i < _spriteLst.Count; i++)
			{
				_spriteLst[i].depth = _spriteOriDepth[i];
				if (i > 0)
				{
					_spriteLst[i].enabled = false;
				}
			}
			break;
		}
		}
		return true;
	}
}
