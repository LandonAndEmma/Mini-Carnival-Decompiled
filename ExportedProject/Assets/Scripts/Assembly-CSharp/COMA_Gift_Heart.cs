using System.Collections.Generic;

public class COMA_Gift_Heart : COMA_ResetInstance
{
	private class Heart
	{
		public string friendID = string.Empty;

		public double giveTime;
	}

	private static COMA_Gift_Heart _instance;

	private List<Heart> lst_heart = new List<Heart>();

	private char sepSign = 'X';

	private char subSepSign = 'x';

	public static COMA_Gift_Heart Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_Gift_Heart();
				COMA_InstanceManager.Instance.RegistResetInstance(_instance);
			}
			return _instance;
		}
	}

	public string content
	{
		get
		{
			string text = string.Empty;
			foreach (Heart item in lst_heart)
			{
				string text2 = text;
				text = text2 + sepSign + item.friendID + subSepSign + item.giveTime.ToString();
			}
			if (text != string.Empty)
			{
				return text.Substring(1);
			}
			return string.Empty;
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				lst_heart.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(subSepSign);
					Heart heart = new Heart();
					heart.friendID = array2[0];
					heart.giveTime = float.Parse(array2[1]);
					lst_heart.Add(heart);
				}
			}
		}
	}

	public override void ResetInstance()
	{
		_instance = null;
	}

	public double GetGiftTime(string friendID)
	{
		foreach (Heart item in lst_heart)
		{
			if (item.friendID == friendID)
			{
				return item.giveTime;
			}
		}
		return 0.0;
	}

	public void SetGiftTime(string friendID)
	{
		for (int i = 0; i < lst_heart.Count; i++)
		{
			if (lst_heart[i].friendID == friendID)
			{
				lst_heart[i].giveTime = COMA_Server_Account.Instance.GetServerTime();
				return;
			}
		}
		Heart heart = new Heart();
		heart.friendID = friendID;
		heart.giveTime = COMA_Server_Account.Instance.GetServerTime();
		lst_heart.Add(heart);
	}
}
