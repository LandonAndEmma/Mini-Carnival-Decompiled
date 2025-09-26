namespace Protocol.Role.C2S
{
	public class MountBagItemCmd : BaseCmd
	{
		public enum Part
		{
			head_top = 0,
			head_front = 1,
			head_back = 2,
			head_left = 3,
			head_right = 4,
			chest_front = 5,
			chest_back = 6,
			head = 7,
			body = 8,
			leg = 9
		}

		public ulong m_unique_id;

		public byte m_mount_part;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushUInt64(m_unique_id);
			bufferWriter.PushByte(m_mount_part);
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 38;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
