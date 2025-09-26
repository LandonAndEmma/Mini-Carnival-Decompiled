using System.Text;

namespace Protocol.Role.C2S
{
	public class ModifyNicknameCmd : BaseCmd
	{
		public string m_nickName;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_nickName);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 64;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
