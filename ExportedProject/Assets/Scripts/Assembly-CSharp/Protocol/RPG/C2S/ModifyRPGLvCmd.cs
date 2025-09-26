namespace Protocol.RPG.C2S
{
	public class ModifyRPGLvCmd : BaseCmd
	{
		public uint m_rpg_level;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt32(m_rpg_level);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 63;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
