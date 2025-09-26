using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Protocol.RPG.S2C
{
	public class NotifyRPGDataCmd
	{
		public uint m_rpg_level;

		public uint m_rpg_lv_exp;

		public uint m_medal;

		public uint m_mobility_time;

		public uint m_coupon;

		public byte m_leader_pos;

		public MemberSlot[] m_member_slot = new MemberSlot[6];

		public uint m_card_capacity;

		public Dictionary<uint, List<ulong>> m_card_list = new Dictionary<uint, List<ulong>>();

		public uint m_jewel_capacity;

		public Dictionary<ushort, uint> m_jewel_list = new Dictionary<ushort, uint>();

		public uint m_equip_capacity;

		public Dictionary<ulong, Equip> m_equip_bag = new Dictionary<ulong, Equip>();

		public uint m_last_refresh_time;

		public MapPoint[] m_mapPoint = new MapPoint[100];

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_rpg_level))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_rpg_lv_exp))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_medal))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_mobility_time))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_coupon))
			{
				return false;
			}
			Debug.LogWarning("m_coupon" + m_coupon);
			if (!reader.PopByte(ref m_leader_pos))
			{
				return false;
			}
			for (byte b = 0; b < 6; b++)
			{
				m_member_slot[b] = new MemberSlot();
				if (!reader.PopUInt32(ref m_member_slot[b].m_member))
				{
					return false;
				}
				if (!reader.PopUInt64(ref m_member_slot[b].m_unqiue))
				{
					return false;
				}
				if (!reader.PopUInt64(ref m_member_slot[b].m_head))
				{
					return false;
				}
				if (!reader.PopUInt64(ref m_member_slot[b].m_body))
				{
					return false;
				}
				if (!reader.PopUInt64(ref m_member_slot[b].m_leg))
				{
					return false;
				}
			}
			if (!reader.PopUInt32(ref m_card_capacity))
			{
				return false;
			}
			uint val = 0u;
			if (!reader.PopUInt32(ref val))
			{
				return false;
			}
			for (uint num = 0u; num < val; num++)
			{
				uint val2 = 0u;
				if (!reader.PopUInt32(ref val2))
				{
					return false;
				}
				uint val3 = 0u;
				if (!reader.PopUInt32(ref val3))
				{
					return false;
				}
				if (m_card_list.ContainsKey(val2))
				{
					List<ulong> list = m_card_list[val2];
					if (list == null)
					{
						list = new List<ulong>();
						m_card_list.Add(val2, list);
					}
				}
				else
				{
					m_card_list.Add(val2, new List<ulong>());
				}
				for (uint num2 = 0u; num2 < val3; num2++)
				{
					ulong val4 = 0uL;
					if (!reader.PopUInt64(ref val4))
					{
						return false;
					}
					m_card_list[val2].Add(val4);
				}
			}
			if (!reader.PopUInt32(ref m_jewel_capacity))
			{
				return false;
			}
			uint val5 = 0u;
			if (!reader.PopUInt32(ref val5))
			{
				return false;
			}
			for (uint num3 = 0u; num3 < val5; num3++)
			{
				ushort val6 = 0;
				if (!reader.PopUInt16(ref val6))
				{
					return false;
				}
				uint val7 = 0u;
				if (!reader.PopUInt32(ref val7))
				{
					return false;
				}
				m_jewel_list.Add(val6, val7);
			}
			if (!reader.PopUInt32(ref m_equip_capacity))
			{
				return false;
			}
			uint val8 = 0u;
			if (!reader.PopUInt32(ref val8))
			{
				return false;
			}
			for (uint num4 = 0u; num4 < val8; num4++)
			{
				Equip equip = new Equip();
				if (!reader.PopUInt64(ref equip.m_id))
				{
					return false;
				}
				if (!reader.PopByte(ref equip.m_part))
				{
					return false;
				}
				if (!reader.PopByte(ref equip.m_state))
				{
					return false;
				}
				byte[] val9 = new byte[33];
				if (!reader.PopByteArray(ref val9, val9.Length))
				{
					return false;
				}
				equip.m_md5 = Encoding.UTF8.GetString(val9);
				int num5 = equip.m_md5.IndexOf('\0');
				if (num5 != -1)
				{
					equip.m_md5 = equip.m_md5.Substring(0, equip.m_md5.IndexOf('\0'));
				}
				if (!reader.PopUInt16(ref equip.m_type))
				{
					return false;
				}
				if (!reader.PopByte(ref equip.m_level))
				{
					return false;
				}
				m_equip_bag.Add(equip.m_id, equip);
			}
			if (!reader.PopUInt32(ref m_last_refresh_time))
			{
				return false;
			}
			for (byte b2 = 0; b2 < 100; b2++)
			{
				m_mapPoint[b2] = new MapPoint();
				m_mapPoint[b2].m_index = b2;
				if (!reader.PopByte(ref m_mapPoint[b2].m_status))
				{
					return false;
				}
				if (!reader.PopUInt32(ref m_mapPoint[b2].m_role_id))
				{
					return false;
				}
				if (!reader.PopUInt32(ref m_mapPoint[b2].m_start_time))
				{
					return false;
				}
			}
			return true;
		}
	}
}
