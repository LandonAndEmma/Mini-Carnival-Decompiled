using MessageID;

public class UINGLight_SquareMails : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquarePointMails, this, IntroduceMailsBtn);
		RegisterMessage(EUIMessageID.UING_SquarePointMailsEnd, this, SquareIntroduceMailsBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquarePointMails, this);
		UnregisterMessage(EUIMessageID.UING_SquarePointMailsEnd, this);
	}

	private bool IntroduceMailsBtn(TUITelegram msg)
	{
		LightOn();
		return true;
	}

	private bool SquareIntroduceMailsBtnEnd(TUITelegram msg)
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
