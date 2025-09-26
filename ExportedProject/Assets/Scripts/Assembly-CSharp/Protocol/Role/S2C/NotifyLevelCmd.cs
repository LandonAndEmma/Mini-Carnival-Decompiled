namespace Protocol.Role.S2C
{
	public class NotifyLevelCmd
	{
		public uint m_level;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_level))
			{
				return false;
			}
			return true;
		}
	}
}
