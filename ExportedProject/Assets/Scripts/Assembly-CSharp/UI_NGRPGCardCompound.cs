using MessageID;

public class UI_NGRPGCardCompound : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterSquare);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		if ((int)msg._pExtraInfo == 0)
		{
			CloseAllBoard();
			_objNG.SetActive(false);
			_actorCmp.Ani_Out_to_right();
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
