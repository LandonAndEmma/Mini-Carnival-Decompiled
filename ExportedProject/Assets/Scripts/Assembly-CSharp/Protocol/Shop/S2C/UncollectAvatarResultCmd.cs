namespace Protocol.Shop.S2C
{
	public class UncollectAvatarResultCmd
	{
		public enum Code
		{
			kOk = 0
		}

		public byte m_result;

		public uint m_id;

		public byte m_param;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_id))
			{
				return false;
			}
			if (!reader.PopByte(ref m_param))
			{
				return false;
			}
			return true;
		}
	}
}
