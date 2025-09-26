namespace Protocol.Role
{
	public class EstablishFriendResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kFull = 1,
			kWhoFull = 2
		}

		public byte m_result;

		public uint m_who;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_who))
			{
				return false;
			}
			return true;
		}
	}
}
