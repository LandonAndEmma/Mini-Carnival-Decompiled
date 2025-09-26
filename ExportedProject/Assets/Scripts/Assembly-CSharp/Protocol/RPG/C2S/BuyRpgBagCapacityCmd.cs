namespace Protocol.RPG.C2S
{
	public class BuyRpgBagCapacityCmd : BaseCmd
	{
		public enum BagType
		{
			kCardBag = 0,
			kEquipBag = 1
		}

		public byte m_bag_type;

		public byte m_buy_num;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_bag_type);
			bufferWriter.PushByte(m_buy_num);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 20;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
