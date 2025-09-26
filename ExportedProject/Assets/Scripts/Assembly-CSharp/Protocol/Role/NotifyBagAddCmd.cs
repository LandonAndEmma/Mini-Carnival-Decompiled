using System.Text;

namespace Protocol.Role
{
	public class NotifyBagAddCmd
	{
		public BagItem m_item = new BagItem();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt64(ref m_item.m_unique_id))
			{
				return false;
			}
			if (!reader.PopByte(ref m_item.m_part))
			{
				return false;
			}
			if (!reader.PopByte(ref m_item.m_state))
			{
				return false;
			}
			byte[] val = new byte[33];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_item.m_unit = Encoding.UTF8.GetString(val);
			int num = m_item.m_unit.IndexOf('\0');
			if (num != -1)
			{
				m_item.m_unit = m_item.m_unit.Substring(0, m_item.m_unit.IndexOf('\0'));
			}
			return true;
		}
	}
}
