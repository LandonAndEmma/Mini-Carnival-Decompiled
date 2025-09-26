using System.Text;

namespace Protocol.Role.C2S
{
	public class ChatCmd : BaseCmd
	{
		public string m_sender_name;

		public uint m_receiver_id;

		public byte m_channel;

		public string m_content;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_sender_name);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			bufferWriter.PushUInt32(m_receiver_id);
			bufferWriter.PushByte(m_channel);
			byte[] bytes2 = Encoding.UTF8.GetBytes(m_content);
			ushort num = (ushort)bytes2.Length;
			bufferWriter.PushUInt16(num);
			bufferWriter.PushByteArray(bytes2, bytes2.Length);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 29;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
