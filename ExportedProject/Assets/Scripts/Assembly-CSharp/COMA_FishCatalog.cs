using System.Collections.Generic;
using UnityEngine;

public class COMA_FishCatalog : COMA_ResetInstance
{
	public class Catalog
	{
		public int kind;

		public int num;

		public int maxWeight;

		public byte star;
	}

	private static COMA_FishCatalog _instance;

	public List<Catalog> lst = new List<Catalog>();

	private int fishCount = 40;

	private char sepSign = 'X';

	private char subSepSign = 'x';

	public static COMA_FishCatalog Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_FishCatalog();
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
			for (int i = 0; i < fishCount; i++)
			{
				string text2 = text;
				text = text2 + sepSign + lst[i].num.ToString() + subSepSign + lst[i].maxWeight.ToString() + subSepSign + lst[i].star.ToString();
			}
			if (text != string.Empty)
			{
				return text.Substring(1);
			}
			return string.Empty;
		}
		set
		{
			if (value == null || value == string.Empty)
			{
				return;
			}
			string[] array = value.Split(sepSign);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(subSepSign);
				lst[i].num = int.Parse(array2[0]);
				lst[i].maxWeight = int.Parse(array2[1]);
				if (array2.Length > 2)
				{
					lst[i].star = byte.Parse(array2[2]);
				}
			}
		}
	}

	protected COMA_FishCatalog()
	{
		for (int i = 0; i < fishCount; i++)
		{
			Catalog item = new Catalog
			{
				kind = i
			};
			lst.Add(item);
		}
	}

	public override void ResetInstance()
	{
		_instance = null;
	}

	public void GetFish(int type, int weight)
	{
		int index = type - 1;
		lst[index].num++;
		if (lst[index].maxWeight < weight)
		{
			lst[index].maxWeight = weight;
		}
		Fish_Param fishParam = COMA_Fishing_FishPool.Instance.GetFishParam(type);
		if ((float)weight < Mathf.Lerp(fishParam._nMinWeight, fishParam._nMaxWeight, 0.4f))
		{
			lst[index].star |= 4;
		}
		else if ((float)weight < Mathf.Lerp(fishParam._nMinWeight, fishParam._nMaxWeight, 0.75f))
		{
			lst[index].star |= 2;
		}
		else
		{
			lst[index].star |= 1;
		}
		Debug.Log(lst[index].star);
		COMA_Pref.Instance.Save(true);
	}

	public int GetFishCount()
	{
		return fishCount;
	}
}
