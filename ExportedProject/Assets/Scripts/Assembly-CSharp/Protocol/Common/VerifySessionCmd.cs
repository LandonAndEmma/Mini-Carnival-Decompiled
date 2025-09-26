namespace Protocol.Common
{
	public class VerifySessionCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 0;
			header.m_cCmd = 4;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
