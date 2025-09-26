namespace Protocol.RPG.C2S
{
	public class DeleteEquipCmd : BaseCmd
	{
		public ulong m_equip_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt64(m_equip_id);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 55;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
