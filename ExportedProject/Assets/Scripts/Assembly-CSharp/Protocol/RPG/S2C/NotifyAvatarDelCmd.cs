namespace Protocol.RPG.S2C
{
	public class NotifyAvatarDelCmd
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
