using System.Text;

namespace Protocol.Role.C2S
{
	public class UploadRoleDataCmd : BaseCmd
	{
		public string m_name;

		public uint m_level;

		public uint m_gold;

		public uint m_crystal;

		public byte m_first_buy_head;

		public byte m_first_buy_body;

		public byte m_first_buy_leg;

		public byte m_head;

		public byte m_body;

		public byte m_leg;

		public byte m_head_top;

		public byte m_head_front;

		public byte m_head_back;

		public byte m_head_left;

		public byte m_head_right;

		public byte m_chest_front;

		public byte m_chest_back;

		public byte m_bag_capacity;

		public BagItem[] m_bag = new BagItem[72];

		public uint[] m_friend_list = new uint[30];

		private uint m_save_size;

		public byte[] m_save;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			byte[] bytes = Encoding.UTF8.GetBytes(m_name);
			if (bytes.Length >= 33)
			{
				bytes[32] = 0;
			}
			bufferWriter.PushByteArray(bytes, 33);
			bufferWriter.PushUInt32(m_level);
			bufferWriter.PushUInt32(m_gold);
			bufferWriter.PushUInt32(m_crystal);
			bufferWriter.PushByte(m_first_buy_head);
			bufferWriter.PushByte(m_first_buy_body);
			bufferWriter.PushByte(m_first_buy_leg);
			bufferWriter.PushByte(m_head);
			bufferWriter.PushByte(m_body);
			bufferWriter.PushByte(m_leg);
			bufferWriter.PushByte(m_head_top);
			bufferWriter.PushByte(m_head_front);
			bufferWriter.PushByte(m_head_back);
			bufferWriter.PushByte(m_head_left);
			bufferWriter.PushByte(m_head_right);
			bufferWriter.PushByte(m_chest_front);
			bufferWriter.PushByte(m_chest_back);
			bufferWriter.PushByte(m_bag_capacity);
			for (int i = 0; i < m_bag.Length; i++)
			{
				bufferWriter.PushUInt64(m_bag[i].m_unique_id);
				bufferWriter.PushByte(m_bag[i].m_part);
				bufferWriter.PushByte(m_bag[i].m_state);
				byte[] bytes2 = Encoding.UTF8.GetBytes(m_bag[i].m_unit);
				if (bytes2.Length >= 33)
				{
					bytes2[32] = 0;
				}
				bufferWriter.PushByteArray(bytes2, 33);
			}
			for (int j = 0; j < m_friend_list.Length; j++)
			{
				bufferWriter.PushUInt32(m_friend_list[j]);
			}
			bufferWriter.PushUInt32((uint)m_save.Length);
			if (m_save != null && m_save.Length > 0)
			{
				bufferWriter.PushByteArray(m_save, m_save.Length);
			}
			Header header = new Header();
			header.m_cProtocol = 1;
			header.m_cCmd = 13;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
