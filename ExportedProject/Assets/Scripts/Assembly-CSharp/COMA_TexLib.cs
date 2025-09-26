using System.Collections.Generic;
using UnityEngine;

public class COMA_TexLib
{
	private static COMA_TexLib _instance;

	public string defaultFileName = COMA_FileNameManager.Instance.GetFileName("Tex");

	private char sepSign = '\n';

	public List<string> texNames = new List<string>();

	public Dictionary<string, Texture2D> currentRoomPlayerTextures = new Dictionary<string, Texture2D>();

	public static COMA_TexLib Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_TexLib();
			}
			return _instance;
		}
	}

	public string content
	{
		get
		{
			string text = string.Empty;
			foreach (string texName in texNames)
			{
				text = text + sepSign + texName;
			}
			if (text == string.Empty)
			{
				return string.Empty;
			}
			return text.Substring(1);
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				for (int i = 0; i < array.Length; i++)
				{
					texNames.Add(array[i]);
				}
			}
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}
}
