using System;
using UnityEngine;

public class COMA_GC_TID
{
	private static COMA_GC_TID _instance;

	public string defaultFileName = COMA_FileNameManager.Instance.GetFileName("GCTID");

	public string gcLocal = string.Empty;

	public string gidLocal = string.Empty;

	public string fbLocal = string.Empty;

	public string device_gidLocal = string.Empty;

	private string sepSign = "985 1 1 2        15 1        561";

	public static COMA_GC_TID Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_GC_TID();
			}
			return _instance;
		}
	}

	public string content
	{
		get
		{
			return gcLocal + sepSign + gidLocal + sepSign + fbLocal + sepSign + device_gidLocal;
		}
		set
		{
			if (value == null || value == string.Empty)
			{
				return;
			}
			Debug.Log(value);
			string[] array = value.Split(new string[1] { sepSign }, StringSplitOptions.None);
			if (array.Length >= 2)
			{
				gcLocal = array[0];
				gidLocal = array[1];
				if (array.Length >= 3)
				{
					fbLocal = array[2];
				}
				if (array.Length >= 4)
				{
					device_gidLocal = array[3];
				}
			}
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public void Load()
	{
		content = COMA_FileIO.LoadFile(defaultFileName);
	}

	public void Save()
	{
		COMA_FileIO.SaveFile(defaultFileName, content);
	}
}
