namespace Protocol.RPG.S2C
{
	public class NotifyCouponsCmd
	{
		public uint m_coupons;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_coupons))
			{
				return false;
			}
			return true;
		}
	}
}
