namespace Protocol.RPG.S2C
{
	public class RecvMobilityErrorCmd
	{
		public enum Code
		{
			kFull = 0,
			kHasRecv = 1
		}

		public byte m_result;

		public ushort m_gem_id;

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
