namespace Protocol.RPG.C2S
{
	public class ReqGainAllGoldCmd : BaseCmd
	{
		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 64;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
