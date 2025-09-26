using UnityEngine;

public class UIArmory_HelmentBoxSlot : UI_BoxSlot
{
	[SerializeField]
	private TUIMeshSprite icon;

	[SerializeField]
	private GameObject activeBk;

	[SerializeField]
	private GameObject unactiveBk;

	[SerializeField]
	private GameObject lockIcon;

	[SerializeField]
	private GameObject btnBuy;

	[SerializeField]
	private GameObject lableName;

	[SerializeField]
	private GameObject lableLevel;

	[SerializeField]
	private GameObject lableLevelDesr;

	[SerializeField]
	private GameObject lableOwnNum;

	[SerializeField]
	private GameObject lableOwnNumDesr;

	[SerializeField]
	private GameObject lableAGI;

	[SerializeField]
	private GameObject lableHp;

	[SerializeField]
	private GameObject[] lablePrices;

	[SerializeField]
	private GameObject lableGem;

	[SerializeField]
	private GameObject lableGold;

	[SerializeField]
	private Color[] _fontColor;

	private void Awake()
	{
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
		UIArmory_HelmentBoxData uIArmory_HelmentBoxData = (UIArmory_HelmentBoxData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		if (uIArmory_HelmentBoxData.PurchaseState)
		{
			activeBk.active = true;
			btnBuy.SetActiveRecursively(true);
			unactiveBk.active = false;
			lockIcon.active = false;
			lableName.GetComponent<TUILabel>().color = _fontColor[0];
			lableOwnNum.GetComponent<TUILabel>().color = _fontColor[0];
			lableOwnNumDesr.GetComponent<TUILabel>().color = _fontColor[0];
			lableLevel.GetComponent<TUILabel>().color = _fontColor[0];
			lableLevelDesr.GetComponent<TUILabel>().color = _fontColor[0];
		}
		else
		{
			activeBk.active = false;
			btnBuy.SetActiveRecursively(false);
			UI_ButtonLight component = btnBuy.GetComponent<UI_ButtonLight>();
			if (component != null)
			{
				component.LightOff();
			}
			unactiveBk.active = true;
			lockIcon.active = true;
			lableName.GetComponent<TUILabel>().color = _fontColor[1];
			lableOwnNum.GetComponent<TUILabel>().color = _fontColor[1];
			lableOwnNumDesr.GetComponent<TUILabel>().color = _fontColor[1];
			lableLevel.GetComponent<TUILabel>().color = _fontColor[1];
			lableLevelDesr.GetComponent<TUILabel>().color = _fontColor[1];
		}
		if (btnBuy.active)
		{
			if (uIArmory_HelmentBoxData.PurchaseType == 0)
			{
				lableGem.active = true;
				lableGold.active = false;
			}
			else if (uIArmory_HelmentBoxData.PurchaseType == 1)
			{
				lableGem.active = false;
				lableGold.active = true;
			}
		}
		lableName.GetComponent<TUILabel>().Text = uIArmory_HelmentBoxData.Name;
		lableLevel.GetComponent<TUILabel>().Text = uIArmory_HelmentBoxData.Level.ToString();
		lableOwnNum.GetComponent<TUILabel>().Text = uIArmory_HelmentBoxData.OwnNum.ToString();
		GameObject[] array = lablePrices;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<TUILabel>().Text = uIArmory_HelmentBoxData.Price.ToString("f0");
		}
		string text = ((uIArmory_HelmentBoxData.AGI < 0) ? string.Empty : "+");
		text += uIArmory_HelmentBoxData.AGI.ToString("f0");
		text += "%";
		lableAGI.GetComponent<TUILabel>().Text = text;
		string text2 = ((uIArmory_HelmentBoxData.HP < 0) ? string.Empty : "+");
		text2 += uIArmory_HelmentBoxData.HP.ToString("f0");
		text2 += "%";
		lableHp.GetComponent<TUILabel>().Text = text2;
		icon.UseCustomize = true;
		icon.CustomizeTexture = uIArmory_HelmentBoxData.Tex;
		icon.CustomizeRect = new Rect(0f, 0f, uIArmory_HelmentBoxData.Tex.width, uIArmory_HelmentBoxData.Tex.height);
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
				uIArmory.ProcessHelmetBuy(control, eventType, wparam, lparam, data, this);
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
