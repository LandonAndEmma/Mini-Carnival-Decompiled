public class UITrade_SellBoxData : UI_BoxData
{
	public enum EDoodleState
	{
		Blank = 0,
		Selling = 1,
		Confirming = 2
	}

	private EDoodleState doodleState;

	private float fEarning;

	public EDoodleState DoodleState
	{
		get
		{
			return doodleState;
		}
		set
		{
			doodleState = value;
			DataChanged();
		}
	}

	public float EarningNum
	{
		get
		{
			return fEarning;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
