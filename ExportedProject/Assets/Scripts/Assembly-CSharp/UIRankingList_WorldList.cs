using UnityEngine;

public class UIRankingList_WorldList : MonoBehaviour
{
	private UIRankingList_WorldRankingData _data;

	[SerializeField]
	private TUIMeshSprite _bk;

	[SerializeField]
	private TUILabel _raningNum;

	[SerializeField]
	private TUILabel _name;

	[SerializeField]
	private TUILabel _score;

	[SerializeField]
	private TUIMeshSprite _awardTex;

	[SerializeField]
	private TUILabel _awardNum;

	[SerializeField]
	private TUILabel _awardX;

	[SerializeField]
	private bool _bSelf;

	[SerializeField]
	private UIRanking_RankIcon _rankIcon;

	public UIRankingList_WorldRankingData WorldRankingData
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
		if (WorldRankingData == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (_bk != null)
		{
			if (_bSelf)
			{
				_bk.color = new Color(0.35f, 0.38f, 0.4f);
			}
			else if (WorldRankingData.RankNum % 2 == 0)
			{
				_bk.color = new Color(0.35f, 0.38f, 0.4f);
			}
			else
			{
				_bk.color = new Color(0.68f, 0.68f, 0.68f);
			}
		}
		_rankIcon.SetRankId(WorldRankingData.RankNum, _raningNum.gameObject);
		if (WorldRankingData.RankNum > 999)
		{
			_raningNum.Text = "999+";
		}
		else
		{
			_raningNum.Text = WorldRankingData.RankNum.ToString();
		}
		_name.Text = WorldRankingData.PlayerName;
		_score.Text = WorldRankingData.ScoreNum.ToString();
		if (WorldRankingData.RankNum == 1)
		{
			_raningNum.color = new Color(1f, 0.77f, 0f);
			_name.color = new Color(1f, 0.77f, 0f);
			_score.color = new Color(1f, 0.77f, 0f);
			_awardNum.color = new Color(1f, 0.77f, 0f);
			_awardX.color = new Color(1f, 0.77f, 0f);
		}
		else
		{
			_raningNum.color = new Color(1f, 1f, 1f);
			_name.color = new Color(1f, 1f, 1f);
			_score.color = new Color(1f, 1f, 1f);
			_awardNum.color = new Color(1f, 1f, 1f);
			_awardX.color = new Color(1f, 1f, 1f);
		}
		if (_bSelf)
		{
			_raningNum.color = new Color(0.71f, 1f, 0.01f);
			_name.color = new Color(0.71f, 1f, 0.01f);
			_score.color = new Color(0.71f, 1f, 0.01f);
			_awardNum.color = new Color(0.71f, 1f, 0.01f);
			_awardX.color = new Color(0.71f, 1f, 0.01f);
		}
		if (WorldRankingData.Award.nAwardNum > 0)
		{
			_awardNum.transform.parent.gameObject.SetActive(true);
			_awardNum.Text = WorldRankingData.Award.nAwardNum.ToString();
			_awardTex.UseCustomize = false;
			_awardTex.texture = "title_gem";
		}
		else if (WorldRankingData.Award.part >= 0)
		{
			_awardNum.transform.parent.gameObject.SetActive(true);
			_awardNum.Text = "1";
			_awardTex.UseCustomize = false;
			_awardTex.texture = COMA_Tools.AwardSerialNameToTexture(WorldRankingData.Award.serialName);
		}
		else
		{
			_awardNum.transform.parent.gameObject.SetActive(false);
		}
	}
}
