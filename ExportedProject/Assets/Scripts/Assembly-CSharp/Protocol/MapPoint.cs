namespace Protocol
{
	public class MapPoint
	{
		public enum EState
		{
			kLock = 0,
			kNpc = 1,
			KGold = 2,
			kPlayer = 3
		}

		public byte m_index;

		public byte m_status;

		public uint m_role_id;

		public uint m_start_time;

		public MapPoint()
		{
			m_index = 0;
			m_status = 0;
			m_role_id = 0u;
			m_start_time = 0u;
		}
	}
}
