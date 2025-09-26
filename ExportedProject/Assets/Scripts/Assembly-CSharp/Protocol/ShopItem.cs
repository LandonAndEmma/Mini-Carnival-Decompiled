namespace Protocol
{
	public class ShopItem
	{
		public enum Type
		{
			unknow = 0,
			suit = 1,
			nake = 2,
			decoration = 3
		}

		public enum Price
		{
			unknown = 0,
			gold = 1,
			crystal = 2,
			heart = 3
		}

		public uint m_id;

		public uint m_author;

		public uint m_ad_time;

		public byte m_type;

		public byte m_remain_num;

		public byte m_price_type;

		public uint m_price;

		public string[] m_unit = new string[3];

		public uint m_praise;

		public uint m_create_time;
	}
}
