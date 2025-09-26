namespace Protocol.Common.S2C
{
	public class SessionKeyCmd
	{
		public byte[] m_key = new byte[256];

		public SessionKeyCmd()
		{
			m_key.Initialize();
		}

		public bool Parser(BufferReader reader)
		{
			if (!reader.PopByteArray(ref m_key, m_key.Length))
			{
				return false;
			}
			return true;
		}
	}
}
