using System.Text;

namespace Protocol.Role
{
	public class SearchRoleByFBInfoResultCmd
	{
		private ushort m_result_len;

		public string m_result;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt16(ref m_result_len))
			{
				return false;
			}
			if (m_result_len <= 0)
			{
				m_result = string.Empty;
			}
			else
			{
				byte[] val = new byte[m_result_len];
				if (!reader.PopByteArray(ref val, val.Length))
				{
					return false;
				}
				m_result = Encoding.UTF8.GetString(val);
			}
			return true;
		}
	}
}
