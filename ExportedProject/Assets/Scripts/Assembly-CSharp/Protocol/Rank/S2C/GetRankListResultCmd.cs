using System.Collections.Generic;
using System.Text;

namespace Protocol.Rank.S2C
{
	public class GetRankListResultCmd
	{
		public string m_rank_name;

		public string m_award;

		public string m_last_rank;

		public uint m_end_time;

		public byte m_list_num;

		public List<RankItem> m_list = new List<RankItem>();

		public bool Parse(BufferReader reader)
		{
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
			byte[] val2 = new byte[4096];
			if (!reader.PopByteArray(ref val2, val2.Length))
			{
				return false;
			}
			m_award = Encoding.UTF8.GetString(val2);
			int num2 = m_award.IndexOf('\0');
			if (num2 != -1)
			{
				m_award = m_award.Substring(0, m_award.IndexOf('\0'));
			}
			byte[] val3 = new byte[1024];
			if (!reader.PopByteArray(ref val3, val3.Length))
			{
				return false;
			}
			m_last_rank = Encoding.UTF8.GetString(val3);
			int num3 = m_last_rank.IndexOf('\0');
			if (num3 != -1)
			{
				m_last_rank = m_last_rank.Substring(0, m_last_rank.IndexOf('\0'));
			}
			if (!reader.PopUInt32(ref m_end_time))
			{
				return false;
			}
			byte val4 = 0;
			if (!reader.PopByte(ref val4))
			{
				return false;
			}
			for (byte b = 0; b < val4; b++)
			{
				RankItem rankItem = new RankItem();
				if (!reader.PopUInt32(ref rankItem.m_role_id))
				{
					return false;
				}
				if (!reader.PopUInt32(ref rankItem.m_score))
				{
					return false;
				}
				if (!reader.PopUInt32(ref rankItem.m_time))
				{
					return false;
				}
				m_list.Add(rankItem);
			}
			return true;
		}
	}
}
