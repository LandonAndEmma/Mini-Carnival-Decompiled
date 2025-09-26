namespace Protocol.RPG.S2C
{
	public class BuyCouponsShopResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kNotEnough = 1
		}

		public byte m_result;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			return true;
		}
	}
}
