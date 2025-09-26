using NGUI_COMUI;
using UnityEngine;

public class UIRPG_Map_EnemyBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UISprite _borderSprite;

	[SerializeField]
	private UILabel _bossLabel;

	[SerializeField]
	private UISprite _bossSprite;

	public override void FormatBoxName(int i)
	{
		base.gameObject.name = ((i <= 9) ? ("UIRPG_Map_SummonFriend_0" + i) : ("UIRPG_Map_SummonFriend_" + i));
	}

	public override void BoxDataChanged()
	{
		Debug.Log("UIRPG_Map_EnemyBox : BoxDataChanged");
		_bossLabel.gameObject.SetActive(false);
		_bossSprite.gameObject.SetActive(false);
		UIRPG_Map_EnemyBoxData uIRPG_Map_EnemyBoxData = base.BoxData as UIRPG_Map_EnemyBoxData;
		if (uIRPG_Map_EnemyBoxData.CardId < 500)
		{
			_mainSprite.spriteName = UIRPG_DataBufferCenter.GetCardIconNameByCardId(uIRPG_Map_EnemyBoxData.CardId);
		}
		else
		{
			_bossLabel.gameObject.SetActive(true);
			_bossSprite.gameObject.SetActive(true);
			_mainSprite.spriteName = UIRPG_DataBufferCenter.GetCardIconNameByCardId(uIRPG_Map_EnemyBoxData.CardId);
		}
		_borderSprite.color = UIRPG_DataBufferCenter.GetCardColorByGrade((byte)uIRPG_Map_EnemyBoxData.CardGrade);
	}
}
