namespace Protocol.Shop.C2S
{
	public class PraiseAvatarCmd : BaseCmd
	{
		public uint m_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_id);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 21;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
