using System.Collections.Generic;
using Protocol;

public class RemoteMarketShopListData
{
	public byte _type;

	public List<ShopItem> _lstShopItem;

	public RemoteMarketShopListData(byte type, List<ShopItem> lstItem)
	{
		_type = type;
		_lstShopItem = lstItem;
	}
}
