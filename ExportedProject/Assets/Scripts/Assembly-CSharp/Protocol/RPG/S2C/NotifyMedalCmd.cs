namespace Protocol.RPG.S2C
{
	public class NotifyMedalCmd
	{
		public uint m_medal;

		public bool Parse(BufferReader reader)
		{
			if (!reader.PopUInt32(ref m_medal))
			{
				return false;
			}
			return true;
		}
	}
}
