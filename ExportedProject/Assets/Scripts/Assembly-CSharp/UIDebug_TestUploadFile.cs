using MessageID;
using Protocol.Shop.C2S;
using UnityEngine;

public class UIDebug_TestUploadFile : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		SetFileDataCmd setFileDataCmd = new SetFileDataCmd();
		byte[] array = COMA_FileIO.ReadPngData("Textures/T_Leg");
		Debug.Log("PNG SIZE=" + array.Length);
		setFileDataCmd.m_data = array;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, setFileDataCmd);
		Debug.Log("----------Send a png to srv!");
	}
}
