namespace Protocol.Role.C2S
{
	public class SetImportFBMarkCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 60;
			header.SetBodyLength(0);
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
