using MessageID;

public class UINGLight_SquareChat : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_SquareIntroduceChatBtn, this, IntroduceChatBtn);
		RegisterMessage(EUIMessageID.UING_SquareIntroduceChatBtnEnd, this, SquareIntroduceChatBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_SquareIntroduceChatBtn, this);
		UnregisterMessage(EUIMessageID.UING_SquareIntroduceChatBtnEnd, this);
	}

	private bool IntroduceChatBtn(TUITelegram msg)
	{
		LightOn();
		return true;
	}

	private bool SquareIntroduceChatBtnEnd(TUITelegram msg)
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
