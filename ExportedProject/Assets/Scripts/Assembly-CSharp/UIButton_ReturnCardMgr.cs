using MessageID;
using UIGlobal;
using UnityEngine;

public class UIButton_ReturnCardMgr : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		TLoadScene extraInfo = new TLoadScene("UI.RPG.CardManage", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
	}
}
