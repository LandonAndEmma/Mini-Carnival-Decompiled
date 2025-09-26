namespace Protocol.Role
{
	public class AddBagItemResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kBagFull = 1
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
