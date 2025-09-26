using System.Collections.Generic;

namespace Protocol.RPG.S2C
{
	public class RequestPickCardsResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kCardFull = 1,
			kCouponsNotEnough = 2
		}

		public class Item
		{
			public uint m_card_id;

			public ulong m_unique_id;

			public byte m_new;
		}

		public byte m_result;

		public byte m_card_num;

		public List<Item> m_card_list = new List<Item>();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopByte(ref m_card_num))
			{
				return false;
			}
			for (byte b = 0; b < m_card_num; b++)
			{
				Item item = new Item();
				if (!reader.PopUInt32(ref item.m_card_id))
				{
					return false;
				}
				if (!reader.PopUInt64(ref item.m_unique_id))
				{
					return false;
				}
				if (!reader.PopByte(ref item.m_new))
				{
					return false;
				}
				m_card_list.Add(item);
			}
			return true;
		}
	}
}
