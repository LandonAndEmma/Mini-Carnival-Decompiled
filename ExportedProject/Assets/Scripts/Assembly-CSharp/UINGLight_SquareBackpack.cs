using MessageID;

public class UINGLight_SquareBackpack : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquarePointBackpack, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_SquarePointBackpackEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquarePointBackpack, this);
		UnregisterMessage(EUIMessageID.UING_SquarePointBackpackEnd, this);
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
