namespace Protocol.Role
{
	public class ResponseFriendResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kFull = 1,
			kWhoFull = 2,
			kError = 3
		}

		public byte m_result;

		public uint m_mail_id;

		public byte m_op_code;

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
			if (!reader.PopByte(ref m_op_code))
			{
				return false;
			}
			return true;
		}
	}
}
