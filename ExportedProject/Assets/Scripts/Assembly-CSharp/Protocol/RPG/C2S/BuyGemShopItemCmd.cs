namespace Protocol.RPG.C2S
{
	public class BuyGemShopItemCmd : BaseCmd
	{
		public byte m_day;

		public uint m_item_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_day);
			bufferWriter.PushUInt32(m_item_id);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 14;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
