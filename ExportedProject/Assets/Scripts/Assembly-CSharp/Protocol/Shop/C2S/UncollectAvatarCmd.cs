namespace Protocol.Shop.C2S
{
	public class UncollectAvatarCmd : BaseCmd
	{
		public uint m_id;

		public byte m_param;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_id);
			bufferWriter.PushByte(m_param);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 13;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
