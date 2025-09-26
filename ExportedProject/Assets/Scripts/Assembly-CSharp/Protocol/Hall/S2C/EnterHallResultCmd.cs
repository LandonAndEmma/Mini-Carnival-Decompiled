namespace Protocol.Hall.S2C
{
	public class EnterHallResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kNoChange = 1
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
