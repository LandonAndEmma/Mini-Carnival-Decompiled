namespace Protocol.Role.S2C
{
	public class UploadRoleDataResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kError = 1
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
