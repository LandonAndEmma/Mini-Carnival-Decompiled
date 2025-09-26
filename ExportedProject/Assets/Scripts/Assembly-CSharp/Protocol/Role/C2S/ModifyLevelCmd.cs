namespace Protocol.Role.C2S
{
	public class ModifyLevelCmd : BaseCmd
	{
		public uint m_level;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_level);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 16;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
