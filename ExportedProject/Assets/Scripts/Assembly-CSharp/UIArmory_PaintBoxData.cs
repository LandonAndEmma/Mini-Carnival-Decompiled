using UnityEngine;

public class UIArmory_PaintBoxData : UI_BoxData
{
	private string name;

	private float price;

	private int ownNum;

	private Color color;

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

	public Color PaintColor
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
			DataChanged();
		}
	}

	public UIArmory_PaintBoxData()
	{
		name = "Color";
		ownNum = 10;
		price = 999f;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
