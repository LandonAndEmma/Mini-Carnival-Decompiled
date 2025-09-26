using System.Text;

namespace Protocol.Role.C2S
{
	public class SearchPlayerCmd : BaseCmd
	{
		public byte m_page_size;

		public uint m_page_index;

		public string m_nickname;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_page_size);
			bufferWriter.PushUInt32(m_page_index);
			byte[] bytes = Encoding.UTF8.GetBytes(m_nickname);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 52;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
