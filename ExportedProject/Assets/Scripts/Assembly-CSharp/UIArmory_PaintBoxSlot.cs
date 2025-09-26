using UnityEngine;

public class UIArmory_PaintBoxSlot : UI_BoxSlot
{
	[SerializeField]
	private GameObject lableName;

	[SerializeField]
	private GameObject lableOwnNum;

	[SerializeField]
	private GameObject[] lablePrices;

	[SerializeField]
	private GameObject colorPic;

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
		UIArmory_PaintBoxData uIArmory_PaintBoxData = (UIArmory_PaintBoxData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		lableName.GetComponent<TUILabel>().Text = uIArmory_PaintBoxData.Name;
		lableOwnNum.GetComponent<TUILabel>().Text = uIArmory_PaintBoxData.OwnNum.ToString();
		GameObject[] array = lablePrices;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<TUILabel>().Text = uIArmory_PaintBoxData.Price.ToString("f1");
		}
		colorPic.GetComponent<TUIMeshSprite>().color = uIArmory_PaintBoxData.PaintColor;
		return 0;
	}

	public void HandleEventButton_buy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIArmory uIArmory = (UIArmory)GetTUIMessageHandler(true);
			if (uIArmory != null)
			{
				uIArmory.ProcessPaintBuy(control, eventType, wparam, lparam, data, this);
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}
}
