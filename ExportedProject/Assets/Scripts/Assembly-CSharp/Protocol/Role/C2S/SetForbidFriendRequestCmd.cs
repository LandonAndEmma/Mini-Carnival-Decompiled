namespace Protocol.Role.C2S
{
	public class SetForbidFriendRequestCmd : BaseCmd
	{
		public enum State
		{
			kNo = 0,
			kYes = 1
		}

		public byte m_val;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_val);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 78;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
