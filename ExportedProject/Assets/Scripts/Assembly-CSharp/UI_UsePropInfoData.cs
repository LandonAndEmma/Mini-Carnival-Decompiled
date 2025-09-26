using UnityEngine;

public class UI_UsePropInfoData
{
	private string _strAName;

	private string _strBName;

	private Texture2D _propTxt;

	public string AName
	{
		get
		{
			return _strAName;
		}
	}

	public string BName
	{
		get
		{
			return _strBName;
		}
	}

	public Texture2D PropTexture
	{
		get
		{
			return _propTxt;
		}
	}

	public UI_UsePropInfoData(string a, string b, Texture2D propTxt)
	{
		_strAName = a;
		_strBName = b;
		_propTxt = propTxt;
	}
}
