using System.Text;

namespace Protocol.Shop.S2C
{
	public class NotifyRoleShopAddCmd
	{
		public ShopItem m_item = new ShopItem();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_item.m_id))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_item.m_author))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_item.m_ad_time))
			{
				return false;
			}
			if (!reader.PopByte(ref m_item.m_type))
			{
				return false;
			}
			if (!reader.PopByte(ref m_item.m_remain_num))
			{
				return false;
			}
			if (!reader.PopByte(ref m_item.m_price_type))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_item.m_price))
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				byte[] val = new byte[33];
				if (!reader.PopByteArray(ref val, val.Length))
				{
					return false;
				}
				m_item.m_unit[i] = Encoding.UTF8.GetString(val);
				int num = m_item.m_unit[i].IndexOf('\0');
				if (num != -1)
				{
					m_item.m_unit[i] = m_item.m_unit[i].Substring(0, m_item.m_unit[i].IndexOf('\0'));
				}
			}
			if (!reader.PopUInt32(ref m_item.m_praise))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_item.m_create_time))
			{
				return false;
			}
			return true;
		}
	}
}
