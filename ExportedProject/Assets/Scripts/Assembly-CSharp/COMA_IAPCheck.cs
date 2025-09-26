using System;
using System.Collections.Generic;

public class COMA_IAPCheck
{
	private static COMA_IAPCheck _instance;

	private string sepSign = "zheshiyigechangfengefu";

	private string sepSignSub = "zheshiyigezifengefu";

	private List<string> identities = new List<string>();

	public static COMA_IAPCheck Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_IAPCheck();
			}
			return _instance;
		}
	}

	public string content
	{
		get
		{
			string text = string.Empty;
			foreach (string identity in identities)
			{
				text = text + sepSign + identity;
			}
			if (text != string.Empty)
			{
				text = text.Substring(sepSign.Length);
			}
			return text;
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				identities.Clear();
				string[] array = value.Split(new string[1] { sepSign }, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					identities.Add(array[i]);
				}
			}
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public int GetCheckIAPCount()
	{
		return identities.Count / 4;
	}

	public string GetSubInfo0(int index)
	{
		return identities[index * 4];
	}

	public string GetSubInfo1(int index)
	{
		return identities[index * 4 + 1];
	}

	public string GetSubInfo2(int index)
	{
		return identities[index * 4 + 2];
	}

	public string GetSubInfo3(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		return array[0];
	}

	public string GetSubInfo4(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 1)
		{
			return array[1];
		}
		return "0";
	}

	public string GetSubInfo5(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 2)
		{
			return array[2];
		}
		return "0";
	}

	public string GetSubInfo6(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 3)
		{
			return array[3];
		}
		return "0";
	}

	public string GetSubInfo7(int index)
	{
		string[] array = identities[index * 4 + 3].Split(new string[1] { sepSignSub }, StringSplitOptions.None);
		if (array.Length > 4)
		{
			return array[4];
		}
		return "0";
	}

	public void AddToLocal(string a, string b, string c, string d)
	{
		AddToLocal(a, b, c, d, "0", "0", "0", "0");
	}

	public void AddToLocal(string a, string b, string c, string d, string c1, string c2, string c3, string c4)
	{
		d = d + sepSignSub + c1 + sepSignSub + c2 + sepSignSub + c3 + sepSignSub + c4;
		identities.Add(a);
		identities.Add(b);
		identities.Add(c);
		identities.Add(d);
		COMA_Pref.Instance.Save(true);
	}

	public void RemoveToLocal(string a, string b, string c, string d)
	{
		RemoveToLocal(a, b, c, d, "0", "0", "0", "0");
	}

	public void RemoveToLocal(string a, string b, string c, string d, string c1, string c2, string c3, string c4)
	{
		for (int i = 0; i < identities.Count; i += 4)
		{
			if (identities[i] == a && identities[i + 1] == b && identities[i + 2] == c && identities[i + 3].Contains(d))
			{
				identities.RemoveAt(i + 3);
				identities.RemoveAt(i + 2);
				identities.RemoveAt(i + 1);
				identities.RemoveAt(i);
				break;
			}
		}
		COMA_Pref.Instance.Save(true);
	}
}
