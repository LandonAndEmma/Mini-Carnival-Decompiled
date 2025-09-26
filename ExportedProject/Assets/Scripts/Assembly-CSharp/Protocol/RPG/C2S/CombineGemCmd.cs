namespace Protocol.RPG.C2S
{
	public class CombineGemCmd : BaseCmd
	{
		public ushort m_gem_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt16(m_gem_id);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 8;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
