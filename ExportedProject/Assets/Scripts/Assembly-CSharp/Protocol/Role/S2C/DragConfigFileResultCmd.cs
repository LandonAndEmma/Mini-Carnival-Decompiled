using System.Text;

namespace Protocol.Role.S2C
{
	public class DragConfigFileResultCmd
	{
		public string fileName;

		public byte[] data;

		public bool Parse(BufferReader reader)
		{
			byte[] val = new byte[33];
			if (!reader.PopByteArray(ref val, val.Length))
			{
				return false;
			}
			fileName = Encoding.UTF8.GetString(val);
			int num = fileName.IndexOf('\0');
			if (num != -1)
			{
				fileName = fileName.Substring(0, fileName.IndexOf('\0'));
			}
			uint val2 = 0u;
			if (!reader.PopUInt32(ref val2))
			{
				return false;
			}
			data = new byte[val2];
			if (!reader.PopByteArray(ref data, data.Length))
			{
				return false;
			}
			return true;
		}
	}
}
