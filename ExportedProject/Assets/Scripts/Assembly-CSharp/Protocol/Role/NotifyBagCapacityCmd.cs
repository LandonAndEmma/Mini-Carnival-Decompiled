namespace Protocol.Role
{
	public class NotifyBagCapacityCmd
	{
		public byte m_capacity;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_capacity))
			{
				return false;
			}
			return true;
		}
	}
}
