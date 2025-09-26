using System.Text;

namespace Protocol.Role.C2S
{
	public class ResetFaceImageCmd : BaseCmd
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
			header.m_cProtocol = 1;
			header.m_cCmd = 73;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
