using System.Text;

namespace Protocol.Shop.C2S
{
	public class GetFileDataCmd : BaseCmd
	{
		public string m_md5;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_md5);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 2;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
