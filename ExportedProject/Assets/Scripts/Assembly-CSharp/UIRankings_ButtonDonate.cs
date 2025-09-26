using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using UnityEngine;

public class UIRankings_ButtonDonate : MonoBehaviour
{
	public uint tarID;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		Debug.Log("--------------------------------Donate--------------------------- " + tarID);
		FriendMobilityCmd friendMobilityCmd = new FriendMobilityCmd();
		friendMobilityCmd.m_friend_id = tarID;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, friendMobilityCmd);
		FriendTicketCmd friendTicketCmd = new FriendTicketCmd();
		friendTicketCmd.m_friend_id = tarID;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, friendTicketCmd);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		base.gameObject.SetActive(false);
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("haoyoujiemian_desc18"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}
}
