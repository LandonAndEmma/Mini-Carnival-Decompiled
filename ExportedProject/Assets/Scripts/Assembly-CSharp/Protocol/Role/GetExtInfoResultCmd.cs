namespace Protocol.Role
{
	public class GetExtInfoResultCmd
	{
		public uint m_Who;

		public ExtInfo m_extInfo = new ExtInfo();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_Who))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_extInfo.m_fans_num))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_extInfo.m_sells_num))
			{
				return false;
			}
			return true;
		}
	}
}
