using UnityEngine;

public class COMA_PlayerSync_WaitingRoom : COMA_PlayerSync
{
	protected new void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_TRANSFORM, base.ReceiveTransform);
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.CHAT, ReceiveChatting);
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_TRANSFORM, base.ReceiveTransform);
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.CHAT, ReceiveChatting);
		base.OnDisable();
	}

	protected void ReceiveChatting(COMA_CommandDatas commandDatas)
	{
		if (!(commandDatas.dataSender.Id.ToString() != base.gameObject.name))
		{
			COMA_CD_Chatting cOMA_CD_Chatting = commandDatas as COMA_CD_Chatting;
			Debug.Log(base.name + "----" + cOMA_CD_Chatting.chatting);
			Chat(cOMA_CD_Chatting.chatting);
		}
	}
}
