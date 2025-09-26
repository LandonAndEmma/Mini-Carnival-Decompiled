namespace Protocol.Shop.C2S
{
	public class SetFileDataCmd : BaseCmd
	{
		public ulong m_mark;

		public byte[] m_data;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt64(m_mark);
			if (m_data == null || m_data.Length == 0)
			{
				bufferWriter.PushUInt32(0u);
			}
			else
			{
				bufferWriter.PushUInt32((uint)m_data.Length);
				bufferWriter.PushByteArray(m_data, m_data.Length);
			}
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 0;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
