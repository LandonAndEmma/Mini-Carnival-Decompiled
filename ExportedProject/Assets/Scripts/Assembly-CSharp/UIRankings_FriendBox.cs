using NGUI_COMUI;
using UnityEngine;

public class UIRankings_FriendBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UILabel _labelRank;

	[SerializeField]
	private UISprite[] _spriteRankTop3;

	[SerializeField]
	private UITexture _texPlayer;

	[SerializeField]
	private UILabel _labelName;

	[SerializeField]
	private UILabel _labelScore;

	[SerializeField]
	private GameObject _objRPGSpec;

	[SerializeField]
	private UILabel _labelLV;

	[SerializeField]
	private UILabel _labelScoreRPG;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIRankingsFriendBox" + i;
		}
		else
		{
			base.gameObject.name = "UIRankingsFriendBox0" + i;
		}
		_labelRank.gameObject.SetActive(false);
		_spriteRankTop3[0].gameObject.SetActive(false);
		_spriteRankTop3[1].gameObject.SetActive(false);
		_spriteRankTop3[2].gameObject.SetActive(false);
		if (i < 3)
		{
			_spriteRankTop3[i].gameObject.SetActive(true);
			return;
		}
		_labelRank.text = (i + 1).ToString();
		_labelRank.gameObject.SetActive(true);
	}

	public override void BoxDataChanged()
	{
		UIRankings_FriendBoxData uIRankings_FriendBoxData = base.BoxData as UIRankings_FriendBoxData;
		if (uIRankings_FriendBoxData == null)
		{
			SetLoseSelected();
			return;
		}
		_objRPGSpec.SetActive(uIRankings_FriendBoxData.IsRPG);
		_labelScore.enabled = !uIRankings_FriendBoxData.IsRPG;
		if (uIRankings_FriendBoxData.IsRPG)
		{
			_labelLV.text = uIRankings_FriendBoxData.LV.ToString();
			if (uIRankings_FriendBoxData.PlayerScore > 99999)
			{
				_labelScoreRPG.text = uIRankings_FriendBoxData.PlayerScore / 1000 + "K";
			}
			else
			{
				_labelScoreRPG.text = uIRankings_FriendBoxData.PlayerScore.ToString();
			}
		}
		_texPlayer.mainTexture = uIRankings_FriendBoxData.Tex;
		_labelName.text = uIRankings_FriendBoxData.PlayerName;
		if (uIRankings_FriendBoxData.PlayerScore > 99999)
		{
			_labelScore.text = uIRankings_FriendBoxData.PlayerScore / 1000 + "K";
		}
		else
		{
			_labelScore.text = uIRankings_FriendBoxData.PlayerScore.ToString();
		}
	}
}
