namespace Protocol.RPG.S2C
{
	public class BuyMobilityResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kFull = 1,
			kNotEnough = 2
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
