using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using Protocol.Role.S2C;
using UIGlobal;
using UnityEngine;

public class UIRPG_Map_EnemyChallengeBtn : UIEntity
{
	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPG_NotifyReqBattleResult, this, HandleNotifyReqBattleResult);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_NotifyReqBattleResult, this);
	}

	public void OnClick()
	{
		Debug.Log("UIRPG_Map_EnemyChallengeBtn OnClick");
		UIRPG_DataBufferCenter._challangePrePersonLv = UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		UIRPG_DataBufferCenter._cardMemberNumPre = UIRPG_DataBufferCenter.GetAvailableMemberSlot();
		if (UIRPG_DataBufferCenter.GetCardMemberNum() == 0)
		{
			return;
		}
		if (_vertexMgr._specialDropOut)
		{
			int num = 0;
			foreach (uint key in UIDataBufferCenter.Instance.RPGData.m_card_list.Keys)
			{
				num += UIDataBufferCenter.Instance.RPGData.m_card_list[key].Count;
			}
			if (num >= UIDataBufferCenter.Instance.RPGData.m_card_capacity)
			{
				string des = TUITool.StringFormat(Localization.instance.Get("rpgmap_desc2"));
				UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, des);
				uIMessage_CommonBoxData.Mark = "AvatarBackpackOver";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
				return;
			}
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			List<BagItem> bag_list = notifyRoleDataCmd.m_bag_data.m_bag_list;
			if (bag_list.Count == notifyRoleDataCmd.m_bag_data.m_bag_capacity)
			{
				string des2 = TUITool.StringFormat(Localization.instance.Get("rpgmap_desc3"));
				UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(1, des2);
				uIMessage_CommonBoxData2.Mark = "CardBackpackOver";
				UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
				return;
			}
		}
		if (RPGGlobalClock.Instance.GetMobilityValue() < 1)
		{
			string des3 = TUITool.StringFormat(Localization.instance.Get("rpgmap_desc1"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData3 = new UIMessage_CommonBoxData(0, des3);
			uIMessage_CommonBoxData3.Mark = "IsBuyMobility";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData3);
		}
		else
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			ReqBattleCmd reqBattleCmd = new ReqBattleCmd();
			reqBattleCmd.m_map_point = (byte)_vertexMgr.CurVertexIndex;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, reqBattleCmd);
		}
	}

	public bool HandleNotifyReqBattleResult(TUITelegram msg)
	{
		ReqBattleResultCmd reqBattleResultCmd = msg._pExtraInfo as ReqBattleResultCmd;
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		if (reqBattleResultCmd.m_result == 0)
		{
			Debug.Log("_vertexMgr.CurVertexIndex " + _vertexMgr.CurVertexIndex);
			UIDataBufferCenter.Instance.CurBattleLevelIndex = _vertexMgr.CurVertexIndex;
			UIRPG_DataBufferCenter._isPreSceneBattle = true;
			if (_vertexMgr.CurVertexType == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KPlayer)
			{
				UIDataBufferCenter.Instance.FetchPlayerRPGData(_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].Role_id, delegate(PlayerRpgDataCmd playData)
				{
					UIDataBufferCenter.Instance.RPGData_Enemy = playData;
					UIDataBufferCenter.Instance.CurBattleLevelLV = (int)UIDataBufferCenter.Instance.RPGData_Enemy.m_rpg_level;
					Debug.Log("UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KPlayer");
					Debug.Log("UIDataBufferCenter.Instance.CurBattleLevelLV : " + UIDataBufferCenter.Instance.CurBattleLevelLV);
					string sceneName2 = RPGGlobalData.Instance.Level_Scene._dict[_vertexMgr.CurVertexIndex + 1]._sceneName;
					Debug.Log(sceneName2);
					TLoadScene extraInfo2 = new TLoadScene(sceneName2, ELoadLevelParam.LoadOnly);
					UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo2);
				});
			}
			else
			{
				string sceneName = RPGGlobalData.Instance.Level_Scene._dict[_vertexMgr.CurVertexIndex + 1]._sceneName;
				Debug.Log(sceneName);
				TLoadScene extraInfo = new TLoadScene(sceneName, ELoadLevelParam.LoadOnly);
				UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			}
			if (COMA_Platform.Instance != null)
			{
				Debug.Log("COMA_Platform.Instance != null");
				COMA_Platform.Instance.DestroyPlatform();
			}
		}
		return true;
	}
}
