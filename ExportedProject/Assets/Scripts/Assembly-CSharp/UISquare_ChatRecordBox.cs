using NGUI_COMUI;
using Protocol;
using UnityEngine;

public class UISquare_ChatRecordBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UILabel _otherPeopleName;

	[SerializeField]
	private UILabel _otherPeopleChatContent;

	[SerializeField]
	private UITexture _otherPeopleChatIcon;

	[SerializeField]
	private UILabel _selfChatContent;

	[SerializeField]
	private UILabel _selfChatPM;

	[SerializeField]
	private UITexture _selfChatIcon;

	[SerializeField]
	private GameObject _selfObj;

	[SerializeField]
	private GameObject _otherPlayerObj;

	[SerializeField]
	private UISprite _blockIcon;

	[SerializeField]
	private UISprite _showIcon;

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UISquareChatRecordBox" + i;
		}
		else
		{
			base.gameObject.name = "UISquareChatRecordBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UISquare_ChatRecordBoxData uISquare_ChatRecordBoxData = base.BoxData as UISquare_ChatRecordBoxData;
		if (uISquare_ChatRecordBoxData == null)
		{
			SetLoseSelected();
		}
		else if (uISquare_ChatRecordBoxData.SelfRecord)
		{
			_selfObj.SetActive(true);
			_otherPlayerObj.SetActive(false);
			_selfChatContent.text = uISquare_ChatRecordBoxData.OtherPeopleChatContent;
			_selfChatIcon.mainTexture = uISquare_ChatRecordBoxData.OtherPeopleIcon;
			if (uISquare_ChatRecordBoxData.Channel == Channel.person)
			{
				Debug.Log("data.Channel == Protocol.Channel.person");
				_selfChatContent.color = new Color(0f, 0.73f, 1f);
				if (_selfChatPM != null)
				{
					_selfChatPM.enabled = true;
					_selfChatPM.text = "@" + uISquare_ChatRecordBoxData.PrivatePeopleName;
				}
			}
			else
			{
				_selfChatContent.color = new Color(1f, 1f, 1f);
				if (_selfChatPM != null)
				{
					_selfChatPM.enabled = false;
				}
			}
		}
		else
		{
			_selfObj.SetActive(false);
			_otherPlayerObj.SetActive(true);
			_otherPeopleName.text = uISquare_ChatRecordBoxData.OtherPeopleName;
			_otherPeopleChatContent.text = uISquare_ChatRecordBoxData.OtherPeopleChatContent;
			_otherPeopleChatIcon.mainTexture = uISquare_ChatRecordBoxData.OtherPeopleIcon;
			if (uISquare_ChatRecordBoxData.Channel == Channel.person)
			{
				Debug.Log("data.Channel == Protocol.Channel.person");
				_otherPeopleChatContent.color = new Color(0f, 0.73f, 1f);
			}
			else
			{
				_otherPeopleChatContent.color = new Color(1f, 1f, 1f);
			}
			if (UIDataBufferCenter.Instance.IsPlayerIDInBlockMap(uISquare_ChatRecordBoxData.OtherPeopleID))
			{
				_showIcon.enabled = false;
				_blockIcon.enabled = true;
				_otherPeopleChatContent.text = string.Empty;
			}
			else
			{
				_showIcon.enabled = true;
				_blockIcon.enabled = false;
			}
		}
	}
}
