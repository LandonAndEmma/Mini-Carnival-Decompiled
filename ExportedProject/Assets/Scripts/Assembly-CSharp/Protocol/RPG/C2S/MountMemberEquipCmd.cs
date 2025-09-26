namespace Protocol.RPG.C2S
{
	public class MountMemberEquipCmd : BaseCmd
	{
		public byte m_member_pos;

		public byte m_part;

		public ulong m_equip;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_member_pos);
			bufferWriter.PushByte(m_part);
			bufferWriter.PushUInt64(m_equip);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 18;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
