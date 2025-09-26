namespace Protocol.Shop.S2C
{
	public class SellAvatarResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kFull = 1,
			kNoEnough = 2,
			kError = 3
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
