namespace Protocol.RPG.S2C
{
	public class ChangeMemberResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kDataError = 1
		}

		public byte m_result;

		public byte m_pos;

		public uint m_member_card;

		public ulong m_member_unique_id;

		public uint m_unmember_card;

		public ulong m_unmember_unique_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopByte(ref m_pos))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_member_card))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_member_unique_id))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_unmember_card))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_unmember_unique_id))
			{
				return false;
			}
			return true;
		}
	}
}
