using NGUI_COMUI;
using UnityEngine;

public class UIRankings_RPGBox : NGUI_COMUI.UI_Box
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
	private UILabel _labelLV;

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
			base.gameObject.name = "UIRankingsRPGBox" + i;
		}
		else
		{
			base.gameObject.name = "UIRankingsRPGBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIRankings_RPGBoxData uIRankings_RPGBoxData = base.BoxData as UIRankings_RPGBoxData;
		if (uIRankings_RPGBoxData == null)
		{
			SetLoseSelected();
			return;
		}
		_texPlayer.mainTexture = uIRankings_RPGBoxData.Tex;
		_labelName.text = uIRankings_RPGBoxData.PlayerName;
		if (uIRankings_RPGBoxData.PlayerScore > 99999)
		{
			_labelScore.text = uIRankings_RPGBoxData.PlayerScore / 1000 + "K";
		}
		else
		{
			_labelScore.text = uIRankings_RPGBoxData.PlayerScore.ToString();
		}
		_labelLV.text = uIRankings_RPGBoxData.LV.ToString();
		_labelRank.gameObject.SetActive(false);
		_spriteRankTop3[0].gameObject.SetActive(false);
		_spriteRankTop3[1].gameObject.SetActive(false);
		_spriteRankTop3[2].gameObject.SetActive(false);
		if (uIRankings_RPGBoxData.Rank < 3)
		{
			_spriteRankTop3[uIRankings_RPGBoxData.Rank].gameObject.SetActive(true);
			return;
		}
		_labelRank.text = (uIRankings_RPGBoxData.Rank + 1).ToString();
		_labelRank.gameObject.SetActive(true);
	}
}
