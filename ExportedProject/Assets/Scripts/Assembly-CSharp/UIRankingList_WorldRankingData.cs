using UnityEngine;

public class UIRankingList_WorldRankingData : UIRankingList_RankingData
{
	public class SWorldRankingAward
	{
		public int nAwardId;

		public int nAwardNum;

		public Texture2D _awardTex;

		public string serialName;

		public string itemName;

		public int part;

		public SWorldRankingAward()
		{
			nAwardId = 0;
			nAwardNum = -1;
			_awardTex = null;
			serialName = string.Empty;
			itemName = string.Empty;
			part = -1;
		}
	}

	private SWorldRankingAward _award;

	public SWorldRankingAward Award
	{
		get
		{
			return _award;
		}
		set
		{
			_award = value;
		}
	}

	public UIRankingList_WorldRankingData()
	{
		base.RankNum = UIRankingList_RankingData._nTestRaningId++;
		base.PlayerName = "cc";
		base.ScoreNum = 999999;
		Award = new SWorldRankingAward();
	}
}
