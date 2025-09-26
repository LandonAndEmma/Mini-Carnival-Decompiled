using UnityEngine;

namespace BaseCoder
{
	public class Base64
	{
		private static char[] source = null;

		private static int length = 0;

		private static int length2 = 0;

		private static int blockCount = 0;

		private static int paddingCount = 0;

		private static char[] BaseTable = new char[64]
		{
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
			'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
			'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
			'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
			'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
			'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
			'8', '9', '+', '/'
		};

		public static char[] Encode(byte[] input)
		{
			length = input.Length;
			if (length % 3 == 0)
			{
				paddingCount = 0;
				blockCount = length / 3;
			}
			else
			{
				paddingCount = 3 - length % 3;
				blockCount = (length + paddingCount) / 3;
			}
			length2 = length + paddingCount;
			byte[] array = new byte[length2];
			for (int i = 0; i < length2; i++)
			{
				if (i < length)
				{
					array[i] = input[i];
				}
				else
				{
					array[i] = 0;
				}
			}
			byte[] array2 = new byte[blockCount * 4];
			char[] array3 = new char[blockCount * 4];
			for (int j = 0; j < blockCount; j++)
			{
				byte b = array[j * 3];
				byte b2 = array[j * 3 + 1];
				byte b3 = array[j * 3 + 2];
				byte b4 = (byte)((b & 0xFC) >> 2);
				byte b5 = (byte)((b & 3) << 4);
				byte b6 = (byte)((b2 & 0xF0) >> 4);
				b6 += b5;
				b5 = (byte)((b2 & 0xF) << 2);
				byte b7 = (byte)((b3 & 0xC0) >> 6);
				b7 += b5;
				byte b8 = (byte)(b3 & 0x3F);
				array2[j * 4] = b4;
				array2[j * 4 + 1] = b6;
				array2[j * 4 + 2] = b7;
				array2[j * 4 + 3] = b8;
			}
			for (int k = 0; k < blockCount * 4; k++)
			{
				if (array2[k] >= 0 && array2[k] <= 63)
				{
					array3[k] = BaseTable[array2[k]];
				}
				else
				{
					Debug.LogError("should not happen");
				}
			}
			switch (paddingCount)
			{
			case 1:
				array3[blockCount * 4 - 1] = '=';
				break;
			case 2:
				array3[blockCount * 4 - 1] = '=';
				array3[blockCount * 4 - 2] = '=';
				break;
			}
			return array3;
		}

		public static byte[] Decode(char[] input)
		{
			source = input;
			length = input.Length;
			if (length % 4 != 0)
			{
				return null;
			}
			paddingCount = 0;
			while (input[length - paddingCount - 1] == '=' && paddingCount < 3)
			{
				paddingCount++;
			}
			blockCount = length / 4;
			length2 = blockCount * 3;
			byte[] array = new byte[length];
			byte[] array2 = new byte[length2];
			for (int i = 0; i < length; i++)
			{
				array[i] = 0;
				for (int j = 0; j < 64; j++)
				{
					if (BaseTable[j] == source[i])
					{
						array[i] = (byte)j;
						break;
					}
				}
			}
			for (int k = 0; k < blockCount; k++)
			{
				byte b = array[k * 4];
				byte b2 = array[k * 4 + 1];
				byte b3 = array[k * 4 + 2];
				byte b4 = array[k * 4 + 3];
				byte b5 = (byte)(b << 2);
				byte b6 = (byte)((b2 & 0x30) >> 4);
				b6 += b5;
				b5 = (byte)((b2 & 0xF) << 4);
				byte b7 = (byte)((b3 & 0x3C) >> 2);
				b7 += b5;
				b5 = (byte)((b3 & 3) << 6);
				byte b8 = b4;
				b8 += b5;
				array2[k * 3] = b6;
				array2[k * 3 + 1] = b7;
				array2[k * 3 + 2] = b8;
			}
			int num = length2 - paddingCount;
			byte[] array3 = new byte[num];
			for (int l = 0; l < num; l++)
			{
				array3[l] = array2[l];
			}
			return array3;
		}
	}
}
