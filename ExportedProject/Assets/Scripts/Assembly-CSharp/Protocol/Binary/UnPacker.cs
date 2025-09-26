using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using LtcpSdk;

namespace Protocol.Binary
{
	public class UnPacker : BufferReader
	{
		public bool ParserPacket(Packet packet, byte[] key)
		{
			SetData(packet.ByteArray());
			Header header = new Header();
			if (!PopUInt32(ref header.m_iLength))
			{
				return false;
			}
			if (!PopByte(ref header.m_cCompress))
			{
				return false;
			}
			if (!PopByte(ref header.m_cSecurity))
			{
				return false;
			}
			byte[] val = new byte[m_data.Length - m_offset];
			PopByteArray(ref val, val.Length);
			SetData(val);
			if (header.m_cSecurity != 0 || header.m_cCompress != 0)
			{
				Security cSecurity = (Security)header.m_cSecurity;
				if (cSecurity == Security.kXXTEA)
				{
					val = XXTEAUtils.Decrypt(val, key);
					SetData(val);
				}
				Compress cCompress = (Compress)header.m_cCompress;
				if (cCompress == Compress.kGzipNoHeader)
				{
					InflaterInputStream inflaterInputStream = new InflaterInputStream(new MemoryStream(val, 0, val.Length));
					MemoryStream memoryStream = new MemoryStream();
					int num = 0;
					byte[] array = new byte[4096];
					while ((num = inflaterInputStream.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, num);
					}
					inflaterInputStream.Close();
					SetData(memoryStream.ToArray());
				}
			}
			return true;
		}
	}
}
