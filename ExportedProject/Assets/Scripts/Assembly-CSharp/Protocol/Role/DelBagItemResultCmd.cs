namespace Protocol.Role
{
	public class DelBagItemResultCmd
	{
		public ulong m_unique_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt64(ref m_unique_id))
			{
				return false;
			}
			return true;
		}
	}
}
