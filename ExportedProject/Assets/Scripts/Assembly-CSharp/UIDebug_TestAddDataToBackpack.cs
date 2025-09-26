using MessageID;
using Protocol.Role.C2S;
using UnityEngine;

public class UIDebug_TestAddDataToBackpack : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		AddBagItemCmd addBagItemCmd = new AddBagItemCmd();
		addBagItemCmd.m_part = 1;
		addBagItemCmd.m_state = 2;
		addBagItemCmd.m_unit = "269b1aa2b06a4a89da218b8e7b3da0a9";
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, addBagItemCmd);
		Debug.Log("----------Send a png to srv!");
	}
}
