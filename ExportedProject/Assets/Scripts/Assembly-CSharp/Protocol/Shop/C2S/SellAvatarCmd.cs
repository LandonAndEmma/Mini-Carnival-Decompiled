namespace Protocol.Shop.C2S
{
	public class SellAvatarCmd : BaseCmd
	{
		public ulong m_head_id;

		public ulong m_body_id;

		public ulong m_leg_id;

		public byte m_remain_num;

		public byte m_price_type;

		public uint m_price;

		public byte m_is_ad;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt64(m_head_id);
			bufferWriter.PushUInt64(m_body_id);
			bufferWriter.PushUInt64(m_leg_id);
			bufferWriter.PushByte(m_remain_num);
			bufferWriter.PushByte(m_price_type);
			bufferWriter.PushUInt32(m_price);
			bufferWriter.PushByte(m_is_ad);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 4;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
