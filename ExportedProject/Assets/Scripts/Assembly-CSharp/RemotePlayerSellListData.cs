using System.Collections.Generic;
using Protocol;

public class RemotePlayerSellListData
{
	public uint _id;

	public List<ShopItem> _lstShopItem;

	public RemotePlayerSellListData(uint id, List<ShopItem> lstItem)
	{
		_id = id;
		_lstShopItem = lstItem;
	}
}
