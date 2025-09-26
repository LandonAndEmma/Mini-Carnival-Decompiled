using System.Text;

namespace Protocol.Shop.S2C
{
	public class GetFileDataResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kMiss = 1
		}

		public byte m_result;

		public string m_md5 = string.Empty;

		public byte[] m_data;

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
			m_md5 = Encoding.UTF8.GetString(val);
			int num = m_md5.IndexOf('\0');
			if (num != -1)
			{
				m_md5 = m_md5.Substring(0, m_md5.IndexOf('\0'));
			}
			uint val2 = 0u;
			if (!reader.PopUInt32(ref val2))
			{
				return false;
			}
			if (val2 != 0)
			{
				m_data = new byte[val2];
				m_data.Initialize();
				if (!reader.PopByteArray(ref m_data, (int)val2))
				{
					return false;
				}
			}
			return true;
		}
	}
}
