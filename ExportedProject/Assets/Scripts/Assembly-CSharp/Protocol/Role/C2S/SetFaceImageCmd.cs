using System.Text;

namespace Protocol.Role.C2S
{
	public class SetFaceImageCmd : BaseCmd
	{
		public string m_facebookid;

		public string m_md5;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_facebookid);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			byte[] bytes2 = Encoding.UTF8.GetBytes(m_md5);
			if (bytes2.Length >= 33)
			{
				bytes2[32] = 0;
			}
			bufferWriter.PushByteArray(bytes2, 33);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 56;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
