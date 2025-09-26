namespace Protocol.RPG.S2C
{
	public class GetMedalRankPosResultCmd
	{
		public uint m_pos;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_pos))
			{
				return false;
			}
			return true;
		}
	}
}
