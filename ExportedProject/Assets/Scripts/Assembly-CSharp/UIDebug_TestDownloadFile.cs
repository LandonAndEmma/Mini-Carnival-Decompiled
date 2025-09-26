using MessageID;
using Protocol.Shop.C2S;
using UnityEngine;

public class UIDebug_TestDownloadFile : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		GetFileDataCmd getFileDataCmd = new GetFileDataCmd();
		getFileDataCmd.m_md5 = "269b1aa2b06a4a89da218b8e7b3da0a9";
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, getFileDataCmd);
		Debug.Log("----------get a png from srv!");
	}
}
