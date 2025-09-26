namespace Protocol.RPG.S2C
{
	public class NotifyExpCmd
	{
		public uint m_rpg_level;

		public uint m_next_exp;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_rpg_level))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_next_exp))
			{
				return false;
			}
			return true;
		}
	}
}
