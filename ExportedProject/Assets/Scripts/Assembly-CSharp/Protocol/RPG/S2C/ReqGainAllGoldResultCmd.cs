namespace Protocol.RPG.S2C
{
	public class ReqGainAllGoldResultCmd
	{
		public uint m_gain_gold;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_gain_gold))
			{
				return false;
			}
			return true;
		}
	}
}
