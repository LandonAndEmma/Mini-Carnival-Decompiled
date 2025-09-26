using System.Text;

namespace Protocol.Role
{
	public class NotifyGlobalMessageCmd
	{
		public ushort m_msg_len;

		public string m_msg;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt16(ref m_msg_len))
			{
				return false;
			}
			byte[] val = new byte[m_msg_len];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_msg = Encoding.UTF8.GetString(val);
			int num = m_msg.IndexOf('\0');
			if (num != -1)
			{
				m_msg = m_msg.Substring(0, m_msg.IndexOf('\0'));
			}
			return true;
		}
	}
}
