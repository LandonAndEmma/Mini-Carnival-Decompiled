using System.Text.RegularExpressions;
using UnityEngine;

public class UIChatAble : UIMessageHandler
{
	[SerializeField]
	protected UIInputChatBox _chatBox;

	public virtual void NotifyInputString(string str)
	{
		if (!(str == string.Empty))
		{
			str = Regex.Replace(str, "[^0-9A-Za-z!#$%+,-/:;<=>?@_|]", " ");
			Debug.Log("Input content:" + str);
			if (COMA_PlayerSelf.Instance != null)
			{
				COMA_PlayerSelf.Instance.Chat(str);
			}
			COMA_CD_Chatting cOMA_CD_Chatting = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.CHAT) as COMA_CD_Chatting;
			cOMA_CD_Chatting.chatting = str;
			cOMA_CD_Chatting.sendName = COMA_Pref.Instance.nickname;
			COMA_CommandHandler.Instance.Send(cOMA_CD_Chatting);
			Debug.Log("Sending-------------------End:" + str);
		}
	}

	public virtual void CancelOrHide()
	{
	}
}
