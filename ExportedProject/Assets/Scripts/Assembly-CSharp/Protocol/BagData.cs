using System.Collections.Generic;

namespace Protocol
{
	public class BagData
	{
		public byte m_first_buy_head;

		public byte m_first_buy_body;

		public byte m_first_buy_leg;

		public byte m_bag_capacity;

		public List<BagItem> m_bag_list = new List<BagItem>();
	}
}
