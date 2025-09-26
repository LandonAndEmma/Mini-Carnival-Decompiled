namespace Protocol.RPG.S2C
{
	public class CombineCardResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kGoldNotEnough = 1,
			kDataError = 2
		}

		public byte m_result;

		public uint m_card_id;

		public ulong m_unique_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
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
