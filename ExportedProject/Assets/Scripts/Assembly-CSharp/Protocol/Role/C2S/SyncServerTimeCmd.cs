namespace Protocol.Role.C2S
{
	public class SyncServerTimeCmd : BaseCmd
	{
		public uint m_local_time;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_local_time);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 70;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
