namespace Protocol.Role.C2S
{
	public class LoginCmd : BaseCmd
	{
		public uint m_player_id;

		public override void Serialize(BufferWriter writer)
		{
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 0;
			header.m_iReserve = m_player_id;
			header.SetBodyLength(0);
			header.Serialize(writer);
		}
	}
}
