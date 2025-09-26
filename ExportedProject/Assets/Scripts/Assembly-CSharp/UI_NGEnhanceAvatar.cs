using MessageID;
using UnityEngine;

public class UI_NGEnhanceAvatar : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterEnhance);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		switch (num)
		{
		case 0:
			_spriteBlockBG.enabled = false;
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			break;
		case 1:
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			break;
		case 2:
			Debug.Log(" End NG!!");
			COMA_Pref.Instance.NG2_1_FirstEnterEnhance = false;
			COMA_Pref.Instance.Save(true);
			CloseAllBoard();
			_objNG.SetActive(false);
			break;
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
