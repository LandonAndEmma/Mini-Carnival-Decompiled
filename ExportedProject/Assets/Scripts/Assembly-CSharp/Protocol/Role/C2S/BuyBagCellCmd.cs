namespace Protocol.Role.C2S
{
	public class BuyBagCellCmd : BaseCmd
	{
		public byte m_buy_num;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_buy_num);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 40;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
