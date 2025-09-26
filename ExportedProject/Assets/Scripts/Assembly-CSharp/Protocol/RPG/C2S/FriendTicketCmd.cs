namespace Protocol.RPG.C2S
{
	public class FriendTicketCmd : BaseCmd
	{
		public uint m_friend_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_friend_id);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 58;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
