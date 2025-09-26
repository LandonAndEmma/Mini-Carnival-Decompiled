using System.Collections.Generic;
using System.Text;

namespace Protocol.Role
{
	public class SearchPlayerResultCmd
	{
		public uint m_page_sum;

		public uint m_page_index;

		public List<SearchRoleInfo> m_list = new List<SearchRoleInfo>();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_page_sum))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_page_index))
			{
				return false;
			}
			byte val = 0;
			if (!reader.PopByte(ref val))
			{
				return false;
			}
			for (byte b = 0; b < val; b++)
			{
				SearchRoleInfo searchRoleInfo = new SearchRoleInfo();
				if (!reader.PopUInt32(ref searchRoleInfo.m_player_id))
				{
					return false;
				}
				byte[] val2 = new byte[33];
				if (!reader.PopByteArray(ref val2, val2.Length))
				{
					return false;
				}
				searchRoleInfo.m_name = Encoding.UTF8.GetString(val2);
				int num = searchRoleInfo.m_name.IndexOf('\0');
				if (num != -1)
				{
					searchRoleInfo.m_name = searchRoleInfo.m_name.Substring(0, searchRoleInfo.m_name.IndexOf('\0'));
				}
				m_list.Add(searchRoleInfo);
			}
			return true;
		}
	}
}
