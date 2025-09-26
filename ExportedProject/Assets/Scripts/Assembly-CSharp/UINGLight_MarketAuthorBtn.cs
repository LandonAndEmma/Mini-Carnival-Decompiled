using MessageID;
using UnityEngine;

public class UINGLight_MarketAuthorBtn : UINGLight_Square
{
	[SerializeField]
	private GameObject _aniLayer;

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
		_aniLayer.SetActive(true);
		LightOn();
		return true;
	}

	private bool IntroduceBtnEnd(TUITelegram msg)
	{
		_aniLayer.animation.Play("UIMarket_AuthorInfoUnstretch");
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
