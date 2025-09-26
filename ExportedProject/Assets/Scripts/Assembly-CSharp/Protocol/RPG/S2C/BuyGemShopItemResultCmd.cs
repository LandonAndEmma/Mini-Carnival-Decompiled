namespace Protocol.RPG.S2C
{
	public class BuyGemShopItemResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kPriceNotEnough = 1,
			kDataError = 2
		}

		public byte m_result;

		public ushort m_gem_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt16(ref m_gem_id))
			{
				return false;
			}
			return true;
		}
	}
}
