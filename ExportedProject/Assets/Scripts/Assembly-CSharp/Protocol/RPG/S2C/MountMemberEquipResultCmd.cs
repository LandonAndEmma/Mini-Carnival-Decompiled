namespace Protocol.RPG.S2C
{
	public class MountMemberEquipResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kDataError = 1
		}

		public byte m_result;

		public byte m_member_pos;

		public byte m_part;

		public ulong m_mount_equip;

		public ulong m_unmount_equip;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopByte(ref m_member_pos))
			{
				return false;
			}
			if (!reader.PopByte(ref m_part))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_mount_equip))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_unmount_equip))
			{
				return false;
			}
			return true;
		}
	}
}
