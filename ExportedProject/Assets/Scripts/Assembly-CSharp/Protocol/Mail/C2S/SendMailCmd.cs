using System.Text;

namespace Protocol.Mail.C2S
{
	public class SendMailCmd : BaseCmd
	{
		public Email mail;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(mail.m_id);
			bufferWriter.PushByte(mail.m_type);
			bufferWriter.PushByte(mail.m_status);
			bufferWriter.PushUInt32(mail.m_time);
			bufferWriter.PushUInt32(mail.m_receiver);
			bufferWriter.PushUInt32(mail.m_sender);
			byte[] bytes = Encoding.UTF8.GetBytes(mail.m_sender_name);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			byte[] bytes2 = Encoding.UTF8.GetBytes(mail.m_title);
			if (bytes2.Length >= 128)
			{
				bytes2[127] = 0;
			}
			bufferWriter.PushByteArray(bytes2, 128);
			byte[] bytes3 = Encoding.UTF8.GetBytes(mail.m_content);
			if (bytes3.Length >= 1024)
			{
				bytes3[1023] = 0;
			}
			bufferWriter.PushByteArray(bytes3, 1024);
			byte[] bytes4 = Encoding.UTF8.GetBytes(mail.m_attach);
			if (bytes4.Length >= 512)
			{
				bytes4[511] = 0;
			}
			bufferWriter.PushByteArray(bytes4, 512);
			Header header = new Header();
			header.m_cProtocol = 4;
			header.m_cCmd = 4;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
