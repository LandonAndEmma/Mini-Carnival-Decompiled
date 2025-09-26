namespace Protocol.Role.S2C
{
	public class NotifyFriendOnlineCmd
	{
		public uint m_who;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_who))
			{
				return false;
			}
			return true;
		}
	}
}
