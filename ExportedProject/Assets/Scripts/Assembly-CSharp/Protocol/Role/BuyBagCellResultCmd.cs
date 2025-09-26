namespace Protocol.Role
{
	public class BuyBagCellResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kNoEnough = 1,
			kMax = 2
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
