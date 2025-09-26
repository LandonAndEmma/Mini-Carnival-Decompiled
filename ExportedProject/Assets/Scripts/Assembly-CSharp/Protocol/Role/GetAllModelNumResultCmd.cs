using System.Collections.Generic;
using System.Text;

namespace Protocol.Role
{
	public class GetAllModelNumResultCmd
	{
		public Dictionary<string, uint> _dict = new Dictionary<string, uint>();

		public bool Parse(BufferReader reader)
		{
			byte val = 0;
			if (!reader.PopByte(ref val))
			{
				return false;
			}
			for (byte b = 0; b < val; b++)
			{
				byte[] val2 = new byte[17];
				if (!reader.PopByteArray(ref val2, val2.Length))
				{
					return false;
				}
				string text = Encoding.UTF8.GetString(val2);
				int num = text.IndexOf('\0');
				if (num != -1)
				{
					text = text.Substring(0, text.IndexOf('\0'));
				}
				uint val3 = 0u;
				if (!reader.PopUInt32(ref val3))
				{
					return false;
				}
				_dict.Add(text, val3);
			}
			return true;
		}
	}
}
