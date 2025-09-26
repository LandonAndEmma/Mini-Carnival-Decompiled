namespace Protocol.RPG.C2S
{
	public class ChangeMemberCmd : BaseCmd
	{
		public byte m_pos;

		public uint m_card_id;

		public ulong m_card_unique_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_pos);
			bufferWriter.PushUInt32(m_card_id);
			bufferWriter.PushUInt64(m_card_unique_id);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 16;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
