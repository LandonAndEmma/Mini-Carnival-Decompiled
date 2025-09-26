namespace Protocol.Role.S2C
{
	public class NotifyHeartCmd
	{
		public uint m_heart;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_heart))
			{
				return false;
			}
			return true;
		}
	}
}
