namespace Protocol
{
	public class Email
	{
		public enum Type
		{
			kUnknown = 0,
			kNotice = 1,
			kSystem = 2,
			kMobility = 3,
			kTicket = 4
		}

		public enum Status
		{
			kNew = 0,
			kHasRead = 1,
			kHasGain = 2
		}

		public uint m_id;

		public byte m_type;

		public byte m_status;

		public uint m_time;

		public uint m_receiver;

		public uint m_sender;

		public string m_sender_name;

		public string m_title;

		public string m_content;

		public string m_attach;
	}
}
