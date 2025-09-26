namespace Protocol.Role.C2S
{
	public class GetAllModelNumCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 76;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
