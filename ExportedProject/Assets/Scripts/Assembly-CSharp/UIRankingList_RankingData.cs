public class UIRankingList_RankingData
{
	public static int _nTestRaningId = 1;

	private int _nRank;

	private string _strName;

	private string _strId;

	private int _nScore;

	public int RankNum
	{
		get
		{
			return _nRank;
		}
		set
		{
			_nRank = value;
		}
	}

	public string PlayerName
	{
		get
		{
			return _strName;
		}
		set
		{
			_strName = value;
		}
	}

	public string PlayerId
	{
		get
		{
			return _strId;
		}
		set
		{
			_strId = value;
		}
	}

	public int ScoreNum
	{
		get
		{
			return _nScore;
		}
		set
		{
			_nScore = value;
		}
	}
}
