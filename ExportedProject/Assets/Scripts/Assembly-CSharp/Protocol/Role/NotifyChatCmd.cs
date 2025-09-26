using System.Text;

namespace Protocol.Role
{
	public class NotifyChatCmd
	{
		public string m_sender_name;

		public uint m_sender_id;

		public byte m_channel;

		public string m_content;

		public bool Parse(BufferReader reader)
		{
			byte[] val = new byte[33];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_sender_name = Encoding.UTF8.GetString(val);
			int num = m_sender_name.IndexOf('\0');
			if (num != -1)
			{
				m_sender_name = m_sender_name.Substring(0, m_sender_name.IndexOf('\0'));
			}
			reader.PopUInt32(ref m_sender_id);
			reader.PopByte(ref m_channel);
			ushort val2 = 0;
			if (!reader.PopUInt16(ref val2))
			{
				return false;
			}
			byte[] val3 = new byte[val2];
			if (!reader.PopByteArray(ref val3, val3.Length))
			{
				return false;
			}
			m_content = Encoding.UTF8.GetString(val3);
			return true;
		}
	}
}
