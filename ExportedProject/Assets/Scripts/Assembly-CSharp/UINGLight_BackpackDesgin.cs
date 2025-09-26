using MessageID;

public class UINGLight_BackpackDesgin : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_FstUseEditBtn, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_FstUseEditBtnEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_FstUseEditBtn, this);
		UnregisterMessage(EUIMessageID.UING_FstUseEditBtnEnd, this);
	}

	private bool IntroduceBtn(TUITelegram msg)
	{
		LightOn();
		return true;
	}

	private bool IntroduceBtnEnd(TUITelegram msg)
	{
		LightOff();
		return true;
	}

	private new void Awake()
	{
		base.Awake();
	}

	protected override void Tick()
	{
	}
}
