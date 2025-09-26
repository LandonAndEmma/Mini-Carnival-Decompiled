using System.Text.RegularExpressions;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Role;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using UnityEngine;

public class UIChatInputMgr : MonoBehaviour
{
	[SerializeField]
	private UISquare_ChatFriendsContainer _chatFriendsContainer;

	[SerializeField]
	private UIChatInput _uiChatInput;

	public void OnPrivateChatPopup()
	{
		_chatFriendsContainer.InitFriendsContainer();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnSubmit(string inputString)
	{
		inputString = Regex.Replace(inputString, "[^0-9A-Za-z!#$%+,-/:;<=>?@_|]", " ");
		Debug.Log("Chat:" + inputString);
		if (inputString == string.Empty || inputString == "|")
		{
			_uiChatInput.text = string.Empty;
			return;
		}
		string content = inputString;
		string text = string.Empty;
		ChatCmd cmd = new ChatCmd();
		NotifyRoleDataCmd notifyRoleDataCmd = UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role) as NotifyRoleDataCmd;
		cmd.m_channel = 2;
		cmd.m_sender_name = notifyRoleDataCmd.m_info.m_name;
		cmd.m_receiver_id = 0u;
		cmd.m_content = content;
		string extraInfo = string.Empty;
		if (inputString.StartsWith("@"))
		{
			cmd.m_channel = 0;
			int num = inputString.IndexOf(' ');
			string tarName = inputString.Substring(1, num - 1);
			text = "@" + tarName + " ";
			content = inputString.Substring(num + 1);
			cmd.m_content = content;
			extraInfo = tarName;
			if (content == string.Empty || inputString == "|")
			{
				_uiChatInput.text = text;
				return;
			}
			content = SS_Harmonious.Instance.GetHarmoniousSentence(content);
			int i = 0;
			for (int count = notifyRoleDataCmd.m_friend_list.Count; i < count; i++)
			{
				UIDataBufferCenter.Instance.FetchPlayerProfile(notifyRoleDataCmd.m_friend_list[i], delegate(WatchRoleInfo playerInfo)
				{
					if (playerInfo.m_name == tarName)
					{
						cmd.m_receiver_id = playerInfo.m_player_id;
						cmd.m_content = content;
						UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, cmd);
					}
				});
			}
		}
		else
		{
			content = SS_Harmonious.Instance.GetHarmoniousSentence(content);
			cmd.m_content = content;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_SelfChatContent, null, content);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, cmd);
		}
		NotifyChatCmd notifyChatCmd = new NotifyChatCmd();
		notifyChatCmd.m_channel = cmd.m_channel;
		notifyChatCmd.m_sender_name = cmd.m_sender_name;
		notifyChatCmd.m_sender_id = UIGolbalStaticFun.GetSelfTID();
		notifyChatCmd.m_content = content;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_SpawnNewChatRecord, null, notifyChatCmd, extraInfo);
		_uiChatInput.text = text;
	}
}
