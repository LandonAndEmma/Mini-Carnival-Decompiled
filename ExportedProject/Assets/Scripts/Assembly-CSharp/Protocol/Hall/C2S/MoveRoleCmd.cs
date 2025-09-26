namespace Protocol.Hall.C2S
{
	public class MoveRoleCmd : BaseCmd
	{
		public Position m_pos;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32((uint)m_pos.m_x);
			bufferWriter.PushUInt32((uint)m_pos.m_y);
			bufferWriter.PushUInt32((uint)m_pos.m_z);
			bufferWriter.PushUInt32((uint)m_pos.m_d);
			Header header = new Header();
			header.m_cProtocol = 3;
			header.m_cCmd = 4;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
