namespace Protocol.Role
{
	public class SyncServerTimeResultCmd
	{
		public uint m_local_time;

		public uint m_server_time;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_local_time))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_server_time))
			{
				return false;
			}
			return true;
		}
	}
}
