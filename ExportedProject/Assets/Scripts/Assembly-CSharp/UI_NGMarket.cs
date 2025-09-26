using MessageID;
using UnityEngine;

public class UI_NGMarket : UI_NGBoardMgr
{
	[SerializeField]
	private UISprite _blockSprite;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UING_MarketBoardBtnDown, this, MarketBoardBtnDown);
		RegisterMessage(EUIMessageID.UING_FstWatchShopItem, this, FstWatchShopItem);
		RegisterMessage(EUIMessageID.UING_FstWatchShopItemEnd, this, FstWatchShopItemEnd);
		RegisterMessage(EUIMessageID.UING_FstEnterMarketTmp, this, FstEnterMarketTmp);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_MarketBoardBtnDown, this);
		UnregisterMessage(EUIMessageID.UING_FstWatchShopItem, this);
		UnregisterMessage(EUIMessageID.UING_FstWatchShopItemEnd, this);
		UnregisterMessage(EUIMessageID.UING_FstEnterMarketTmp, this);
	}

	private bool MarketBoardBtnDown(TUITelegram msg)
	{
		int num = (int)msg._pExtraInfo;
		switch (num)
		{
		case 0:
			CloseAllBoard();
			_objNG.SetActive(false);
			COMA_Pref.Instance.NG2_FirstEnterMarket = false;
			COMA_Pref.Instance.Save(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstWatchShopItemEnd, null, null);
			break;
		case 1:
			OpenBoard(num + 1);
			break;
		case 2:
			CloseAllBoard();
			_objNG.SetActive(false);
			COMA_Pref.Instance.NG2_FirstEnterMarketTmp = false;
			COMA_Pref.Instance.Save(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_FstEnterMarketTmpEnd, null, null);
			break;
		}
		return true;
	}

	private bool FstWatchShopItem(TUITelegram msg)
	{
		_objNG.SetActive(true);
		OpenBoard(0);
		return true;
	}

	private bool FstWatchShopItemEnd(TUITelegram msg)
	{
		return true;
	}

	private bool FstEnterMarketTmp(TUITelegram msg)
	{
		_objNG.SetActive(true);
		_blockSprite.enabled = false;
		OpenBoard(1);
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
