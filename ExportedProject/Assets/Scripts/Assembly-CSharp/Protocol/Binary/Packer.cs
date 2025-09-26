using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using LtcpSdk;

namespace Protocol.Binary
{
	public class Packer : BufferWriter
	{
		private List<byte> m_data = new List<byte>();

		public Packer()
		{
			SetData(m_data);
		}

		public Packet MakePacket(byte security, byte compress, byte[] key)
		{
			byte[] array = m_data.ToArray();
			if (compress == 1)
			{
				MemoryStream memoryStream = new MemoryStream();
				DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(memoryStream);
				deflaterOutputStream.Write(array, 0, array.Length);
				deflaterOutputStream.Close();
				array = memoryStream.ToArray();
			}
			if (security == 1)
			{
				array = XXTEAUtils.Encrypt(array, key);
			}
			int num = 6 + array.Length;
			Header header = new Header();
			header.m_iLength = (uint)num;
			header.m_cCompress = compress;
			header.m_cSecurity = security;
			Packet packet = new Packet(num);
			packet.PushUInt32(header.m_iLength);
			packet.PushByte(header.m_cCompress);
			packet.PushByte(header.m_cSecurity);
			packet.PushByteArray(array, array.Length);
			return packet;
		}
	}
}
