using UnityEngine;

public class UIArmory_HelmentBoxData : UI_BoxData
{
	private Texture2D _tex;

	private string serial;

	private string name;

	private float price;

	private int num;

	private int part;

	private int level;

	private int ownNum;

	private int agi;

	private int hp;

	private byte purchaseType;

	private bool purchaseState;

	public Texture2D Tex
	{
		get
		{
			return _tex;
		}
		set
		{
			_tex = value;
			DataChanged();
		}
	}

	public string Serial
	{
		get
		{
			return serial;
		}
		set
		{
			serial = value;
			DataChanged();
		}
	}

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

	public int Num
	{
		get
		{
			return num;
		}
		set
		{
			num = value;
			DataChanged();
		}
	}

	public int Part
	{
		get
		{
			return part;
		}
		set
		{
			part = value;
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

	public int AGI
	{
		get
		{
			return agi;
		}
		set
		{
			agi = value;
			DataChanged();
		}
	}

	public int HP
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
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

	private void Start()
	{
	}

	private void Update()
	{
	}
}
