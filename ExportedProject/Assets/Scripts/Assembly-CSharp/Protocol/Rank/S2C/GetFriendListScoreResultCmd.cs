using System.Collections.Generic;
using System.Text;

namespace Protocol.Rank.S2C
{
	public class GetFriendListScoreResultCmd
	{
		public string m_rank_name;

		public uint m_end_time;

		public uint m_list_num;

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
			if (!reader.PopUInt32(ref m_end_time))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_list_num))
			{
				return false;
			}
			for (byte b = 0; b < m_list_num; b++)
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
