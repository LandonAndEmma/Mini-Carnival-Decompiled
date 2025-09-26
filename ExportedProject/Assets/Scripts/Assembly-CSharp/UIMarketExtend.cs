using MC_UIToolKit;
using MessageID;
using UIGlobal;
using UnityEngine;

public class UIMarketExtend : UIEntity
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIMarket_GotoBackpackClick, this, GotoBackpackOnClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMarket_GotoBackpackClick, this);
	}

	private bool GotoBackpackScene(object obj)
	{
		Debug.Log("GotoBackpackScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Backpack", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, this, extraInfo);
		return true;
	}

	private bool GotoBackpackOnClick(TUITelegram msg)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Backpack);
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoBackpackScene);
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
