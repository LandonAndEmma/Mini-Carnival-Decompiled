namespace Protocol.Shop.S2C
{
	public class AdAvatarResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kMiss = 1,
			kNoEnough = 2,
			kError = 3
		}

		public byte m_result;

		public uint m_id;

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
			return true;
		}
	}
}
