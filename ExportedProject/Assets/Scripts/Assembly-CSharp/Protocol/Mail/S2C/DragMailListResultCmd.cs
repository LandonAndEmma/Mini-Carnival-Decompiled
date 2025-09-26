using System.Collections.Generic;
using System.Text;

namespace Protocol.Mail.S2C
{
	public class DragMailListResultCmd
	{
		public uint m_num_of_mail;

		public byte m_num_of_new;

		public List<Email> m_mail_list = new List<Email>();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_num_of_mail))
			{
				return false;
			}
			if (!reader.PopByte(ref m_num_of_new))
			{
				return false;
			}
			byte val = 0;
			if (!reader.PopByte(ref val))
			{
				return false;
			}
			for (byte b = 0; b < val; b++)
			{
				Email email = new Email();
				if (!reader.PopUInt32(ref email.m_id))
				{
					return false;
				}
				if (!reader.PopByte(ref email.m_type))
				{
					return false;
				}
				if (!reader.PopByte(ref email.m_status))
				{
					return false;
				}
				if (!reader.PopUInt32(ref email.m_time))
				{
					return false;
				}
				if (!reader.PopUInt32(ref email.m_receiver))
				{
					return false;
				}
				if (!reader.PopUInt32(ref email.m_sender))
				{
					return false;
				}
				byte[] val2 = new byte[33];
				if (!reader.PopByteArray(ref val2, val2.Length))
				{
					return false;
				}
				email.m_sender_name = Encoding.UTF8.GetString(val2);
				int num = email.m_sender_name.IndexOf('\0');
				if (num != -1)
				{
					email.m_sender_name = email.m_sender_name.Substring(0, email.m_sender_name.IndexOf('\0'));
				}
				byte[] val3 = new byte[128];
				if (!reader.PopByteArray(ref val3, val3.Length))
				{
					return false;
				}
				email.m_title = Encoding.UTF8.GetString(val3);
				int num2 = email.m_title.IndexOf('\0');
				if (num2 != -1)
				{
					email.m_title = email.m_title.Substring(0, email.m_title.IndexOf('\0'));
				}
				byte[] val4 = new byte[1024];
				if (!reader.PopByteArray(ref val4, val4.Length))
				{
					return false;
				}
				email.m_content = Encoding.UTF8.GetString(val4);
				int num3 = email.m_content.IndexOf('\0');
				if (num3 != -1)
				{
					email.m_content = email.m_content.Substring(0, email.m_content.IndexOf('\0'));
				}
				byte[] val5 = new byte[512];
				if (!reader.PopByteArray(ref val5, val5.Length))
				{
					return false;
				}
				email.m_attach = Encoding.UTF8.GetString(val5);
				int num4 = email.m_attach.IndexOf('\0');
				if (num4 != -1)
				{
					email.m_attach = email.m_attach.Substring(0, email.m_attach.IndexOf('\0'));
				}
				m_mail_list.Add(email);
			}
			return true;
		}
	}
}
