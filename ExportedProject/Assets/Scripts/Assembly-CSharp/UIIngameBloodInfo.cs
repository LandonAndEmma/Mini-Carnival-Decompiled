using System;
using UnityEngine;

[Serializable]
public class UIIngameBloodInfo
{
	[SerializeField]
	private Texture2D _headTex;

	[SerializeField]
	private float _hp;

	[SerializeField]
	private string _name;

	[SerializeField]
	private int _num_kill;

	[SerializeField]
	private int _num_die;

	[SerializeField]
	private int _num_selfTeamKill;

	[SerializeField]
	private int _num_opponentKill;

	private UIIngame_BloodUI _uiInfo;

	public UIIngame_BloodUI UIInfo
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

	public int NumKill
	{
		get
		{
			return _num_kill;
		}
		set
		{
			_num_kill = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public int NumDie
	{
		get
		{
			return _num_die;
		}
		set
		{
			_num_die = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public int NumSelfTeamKill
	{
		get
		{
			return _num_selfTeamKill;
		}
		set
		{
			_num_selfTeamKill = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public int NumOpponentKill
	{
		get
		{
			return _num_opponentKill;
		}
		set
		{
			_num_opponentKill = value;
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

	public UIIngameBloodInfo(Texture2D tex, float hp, string name, int numkill, int numdie)
	{
		_headTex = tex;
		_hp = hp;
		_name = name;
		_num_kill = numkill;
		_num_die = numdie;
	}

	public UIIngameBloodInfo()
	{
		_headTex = null;
		_hp = 1f;
		_name = string.Empty;
		_num_die = 0;
		_num_kill = 0;
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
