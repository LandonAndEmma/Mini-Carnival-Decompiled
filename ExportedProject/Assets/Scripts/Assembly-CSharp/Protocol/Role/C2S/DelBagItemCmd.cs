namespace Protocol.Role.C2S
{
	public class DelBagItemCmd : BaseCmd
	{
		public ulong m_unique_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt64(m_unique_id);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 36;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
