namespace Protocol
{
	public class Position
	{
		public int m_x;

		public int m_y;

		public int m_z;

		public int m_d;

		public Position()
		{
			m_x = 0;
			m_y = 0;
			m_z = 0;
			m_d = 0;
		}

		public Position(int x, int y, int z, int d)
		{
			m_x = x;
			m_y = y;
			m_z = z;
			m_d = d;
		}
	}
}
