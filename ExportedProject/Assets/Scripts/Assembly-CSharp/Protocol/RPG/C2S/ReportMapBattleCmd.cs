namespace Protocol.RPG.C2S
{
	public class ReportMapBattleCmd : BaseCmd
	{
		public enum BattleResult
		{
			kVictory = 0,
			kFailure = 1
		}

		public byte m_map_point;

		public byte m_battle_result;

		public uint m_rpg_level;

		public override void Serialize(BufferWriter writer)
		{
			BufferWriter bufferWriter = new BufferWriter();
			bufferWriter.PushByte(m_map_point);
			bufferWriter.PushByte(m_battle_result);
			bufferWriter.PushUInt32(m_rpg_level);
			Header header = new Header();
			header.m_cProtocol = 7;
			header.m_cCmd = 22;
			header.SetBodyLength(bufferWriter.Size());
			header.Serialize(writer);
			writer.PushByteArray(bufferWriter.ToByteArray(), bufferWriter.Size());
		}
	}
}
