namespace Protocol.Role.S2C
{
	public class NotifyGoldCmd
	{
		public uint m_gold;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_gold))
			{
				return false;
			}
			return true;
		}
	}
}
