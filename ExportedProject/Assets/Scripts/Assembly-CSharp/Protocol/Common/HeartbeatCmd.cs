namespace Protocol.Common
{
	public class HeartbeatCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 0;
			header.m_cCmd = 0;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
