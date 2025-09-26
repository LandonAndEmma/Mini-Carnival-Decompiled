namespace Protocol.RPG.S2C
{
	public class NotifyGemCmd
	{
		public ushort m_gem_id;

		public uint m_num;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt16(ref m_gem_id))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_num))
			{
				return false;
			}
			return true;
		}
	}
}
