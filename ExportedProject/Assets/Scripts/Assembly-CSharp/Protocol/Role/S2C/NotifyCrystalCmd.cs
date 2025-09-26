namespace Protocol.Role.S2C
{
	public class NotifyCrystalCmd
	{
		public uint m_crystal;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_crystal))
			{
				return false;
			}
			return true;
		}
	}
}
