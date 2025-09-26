using System.Text;

namespace Protocol.Role
{
	public class NotifyWatchInfoCmd
	{
		public WatchRoleInfo m_info = new WatchRoleInfo();

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
			m_info.m_face_image_md5 = Encoding.UTF8.GetString(val);
			int num = m_info.m_face_image_md5.IndexOf('\0');
			if (num != -1)
			{
				m_info.m_face_image_md5 = m_info.m_face_image_md5.Substring(0, m_info.m_face_image_md5.IndexOf('\0'));
			}
			byte[] val2 = new byte[33];
			if (!reader.PopByteArray(ref val2, val2.Length))
			{
				return false;
			}
			m_info.m_name = Encoding.UTF8.GetString(val2);
			int num2 = m_info.m_name.IndexOf('\0');
			if (num2 != -1)
			{
				m_info.m_name = m_info.m_name.Substring(0, m_info.m_name.IndexOf('\0'));
			}
			if (!reader.PopUInt32(ref m_info.m_level))
			{
				return false;
			}
			byte[] val3 = new byte[33];
			if (!reader.PopByteArray(ref val3, val3.Length))
			{
				return false;
			}
			m_info.m_head = Encoding.UTF8.GetString(val3);
			int num3 = m_info.m_head.IndexOf('\0');
			if (num3 != -1)
			{
				m_info.m_head = m_info.m_head.Substring(0, m_info.m_head.IndexOf('\0'));
			}
			byte[] val4 = new byte[33];
			if (!reader.PopByteArray(ref val4, val4.Length))
			{
				return false;
			}
			m_info.m_body = Encoding.UTF8.GetString(val4);
			int num4 = m_info.m_body.IndexOf('\0');
			if (num4 != -1)
			{
				m_info.m_body = m_info.m_body.Substring(0, m_info.m_body.IndexOf('\0'));
			}
			byte[] val5 = new byte[33];
			if (!reader.PopByteArray(ref val5, val5.Length))
			{
				return false;
			}
			m_info.m_leg = Encoding.UTF8.GetString(val5);
			int num5 = m_info.m_leg.IndexOf('\0');
			if (num5 != -1)
			{
				m_info.m_leg = m_info.m_leg.Substring(0, m_info.m_leg.IndexOf('\0'));
			}
			byte[] val6 = new byte[9];
			if (!reader.PopByteArray(ref val6, val6.Length))
			{
				return false;
			}
			m_info.m_head_top = Encoding.UTF8.GetString(val6);
			int num6 = m_info.m_head_top.IndexOf('\0');
			if (num6 != -1)
			{
				m_info.m_head_top = m_info.m_head_top.Substring(0, m_info.m_head_top.IndexOf('\0'));
			}
			byte[] val7 = new byte[9];
			if (!reader.PopByteArray(ref val7, val7.Length))
			{
				return false;
			}
			m_info.m_head_front = Encoding.UTF8.GetString(val7);
			int num7 = m_info.m_head_front.IndexOf('\0');
			if (num7 != -1)
			{
				m_info.m_head_front = m_info.m_head_front.Substring(0, m_info.m_head_front.IndexOf('\0'));
			}
			byte[] val8 = new byte[9];
			if (!reader.PopByteArray(ref val8, val8.Length))
			{
				return false;
			}
			m_info.m_head_back = Encoding.UTF8.GetString(val8);
			int num8 = m_info.m_head_back.IndexOf('\0');
			if (num8 != -1)
			{
				m_info.m_head_back = m_info.m_head_back.Substring(0, m_info.m_head_back.IndexOf('\0'));
			}
			byte[] val9 = new byte[9];
			if (!reader.PopByteArray(ref val9, val9.Length))
			{
				return false;
			}
			m_info.m_head_left = Encoding.UTF8.GetString(val9);
			int num9 = m_info.m_head_left.IndexOf('\0');
			if (num9 != -1)
			{
				m_info.m_head_left = m_info.m_head_left.Substring(0, m_info.m_head_left.IndexOf('\0'));
			}
			byte[] val10 = new byte[9];
			if (!reader.PopByteArray(ref val10, val10.Length))
			{
				return false;
			}
			m_info.m_head_right = Encoding.UTF8.GetString(val10);
			int num10 = m_info.m_head_right.IndexOf('\0');
			if (num10 != -1)
			{
				m_info.m_head_right = m_info.m_head_right.Substring(0, m_info.m_head_right.IndexOf('\0'));
			}
			byte[] val11 = new byte[9];
			if (!reader.PopByteArray(ref val11, val11.Length))
			{
				return false;
			}
			m_info.m_chest_front = Encoding.UTF8.GetString(val11);
			int num11 = m_info.m_chest_front.IndexOf('\0');
			if (num11 != -1)
			{
				m_info.m_chest_front = m_info.m_chest_front.Substring(0, m_info.m_chest_front.IndexOf('\0'));
			}
			byte[] val12 = new byte[9];
			if (!reader.PopByteArray(ref val12, val12.Length))
			{
				return false;
			}
			m_info.m_chest_back = Encoding.UTF8.GetString(val12);
			int num12 = m_info.m_chest_back.IndexOf('\0');
			if (num12 != -1)
			{
				m_info.m_chest_back = m_info.m_chest_back.Substring(0, m_info.m_chest_back.IndexOf('\0'));
			}
			return true;
		}
	}
}
