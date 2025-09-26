using System;
using UnityEngine;

[Serializable]
public class UIIngameDefendInfo
{
	[SerializeField]
	private Texture2D _headTex;

	[SerializeField]
	private float _hp;

	[SerializeField]
	private string _name;

	[SerializeField]
	private int _num;

	private UIIngame_DefendUI _uiInfo;

	public UIIngame_DefendUI UIInfo
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

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public int Num
	{
		get
		{
			return _num;
		}
		set
		{
			_num = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public float HP
	{
		get
		{
			return _hp;
		}
		set
		{
			_hp = value;
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
			return _headTex;
		}
		set
		{
			_headTex = value;
		}
	}

	public UIIngameDefendInfo(Texture2D tex, float hp, string name, int num)
	{
		_headTex = tex;
		_hp = hp;
		_name = name;
		_num = num;
	}

	public UIIngameDefendInfo()
	{
		_headTex = null;
		_hp = 1f;
		_name = string.Empty;
		_num = 0;
	}

	public void DelayAssignment(Texture2D tex)
	{
		Tex2D = tex;
		if (UIInfo != null && Tex2D != null)
		{
			UIInfo._info.Tex2D = tex;
			UIInfo.RefreshHeadIconUI();
		}
	}
}
