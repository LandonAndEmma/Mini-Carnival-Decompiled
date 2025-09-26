public class UITrade_BuyBoxData : UI_BoxData
{
	private int price;

	public int Price
	{
		get
		{
			return price;
		}
		set
		{
			price = value;
			if (base.Ower != null)
			{
				base.Ower.NotifyDataUpdate();
			}
		}
	}

	public UITrade_BuyBoxData()
	{
		price = 0;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
