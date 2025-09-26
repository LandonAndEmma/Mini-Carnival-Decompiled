using System.Text;

namespace Protocol.RPG.C2S
{
	public class RequestGameDropCmd : BaseCmd
	{
		public string m_game_name;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_game_name);
			if (bytes.Length >= 17)
			{
				bytes[16] = 0;
			}
			bufferWriter.PushByteArray(bytes, 17);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 53;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
