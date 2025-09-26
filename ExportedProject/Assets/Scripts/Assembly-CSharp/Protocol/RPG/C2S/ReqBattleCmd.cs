namespace Protocol.RPG.C2S
{
	public class ReqBattleCmd : BaseCmd
	{
		public byte m_map_point;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_map_point);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 32;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
