using System.Collections.Generic;
using UnityEngine;

public class COMA_Fishing : COMA_ResetInstance
{
	private class FishingRod
	{
		public int id;

		public double time;
	}

	private static COMA_Fishing _instance;

	private List<FishingRod> lst = new List<FishingRod>();

	public int bFished0;

	public int bFished1;

	public int bFished2;

	public int nTime0 = 7200;

	public int nTime1 = 3600;

	public int nTime2;

	private char sepSign = 'X';

	private char subSepSign = 'x';

	public static COMA_Fishing Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_Fishing();
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
			foreach (FishingRod item in lst)
			{
				string text2 = text;
				text = text2 + sepSign + item.id.ToString() + subSepSign + item.time.ToString();
			}
			Debug.Log("fishing content : " + text);
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
				lst.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(subSepSign);
					FishingRod fishingRod = new FishingRod();
					fishingRod.id = int.Parse(array2[0]);
					fishingRod.time = double.Parse(array2[1]);
					lst.Add(fishingRod);
				}
			}
		}
	}

	public override void ResetInstance()
	{
		_instance = null;
	}

	public double GetPoleTime(int id)
	{
		foreach (FishingRod item in lst)
		{
			if (item.id == id)
			{
				return item.time;
			}
		}
		return 0.0;
	}

	public void SetPoleTime(int id)
	{
		for (int i = 0; i < lst.Count; i++)
		{
			if (lst[i].id == id)
			{
				lst[i].time = COMA_Server_Account.Instance.GetServerTime();
				return;
			}
		}
		FishingRod fishingRod = new FishingRod();
		fishingRod.id = id;
		fishingRod.time = COMA_Server_Account.Instance.GetServerTime();
		lst.Add(fishingRod);
	}
}
