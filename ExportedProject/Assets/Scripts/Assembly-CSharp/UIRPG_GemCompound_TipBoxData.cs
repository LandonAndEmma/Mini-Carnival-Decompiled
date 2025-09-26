public class UIRPG_GemCompound_TipBoxData
{
	private int _gemCompoundType;

	private int _gemLevel;

	public int GemCompoundType
	{
		get
		{
			return _gemCompoundType;
		}
	}

	public int GemLevel
	{
		get
		{
			return _gemLevel;
		}
	}

	public UIRPG_GemCompound_TipBoxData(int type, int level)
	{
		_gemCompoundType = type;
		_gemLevel = level;
	}
}
