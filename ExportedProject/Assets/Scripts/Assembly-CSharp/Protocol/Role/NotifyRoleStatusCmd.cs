namespace Protocol.Role
{
	public class NotifyRoleStatusCmd
	{
		public uint m_who;

		public uint m_status;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_who))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_status))
			{
				return false;
			}
			return true;
		}
	}
}
