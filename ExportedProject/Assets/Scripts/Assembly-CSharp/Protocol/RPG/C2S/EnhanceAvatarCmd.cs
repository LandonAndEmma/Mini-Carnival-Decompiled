namespace Protocol.RPG.C2S
{
	public class EnhanceAvatarCmd : BaseCmd
	{
		public ulong m_avatar_id;

		public byte m_gem_level;

		public byte m_gem1_type;

		public byte m_gem2_type;

		public byte m_gem3_type;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt64(m_avatar_id);
			bufferWriter.PushByte(m_gem_level);
			bufferWriter.PushByte(m_gem1_type);
			bufferWriter.PushByte(m_gem2_type);
			bufferWriter.PushByte(m_gem3_type);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 11;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
