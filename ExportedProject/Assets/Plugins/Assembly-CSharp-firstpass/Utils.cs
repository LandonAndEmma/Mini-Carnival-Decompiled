using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using UnityEngine;

public class Utils
{
	private static string m_SavePath;

	public static long ServerTime;

	public static bool MusicOn
	{
		get
		{
			return PlayerPrefs.GetInt("MusicOff", 0) == 0;
		}
		set
		{
			if (value)
			{
				PlayerPrefs.SetInt("MusicOff", 0);
			}
			else
			{
				PlayerPrefs.SetInt("MusicOff", 1);
			}
		}
	}

	public static bool SoundOn
	{
		get
		{
			return PlayerPrefs.GetInt("SoundOff", 0) == 0;
		}
		set
		{
			if (value)
			{
				PlayerPrefs.SetInt("SoundOff", 0);
			}
			else
			{
				PlayerPrefs.SetInt("SoundOff", 1);
			}
		}
	}

	static Utils()
	{
		string dataPath = Application.dataPath;
		dataPath = Application.persistentDataPath;
		dataPath += "/CoMInfinityData/Documents";
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		dataPath += "/CoMInfinityData";
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		m_SavePath = dataPath;
	}

	public static string SavePath()
	{
		return m_SavePath;
	}

	public static bool IsNetworkConnected()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			return false;
		}
		return true;
	}

	public static bool inEditor()
	{
		string text = Application.platform.ToString();
		if (text.IndexOf("Editor") != -1)
		{
			return true;
		}
		return false;
	}

	public static bool inWindow()
	{
		string text = Application.platform.ToString();
		if (text.IndexOf("Windows") != -1)
		{
			return true;
		}
		return false;
	}

	public static bool IsNumeric(string str)
	{
		try
		{
			long.Parse(str);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static string[] Split(string str, string separator)
	{
		ArrayList arrayList = new ArrayList();
		int length = separator.Length;
		while (str.IndexOf(separator) != -1)
		{
			int num = str.IndexOf(separator);
			arrayList.Add(str.Substring(0, num));
			str = str.Substring(num + length, str.Length - num - length);
		}
		if (str.Length > 0)
		{
			arrayList.Add(str);
		}
		string[] array = new string[arrayList.Count];
		for (int i = 0; i < arrayList.Count; i++)
		{
			array[i] = arrayList[i].ToString();
		}
		return array;
	}

	public static string TimeLeft(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			text = num4 + "d ";
			num5++;
		}
		if (num3 > 0)
		{
			text = text + num3 + "h ";
			num5++;
		}
		if (num > 0 && num5 < 2)
		{
			text = text + num + "m ";
			num5++;
		}
		if (num2 > 0 && num5 < 2)
		{
			text = text + num2 + "s";
			num5++;
		}
		return text;
	}

	public static string TimeLeftFullShow(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			num5++;
			text = num4 + " D" + ((num4 <= 1) ? string.Empty : string.Empty) + ((num5 >= 2) ? string.Empty : ", ");
		}
		if (num3 > 0)
		{
			num5++;
			string text2 = text;
			text = text2 + num3 + " H" + ((num3 <= 1) ? string.Empty : string.Empty) + ((num5 >= 2) ? string.Empty : ", ");
		}
		if (num > 0 && num5 < 2)
		{
			num5++;
			string text2 = text;
			text = text2 + num + " Min" + ((num <= 1) ? string.Empty : string.Empty) + ((num5 >= 2) ? string.Empty : ", ");
		}
		if (num2 > 0 && num5 < 2)
		{
			num5++;
		}
		return text;
	}

	public static string TimeLeftFullShowDHM(long seconds)
	{
		long num = seconds % 60;
		long num2 = seconds / 60;
		long num3 = num2 / 60;
		num2 %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			num5++;
			text = num4 + " Day" + ((num4 <= 1) ? string.Empty : "s") + ((num3 <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num3 > 0)
		{
			num5++;
			string text2 = text;
			text = text2 + num3 + " Hour" + ((num3 <= 1) ? string.Empty : "s") + ((num2 <= 0) ? string.Empty : ", ");
		}
		if (num2 > 0)
		{
			num5++;
			text = text + num2 + " Min";
		}
		return text;
	}

	public static string MinuteTimeBase(int minutes)
	{
		int num = minutes % 60;
		int num2 = minutes / 60;
		int num3 = num2 / 24;
		num2 %= 24;
		string text = string.Empty;
		if (num3 > 0)
		{
			text = text + num3 + "d ";
		}
		if (num2 > 0)
		{
			text = text + num2 + "h ";
		}
		if (num > 0)
		{
			text = text + num + "m";
		}
		return text;
	}

	public static string TimeToStr_HMS(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string empty = string.Empty;
		num5++;
		string text = num3.ToString();
		if (num3 < 10)
		{
			text = "0" + num3;
		}
		empty = empty + text + ":";
		num5++;
		string text2 = num.ToString();
		if (num < 10)
		{
			text2 = "0" + num;
		}
		empty = empty + text2 + ":";
		num5++;
		string text3 = num2.ToString();
		if (num2 < 10)
		{
			text3 = "0" + num2;
		}
		return empty + text3;
	}

	public static string TimeToStr_HM(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string empty = string.Empty;
		num5++;
		string text = num3.ToString();
		if (num3 < 10)
		{
			text = "0" + num3;
		}
		empty = empty + text + ":";
		num5++;
		string text2 = num.ToString();
		if (num < 10)
		{
			text2 = "0" + num;
		}
		return empty + text2;
	}

	public static string TimeToStr_D_HM(long seconds)
	{
		string empty = string.Empty;
		if (seconds > 0)
		{
			long num = seconds % 60;
			long num2 = seconds / 60;
			long num3 = num2 % 60;
			long num4 = num2 / 60;
			long num5 = num4 / 24;
			long num6 = num4 % 24;
			if (num5 > 0)
			{
				string text = num5.ToString();
				return text + " Day";
			}
			string text2 = num6.ToString();
			if (num6 < 10)
			{
				text2 = "0" + num6;
			}
			empty = empty + text2 + ":";
			if (num3 > 0)
			{
				string text3 = num3.ToString();
				if (num3 < 10)
				{
					text3 = "0" + num3;
				}
				return empty + text3;
			}
			if (num > 0)
			{
				return empty + "01";
			}
			return empty + "00";
		}
		return "00:00";
	}

	public static string TimeToStr_DHM(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string empty = string.Empty;
		num5++;
		string text = num4.ToString();
		if (num4 < 10)
		{
			text = "0" + num4;
		}
		empty = empty + text + ":";
		num5++;
		string text2 = num3.ToString();
		if (num3 < 10)
		{
			text2 = "0" + num3;
		}
		empty = empty + text2 + ":";
		num5++;
		string text3 = num.ToString();
		if (num < 10)
		{
			text3 = "0" + num;
		}
		return empty + text3;
	}

	public static Vector2 calcTextSize(string text, Font tfont)
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.font = tfont;
		return gUIStyle.CalcSize(new GUIContent(text));
	}

	public static Vector3 toWorldPosition(Vector3 pos)
	{
		return Camera.main.ScreenPointToRay(pos).GetPoint(20f);
	}

	public static string GetDateTimeDebugString(DateTime date)
	{
		return date.ToString("MM/dd/yyyy HH:mm");
	}

	public static DateTime GetDateTimeOfSeconds(long seconds)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(seconds);
	}

	public static DateTime GetDateTimeOfServer()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		long num = (long)timeSpan.TotalSeconds;
		long num2 = num + PlayerPrefs.GetInt("DiffTime");
		return new DateTime(new DateTime(1970, 1, 1, 0, 0, 0).Ticks + num2 * 10000000);
	}

	public static long getNowDateSeconds()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		long num = (long)timeSpan.TotalSeconds;
		return num + PlayerPrefs.GetInt("DiffTime");
	}

	public static long getNowDateMinutes()
	{
		return (long)new TimeSpan(DateTime.Now.ToUniversalTime().Ticks).TotalMinutes;
	}

	public static long getNowIntervalSeconds()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		return (long)timeSpan.TotalSeconds;
	}

	public static long getNowIntervalMinutes()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		return (long)timeSpan.TotalMinutes;
	}

	public static string CurrentMethod(StackTrace st)
	{
		if (st.FrameCount > 0)
		{
			StackFrame frame = st.GetFrame(0);
			return frame.GetMethod().Name;
		}
		return string.Empty;
	}

	public static float AngleToRadian(float angle)
	{
		return (float)Math.PI / 180f * angle;
	}

	public static float RadianToAngle(float radian)
	{
		return radian * (float)Math.PI / 180f;
	}

	public static bool IsNumberLetter(string input)
	{
		bool result = true;
		for (int i = 0; i < input.Length; i++)
		{
			int num = Convert.ToInt32(Convert.ToChar(input.Substring(i, 1)));
			if ((num < 48 || num > 57) && (num < 65 || num > 90) && (num < 97 || num > 122))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static bool IsChineseLetter(string input)
	{
		for (int i = 0; i < input.Length; i++)
		{
			int num = Convert.ToInt32(Convert.ToChar(input.Substring(i, 1)));
			if (num >= 128)
			{
				return true;
			}
		}
		return false;
	}

	public static Vector3 cameraRoundPos()
	{
		Vector3 position = Camera.main.transform.position;
		return new Vector3(UnityEngine.Random.Range(position.x - 5f, position.x + 5f), 10f, position.x - 10f);
	}

	public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((!(Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f)) ? 1 : (-1));
	}

	public static void OpenWebSite(string url)
	{
	}

	public static bool OSIsJailBreak()
	{
		return true;
	}

	public static string GetMD5(string strContent)
	{
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.Default.GetBytes(strContent);
		byte[] array = mD.ComputeHash(bytes);
		return BitConverter.ToString(array).Replace("-", string.Empty);
	}

	public static string EncryptData(string input_data, string encrypt_key)
	{
		string empty = string.Empty;
		try
		{
			byte[] inArray = XXTEAUtils.Encrypt(Encoding.UTF8.GetBytes(input_data), Encoding.ASCII.GetBytes(encrypt_key));
			return Convert.ToBase64String(inArray);
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public static string DecryptData(string input_data, string encrypt_key)
	{
		string empty = string.Empty;
		try
		{
			byte[] data = Convert.FromBase64String(input_data);
			byte[] bytes = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(encrypt_key));
			return Encoding.UTF8.GetString(bytes);
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public static void ZipString(string content, ref string zipedcontent)
	{
		if (content.Length >= 1)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(content);
			MemoryStream memoryStream = new MemoryStream();
			DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(memoryStream);
			deflaterOutputStream.Write(bytes, 0, bytes.Length);
			deflaterOutputStream.Close();
			bytes = memoryStream.ToArray();
			zipedcontent = Convert.ToBase64String(bytes);
		}
	}

	public static void UnZipString(string content, ref string unzipedcontent)
	{
		if (content.Length >= 1)
		{
			byte[] array = Convert.FromBase64String(content);
			InflaterInputStream inflaterInputStream = new InflaterInputStream(new MemoryStream(array, 0, array.Length));
			MemoryStream memoryStream = new MemoryStream();
			int num = 0;
			byte[] array2 = new byte[4096];
			while ((num = inflaterInputStream.Read(array2, 0, array2.Length)) != 0)
			{
				memoryStream.Write(array2, 0, num);
			}
			unzipedcontent = Encoding.UTF8.GetString(memoryStream.ToArray());
		}
	}

	public static void ObjectSetActiveRecursively(GameObject rootObject, bool active)
	{
		rootObject.SetActive(active);
		foreach (Transform item in rootObject.transform)
		{
			ObjectSetActiveRecursively(item.gameObject, active);
		}
	}
}
