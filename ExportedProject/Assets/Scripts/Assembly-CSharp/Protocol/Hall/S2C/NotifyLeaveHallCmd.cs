namespace Protocol.Hall.S2C
{
	public class NotifyLeaveHallCmd
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
