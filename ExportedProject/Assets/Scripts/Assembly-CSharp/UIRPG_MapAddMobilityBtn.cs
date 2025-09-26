using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UIRPG_MapAddMobilityBtn : UIEntity
{
	[SerializeField]
	private UILabel _mobilityLabel;

	[SerializeField]
	private GameObject _popUpAddMobilityObj;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	public bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		default:
			return true;
		}
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max == RPGGlobalClock.Instance.GetMobilityValue())
		{
			string des = TUITool.StringFormat(Localization.instance.Get("energy_desc2"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, des);
			uIMessage_CommonBoxData.Mark = "MobilityToLimit";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		else
		{
			_popUpAddMobilityObj.SetActive(true);
		}
	}
}
