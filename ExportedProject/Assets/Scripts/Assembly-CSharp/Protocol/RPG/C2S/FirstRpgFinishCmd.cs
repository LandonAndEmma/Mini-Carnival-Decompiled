using System.Collections.Generic;

namespace Protocol.RPG.C2S
{
	public class FirstRpgFinishCmd : BaseCmd
	{
		public byte m_card_num;

		public List<uint> m_card_list = new List<uint>();

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_card_num);
			for (int i = 0; i < m_card_list.Count; i++)
			{
				bufferWriter.PushUInt32(m_card_list[i]);
			}
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 61;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
