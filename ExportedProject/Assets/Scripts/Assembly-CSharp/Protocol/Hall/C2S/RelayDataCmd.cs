using System.Text;
using UnityEngine;

namespace Protocol.Hall.C2S
{
	public class RelayDataCmd : BaseCmd
	{
		public ushort m_data_size;

		public string m_data;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			Debug.LogWarning("m_data_size=" + m_data_size);
			bufferWriter.PushUInt16(m_data_size);
			byte[] bytes = Encoding.UTF8.GetBytes(m_data);
			Debug.LogWarning(bytes.Length);
			bytes[bytes.Length - 1] = 0;
			bufferWriter.PushByteArray(bytes, bytes.Length);
			Header header = new Header();
			header.m_cProtocol = 3;
			header.m_cCmd = 7;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
