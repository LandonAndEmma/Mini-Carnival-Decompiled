using UnityEngine;

public class UIAchievement_BoxSlot : UI_BoxSlot
{
	[SerializeField]
	private TUIMeshSprite _acIcon;

	[SerializeField]
	private TUILabel _acCaption;

	[SerializeField]
	private TUILabel _acCapDetail;

	[SerializeField]
	private TUIMeshSprite _acAwardIconBk;

	[SerializeField]
	private TUIMeshSprite _acAwardIcon;

	[SerializeField]
	private TUILabel _acAwardNum;

	[SerializeField]
	private GameObject _acProcess;

	[SerializeField]
	private TUILabel _acProcessCur;

	[SerializeField]
	private TUILabel _acProcessMax;

	[SerializeField]
	private GameObject _acProcessBar;

	[SerializeField]
	private GameObject _acAcceptBtn;

	private int _nProcessLen = 93;

	private void Start()
	{
	}

	private new void Update()
	{
	}

	private void RefreshProcess(float pro)
	{
		float x = (float)_nProcessLen * (1f - pro) * -0.5f;
		Vector3 localPosition = _acProcessBar.transform.localPosition;
		localPosition.x = x;
		_acProcessBar.transform.localPosition = localPosition;
		Vector3 localScale = _acProcessBar.transform.localScale;
		localScale.x = pro;
		_acProcessBar.transform.localScale = localScale;
	}

	protected override void ProcessNullData()
	{
		base.ProcessNullData();
	}

	public override int NotifyDataUpdate()
	{
		UIAchievement_BoxData uIAchievement_BoxData = (UIAchievement_BoxData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		switch (uIAchievement_BoxData.ACState)
		{
		case 0:
			_acIcon.texture = UI_GlobalData._strACIcons[0];
			_acAcceptBtn.SetActive(false);
			_acProcess.SetActive(true);
			break;
		case 1:
			_acIcon.texture = UI_GlobalData._strACIcons[uIAchievement_BoxData.Id];
			_acAcceptBtn.SetActive(true);
			_acProcess.SetActive(false);
			break;
		case 2:
			_acIcon.texture = UI_GlobalData._strACIcons[uIAchievement_BoxData.Id];
			_acAcceptBtn.SetActive(false);
			_acProcess.SetActive(false);
			break;
		}
		_acProcessCur.Text = uIAchievement_BoxData.CurProcessNum.ToString();
		_acProcessMax.Text = uIAchievement_BoxData.MaxProcessNum.ToString();
		RefreshProcess((float)uIAchievement_BoxData.CurProcessNum / (float)uIAchievement_BoxData.MaxProcessNum);
		_acCaption.Text = TUITextManager.Instance().GetString(UI_GlobalData._strACCaption[uIAchievement_BoxData.Id - 1]);
		_acCapDetail.Text = TUITextManager.Instance().GetString(UI_GlobalData._strACCaptionDetail[uIAchievement_BoxData.Id - 1]);
		if (uIAchievement_BoxData.AwardNum == 0)
		{
			_acAwardNum.gameObject.SetActive(false);
			_acAwardIcon.gameObject.SetActive(false);
			_acAwardIconBk.gameObject.SetActive(true);
			_acAwardIconBk.texture = UI_GlobalData._strACIconsBk[uIAchievement_BoxData.Id];
		}
		else
		{
			_acAwardNum.gameObject.SetActive(true);
			_acAwardNum.Text = uIAchievement_BoxData.AwardNum.ToString();
			_acAwardIcon.gameObject.SetActive(true);
			_acAwardIcon.texture = UI_GlobalData._strACIconsBk[uIAchievement_BoxData.Id];
			_acAwardIconBk.gameObject.SetActive(false);
		}
		return 0;
	}

	public void HandleEventButton_accept(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIAchievement_BoxData uIAchievement_BoxData = (UIAchievement_BoxData)base.BoxData;
			if (uIAchievement_BoxData != null && uIAchievement_BoxData.ACState == 1)
			{
				Debug.Log("Accept award ID:" + uIAchievement_BoxData.Id);
				if (COMA_Achievement.Instance.AcceptAchievement(uIAchievement_BoxData.Id - 1))
				{
					uIAchievement_BoxData.ACState = 2;
				}
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
