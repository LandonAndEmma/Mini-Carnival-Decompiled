namespace Protocol.RPG.C2S
{
	public class RequestCombineCardCmd : BaseCmd
	{
		public class Card
		{
			public uint m_card_id;

			public ulong m_unique_id;
		}

		public byte m_star;

		public Card[] m_card_list = new Card[6];

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_star);
			for (int i = 0; i < 6; i++)
			{
				bufferWriter.PushUInt32(m_card_list[i].m_card_id);
				bufferWriter.PushUInt64(m_card_list[i].m_unique_id);
			}
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 6;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
