using UnityEngine;

public class COMA_PackageItem
{
	public enum PackageItemStatus
	{
		None = 0,
		Equiped = 1,
		Sale = 2,
		LackLv = 3
	}

	private char sepSign = '^';

	public string serialName = string.Empty;

	public string itemName = string.Empty;

	public int num = 1;

	public string tid = string.Empty;

	public int gold;

	public int part;

	public int hpAdd;

	public int spdAdd;

	public PackageItemStatus state;

	public string textureName = string.Empty;

	public Texture2D texture;

	public Texture2D iconTexture;

	public string iconTextureName = string.Empty;

	private string accessoriesPrePath = "FBX/Player/Part/PFB/";

	public string content
	{
		get
		{
			string text = serialName;
			text = text + sepSign + itemName;
			text = text + sepSign + this.num.ToString();
			text = text + sepSign + tid;
			text = text + sepSign + gold.ToString();
			text = text + sepSign + part.ToString();
			text = text + sepSign + hpAdd.ToString();
			text = text + sepSign + spdAdd.ToString();
			string text2 = text;
			object obj = sepSign;
			int num = (int)state;
			text = string.Concat(text2, obj, num.ToString());
			return text + sepSign + textureName;
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				int num = 0;
				serialName = array[num++];
				itemName = array[num++];
				this.num = int.Parse(array[num++]);
				tid = array[num++];
				gold = int.Parse(array[num++]);
				part = int.Parse(array[num++]);
				hpAdd = int.Parse(array[num++]);
				spdAdd = int.Parse(array[num++]);
				state = (PackageItemStatus)int.Parse(array[num++]);
				textureName = array[num++];
			}
		}
	}

	public COMA_PackageItem()
	{
	}

	public COMA_PackageItem(string content)
	{
		this.content = content;
	}

	public static int NameToPart(string name)
	{
		if (name.StartsWith("HT"))
		{
			return 1;
		}
		if (name.StartsWith("HF"))
		{
			return 2;
		}
		if (name.StartsWith("HB"))
		{
			return 3;
		}
		if (name.StartsWith("HL"))
		{
			return 4;
		}
		if (name.StartsWith("HR"))
		{
			return 5;
		}
		if (name.StartsWith("CF"))
		{
			return 6;
		}
		if (name.StartsWith("CB"))
		{
			return 7;
		}
		if (name.StartsWith("AA08") || name.StartsWith("AA16") || name.StartsWith("AA18") || name.StartsWith("AA20") || name.StartsWith("AA23") || name.StartsWith("AA25") || name.StartsWith("AA26") || name.StartsWith("AA28") || name.StartsWith("AA37") || name.StartsWith("AA39") || name.StartsWith("AA40") || name.StartsWith("AA42") || name.StartsWith("AA46"))
		{
			return 1;
		}
		if (name.StartsWith("AA09") || name.StartsWith("AA22") || name.StartsWith("AA36") || name.StartsWith("AA38") || name.StartsWith("AA43") || name.StartsWith("AA44") || name.StartsWith("AA45") || name.StartsWith("AA47") || name.StartsWith("AA32"))
		{
			return 7;
		}
		if (name.StartsWith("AA10") || name.StartsWith("AA17"))
		{
			return 2;
		}
		return 0;
	}

	public static int SysNameToPart(string name)
	{
		switch (name)
		{
		case "Head01":
			return -1;
		case "Body01":
			return -2;
		case "Leg01":
			return -3;
		case "HBL01":
			return -4;
		default:
			if (name.StartsWith("HT"))
			{
				return 1;
			}
			if (name.StartsWith("HF"))
			{
				return 2;
			}
			if (name.StartsWith("HB"))
			{
				return 3;
			}
			if (name.StartsWith("HL"))
			{
				return 4;
			}
			if (name.StartsWith("HR"))
			{
				return 5;
			}
			if (name.StartsWith("CF"))
			{
				return 6;
			}
			if (name.StartsWith("CB"))
			{
				return 7;
			}
			return 0;
		}
	}

	public static int NameToKind(string name)
	{
		if (name.StartsWith("Head"))
		{
			return 0;
		}
		if (name.StartsWith("Body"))
		{
			return 1;
		}
		if (name.StartsWith("Leg"))
		{
			return 2;
		}
		return -1;
	}

	public static string KindToName(int kind)
	{
		switch (kind)
		{
		case 0:
			return "Head01";
		case 1:
			return "Body01";
		case 2:
			return "Leg01";
		default:
			return string.Empty;
		}
	}

	public void SavePNG()
	{
		COMA_FileIO.WritePngData(textureName, texture.EncodeToPNG());
	}

	public bool LoadPNG()
	{
		byte[] array = COMA_FileIO.ReadPngData(textureName);
		if (array != null)
		{
			texture = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
			texture.LoadImage(array);
			texture.filterMode = FilterMode.Point;
		}
		if (array == null || texture.width != COMA_TexBase.Instance.width || texture.height != COMA_TexBase.Instance.height)
		{
			return false;
		}
		return true;
	}

	public void LoadPNGDefault(int index)
	{
		switch (index)
		{
		case 0:
			texture = Resources.Load("FBX/Player/Character/Texture/T_head") as Texture2D;
			break;
		case 1:
			texture = Resources.Load("FBX/Player/Character/Texture/T_body") as Texture2D;
			break;
		case 2:
			texture = Resources.Load("FBX/Player/Character/Texture/T_leg") as Texture2D;
			break;
		}
		SavePNG();
	}

	public void CreateIconTexture()
	{
		iconTextureName = string.Empty;
		if (serialName.StartsWith("Head") || serialName.StartsWith("Body") || serialName.StartsWith("Leg"))
		{
			GameObject gameObject = Object.Instantiate(Resources.Load(accessoriesPrePath + serialName)) as GameObject;
			if (textureName != string.Empty)
			{
				texture.filterMode = FilterMode.Point;
			}
			gameObject.transform.GetChild(0).renderer.material.mainTexture = texture;
			iconTexture = IconShot.Instance.GetIconPic(gameObject);
			Object.DestroyObject(gameObject);
		}
		else
		{
			iconTextureName = COMA_Scene_Shop.iconPrename + serialName;
		}
	}

	public bool IsAccessoriesExist()
	{
		Object obj = Resources.Load(accessoriesPrePath + serialName);
		return obj != null;
	}

	public void Delete()
	{
		if (textureName != string.Empty)
		{
			tid = string.Empty;
			COMA_FileIO.DeletePngData(textureName);
			textureName = string.Empty;
			texture = null;
		}
	}
}
