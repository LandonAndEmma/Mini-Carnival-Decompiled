namespace Protocol
{
	public class BagItem
	{
		public enum Part
		{
			unknown = 0,
			head = 1,
			body = 2,
			leg = 3,
			decoration = 4
		}

		public enum State
		{
			_static = 0,
			_edit = 1,
			_edit_sell = 2
		}

		public ulong m_unique_id;

		public byte m_part;

		public byte m_state;

		public string m_unit = string.Empty;
	}
}
