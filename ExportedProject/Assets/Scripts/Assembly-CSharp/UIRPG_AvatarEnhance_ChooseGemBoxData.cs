public class UIRPG_AvatarEnhance_ChooseGemBoxData
{
	private int[] _gemNums = new int[5];

	private int _curCaptionType;

	private int _gemComposition;

	private bool _isSel;

	private bool _isHasSel;

	public int[] GemNums
	{
		get
		{
			return _gemNums;
		}
	}

	public int CurCaptionType
	{
		get
		{
			return _curCaptionType;
		}
	}

	public int GemComposition
	{
		get
		{
			return _gemComposition;
		}
		set
		{
			_gemComposition = value;
		}
	}

	public bool IsSel
	{
		get
		{
			return _isSel;
		}
		set
		{
			_isSel = value;
		}
	}

	public bool IsHasSel
	{
		get
		{
			return _isHasSel;
		}
		set
		{
			_isHasSel = value;
		}
	}

	public UIRPG_AvatarEnhance_ChooseGemBoxData(int[] gemNums)
	{
		for (int i = 1; i < gemNums.Length; i++)
		{
			_gemNums[i] = gemNums[i];
		}
	}

	public UIRPG_AvatarEnhance_ChooseGemBoxData(int[] gemNums, int type)
		: this(gemNums)
	{
		_curCaptionType = type;
	}
}
