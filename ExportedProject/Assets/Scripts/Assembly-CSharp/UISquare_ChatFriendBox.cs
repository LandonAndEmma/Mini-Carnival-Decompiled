using NGUI_COMUI;
using UnityEngine;

public class UISquare_ChatFriendBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UILabel _friendNameLabel;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UISquareChatFriendBox" + i;
		}
		else
		{
			base.gameObject.name = "UISquareChatFriendBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UISquare_ChatFriendBoxData uISquare_ChatFriendBoxData = base.BoxData as UISquare_ChatFriendBoxData;
		if (uISquare_ChatFriendBoxData == null)
		{
			SetLoseSelected();
			_mainTex.enabled = false;
		}
		else
		{
			_friendNameLabel.text = uISquare_ChatFriendBoxData.FriendName;
			_mainTex.mainTexture = uISquare_ChatFriendBoxData.FriendTexture;
			_mainTex.enabled = true;
		}
	}
}
