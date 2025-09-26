namespace Protocol.Role.C2S
{
	public class ModifyGoldCmd : BaseCmd
	{
		public enum State
		{
			kReplace = 0,
			kAdd = 1
		}

		public byte m_op;

		public int m_val;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_op);
			bufferWriter.PushUInt32((uint)m_val);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 18;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
