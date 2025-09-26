using System.Text;

namespace Protocol.Rank.C2S
{
	public class UpdateScoreCmd : BaseCmd
	{
		public enum Code
		{
			kReplace = 0,
			kAdd = 1
		}

		public string m_rank_name;

		public uint m_score;

		public byte m_op;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_rank_name);
			if (bytes.Length >= 17)
			{
				bytes[16] = 0;
			}
			bufferWriter.PushByteArray(bytes, 17);
			bufferWriter.PushUInt32(m_score);
			bufferWriter.PushByte(m_op);
			Header header = new Header();
			header.m_cProtocol = 5;
			header.m_cCmd = 0;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
