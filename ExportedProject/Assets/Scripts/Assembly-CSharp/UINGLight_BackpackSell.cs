using MessageID;

public class UINGLight_BackpackSell : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_FstUseSellBtn, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_FstUseSellBtnEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_FstUseSellBtn, this);
		UnregisterMessage(EUIMessageID.UING_FstUseSellBtnEnd, this);
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
