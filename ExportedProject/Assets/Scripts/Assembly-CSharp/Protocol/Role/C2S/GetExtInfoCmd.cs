namespace Protocol.Role.C2S
{
	public class GetExtInfoCmd : BaseCmd
	{
		public uint m_who;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_who);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 62;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
