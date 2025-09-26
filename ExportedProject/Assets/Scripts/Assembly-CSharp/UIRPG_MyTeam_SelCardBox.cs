using NGUI_COMUI;
using UnityEngine;

public class UIRPG_MyTeam_SelCardBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private GameObject _isSelIconObj;

	[SerializeField]
	private UISprite _spriteBK;

	public override void FormatBoxName(int i)
	{
		base.gameObject.name = ((i <= 9) ? ("UIRPG_MyTeam_Card_0" + i) : ("UIRPG_MyTeam_Card_" + i));
	}

	public override void BoxDataChanged()
	{
		UIRPG_MyTeam_SelCardBoxData uIRPG_MyTeam_SelCardBoxData = base.BoxData as UIRPG_MyTeam_SelCardBoxData;
		_isSelIconObj.SetActive(uIRPG_MyTeam_SelCardBoxData.IsSel);
		_mainSprite.spriteName = uIRPG_MyTeam_SelCardBoxData.SpriteName;
		_spriteBK.color = UIRPG_DataBufferCenter.GetCardColorByGrade(uIRPG_MyTeam_SelCardBoxData.CardGrade);
	}
}
