namespace Protocol.Role.C2S
{
	public class ReplaceSaveDataCmd : BaseCmd
	{
		public byte[] m_save_data;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32((uint)m_save_data.Length);
			if (m_save_data != null && m_save_data.Length > 0)
			{
				bufferWriter.PushByteArray(m_save_data, m_save_data.Length);
			}
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 54;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
