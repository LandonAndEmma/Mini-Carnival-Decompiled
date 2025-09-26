using MessageID;
using UnityEngine;

public class UI_NGBackpack : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_FstUseSellBtn, this, FstUseSellBtn);
		RegisterMessage(EUIMessageID.UING_FstUseEditBtn, this, FstUseEditBtn);
		RegisterMessage(EUIMessageID.UING_BackpackBoardBtnDown, this, BackpackBoardBtnDown);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_FstUseSellBtn, this);
		UnregisterMessage(EUIMessageID.UING_FstUseEditBtn, this);
		UnregisterMessage(EUIMessageID.UING_BackpackBoardBtnDown, this);
	}

	private bool FstUseSellBtn(TUITelegram msg)
	{
		_objNG.SetActive(true);
		OpenBoard(3);
		return true;
	}

	private bool FstUseEditBtn(TUITelegram msg)
	{
		_objNG.SetActive(true);
		OpenBoard(0);
		return true;
	}

	private bool BackpackBoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		switch (num)
		{
		case 0:
		case 1:
			OpenBoard(num + 1);
			break;
		case 2:
			CloseAllBoard();
			_objNG.SetActive(false);
			COMA_Pref.Instance.NG2_FirstEnterBackpackEdit = false;
			COMA_Pref.Instance.Save(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstUseEditBtnEnd, null, null);
			Debug.Log("-------------------COMA_Pref.Instance.NG2_FirstEnterBackpackEdit=" + COMA_Pref.Instance.NG2_FirstEnterBackpackEdit);
			break;
		case 3:
		case 4:
			OpenBoard(num + 1);
			break;
		case 5:
			CloseAllBoard();
			_objNG.SetActive(false);
			COMA_Pref.Instance.NG2_FirstEnterSellItem = false;
			COMA_Pref.Instance.Save(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstUseSellBtnEnd, null, null);
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
