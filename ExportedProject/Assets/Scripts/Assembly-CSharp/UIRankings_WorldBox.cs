using NGUI_COMUI;
using UnityEngine;

public class UIRankings_WorldBox : NGUI_COMUI.UI_Box
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
	private UISprite _RewardIcon_Accessories;

	[SerializeField]
	private UISprite _RewardIcon_Crystal;

	[SerializeField]
	private UILabel _labelRewardNum;

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
			base.gameObject.name = "UIRankingsWorldBox" + i;
		}
		else
		{
			base.gameObject.name = "UIRankingsWorldBox0" + i;
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
		UIRankings_WorldBoxData uIRankings_WorldBoxData = base.BoxData as UIRankings_WorldBoxData;
		if (uIRankings_WorldBoxData == null)
		{
			SetLoseSelected();
			return;
		}
		_texPlayer.mainTexture = uIRankings_WorldBoxData.Tex;
		_labelName.text = uIRankings_WorldBoxData.PlayerName;
		if (uIRankings_WorldBoxData.PlayerScore > 99999)
		{
			_labelScore.text = uIRankings_WorldBoxData.PlayerScore / 1000 + "K";
		}
		else
		{
			_labelScore.text = uIRankings_WorldBoxData.PlayerScore.ToString();
		}
		if (uIRankings_WorldBoxData.AwardCrystal != 0)
		{
			_RewardIcon_Accessories.gameObject.SetActive(false);
			_RewardIcon_Crystal.gameObject.SetActive(true);
			_labelRewardNum.gameObject.SetActive(true);
			_labelRewardNum.text = "x" + uIRankings_WorldBoxData.AwardCrystal;
		}
		else
		{
			_RewardIcon_Accessories.gameObject.SetActive(true);
			_RewardIcon_Crystal.gameObject.SetActive(false);
			_RewardIcon_Accessories.spriteName = "deco_" + uIRankings_WorldBoxData.AwardSerialName;
			_labelRewardNum.gameObject.SetActive(true);
			_labelRewardNum.text = "x1";
		}
	}
}
