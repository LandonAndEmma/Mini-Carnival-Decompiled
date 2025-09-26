namespace Protocol.RPG.C2S
{
	public class BuyCouponsShopCmd : BaseCmd
	{
		public enum Code
		{
			kFive = 0,
			kFifty = 1
		}

		public byte m_index;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_index);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 48;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
