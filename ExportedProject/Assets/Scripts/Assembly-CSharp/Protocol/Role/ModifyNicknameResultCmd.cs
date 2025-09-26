using System.Text;

namespace Protocol.Role
{
	public class ModifyNicknameResultCmd
	{
		public enum Code
		{
			kOk = 0
		}

		public byte m_result;

		public string m_nickname;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			byte[] val = new byte[33];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_nickname = Encoding.UTF8.GetString(val);
			int num = m_nickname.IndexOf('\0');
			if (num != -1)
			{
				m_nickname = m_nickname.Substring(0, m_nickname.IndexOf('\0'));
			}
			return true;
		}
	}
}
