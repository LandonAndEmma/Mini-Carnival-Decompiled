namespace Protocol.RPG.S2C
{
	public class BuyRpgBagCapacityResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kPriceNotEnough = 1,
			kFull = 2
		}

		public byte m_result;

		public byte m_bag_type;

		public uint m_current_capacity;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopByte(ref m_bag_type))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_current_capacity))
			{
				return false;
			}
			return true;
		}
	}
}
