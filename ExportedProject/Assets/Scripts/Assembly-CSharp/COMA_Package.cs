using UnityEngine;

public class COMA_Package
{
	private char sepSign = ';';

	public static int maxCount = 72;

	public static int unlockPrice = 5;

	public COMA_PackageItem[] pack = new COMA_PackageItem[maxCount + 3];

	public static readonly int SLOTUNLOCKED = 16;

	public static int slotUnlocked = SLOTUNLOCKED;

	public string content
	{
		get
		{
			string text = slotUnlocked.ToString();
			for (int i = 0; i < maxCount; i++)
			{
				text = ((pack[i] != null) ? (text + sepSign + pack[i].content) : (text + sepSign));
			}
			return text;
		}
		set
		{
			if (value == null || value == string.Empty)
			{
				return;
			}
			string[] array = value.Split(sepSign);
			int i = 0;
			slotUnlocked = int.Parse(array[i++]);
			int num = 0;
			Debug.Log(value);
			for (; i < array.Length; i++)
			{
				string text = array[i];
				COMA_PackageItem cOMA_PackageItem = null;
				if (text != string.Empty)
				{
					cOMA_PackageItem = new COMA_PackageItem();
					cOMA_PackageItem.content = text;
				}
				if (cOMA_PackageItem != null && cOMA_PackageItem.textureName != string.Empty && cOMA_PackageItem.texture == null && !cOMA_PackageItem.LoadPNG())
				{
					COMA_Pref.Instance.DownLoadAPackageTexture(cOMA_PackageItem.tid, num);
				}
				pack[num] = cOMA_PackageItem;
				num++;
			}
			Debug.Log("Init Package content over");
		}
	}

	public void Delete(int id)
	{
		if (pack[id] != null)
		{
			pack[id].Delete();
			pack[id] = null;
		}
	}

	public void Clear()
	{
		for (int i = 0; i < maxCount; i++)
		{
			Delete(i);
		}
	}
}
