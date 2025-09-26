namespace Protocol.RPG.S2C
{
	public class NotifyMobilityCmd
	{
		public uint m_mobility_time;

		public uint m_srv_time;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_mobility_time))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_srv_time))
			{
				return false;
			}
			return true;
		}
	}
}
