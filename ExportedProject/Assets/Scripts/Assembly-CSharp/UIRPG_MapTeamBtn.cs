using MessageID;
using UIGlobal;
using UnityEngine;

public class UIRPG_MapTeamBtn : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		UIDataBufferCenter.Instance.CurBattleLevelLV = -1;
		TLoadScene extraInfo = new TLoadScene("UI.RPG.MyTerm", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
	}
}
