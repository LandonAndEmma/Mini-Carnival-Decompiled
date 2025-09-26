namespace Protocol.Role
{
	public class MountBagItemResultCmd
	{
		public enum Code
		{
			kOk = 0,
			kError = 1
		}

		public byte m_result;

		public ulong m_mount_id;

		public ulong m_unmount_id;

		public byte m_mount_part;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopByte(ref m_result))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_mount_id))
			{
				return false;
			}
			if (!reader.PopUInt64(ref m_unmount_id))
			{
				return false;
			}
			if (!reader.PopByte(ref m_mount_part))
			{
				return false;
			}
			return true;
		}
	}
}
