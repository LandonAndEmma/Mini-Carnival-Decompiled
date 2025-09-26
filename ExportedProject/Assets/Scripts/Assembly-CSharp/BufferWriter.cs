using System.Collections.Generic;

public class BufferWriter
{
	private bool m_little_ending;

	private List<byte> m_data;

	public BufferWriter()
	{
		m_data = new List<byte>();
	}

	public void SetData(List<byte> data)
	{
		m_data = data;
	}

	public int Size()
	{
		return m_data.Count;
	}

	public void PushByte(byte b)
	{
		m_data.Add(b);
	}

	public void PushUInt16(ushort Num)
	{
		byte item = (byte)(((Num & 0xFF00) >> 8) & 0xFF);
		byte item2 = (byte)(Num & 0xFF & 0xFF);
		if (m_little_ending)
		{
			m_data.Add(item2);
			m_data.Add(item);
		}
		else
		{
			m_data.Add(item);
			m_data.Add(item2);
		}
	}

	public void PushUInt32(uint Num)
	{
		byte item = (byte)(((Num & 0xFF000000u) >> 24) & 0xFF);
		byte item2 = (byte)(((Num & 0xFF0000) >> 16) & 0xFF);
		byte item3 = (byte)(((Num & 0xFF00) >> 8) & 0xFF);
		byte item4 = (byte)(Num & 0xFF & 0xFF);
		if (m_little_ending)
		{
			m_data.Add(item4);
			m_data.Add(item3);
			m_data.Add(item2);
			m_data.Add(item);
		}
		else
		{
			m_data.Add(item);
			m_data.Add(item2);
			m_data.Add(item3);
			m_data.Add(item4);
		}
	}

	public void PushUInt64(ulong Num)
	{
		uint num = (uint)(Num >> 32);
		uint num2 = (uint)Num;
		if (m_little_ending)
		{
			PushUInt32(num2);
			PushUInt32(num);
		}
		else
		{
			PushUInt32(num);
			PushUInt32(num2);
		}
	}

	public void PushByteArray(byte[] buf, int length)
	{
		if (buf.Length >= length)
		{
			for (int i = 0; i < length; i++)
			{
				m_data.Add(buf[i]);
			}
			return;
		}
		for (int j = 0; j < buf.Length; j++)
		{
			m_data.Add(buf[j]);
		}
		byte[] array = new byte[length - buf.Length];
		array.Initialize();
		for (int k = 0; k < array.Length; k++)
		{
			m_data.Add(array[k]);
		}
	}

	public byte[] ToByteArray()
	{
		return m_data.ToArray();
	}
}
