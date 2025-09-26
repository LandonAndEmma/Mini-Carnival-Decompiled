using System.Text;

namespace Protocol.RPG.S2C
{
	public class NotifyAvatarAddCmd
	{
		public ulong m_unique_id;

		public string m_md5;

		public byte m_part;

		public ushort m_type;

		public byte m_level;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt64(ref m_unique_id))
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
			if (!reader.PopByte(ref m_part))
			{
				return false;
			}
			if (!reader.PopUInt16(ref m_type))
			{
				return false;
			}
			if (!reader.PopByte(ref m_level))
			{
				return false;
			}
			return true;
		}
	}
}
