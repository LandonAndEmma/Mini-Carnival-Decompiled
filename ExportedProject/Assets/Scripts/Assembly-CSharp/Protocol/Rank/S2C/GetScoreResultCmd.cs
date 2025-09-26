using System.Text;

namespace Protocol.Rank.S2C
{
	public class GetScoreResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kNoRank = 1,
			kNoScore = 2
		}

		public byte m_result;

		public uint m_score;

		public uint m_pos;

		public string m_rank_name;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_score))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_pos))
			{
				return false;
			}
			byte[] val = new byte[17];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_rank_name = Encoding.UTF8.GetString(val);
			int num = m_rank_name.IndexOf('\0');
			if (num != -1)
			{
				m_rank_name = m_rank_name.Substring(0, m_rank_name.IndexOf('\0'));
			}
			return true;
		}
	}
}
