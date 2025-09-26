using System;
using UnityEngine;

[Serializable]
public class UILabyrinthPlayerInfo
{
	[SerializeField]
	private float _fHp;

	[SerializeField]
	private int _nGoldNum;

	[SerializeField]
	private Texture2D _tex;

	private UIInGame_LabyrinthUIMgr _mgr;

	private UIInGame_LabyrinthPlayerInfo _uiInfo;

	public UIInGame_LabyrinthUIMgr MGR
	{
		get
		{
			return _mgr;
		}
		set
		{
			_mgr = value;
		}
	}

	public UIInGame_LabyrinthPlayerInfo UIInfo
	{
		get
		{
			return _uiInfo;
		}
		set
		{
			_uiInfo = value;
		}
	}

	public float HP
	{
		get
		{
			return _fHp;
		}
		set
		{
			_fHp = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public int GoldNum
	{
		get
		{
			return _nGoldNum;
		}
		set
		{
			_nGoldNum = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public Texture2D Tex2D
	{
		get
		{
			return _tex;
		}
		set
		{
			_tex = value;
		}
	}

	public UILabyrinthPlayerInfo(int goldNum, float hp)
	{
		_nGoldNum = goldNum;
		_fHp = hp;
	}

	public UILabyrinthPlayerInfo()
	{
		_nGoldNum = 0;
		_fHp = 1f;
	}

	public void DelayAssignment(Texture2D tex)
	{
		Tex2D = tex;
		if (MGR != null && _tex != null)
		{
			MGR.DelayConcentData(this, _tex);
		}
	}
}
