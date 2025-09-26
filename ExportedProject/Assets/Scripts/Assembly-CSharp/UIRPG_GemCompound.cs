using MC_UIToolKit;
using MessageID;
using UIGlobal;
using UnityEngine;

public class UIRPG_GemCompound : UIEntity
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIGemsComposition_GotoSquare, this, GotoSquareClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIGemsComposition_GotoSquare, this);
	}

	private bool GotoSquare(object obj)
	{
		Debug.Log("GotoSquare");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoSquareClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquare);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
