using System.IO;
using UnityEngine;

public class FileUtil
{
	private static string m_savePath;

	static FileUtil()
	{
		string dataPath = Application.dataPath;
		dataPath += "/../Documents";
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		m_savePath = dataPath;
	}

	public static string GetSavePath()
	{
		return m_savePath;
	}

	public static string LoadResourcesFile(string filename)
	{
		TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		return textAsset.text;
	}

	public static void WriteSave(string filename, string content)
	{
		try
		{
			FileStream fileStream = new FileStream(m_savePath + "/" + filename, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(content);
			streamWriter.Close();
			fileStream.Close();
		}
		catch
		{
			Debug.Log("WriteSave error, filename:" + filename);
		}
	}

	public static string ReadSave(string filename)
	{
		if (!File.Exists(m_savePath + "/" + filename))
		{
			Debug.Log("ReadSave error, file not exist, filename:" + filename);
			return null;
		}
		try
		{
			FileStream fileStream = new FileStream(m_savePath + "/" + filename, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			return result;
		}
		catch
		{
			Debug.Log("ReadSave error, filename:" + filename);
			return null;
		}
	}
}
