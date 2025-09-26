using System.Collections.Generic;
using System.Text;

namespace Protocol.Role.S2C
{
	public class DragConfigListResultCmd
	{
		public List<FileDigest> lstFileDigest = new List<FileDigest>();

		public bool Parse(BufferReader reader)
		{
			byte val = 0;
			if (!reader.PopByte(ref val))
			{
				return false;
			}
			for (int i = 0; i < val; i++)
			{
				FileDigest fileDigest = new FileDigest();
				byte[] val2 = new byte[33];
				if (!reader.PopByteArray(ref val2, val2.Length))
				{
					return false;
				}
				fileDigest._fileName = Encoding.UTF8.GetString(val2);
				int num = fileDigest._fileName.IndexOf('\0');
				if (num != -1)
				{
					fileDigest._fileName = fileDigest._fileName.Substring(0, fileDigest._fileName.IndexOf('\0'));
				}
				byte[] val3 = new byte[33];
				if (!reader.PopByteArray(ref val3, val3.Length))
				{
					return false;
				}
				fileDigest._md5 = Encoding.UTF8.GetString(val3);
				int num2 = fileDigest._md5.IndexOf('\0');
				if (num2 != -1)
				{
					fileDigest._md5 = fileDigest._md5.Substring(0, fileDigest._md5.IndexOf('\0'));
				}
				lstFileDigest.Add(fileDigest);
			}
			return true;
		}
	}
}
