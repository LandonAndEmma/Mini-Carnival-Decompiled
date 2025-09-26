using MessageID;
using UIGlobal;
using UnityEngine;

public class UIRPG_Button_BacktoSquare : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		TLoadScene extraInfo = new TLoadScene("UI.RPG.Map", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
	}
}
