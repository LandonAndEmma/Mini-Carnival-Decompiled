using System.Collections.Generic;

public class COMA_TexOnSale
{
	public class COMA_TexOnSaleItem
	{
		private char sepSign = ' ';

		public bool isSuit;

		public string tid = string.Empty;

		public int numGet;

		public int goldGot;

		public string content
		{
			get
			{
				string text = isSuit.ToString();
				text = text + sepSign + tid;
				text = text + sepSign + numGet.ToString();
				return text + sepSign + goldGot.ToString();
			}
			set
			{
				if (value != null && !(value == string.Empty))
				{
					string[] array = value.Split(sepSign);
					int num = 0;
					isSuit = bool.Parse(array[num++]);
					tid = array[num++];
					numGet = int.Parse(array[num++]);
					goldGot = int.Parse(array[num++]);
				}
			}
		}
	}

	private static COMA_TexOnSale _instance;

	private char sepSign = '_';

	public List<COMA_TexOnSaleItem> items = new List<COMA_TexOnSaleItem>();

	public static COMA_TexOnSale Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_TexOnSale();
			}
			return _instance;
		}
	}

	public string content
	{
		get
		{
			string text = string.Empty;
			foreach (COMA_TexOnSaleItem item in items)
			{
				text = text + sepSign + item.content;
			}
			if (text != string.Empty)
			{
				text = text.Substring(1);
			}
			return text;
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				items.Clear();
				string[] array2 = array;
				foreach (string text in array2)
				{
					COMA_TexOnSaleItem cOMA_TexOnSaleItem = new COMA_TexOnSaleItem();
					cOMA_TexOnSaleItem.content = text;
					items.Add(cOMA_TexOnSaleItem);
				}
			}
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public void ResetData()
	{
		items.Clear();
	}

	public void DeleteWithtid(string tid)
	{
		for (int num = items.Count - 1; num >= 0; num--)
		{
			if (items[num].tid == tid)
			{
				items.RemoveAt(num);
			}
		}
	}

	public int GetNumGet(string tid)
	{
		for (int num = items.Count - 1; num >= 0; num--)
		{
			if (items[num].tid == tid)
			{
				return items[num].numGet;
			}
		}
		return 0;
	}

	public void SetNumGet(string tid, int num)
	{
		for (int num2 = items.Count - 1; num2 >= 0; num2--)
		{
			if (items[num2].tid == tid)
			{
				items[num2].numGet = num;
				break;
			}
		}
	}

	public int GetGoldGet(string tid)
	{
		for (int num = items.Count - 1; num >= 0; num--)
		{
			if (items[num].tid == tid)
			{
				return items[num].goldGot;
			}
		}
		return 0;
	}

	public void SetGoldGet(string tid, int gold)
	{
		for (int num = items.Count - 1; num >= 0; num--)
		{
			if (items[num].tid == tid)
			{
				items[num].goldGot += gold;
				break;
			}
		}
	}
}
