using System.Collections.Generic;
using System.Text;

namespace Protocol.Role.S2C
{
	public class NotifyRoleDataCmd
	{
		public RoleInfo m_info = new RoleInfo();

		public BagData m_bag_data = new BagData();

		public byte m_hasInportFB;

		public List<uint> m_friend_list = new List<uint>();

		public List<uint> m_collect_list = new List<uint>();

		public List<uint> m_follow_list = new List<uint>();

		public byte[] m_save_data;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_info.m_player_id))
			{
				return false;
			}
			byte[] val = new byte[33];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			m_info.m_facebook_id = Encoding.UTF8.GetString(val);
			int num = m_info.m_facebook_id.IndexOf('\0');
			if (num != -1)
			{
				m_info.m_facebook_id = m_info.m_facebook_id.Substring(0, m_info.m_facebook_id.IndexOf('\0'));
			}
			byte[] val2 = new byte[33];
			if (!reader.PopByteArray(ref val2, val2.Length))
			{
				return false;
			}
			m_info.m_face_image_md5 = Encoding.UTF8.GetString(val2);
			int num2 = m_info.m_face_image_md5.IndexOf('\0');
			if (num2 != -1)
			{
				m_info.m_face_image_md5 = m_info.m_face_image_md5.Substring(0, m_info.m_face_image_md5.IndexOf('\0'));
			}
			byte[] val3 = new byte[33];
			if (!reader.PopByteArray(ref val3, val3.Length))
			{
				return false;
			}
			m_info.m_name = Encoding.UTF8.GetString(val3);
			int num3 = m_info.m_name.IndexOf('\0');
			if (num3 != -1)
			{
				m_info.m_name = m_info.m_name.Substring(0, m_info.m_name.IndexOf('\0'));
			}
			if (!reader.PopUInt32(ref m_info.m_level))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_head))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_body))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_leg))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_head_top))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_head_front))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_head_back))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_head_left))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_head_right))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_chest_front))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_info.m_chest_back))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_info.m_heart))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_info.m_gold))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_info.m_crystal))
			{
				return false;
			}
			if (!reader.PopByte(ref m_bag_data.m_first_buy_head))
			{
				return false;
			}
			if (!reader.PopByte(ref m_bag_data.m_first_buy_body))
			{
				return false;
			}
			if (!reader.PopByte(ref m_bag_data.m_first_buy_leg))
			{
				return false;
			}
			if (!reader.PopByte(ref m_bag_data.m_bag_capacity))
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
				BagItem bagItem = new BagItem();
				if (!reader.PopUInt64(ref bagItem.m_unique_id))
				{
					return false;
				}
				if (!reader.PopByte(ref bagItem.m_part))
				{
					return false;
				}
				if (!reader.PopByte(ref bagItem.m_state))
				{
					return false;
				}
				byte[] val5 = new byte[33];
				if (!reader.PopByteArray(ref val5, val5.Length))
				{
					return false;
				}
				bagItem.m_unit = Encoding.UTF8.GetString(val5);
				int num4 = bagItem.m_unit.IndexOf('\0');
				if (num4 != -1)
				{
					bagItem.m_unit = bagItem.m_unit.Substring(0, bagItem.m_unit.IndexOf('\0'));
				}
				m_bag_data.m_bag_list.Add(bagItem);
			}
			if (!reader.PopByte(ref m_hasInportFB))
			{
				return false;
			}
			byte val6 = 0;
			if (!reader.PopByte(ref val6))
			{
				return false;
			}
			for (byte b2 = 0; b2 < val6; b2++)
			{
				uint val7 = 0u;
				if (!reader.PopUInt32(ref val7))
				{
					return false;
				}
				m_friend_list.Add(val7);
			}
			byte val8 = 0;
			if (!reader.PopByte(ref val8))
			{
				return false;
			}
			for (byte b3 = 0; b3 < val8; b3++)
			{
				uint val9 = 0u;
				if (!reader.PopUInt32(ref val9))
				{
					return false;
				}
				m_collect_list.Add(val9);
			}
			byte val10 = 0;
			if (!reader.PopByte(ref val10))
			{
				return false;
			}
			for (byte b4 = 0; b4 < val10; b4++)
			{
				uint val11 = 0u;
				if (!reader.PopUInt32(ref val11))
				{
					return false;
				}
				m_follow_list.Add(val11);
			}
			uint val12 = 0u;
			if (!reader.PopUInt32(ref val12))
			{
				return false;
			}
			if (val12 != 0)
			{
				m_save_data = new byte[val12];
				if (!reader.PopByteArray(ref m_save_data, m_save_data.Length))
				{
					return false;
				}
			}
			return true;
		}
	}
}
