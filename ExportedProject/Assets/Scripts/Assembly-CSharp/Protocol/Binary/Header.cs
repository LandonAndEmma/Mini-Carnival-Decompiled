namespace Protocol.Binary
{
	public class Header
	{
		public const int HEADER_LENGTH = 6;

		public uint m_iLength;

		public byte m_cCompress;

		public byte m_cSecurity;
	}
}
