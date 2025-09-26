namespace Protocol.Shop.C2S
{
	public class UnfollowRoleShopCmd : BaseCmd
	{
		public uint m_follow_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_follow_id);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 17;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
