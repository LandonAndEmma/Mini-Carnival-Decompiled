namespace Protocol.Role
{
	public class ReplaceSaveDataResultCmd
	{
		public enum Code
		{
			kOk = 0
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
