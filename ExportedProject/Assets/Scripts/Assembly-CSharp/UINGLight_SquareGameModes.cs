using MessageID;

public class UINGLight_SquareGameModes : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquarePointGameModes, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_SquarePointGameModesEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquarePointGameModes, this);
		UnregisterMessage(EUIMessageID.UING_SquarePointGameModesEnd, this);
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
