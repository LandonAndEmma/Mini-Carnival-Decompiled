using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class COMA_FileIO
{
	public static readonly string dataPath;

	private static readonly string dotName;

	private static bool bNeedEncrypt;

	private static byte[] key;

	private static string CHECK;

	static COMA_FileIO()
	{
		dotName = ".pc";
		bNeedEncrypt = true;
		key = new byte[16]
		{
			161, 233, 184, 245, 193, 162, 208, 178, 184, 245,
			193, 162, 162, 233, 184, 245
		};
		CHECK = "COMA";
		Debug.Log(CHECK);
		string cHECK = CHECK;
		CHECK = cHECK + "_" + SystemInfo.deviceModel + "_" + SystemInfo.deviceUniqueIdentifier + "_" + SystemInfo.graphicsDeviceVersion;
		dataPath = Application.persistentDataPath;
		dataPath = dataPath.Substring(0, dataPath.LastIndexOf('/'));
		dataPath = dataPath.Substring(0, dataPath.LastIndexOf('/'));
		dataPath += "/Documents";
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		string path = dataPath + "/Friends";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		string path2 = dataPath + "/TexCachePath";
		if (!Directory.Exists(path2))
		{
			Directory.CreateDirectory(path2);
		}
		string path3 = dataPath + "/Transfer";
		if (!Directory.Exists(path3))
		{
			Directory.CreateDirectory(path3);
		}
		string path4 = dataPath + "/Levels";
		if (!Directory.Exists(path4))
		{
			Directory.CreateDirectory(path4);
		}
		string path5 = dataPath + "/Configs";
		if (!Directory.Exists(path5))
		{
			Directory.CreateDirectory(path5);
		}
	}

	public static string GetTexCachePath(string name)
	{
		return dataPath + "/TexCachePath/" + name + dotName;
	}

	public static byte[] LoadTexture(string fileName)
	{
		string path = dataPath + "/" + fileName + ".dat";
		if (File.Exists(path))
		{
			byte[] bytes = File.ReadAllBytes(path);
			return DecodeBytes(bytes);
		}
		return null;
	}

	public static void SaveTexture(string fileName, byte[] bytes)
	{
		string path = dataPath + "/" + fileName + ".dat";
		byte[] bytes2 = EncodeBytes(bytes);
		File.WriteAllBytes(path, bytes2);
	}

	public static byte[] EncodeBytes(byte[] bytes)
	{
		List<byte> list = new List<byte>();
		byte b = 0;
		int num = 0;
		for (int i = 0; i < bytes.Length; i++)
		{
			byte b2 = bytes[i];
			if (b2.CompareTo(b) == 0)
			{
				num++;
				continue;
			}
			if (num > 4)
			{
				list.Add(byte.MaxValue);
				list.Add(b);
				list.Add((byte)((0xFF00 & num) >> 8));
				list.Add((byte)(0xFF & num));
			}
			else
			{
				while (num > 0)
				{
					list.Add(b);
					num--;
				}
			}
			b = b2;
			num = 1;
		}
		if (num > 4)
		{
			list.Add(byte.MaxValue);
			list.Add(b);
			list.Add((byte)((0xFF00 & num) >> 8));
			list.Add((byte)(0xFF & num));
		}
		else
		{
			while (num > 0)
			{
				list.Add(b);
				num--;
			}
		}
		return list.ToArray();
	}

	public static byte[] DecodeBytes(byte[] bytes)
	{
		List<byte> list = new List<byte>();
		int num = 0;
		for (int i = 0; i < bytes.Length; i++)
		{
			if (bytes[i].CompareTo(byte.MaxValue) == 0)
			{
				num = (Convert.ToInt32(bytes[i + 2]) << 8) | Convert.ToInt32(bytes[i + 3]);
				byte[] array = new byte[num];
				for (int j = 0; j < num; j++)
				{
					array[j] = bytes[i + 1];
				}
				list.AddRange(array);
				i += 3;
			}
			else
			{
				list.Add(bytes[i]);
			}
		}
		return list.ToArray();
	}

	public static string ReadTextDirectly(string fileName)
	{
		string path = dataPath + "/" + fileName;
		if (File.Exists(path))
		{
			return File.ReadAllText(path);
		}
		return string.Empty;
	}

	public static void WriteTextDirectly(string fileName, string content)
	{
		string path = dataPath + "/" + fileName;
		File.WriteAllText(path, content);
	}

	public static byte[] ReadByteDirectly(string fileName)
	{
		string path = dataPath + "/" + fileName;
		if (File.Exists(path))
		{
			return File.ReadAllBytes(path);
		}
		return null;
	}

	public static void WriteByteDirectly(string fileName, byte[] content)
	{
		string path = dataPath + "/" + fileName;
		File.WriteAllBytes(path, content);
	}

	public static bool IsExistFile(string path, string fileName)
	{
		string path2 = dataPath + "/" + path + "/" + fileName + ".dat";
		return File.Exists(path2);
	}

	public static bool IsExistFile(string fileName)
	{
		string path = dataPath + "/" + fileName + ".dat";
		return File.Exists(path);
	}

	private static string ReadFile(string path, string fileName)
	{
		string path2 = dataPath + "/" + path + "/" + fileName + ".dat";
		if (File.Exists(path2))
		{
			byte[] bytes = File.ReadAllBytes(path2);
			return Decrypt(bytes);
		}
		return string.Empty;
	}

	private static string ReadFile(string fileName)
	{
		string path = dataPath + "/" + fileName + ".dat";
		if (File.Exists(path))
		{
			byte[] bytes = File.ReadAllBytes(path);
			return Decrypt(bytes);
		}
		return string.Empty;
	}

	private static void WriteFile(string path, string fileName, string content)
	{
		string path2 = dataPath + "/" + path + "/" + fileName + ".dat";
		byte[] bytes = Encrypt(content);
		File.WriteAllBytes(path2, bytes);
	}

	private static void WriteFile(string fileName, string content)
	{
		string path = dataPath + "/" + fileName + ".dat";
		byte[] bytes = Encrypt(content);
		File.WriteAllBytes(path, bytes);
	}

	private static byte[] Encrypt(string content)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(content);
		if (!bNeedEncrypt)
		{
			return bytes;
		}
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] ^= key[i % 8];
		}
		return bytes;
	}

	private static string Decrypt(byte[] bytes)
	{
		if (!bNeedEncrypt)
		{
			return Encoding.UTF8.GetString(bytes);
		}
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] ^= key[i % 8];
		}
		return Encoding.UTF8.GetString(bytes);
	}

	public static void InitLocalTexture()
	{
	}

	public static string LoadFile(string path, string fileName)
	{
		string text = ReadFile(path, fileName);
		string[] array = text.Split('&');
		if (array[0] == CHECK)
		{
			return text.Substring(array[0].Length + 1);
		}
		return string.Empty;
	}

	public static string LoadFile(string fileName)
	{
		string text = ReadFile(fileName);
		string[] array = text.Split('&');
		if (array[0] == CHECK)
		{
			return text.Substring(array[0].Length + 1);
		}
		return string.Empty;
	}

	public static void SaveFile(string path, string fileName, string content)
	{
		content = CHECK + "&" + content;
		WriteFile(path, fileName, content);
	}

	public static void SaveFile(string fileName, string content)
	{
		content = CHECK + "&" + content;
		WriteFile(fileName, content);
	}

	public static void DeleteFile(string fileName)
	{
		string path = dataPath + "/" + fileName + ".dat";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static void WritePngData(string fileName, byte[] bytes)
	{
		if (fileName.Length < 8)
		{
			Debug.LogError("fileName length must longer than 8!");
		}
		string path = dataPath + "/" + fileName + dotName;
		File.WriteAllBytes(path, bytes);
	}

	public static byte[] ReadPngData(string fileName)
	{
		string path = dataPath + "/" + fileName + dotName;
		if (File.Exists(path))
		{
			return File.ReadAllBytes(path);
		}
		return null;
	}

	public static void DeletePngData(string fileName)
	{
		string path = dataPath + "/" + fileName + dotName;
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static List<string> SearchFriendFiles(string floderName, int count)
	{
		string path = dataPath + "/" + floderName + "/";
		string[] files = Directory.GetFiles(path, "*.dat");
		List<string> list = new List<string>();
		int num = Mathf.Min(files.Length, count);
		for (int i = 0; i < num; i++)
		{
			string text = files[i].Substring(files[i].LastIndexOf('/') + 1);
			text = text.Substring(0, text.LastIndexOf('.'));
			list.Add(text);
		}
		return list;
	}

	public static List<string> SearchTexCacheFiles(int count)
	{
		string path = dataPath + "/TexCachePath/";
		string searchPattern = "*" + dotName;
		string[] files = Directory.GetFiles(path, searchPattern);
		List<string> list = new List<string>();
		int num = Mathf.Min(files.Length, count);
		for (int i = 0; i < num; i++)
		{
			string text = files[i].Substring(files[i].LastIndexOf('/') + 1);
			text = text.Substring(0, text.LastIndexOf('.'));
			list.Add(text);
		}
		return list;
	}

	public static Texture2D GetTexFromCache(string fileName)
	{
		string path = dataPath + "/TexCachePath/" + fileName + dotName;
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
			texture2D.LoadImage(data);
			texture2D.Apply(false);
			return texture2D;
		}
		return null;
	}

	public static byte[] GetTexBufferFromCache(string fileName)
	{
		string path = dataPath + "/TexCachePath/" + fileName + dotName;
		if (File.Exists(path))
		{
			return File.ReadAllBytes(path);
		}
		return null;
	}

	public static void AddFileToCache(string fileName, byte[] bytes)
	{
		string path = dataPath + "/TexCachePath/" + fileName + dotName;
		File.WriteAllBytes(path, bytes);
	}

	public static void DelTexFileFromCache(string fileName)
	{
		string path = dataPath + "/TexCachePath/" + fileName + dotName;
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static string GetFileMD5(string fileContent)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(fileContent);
		return GetFileMD5(bytes);
	}

	public static string GetFileMD5(byte[] buffer)
	{
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array = mD.ComputeHash(buffer);
		string text = BitConverter.ToString(array);
		return text.Replace("-", string.Empty).ToLower();
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

	public static void RemoveEvent<T>(T c, string name)
	{
		Delegate[] objectEventList = GetObjectEventList(c, name);
		if (objectEventList != null)
		{
			Delegate[] array = objectEventList;
			foreach (Delegate handler in array)
			{
				typeof(T).GetEvent(name).RemoveEventHandler(c, handler);
			}
		}
	}

	public static Delegate[] GetObjectEventList(object p_Object, string p_EventName)
	{
		FieldInfo field = p_Object.GetType().GetField(p_EventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (field == null)
		{
			return null;
		}
		object value = field.GetValue(p_Object);
		if (value != null && value is Delegate)
		{
			Delegate obj = (Delegate)value;
			return obj.GetInvocationList();
		}
		return null;
	}
}
