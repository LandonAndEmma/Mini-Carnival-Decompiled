using UnityEngine;

public class UITrade_BuyBoxSlot : UI_BoxSlot
{
	public enum ESlotType
	{
		Official = 0,
		Unofficial = 1
	}

	private ESlotType slotType = ESlotType.Unofficial;

	[SerializeField]
	private GameObject bkOfficialPic;

	[SerializeField]
	private GameObject btnGold;

	[SerializeField]
	private GameObject doodlePic;

	[SerializeField]
	private GameObject[] priceLable;

	public ESlotType SlotType
	{
		get
		{
			return slotType;
		}
		set
		{
			slotType = value;
			NotifyDataUpdate();
		}
	}

	public void SetIconPic(Texture2D pic)
	{
		TUIMeshSprite component = doodlePic.GetComponent<TUIMeshSprite>();
		component.UseCustomize = true;
		component.CustomizeTexture = pic;
		component.CustomizeRect = new Rect(0f, 0f, pic.width, pic.height);
	}

	public void DeleteIconPic()
	{
		TUIMeshSprite component = doodlePic.GetComponent<TUIMeshSprite>();
		Object.DestroyObject(component.CustomizeTexture);
		component.CustomizeTexture = null;
		component.UseCustomize = false;
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	protected override void ProcessNullData()
	{
		base.ProcessNullData();
	}

	public override int NotifyDataUpdate()
	{
		UITrade_BuyBoxData uITrade_BuyBoxData = (UITrade_BuyBoxData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		if (SlotType == ESlotType.Official)
		{
			bkOfficialPic.active = true;
		}
		else
		{
			bkOfficialPic.active = false;
		}
		GameObject[] array = priceLable;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<TUILabel>().Text = uITrade_BuyBoxData.Price.ToString("f0");
		}
		return 0;
	}

	public void HandleEventButton_gold(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_gold-CommandClick");
			UITrade uITrade = (UITrade)GetTUIMessageHandler(true);
			if (uITrade != null)
			{
				uITrade.ProcessAvatarShopBuy(control, eventType, wparam, lparam, data, this);
			}
		}
	}

	public void HandleEventButton_Preview(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Preview-CommandClick");
			UITrade uITrade = (UITrade)GetTUIMessageHandler(true);
			if (uITrade != null)
			{
				uITrade.ProcessAvatarShopPreview(control, eventType, wparam, lparam, data, this);
			}
		}
	}
}
