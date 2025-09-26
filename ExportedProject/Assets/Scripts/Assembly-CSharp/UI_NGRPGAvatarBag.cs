using MessageID;
using UnityEngine;

public class UI_NGRPGAvatarBag : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		RegisterMessage(EUIMessageID.UING_RPG_FirstClickAvatar, this, FirstClickAvatar);
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterRPGBag);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
		UnregisterMessage(EUIMessageID.UING_RPG_FirstClickAvatar, this);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		switch ((int)msg._pExtraInfo)
		{
		case 0:
			Debug.Log(" End NG!!");
			COMA_Pref.Instance.NG2_1_FirstEnterRPGBag = false;
			COMA_Pref.Instance.Save(true);
			CloseAllBoard();
			_objNG.SetActive(false);
			break;
		case 1:
			COMA_Pref.Instance.NG2_1_FirstRPGBag_Click = false;
			COMA_Pref.Instance.Save(true);
			CloseAllBoard();
			_objNG.SetActive(false);
			break;
		}
		return true;
	}

	private bool FirstClickAvatar(TUITelegram msg)
	{
		_objNG.SetActive(true);
		OpenBoard(1);
		_spriteBlockBG.enabled = false;
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
