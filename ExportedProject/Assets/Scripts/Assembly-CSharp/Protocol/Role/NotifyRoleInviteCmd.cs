using System.Text;

namespace Protocol.Role
{
	public class NotifyRoleInviteCmd
	{
		public uint m_who;

		public string m_whoname;

		private ushort m_param_len;

		public string m_param;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_who))
			{
				return false;
			}
			byte[] val = new byte[33];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_whoname = Encoding.UTF8.GetString(val);
			int num = m_whoname.IndexOf('\0');
			if (num != -1)
			{
				m_whoname = m_whoname.Substring(0, m_whoname.IndexOf('\0'));
			}
			if (!reader.PopUInt16(ref m_param_len))
			{
				return false;
			}
			byte[] val2 = new byte[m_param_len];
			if (!reader.PopByteArray(ref val2, val2.Length))
			{
				return false;
			}
			m_param = Encoding.UTF8.GetString(val2);
			int num2 = m_param.IndexOf('\0');
			if (num2 != -1)
			{
				m_param = m_param.Substring(0, m_param.IndexOf('\0'));
			}
			return true;
		}
	}
}
