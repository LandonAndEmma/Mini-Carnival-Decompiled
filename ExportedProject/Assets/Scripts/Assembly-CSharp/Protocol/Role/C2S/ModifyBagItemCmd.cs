using System.Text;

namespace Protocol.Role.C2S
{
	public class ModifyBagItemCmd : BaseCmd
	{
		public ulong m_unique_id;

		public string m_unit;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt64(m_unique_id);
			byte[] bytes = Encoding.UTF8.GetBytes(m_unit);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 34;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
