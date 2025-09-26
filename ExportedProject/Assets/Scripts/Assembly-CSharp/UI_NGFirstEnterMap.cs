using MessageID;

public class UI_NGFirstEnterMap : UI_NGBoardMgr
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_RPG_BtnDown, this, BoardBtnDown);
		_objNG.SetActive(COMA_Pref.Instance.NG2_1_FirstEnterMap);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_BtnDown, this);
	}

	private bool BoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		int num2 = num;
		if (num2 == 1000)
		{
			CloseAllBoard();
			_objNG.SetActive(false);
			_actorCmp.Ani_Out_to_right();
			COMA_Pref.Instance.NG2_1_FirstEnterMap = false;
			COMA_Pref.Instance.Save(true);
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
