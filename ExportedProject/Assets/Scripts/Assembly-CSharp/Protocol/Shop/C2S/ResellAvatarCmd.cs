namespace Protocol.Shop.C2S
{
	public class ResellAvatarCmd : BaseCmd
	{
		public uint m_id;

		public byte m_num;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_id);
			bufferWriter.PushByte(m_num);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 7;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
