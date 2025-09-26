using System;
using System.Collections.Generic;

namespace Protocol.RPG.S2C
{
	public class GetFriendMedalListResultCmd
	{
		public class Item : IComparable
		{
			public uint m_player_id;

			public uint m_medal;

			public int CompareTo(object obj)
			{
				Item item = obj as Item;
				if (item.m_medal == m_medal)
				{
					return 0;
				}
				return (m_medal < item.m_medal) ? 1 : (-1);
			}
		}

		public byte m_count;

		public List<Item> m_list = new List<Item>();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_count))
			{
				return false;
			}
			m_list.Clear();
			for (int i = 0; i < m_count; i++)
			{
				Item item = new Item();
				if (!reader.PopUInt32(ref item.m_player_id))
				{
					return false;
				}
				if (!reader.PopUInt32(ref item.m_medal))
				{
					return false;
				}
				m_list.Add(item);
			}
			return true;
		}
	}
}
