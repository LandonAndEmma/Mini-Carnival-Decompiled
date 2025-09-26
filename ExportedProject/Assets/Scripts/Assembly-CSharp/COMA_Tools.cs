using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class COMA_Tools
{
	private COMA_Tools()
	{
	}

	public static int[] GetRandomArray(int nSize)
	{
		int[] array = new int[nSize];
		int[] array2 = new int[nSize];
		int num = 0;
		int num2 = nSize;
		for (num = 0; num < nSize; num++)
		{
			array[num] = num;
		}
		for (num = 0; num < nSize; num++)
		{
			int num3 = Random.Range(0, num2);
			array2[num] = array[num3];
			int num4 = array[num3];
			array[num3] = array[num2 - 1];
			array[num2 - 1] = num4;
			num2--;
		}
		return array2;
	}

	public static int GetRandomArray_AppointArray(ref int[] inputArray, ref int[] outArray)
	{
		int num = inputArray.Length;
		int[] array = new int[num];
		int num2 = 0;
		int num3 = num;
		for (num2 = 0; num2 < num; num2++)
		{
			array[num2] = inputArray[num2];
		}
		for (num2 = 0; num2 < outArray.Length; num2++)
		{
			int num4 = Random.Range(0, num3);
			outArray[num2] = array[num4];
			int num5 = array[num4];
			array[num4] = array[num3 - 1];
			array[num3 - 1] = num5;
			num3--;
		}
		return outArray.Length;
	}

	public static int GetRandomArray_AppointSize(int nSize, ref int[] outArray)
	{
		int[] array = new int[nSize];
		int num = 0;
		int num2 = nSize;
		for (num = 0; num < nSize; num++)
		{
			array[num] = num;
		}
		for (num = 0; num < outArray.Length; num++)
		{
			int num3 = Random.Range(0, num2);
			outArray[num] = array[num3];
			int num4 = array[num3];
			array[num3] = array[num2 - 1];
			array[num2 - 1] = num4;
			num2--;
		}
		return outArray.Length;
	}

	public static string UTF8ByteArrayToString(byte[] characters)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(characters);
	}

	public static byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetBytes(pXmlString);
	}

	public static string SerializeObject<T>(object pObject)
	{
		string empty = string.Empty;
		MemoryStream stream = new MemoryStream();
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		xmlSerializer.Serialize(xmlTextWriter, pObject);
		stream = (MemoryStream)xmlTextWriter.BaseStream;
		return UTF8ByteArrayToString(stream.ToArray());
	}

	public static object DeserializeObject<T>(string pXmlizedString)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		MemoryStream stream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		return xmlSerializer.Deserialize(stream);
	}

	public static string AwardSerialNameToTexture(string strSerialName)
	{
		if (strSerialName == "HF01")
		{
			return "nanguatou";
		}
		if (strSerialName == "HF02")
		{
			return "jinglingyanjing";
		}
		if (strSerialName == "HF03")
		{
			return "luosaihu";
		}
		if (strSerialName == "HF04")
		{
			return "xiaohuangren";
		}
		if (strSerialName == "CB03")
		{
			return "budai";
		}
		switch (strSerialName)
		{
		case "CB03":
			return "shubao";
		case "HT11":
			return "erji";
		case "HT12":
			return "falao";
		case "HT13":
			return "tianshi";
		case "HT14":
			return "lujiao";
		case "HT15":
			return "shengdanmao";
		case "HT16":
			return "guo";
		case "HT17":
			return "laohu";
		case "HT18":
			return "shizi";
		case "HT19":
			return "gongji";
		case "HT20":
			return "huangya";
		case "HT21":
			return "xiongmao";
		case "HT22":
			return "changjinlu";
		case "HT23":
			return "yindian";
		case "HT24":
			return "aijiyanhou";
		case "HT55":
			return "pengke";
		case "CB05":
			return "deco_CB05";
		case "CB06":
			return "deco_CB06";
		case "CB07":
			return "deco_CB07";
		case "CB08":
			return "deco_CB08";
		case "HF05":
			return "deco_HF05";
		case "HF06":
			return "deco_HF06";
		case "HF07":
			return "deco_HF07";
		case "HT25":
			return "deco_HT25";
		case "HT26":
			return "deco_HT26";
		case "HT27":
			return "deco_HT27";
		case "HT28":
			return "deco_HT28";
		case "HT29":
			return "deco_HT29";
		case "HT30":
			return "deco_HT30";
		case "HT31":
			return "deco_HT31";
		case "HT32":
			return "deco_HT32";
		case "CB16":
			return "jiguangjian";
		default:
			return string.Empty;
		}
	}
}
