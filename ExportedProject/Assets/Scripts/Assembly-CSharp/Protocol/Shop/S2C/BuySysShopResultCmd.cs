using System.Text;

namespace Protocol.Shop.S2C
{
	public class BuySysShopResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kNoSpace = 1,
			kNoEnough = 2
		}

		public byte m_result;

		public string m_unit = string.Empty;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			byte[] val = new byte[9];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_unit = Encoding.UTF8.GetString(val);
			int num = m_unit.IndexOf('\0');
			if (num != -1)
			{
				m_unit = m_unit.Substring(0, m_unit.IndexOf('\0'));
			}
			return true;
		}
	}
}
