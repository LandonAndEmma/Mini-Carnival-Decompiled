using UnityEngine;

namespace Protocol.Shop.C2S
{
	public class GetShopListCmd : BaseCmd
	{
		public enum Code
		{
			kRand = 0,
			kAd = 1,
			kPraise = 2
		}

		public byte m_type;

		public byte m_size;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_type);
			bufferWriter.PushByte(m_size);
			Debug.Log("m_type=" + m_type);
			Header header = new Header();
			header.m_cProtocol = 2;
			header.m_cCmd = 29;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
