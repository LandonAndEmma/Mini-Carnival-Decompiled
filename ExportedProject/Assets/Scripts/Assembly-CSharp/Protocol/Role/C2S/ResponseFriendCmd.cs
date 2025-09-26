namespace Protocol.Role.C2S
{
	public class ResponseFriendCmd : BaseCmd
	{
		public enum Op
		{
			kAgree = 0,
			kIgnore = 1
		}

		public uint m_mail_id;

		public byte m_op_code;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_mail_id);
			bufferWriter.PushByte(m_op_code);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 47;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
