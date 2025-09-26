namespace Protocol.Mail.C2S
{
	public class GainMailCmd : BaseCmd
	{
		public uint m_mail_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_mail_id);
			Header header = new Header();
			header.m_cProtocol = 4;
			header.m_cCmd = 8;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
