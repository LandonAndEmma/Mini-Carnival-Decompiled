using System.Collections.Generic;
using System.Text;

namespace Protocol.RPG.S2C
{
	public class PlayerRpgDataCmd
	{
		public uint who;

		public uint m_rpg_level;

		public uint m_medal;

		public byte m_leader_pos;

		public MemberSlot[] m_member_slot = new MemberSlot[6];

		public uint m_equip_capacity;

		public Dictionary<ulong, Equip> m_equip_bag = new Dictionary<ulong, Equip>();

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref who))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_rpg_level))
			{
				return false;
			}
			if (!reader.PopUInt32(ref m_medal))
			{
				return false;
			}
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
			if (!reader.PopUInt32(ref m_equip_capacity))
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
				byte[] val2 = new byte[33];
				if (!reader.PopByteArray(ref val2, val2.Length))
				{
					return false;
				}
				equip.m_md5 = Encoding.UTF8.GetString(val2);
				int num2 = equip.m_md5.IndexOf('\0');
				if (num2 != -1)
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
			return true;
		}
	}
}
