namespace Protocol.Shop.C2S
{
	public class FollowRoleShopCmd : BaseCmd
	{
		public uint m_follow_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_follow_id);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 15;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
