using System.Text;

namespace Protocol.Shop.C2S
{
	public class BuySysShopCmd : BaseCmd
	{
		public string m_id;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_id);
			if (bytes.Length >= 9)
			{
				bytes[8] = 0;
			}
			bufferWriter.PushByteArray(bytes, 9);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 31;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
