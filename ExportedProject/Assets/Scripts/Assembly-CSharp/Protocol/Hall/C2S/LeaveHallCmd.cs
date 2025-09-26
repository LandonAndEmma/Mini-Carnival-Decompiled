namespace Protocol.Hall.C2S
{
	public class LeaveHallCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 3;
			header.m_cCmd = 6;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
