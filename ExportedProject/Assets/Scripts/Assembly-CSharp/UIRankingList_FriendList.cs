using UnityEngine;

public class UIRankingList_FriendList : MonoBehaviour
{
	private UIRankingList_FriendRankingData _data;

	[SerializeField]
	private TUIMeshSprite _bk;

	[SerializeField]
	private TUILabel _raningNum;

	[SerializeField]
	private TUILabel _name;

	[SerializeField]
	private TUILabel _score;

	[SerializeField]
	private UIRanking_RankIcon _rankIcon;

	public UIRankingList_FriendRankingData FriendRankingData
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			DataChanged();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void DataChanged()
	{
		if (FriendRankingData == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (_bk != null)
		{
			if (FriendRankingData.RankNum % 2 == 0)
			{
				_bk.color = new Color(0.35f, 0.38f, 0.4f);
			}
			else
			{
				_bk.color = new Color(0.68f, 0.68f, 0.68f);
			}
		}
		_rankIcon.SetRankId(FriendRankingData.RankNum, _raningNum.gameObject);
		_raningNum.Text = FriendRankingData.RankNum.ToString();
		_name.Text = FriendRankingData.PlayerName;
		_score.Text = FriendRankingData.ScoreNum.ToString();
		if (FriendRankingData.RankNum == 1)
		{
			_raningNum.color = new Color(1f, 0.77f, 0f);
			_name.color = new Color(1f, 0.77f, 0f);
			_score.color = new Color(1f, 0.77f, 0f);
		}
		else
		{
			_raningNum.color = new Color(1f, 1f, 1f);
			_name.color = new Color(1f, 1f, 1f);
			_score.color = new Color(1f, 1f, 1f);
		}
	}
}
