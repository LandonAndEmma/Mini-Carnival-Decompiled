namespace Protocol.RPG.C2S
{
	public class DragPlayerRpgDataCmd : BaseCmd
	{
		public uint m_role_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_role_id);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 36;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
