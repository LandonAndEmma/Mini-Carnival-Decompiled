using System.Text;

namespace Protocol.RPG.S2C
{
	public class ReportMapBattleResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kError = 1
		}

		public byte m_result;

		public byte m_medal;

		public uint m_gold;

		public uint m_exp;

		public uint m_crystal;

		public uint m_card_id;

		public string m_deco_id;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopByte(ref m_medal))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_gold))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_exp))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_crystal))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_card_id))
			{
				return false;
			}
			byte[] val = new byte[9];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_deco_id = Encoding.UTF8.GetString(val);
			int num = m_deco_id.IndexOf('\0');
			if (num != -1)
			{
				m_deco_id = m_deco_id.Substring(0, m_deco_id.IndexOf('\0'));
			}
			return true;
		}
	}
}
