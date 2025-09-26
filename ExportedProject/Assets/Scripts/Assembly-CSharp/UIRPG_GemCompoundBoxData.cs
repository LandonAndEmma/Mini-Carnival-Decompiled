using UnityEngine;

public class UIRPG_GemCompoundBoxData
{
	[SerializeField]
	private int _gemId;

	[SerializeField]
	private string _gemDefineLabel;

	[SerializeField]
	private string _gemCompoundPriceLabel;

	public int GemId
	{
		get
		{
			return _gemId;
		}
		set
		{
			_gemId = value;
		}
	}

	public string GemDefineLabel
	{
		get
		{
			return _gemDefineLabel;
		}
		set
		{
			_gemDefineLabel = value;
		}
	}

	public string GemCompoundPriceLabel
	{
		get
		{
			return _gemCompoundPriceLabel;
		}
		set
		{
			_gemCompoundPriceLabel = value;
		}
	}

	public UIRPG_GemCompoundBoxData(string arg1, string arg2, int id)
	{
		GemDefineLabel = arg1;
		GemCompoundPriceLabel = arg2;
		GemId = id;
	}
}
