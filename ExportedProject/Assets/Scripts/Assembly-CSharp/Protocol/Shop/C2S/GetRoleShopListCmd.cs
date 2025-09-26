namespace Protocol.Shop.C2S
{
	public class GetRoleShopListCmd : BaseCmd
	{
		public uint m_who;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_who);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 25;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
