namespace Protocol.Shop.S2C
{
	public class PraiseAvatarResultCmd
	{
		public uint m_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_id))
			{
				return false;
			}
			return true;
		}
	}
}
