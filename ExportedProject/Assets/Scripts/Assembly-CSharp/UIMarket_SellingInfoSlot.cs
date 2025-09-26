using UnityEngine;

public class UIMarket_SellingInfoSlot : UI_BoxSlot
{
	[SerializeField]
	private GameObject[] _claimedIcons;

	[SerializeField]
	private TUIMeshSprite _avatarIcon;

	[SerializeField]
	private TUILabel _dayLabel;

	[SerializeField]
	private TUILabel _hourLabel;

	[SerializeField]
	private TUILabel _minuteLabel;

	[SerializeField]
	private TUILabel _soldLabel;

	[SerializeField]
	private TUILabel _balanceLabel;

	private void Awake()
	{
	}

	public void HandleEventButton_Claim(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIMarket uIMarket = (UIMarket)GetTUIMessageHandler(true);
			if (uIMarket != null)
			{
				uIMarket.ProcessClaim(this);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			}
			break;
		}
		}
	}

	protected override void ProcessNullData()
	{
		base.ProcessNullData();
		_avatarIcon.gameObject.SetActive(false);
		_dayLabel.gameObject.SetActive(false);
		_hourLabel.gameObject.SetActive(false);
		_minuteLabel.gameObject.SetActive(false);
		_soldLabel.gameObject.SetActive(false);
		_balanceLabel.gameObject.SetActive(false);
		_claimedIcons[0].SetActive(false);
		_claimedIcons[1].SetActive(false);
	}

	public override int NotifyDataUpdate()
	{
		UIMarket_SellingInfoData uIMarket_SellingInfoData = (UIMarket_SellingInfoData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		if (uIMarket_SellingInfoData.AvatarIcon != null)
		{
			_avatarIcon.gameObject.SetActive(true);
			_avatarIcon.UseCustomize = true;
			_avatarIcon.CustomizeTexture = uIMarket_SellingInfoData.AvatarIcon;
			_avatarIcon.CustomizeRect = new Rect(0f, 0f, uIMarket_SellingInfoData.AvatarIcon.width, uIMarket_SellingInfoData.AvatarIcon.height);
		}
		if (uIMarket_SellingInfoData.Cliamed)
		{
			_claimedIcons[0].SetActive(true);
			_claimedIcons[1].SetActive(false);
			_claimedIcons[0].transform.parent.gameObject.GetComponent<TUIButtonClick>().m_bDisable = false;
		}
		else
		{
			_claimedIcons[0].SetActive(false);
			_claimedIcons[1].SetActive(true);
			_claimedIcons[0].transform.parent.gameObject.GetComponent<TUIButtonClick>().m_bDisable = true;
		}
		_dayLabel.Text = uIMarket_SellingInfoData.DayNum.ToString();
		_hourLabel.Text = uIMarket_SellingInfoData.HourNum.ToString();
		_minuteLabel.Text = uIMarket_SellingInfoData.MinuteNum.ToString();
		_soldLabel.Text = uIMarket_SellingInfoData.SoldNum.ToString();
		_balanceLabel.Text = uIMarket_SellingInfoData.BalanceNum.ToString();
		_dayLabel.gameObject.SetActive(true);
		_hourLabel.gameObject.SetActive(true);
		_minuteLabel.gameObject.SetActive(true);
		_soldLabel.gameObject.SetActive(true);
		_balanceLabel.gameObject.SetActive(true);
		return 0;
	}
}
