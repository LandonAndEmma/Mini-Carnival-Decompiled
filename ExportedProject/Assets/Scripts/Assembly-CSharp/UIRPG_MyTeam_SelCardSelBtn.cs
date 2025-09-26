using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UIRPG_MyTeam_SelCardSelBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	[SerializeField]
	private UIRPG_MyTeam_SelCardMgr _selCardMgr;

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIRPG_MyTeam_SelCardBox uIRPG_MyTeam_SelCardBox = _selCardMgr.SelCardContainer.CurSelBox as UIRPG_MyTeam_SelCardBox;
		UIRPG_MyTeam_SelCardBoxData uIRPG_MyTeam_SelCardBoxData = uIRPG_MyTeam_SelCardBox.BoxData as UIRPG_MyTeam_SelCardBoxData;
		UIRPG_MyTeam_SelCardSelBtnData uIRPG_MyTeam_SelCardSelBtnData = new UIRPG_MyTeam_SelCardSelBtnData(uIRPG_MyTeam_SelCardBoxData.CardId);
		uIRPG_MyTeam_SelCardSelBtnData.ItemId = uIRPG_MyTeam_SelCardBoxData.ItemId;
		Debug.Log("_selCardMgr.SelBtnStat " + _selCardMgr.SelBtnStat);
		if (_selCardMgr.SelBtnStat)
		{
			uIRPG_MyTeam_SelCardSelBtnData.IsPutOn = true;
		}
		else
		{
			if (uIRPG_MyTeam_SelCardBoxData.IsCaptain)
			{
				return;
			}
			if (_myTeamMgr._dict.Count <= 1)
			{
				Debug.Log("_myTeamMgr._dict.Count <= 1");
				string des = TUITool.StringFormat(Localization.instance.Get("myteam_desc2"));
				UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, des);
				uIMessage_CommonBoxData.Mark = "Can'tWithDraw";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
				return;
			}
			uIRPG_MyTeam_SelCardSelBtnData.CurPos = _myTeamMgr._dict[uIRPG_MyTeam_SelCardSelBtnData.ItemId];
			uIRPG_MyTeam_SelCardSelBtnData.IsPutOn = false;
			uIRPG_MyTeam_SelCardBoxData.IsSel = false;
			_selCardMgr.SelBtnStat = true;
			uIRPG_MyTeam_SelCardBox.BoxDataChanged();
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPGTeam_SelNewCardClick, null, uIRPG_MyTeam_SelCardSelBtnData);
	}
}
