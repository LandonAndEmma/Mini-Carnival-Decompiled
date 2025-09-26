using MessageID;

public class UINGLight_SquareMarket : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquarePointMarket, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_SquarePointMarketEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquarePointMarket, this);
		UnregisterMessage(EUIMessageID.UING_SquarePointMarketEnd, this);
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
