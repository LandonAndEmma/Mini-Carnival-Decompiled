using MessageID;

public class UI_NGRPGCardMgr : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterSquare);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			if (UIDataBufferCenter.Instance.CurNGIndex == 1)
			{
				OpenBoard(0);
				_actorCmp.Ani_In_from_left();
			}
			else if (UIDataBufferCenter.Instance.CurNGIndex == 2)
			{
				OpenBoard(1);
				_actorCmp.Ani_In_from_left(1);
			}
			else if (UIDataBufferCenter.Instance.CurNGIndex == 3)
			{
				OpenBoard(3);
				_actorCmp.Ani_In_from_left(2);
			}
		}
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		switch ((int)msg._pExtraInfo)
		{
		case 1:
			OpenBoard(2);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_RPG_CardMgr_Click1, null, null);
			break;
		case 33:
			CloseAllBoard();
			_actorCmp.Ani_Out_to_left();
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
