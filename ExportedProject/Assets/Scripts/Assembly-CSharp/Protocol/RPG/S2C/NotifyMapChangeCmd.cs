using System.Collections.Generic;

namespace Protocol.RPG.S2C
{
	public class NotifyMapChangeCmd
	{
		public uint m_last_refresh_time;

		public byte m_count;

		public List<MapPoint> m_points = new List<MapPoint>();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_last_refresh_time))
			{
				return false;
			}
			if (!reader.PopByte(ref m_count))
			{
				return false;
			}
			for (uint num = 0u; num < m_count; num++)
			{
				MapPoint mapPoint = new MapPoint();
				if (!reader.PopByte(ref mapPoint.m_index))
				{
					return false;
				}
				if (!reader.PopByte(ref mapPoint.m_status))
				{
					return false;
				}
				if (!reader.PopUInt32(ref mapPoint.m_role_id))
				{
					return false;
				}
				if (!reader.PopUInt32(ref mapPoint.m_start_time))
				{
					return false;
				}
				m_points.Add(mapPoint);
			}
			return true;
		}
	}
}
