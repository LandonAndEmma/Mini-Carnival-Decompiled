using System.Text;

namespace Protocol.Hall.S2C
{
	public class NotifyRelayDataCmd
	{
		public uint m_who;

		public ushort m_data_size;

		public string m_data;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_who))
			{
				return false;
			}
			if (!reader.PopUInt16(ref m_data_size))
			{
				return false;
			}
			byte[] val = new byte[m_data_size + 1];
			if (!reader.PopByteArray(ref val, m_data_size))
			{
				return false;
			}
			m_data = Encoding.UTF8.GetString(val);
			return true;
		}
	}
}
