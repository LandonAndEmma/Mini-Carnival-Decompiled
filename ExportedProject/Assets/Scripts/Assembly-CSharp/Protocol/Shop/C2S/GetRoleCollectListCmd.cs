namespace Protocol.Shop.C2S
{
	public class GetRoleCollectListCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 27;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
