namespace Protocol.RPG.S2C
{
	public class NotifyCardDelCmd
	{
		public uint m_card_id;

		public ulong m_unique_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_card_id))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_unique_id))
			{
				return false;
			}
			return true;
		}
	}
}
