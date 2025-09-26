using MC_UIToolKit;
using NGUI_COMUI;
using UnityEngine;

public class UIRankings_SearchFriendBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UILabel _labelID;

	[SerializeField]
	private UILabel _labelName;

	[SerializeField]
	private UITexture _texPlayer;

	[SerializeField]
	private GameObject _btnAddFriend;

	private uint _playerID;

	public uint PlayerID
	{
		get
		{
			return _playerID;
		}
	}

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
			base.gameObject.name = "UIRankingsSearchFriendBox" + i;
		}
		else
		{
			base.gameObject.name = "UIRankingsSearchFriendBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		UIRankings_SearchFriendBoxData uIRankings_SearchFriendBoxData = base.BoxData as UIRankings_SearchFriendBoxData;
		if (uIRankings_SearchFriendBoxData == null)
		{
			SetLoseSelected();
			return;
		}
		_playerID = uIRankings_SearchFriendBoxData.PlayerID;
		_labelID.text = "ID:" + _playerID;
		_labelName.text = uIRankings_SearchFriendBoxData.PlayerName;
		_texPlayer.mainTexture = uIRankings_SearchFriendBoxData.Tex;
		if (UIGolbalStaticFun.GetSelfTID() == _playerID || UIDataBufferCenter.Instance.IsMyFriend(_playerID))
		{
			_btnAddFriend.SetActive(false);
		}
	}
}
