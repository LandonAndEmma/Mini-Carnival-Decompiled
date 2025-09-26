namespace Protocol.Role
{
	public class GetForbidFriendRequestResultCmd
	{
		public byte m_val;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_val))
			{
				return false;
			}
			return true;
		}
	}
}
