namespace Protocol.Role.S2C
{
	public class LoginResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kNeedRegister = 1,
			kFull = 2
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
