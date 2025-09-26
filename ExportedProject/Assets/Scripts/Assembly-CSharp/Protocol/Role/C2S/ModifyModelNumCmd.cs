using System.Text;

namespace Protocol.Role.C2S
{
	public class ModifyModelNumCmd : BaseCmd
	{
		public enum State
		{
			kAdd = 0,
			kDel = 1
		}

		public string m_mode_name;

		public byte m_op;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_mode_name);
			if (bytes.Length >= 17)
			{
				bytes[16] = 0;
			}
			bufferWriter.PushByteArray(bytes, 17);
			bufferWriter.PushByte(m_op);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 75;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
