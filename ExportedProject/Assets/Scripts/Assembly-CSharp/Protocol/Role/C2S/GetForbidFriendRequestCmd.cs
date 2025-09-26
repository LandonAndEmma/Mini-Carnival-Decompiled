namespace Protocol.Role.C2S
{
	public class GetForbidFriendRequestCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 79;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
