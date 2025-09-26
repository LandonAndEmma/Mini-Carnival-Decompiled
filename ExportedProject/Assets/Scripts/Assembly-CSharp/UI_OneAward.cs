using UnityEngine;

public class UI_OneAward : UIMessageHandler
{
	private UI_OneAwardData[] _data;

	[SerializeField]
	private TUIMeshSprite[] _awardTex;

	[SerializeField]
	private TUILabel[] _awardNum;

	public UI_OneAwardData[] OneAwardData
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			DataChanged();
		}
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void HandleEventButton_OK(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIAwards component = base.transform.parent.parent.parent.GetComponent<UIAwards>();
			if (component != null)
			{
				component.ProcessGetAward(this);
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

	public void DataChanged()
	{
		if (OneAwardData == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		for (int i = 0; i < 3; i++)
		{
			if (OneAwardData[i] == null)
			{
				_awardNum[i].transform.parent.gameObject.SetActive(false);
				continue;
			}
			_awardNum[i].transform.parent.gameObject.SetActive(true);
			if (OneAwardData[i].Award.nAwardNum > 0)
			{
				_awardNum[i].transform.parent.gameObject.SetActive(true);
				_awardNum[i].Text = OneAwardData[i].Award.nAwardNum.ToString();
				_awardTex[i].UseCustomize = false;
				_awardTex[i].texture = "title_gem";
			}
			else if (OneAwardData[i].Award.part >= 0)
			{
				_awardNum[i].transform.parent.gameObject.SetActive(true);
				_awardNum[i].Text = "1";
				_awardTex[i].UseCustomize = false;
				_awardTex[i].texture = COMA_Tools.AwardSerialNameToTexture(OneAwardData[i].Award.serialName);
			}
			else
			{
				_awardNum[i].transform.parent.gameObject.SetActive(false);
			}
		}
	}
}
