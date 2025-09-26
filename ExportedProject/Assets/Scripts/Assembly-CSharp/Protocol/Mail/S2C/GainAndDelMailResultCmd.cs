namespace Protocol.Mail.S2C
{
	public class GainAndDelMailResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kError = 1,
			kBagFull = 2
		}

		public byte m_result;

		public uint m_mail_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_mail_id))
			{
				return false;
			}
			return true;
		}
	}
}
