using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_Map_EnemyChangeBtn : UIEntity
{
	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	[SerializeField]
	private UIRPG_Map_EnemyMgr _enemyMgr;

	[SerializeField]
	private GameObject _changeBtn;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPG_NotifyReplacePlayerResult, this, HandleNotifyReplacePlayerResult);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_NotifyReplacePlayerResult, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	public void OnClick()
	{
		string des = TUITool.StringFormat(Localization.instance.Get("rpgmap_biaoti8"));
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
		uIMessage_CommonBoxData.Mark = "IsChange";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
	}

	public bool HandleNotifyReplacePlayerResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		ChangePlayerLevelResultCmd changePlayerLevelResultCmd = msg._pExtraInfo as ChangePlayerLevelResultCmd;
		if (changePlayerLevelResultCmd == null)
		{
			Debug.Log("cmd == null");
		}
		else
		{
			Debug.Log("========================" + changePlayerLevelResultCmd.m_result);
		}
		if (changePlayerLevelResultCmd.m_result == 0)
		{
			_changeBtn.SetActive(false);
			_vertexMgr.InitSingleCheckPointsVertex(_vertexMgr.CurVertexIndex);
			_enemyMgr.InitContentData();
		}
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "IsChange":
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			ChangePlayerLevelCmd changePlayerLevelCmd = new ChangePlayerLevelCmd();
			changePlayerLevelCmd.m_map_point = (byte)_vertexMgr.CurVertexIndex;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, changePlayerLevelCmd);
			break;
		}
		}
		return true;
	}
}
