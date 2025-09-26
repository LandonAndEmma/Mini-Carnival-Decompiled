using System.Collections.Generic;
using System.Text;

namespace Protocol.Shop.S2C
{
	public class GetRoleCollectListResultCmd
	{
		public List<ShopItem> m_list = new List<ShopItem>();

		public bool Parse(BufferReader reader)
		{
			byte val = 0;
			if (!reader.PopByte(ref val))
			{
				return false;
			}
			if (val > 0)
			{
				for (byte b = 0; b < val; b++)
				{
					ShopItem shopItem = new ShopItem();
					if (!reader.PopUInt32(ref shopItem.m_id))
					{
						return false;
					}
					if (!reader.PopUInt32(ref shopItem.m_author))
					{
						return false;
					}
					if (!reader.PopUInt32(ref shopItem.m_ad_time))
					{
						return false;
					}
					if (!reader.PopByte(ref shopItem.m_type))
					{
						return false;
					}
					if (!reader.PopByte(ref shopItem.m_remain_num))
					{
						return false;
					}
					if (!reader.PopByte(ref shopItem.m_price_type))
					{
						return false;
					}
					if (!reader.PopUInt32(ref shopItem.m_price))
					{
						return false;
					}
					for (int i = 0; i < 3; i++)
					{
						byte[] val2 = new byte[33];
						if (!reader.PopByteArray(ref val2, val2.Length))
						{
							return false;
						}
						shopItem.m_unit[i] = Encoding.UTF8.GetString(val2);
						int num = shopItem.m_unit[i].IndexOf('\0');
						if (num != -1)
						{
							shopItem.m_unit[i] = shopItem.m_unit[i].Substring(0, shopItem.m_unit[i].IndexOf('\0'));
						}
					}
					if (!reader.PopUInt32(ref shopItem.m_praise))
					{
						return false;
					}
					if (!reader.PopUInt32(ref shopItem.m_create_time))
					{
						return false;
					}
					m_list.Add(shopItem);
				}
			}
			return true;
		}
	}
}
