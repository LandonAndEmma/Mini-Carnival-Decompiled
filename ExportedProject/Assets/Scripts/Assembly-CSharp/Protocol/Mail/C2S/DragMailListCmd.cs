namespace Protocol.Mail.C2S
{
	public class DragMailListCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 4;
			header.m_cCmd = 0;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
