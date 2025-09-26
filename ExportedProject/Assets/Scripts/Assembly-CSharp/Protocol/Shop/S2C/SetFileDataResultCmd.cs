using System.Text;

namespace Protocol.Shop.S2C
{
	public class SetFileDataResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kBig = 1,
			kError = 2
		}

		public byte m_result;

		public ulong m_mark;

		public string m_md5;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_mark))
			{
				return false;
			}
			byte[] val = new byte[33];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_md5 = Encoding.UTF8.GetString(val);
			int num = m_md5.IndexOf('\0');
			if (num != -1)
			{
				m_md5 = m_md5.Substring(0, m_md5.IndexOf('\0'));
			}
			return true;
		}
	}
}
