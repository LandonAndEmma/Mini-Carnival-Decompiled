using UnityEngine;

public class UIArmory_WeaponBoxSlot : UI_BoxSlot
{
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
	private GameObject lableDamage;

	[SerializeField]
	private GameObject lableSpeed;

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
		UIArmory_WeaponBoxData uIArmory_WeaponBoxData = (UIArmory_WeaponBoxData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		if (uIArmory_WeaponBoxData.PurchaseState)
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
			if (uIArmory_WeaponBoxData.PurchaseType == 0)
			{
				lableGem.active = true;
				lableGold.active = false;
			}
			else if (uIArmory_WeaponBoxData.PurchaseType == 1)
			{
				lableGem.active = false;
				lableGold.active = true;
			}
		}
		lableName.GetComponent<TUILabel>().Text = uIArmory_WeaponBoxData.Name;
		lableLevel.GetComponent<TUILabel>().Text = uIArmory_WeaponBoxData.Level.ToString();
		lableOwnNum.GetComponent<TUILabel>().Text = uIArmory_WeaponBoxData.OwnNum.ToString();
		GameObject[] array = lablePrices;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<TUILabel>().Text = uIArmory_WeaponBoxData.Price.ToString("f1");
		}
		lableDamage.GetComponent<TUILabel>().Text = uIArmory_WeaponBoxData.Damage;
		lableSpeed.GetComponent<TUILabel>().Text = uIArmory_WeaponBoxData.Speed;
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
				uIArmory.ProcessWeaponBuy(control, eventType, wparam, lparam, data, this);
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
