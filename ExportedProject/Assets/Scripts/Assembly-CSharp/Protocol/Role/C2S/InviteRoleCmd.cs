using System.Text;

namespace Protocol.Role.C2S
{
	public class InviteRoleCmd : BaseCmd
	{
		public uint m_who;

		private ushort m_param_len;

		public string m_param;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_who);
			m_param_len = (ushort)m_param.Length;
			bufferWriter.PushUInt16(m_param_len);
			byte[] bytes = Encoding.UTF8.GetBytes(m_param);
			bufferWriter.PushByteArray(bytes, m_param_len);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 66;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
