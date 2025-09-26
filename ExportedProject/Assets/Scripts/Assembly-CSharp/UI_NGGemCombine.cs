using MessageID;
using UnityEngine;

public class UI_NGGemCombine : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterGemCombine);
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
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			break;
		case 1:
			_ngBoards[num].SetActive(false);
			_ngBoards[num + 1].SetActive(true);
			break;
		case 2:
			Debug.Log(" End NG!!");
			COMA_Pref.Instance.NG2_1_FirstEnterGemCombine = false;
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
