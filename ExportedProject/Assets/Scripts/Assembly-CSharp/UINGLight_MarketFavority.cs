using MessageID;

public class UINGLight_MarketFavority : UINGLight_Square
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_FstWatchShopItem, this, IntroduceBtn);
		RegisterMessage(EUIMessageID.UING_FstWatchShopItemEnd, this, IntroduceBtnEnd);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_FstWatchShopItem, this);
		UnregisterMessage(EUIMessageID.UING_FstWatchShopItemEnd, this);
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
