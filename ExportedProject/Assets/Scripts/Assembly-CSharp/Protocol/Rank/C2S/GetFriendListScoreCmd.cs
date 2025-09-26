using System.Text;

namespace Protocol.Rank.C2S
{
	public class GetFriendListScoreCmd : BaseCmd
	{
		public string m_rank_name;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_rank_name);
			if (bytes.Length >= 17)
			{
				bytes[16] = 0;
			}
			bufferWriter.PushByteArray(bytes, 17);
			Header header = new Header();
			header.m_cProtocol = 5;
			header.m_cCmd = 6;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
