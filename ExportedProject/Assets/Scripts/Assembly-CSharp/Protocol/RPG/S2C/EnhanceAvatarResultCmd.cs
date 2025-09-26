namespace Protocol.RPG.S2C
{
	public class EnhanceAvatarResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kGoldNotEnough = 1,
			kDataError = 2,
			kNoSpace = 3
		}

		public byte m_result;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			return true;
		}
	}
}
