namespace Protocol.Role.C2S
{
	public class ChangeStatusCmd : BaseCmd
	{
		public uint m_status;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_status);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 68;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
