namespace Protocol.Shop.S2C
{
	public class FollowRoleShopResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kFull = 1
		}

		public byte m_result;

		public uint m_follow_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_follow_id))
			{
				return false;
			}
			return true;
		}
	}
}
