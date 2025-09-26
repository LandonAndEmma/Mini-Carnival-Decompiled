namespace Protocol.RPG.S2C
{
	public class DragPlayerRpgDataErrorCmd
	{
		public enum Code
		{
			kNoData = 0
		}

		public uint m_role_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_role_id))
			{
				return false;
			}
			return true;
		}
	}
}
