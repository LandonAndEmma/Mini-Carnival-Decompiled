using UnityEngine;

public class UITrade_SellBoxSlot : UI_BoxSlot
{
	private const int nMaxSlotCountPerBox = 3;

	[SerializeField]
	private GameObject bkPic;

	[SerializeField]
	private GameObject lockPic;

	[SerializeField]
	private GameObject btnGold;

	[SerializeField]
	private GameObject btnSlot;

	[SerializeField]
	private GameObject doodlePic;

	[SerializeField]
	private GameObject[] incomeLable;

	public int MaxSlotCountPerBox
	{
		get
		{
			return 3;
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
		if (IsLocked())
		{
			bkPic.GetComponent<TUIMeshSprite>().GrayStyle = true;
			lockPic.active = true;
			btnGold.SetActiveRecursively(false);
			btnSlot.active = true;
			doodlePic.active = false;
			return -2;
		}
		bkPic.GetComponent<TUIMeshSprite>().GrayStyle = false;
		lockPic.active = false;
		UITrade_SellBoxData uITrade_SellBoxData = (UITrade_SellBoxData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		switch (uITrade_SellBoxData.DoodleState)
		{
		case UITrade_SellBoxData.EDoodleState.Blank:
			btnGold.SetActiveRecursively(false);
			doodlePic.active = false;
			break;
		case UITrade_SellBoxData.EDoodleState.Selling:
			btnGold.SetActiveRecursively(false);
			doodlePic.active = true;
			break;
		case UITrade_SellBoxData.EDoodleState.Confirming:
		{
			btnGold.SetActiveRecursively(true);
			doodlePic.active = true;
			GameObject[] array = incomeLable;
			foreach (GameObject gameObject in array)
			{
				TUILabel component = gameObject.GetComponent<TUILabel>();
				if (component != null)
				{
					component.Text = uITrade_SellBoxData.EarningNum.ToString("f0");
					Debug.Log("Income--- " + component.Text);
				}
			}
			break;
		}
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
				uITrade.ProcessAvatarShopSellButton_gold(control, eventType, wparam, lparam, data, this);
			}
		}
	}

	public void HandleEventButton_slot(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_slot-CommandClick");
			UITrade uITrade = (UITrade)GetTUIMessageHandler(true);
			if (uITrade != null)
			{
				uITrade.ProcessAvatarShopSellButton_Slot(control, eventType, wparam, lparam, data, this);
			}
		}
	}
}
