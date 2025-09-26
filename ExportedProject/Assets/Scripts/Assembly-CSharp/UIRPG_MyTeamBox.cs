using NGUI_COMUI;
using UnityEngine;

public class UIRPG_MyTeamBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	public int _index;

	[SerializeField]
	private GameObject _captainIconObj;

	[SerializeField]
	private GameObject _addIconObj;

	[SerializeField]
	private GameObject _lockIconObj;

	[SerializeField]
	private UILabel _lockRankLabel;

	[SerializeField]
	private UISprite _spriteBK;

	public override void BoxDataChanged()
	{
		UIRPG_MyTeamBoxData uIRPG_MyTeamBoxData = base.BoxData as UIRPG_MyTeamBoxData;
		_captainIconObj.SetActive(uIRPG_MyTeamBoxData.IsCaptain);
		_addIconObj.SetActive(uIRPG_MyTeamBoxData.DataType == 0);
		_mainSprite.gameObject.SetActive(false);
		_spriteBK.gameObject.SetActive(false);
		switch (uIRPG_MyTeamBoxData.DataType)
		{
		case 1:
		{
			_mainSprite.spriteName = uIRPG_MyTeamBoxData.SpriteName;
			_mainSprite.gameObject.SetActive(true);
			if (uIRPG_MyTeamBoxData.CurPos == 3 || uIRPG_MyTeamBoxData.CurPos == 4 || uIRPG_MyTeamBoxData.CurPos == 5)
			{
				_lockRankLabel.gameObject.SetActive(false);
				_lockIconObj.SetActive(false);
				_addIconObj.SetActive(true);
			}
			byte starGrade = RPGGlobalData.Instance.CareerUnitPool._dict[(int)uIRPG_MyTeamBoxData.CardId].StarGrade;
			_spriteBK.color = UIRPG_DataBufferCenter.GetCardColorByGrade(starGrade);
			_spriteBK.gameObject.SetActive(true);
			break;
		}
		case 0:
			_spriteBK.color = Color.white;
			if (uIRPG_MyTeamBoxData.CurPos == 3 || uIRPG_MyTeamBoxData.CurPos == 4 || uIRPG_MyTeamBoxData.CurPos == 5)
			{
				_lockRankLabel.gameObject.SetActive(false);
				_lockIconObj.SetActive(false);
				_addIconObj.SetActive(true);
			}
			break;
		case 2:
			_lockIconObj.SetActive(uIRPG_MyTeamBoxData.DataType == 2);
			if (uIRPG_MyTeamBoxData.CurPos == 3)
			{
				_lockRankLabel.text = "lv " + RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos4;
			}
			else if (uIRPG_MyTeamBoxData.CurPos == 4)
			{
				_lockRankLabel.text = "lv " + RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos5;
			}
			else if (uIRPG_MyTeamBoxData.CurPos == 5)
			{
				_lockRankLabel.text = "lv " + RPGGlobalData.Instance.RpgMiscUnit._lvLimit_TeamPos6;
			}
			break;
		}
	}
}
