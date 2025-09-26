namespace Protocol.Hall.S2C
{
	public class NotifyMoveRoleCmd
	{
		public uint m_who;

		public Position m_pos = new Position();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_who))
			{
				return false;
			}
			uint val = 0u;
			if (!reader.PopUInt32(ref val))
			{
				return false;
			}
			m_pos.m_x = (int)val;
			if (!reader.PopUInt32(ref val))
			{
				return false;
			}
			m_pos.m_y = (int)val;
			if (!reader.PopUInt32(ref val))
			{
				return false;
			}
			m_pos.m_z = (int)val;
			if (!reader.PopUInt32(ref val))
			{
				return false;
			}
			m_pos.m_d = (int)val;
			return true;
		}
	}
}
