public class BufferUtils
{
	private static bool m_little_ending;

	public static ushort WatchUInt16(byte[] data, int pos)
	{
		if (m_little_ending)
		{
			return (ushort)((data[pos + 1] << 8) | data[pos]);
		}
		return (ushort)((data[pos] << 8) | data[pos + 1]);
	}

	public static uint WatchUInt32(byte[] data, int pos)
	{
		if (m_little_ending)
		{
			return (uint)((data[pos + 3] << 24) | (data[pos + 2] << 16) | (data[pos + 1] << 8) | data[pos]);
		}
		return (uint)((data[pos] << 24) | (data[pos + 1] << 16) | (data[pos + 2] << 8) | data[pos + 3]);
	}
}
