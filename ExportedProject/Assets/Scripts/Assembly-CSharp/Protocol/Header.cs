namespace Protocol
{
	public class Header
	{
		public const uint HEADER_LENGTH = 10u;

		public uint m_iLength;

		public byte m_cProtocol;

		public byte m_cCmd;

		public uint m_iReserve;

		public virtual void SetBodyLength(int body_length)
		{
			m_iLength = (uint)(10 + body_length);
		}

		public void Serialize(BufferWriter writer)
		{
			writer.PushUInt32(m_iLength);
			writer.PushByte(m_cProtocol);
			writer.PushByte(m_cCmd);
			writer.PushUInt32(m_iReserve);
		}

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_iLength))
			{
				return false;
			}
			if (!reader.PopByte(ref m_cProtocol))
			{
				return false;
			}
			if (!reader.PopByte(ref m_cCmd))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_iReserve))
			{
				return false;
			}
			return true;
		}
	}
}
