using UnityEngine;

public class UIArmory_WeaponBoxData : UI_BoxData
{
	public static string[] DSLib = new string[5] { "D", "C", "B", "A", "S" };

	private string name;

	private float price;

	private int level;

	private int ownNum;

	private string damage;

	private string speed;

	private byte purchaseType;

	private bool purchaseState;

	private RenderTexture weaponPic;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
			DataChanged();
		}
	}

	public float Price
	{
		get
		{
			return price;
		}
		set
		{
			price = value;
			DataChanged();
		}
	}

	public int Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
			DataChanged();
		}
	}

	public int OwnNum
	{
		get
		{
			return ownNum;
		}
		set
		{
			ownNum = value;
			DataChanged();
		}
	}

	public string Damage
	{
		get
		{
			return damage;
		}
		set
		{
			damage = value;
			DataChanged();
		}
	}

	public string Speed
	{
		get
		{
			return speed;
		}
		set
		{
			speed = value;
			DataChanged();
		}
	}

	public byte PurchaseType
	{
		get
		{
			return purchaseType;
		}
		set
		{
			purchaseType = value;
			DataChanged();
		}
	}

	public bool PurchaseState
	{
		get
		{
			return purchaseState;
		}
		set
		{
			purchaseState = value;
			DataChanged();
		}
	}

	public RenderTexture WeaponPic
	{
		get
		{
			return weaponPic;
		}
		set
		{
			weaponPic = value;
			DataChanged();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
