using System.Text;

namespace Protocol.Role.C2S
{
	public class SearchRoleByFBInfoCmd : BaseCmd
	{
		private ushort m_param_len;

		public string m_param;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			ushort num = (ushort)m_param.Length;
			bufferWriter.PushUInt16(num);
			byte[] bytes = Encoding.UTF8.GetBytes(m_param);
			bufferWriter.PushByteArray(bytes, num);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 58;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
