using MessageID;
using UnityEngine;

public class UISquare_ChatRecordButtonBlock : MonoBehaviour
{
	[SerializeField]
	private UISquare_ChatRecordBox _box;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UISquare_ChatRecordBoxData uISquare_ChatRecordBoxData = (UISquare_ChatRecordBoxData)_box.BoxData;
		Debug.Log("-----------------Block Btn Click! " + uISquare_ChatRecordBoxData.OtherPeopleID);
		if (UIDataBufferCenter.Instance.IsPlayerIDInBlockMap(uISquare_ChatRecordBoxData.OtherPeopleID))
		{
			UIDataBufferCenter.Instance.RemovePlayerIDFromBlockMap(uISquare_ChatRecordBoxData.OtherPeopleID);
		}
		else
		{
			UIDataBufferCenter.Instance.AddPlayerIDToBlockMap(uISquare_ChatRecordBoxData.OtherPeopleID);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_RefreshChatHistory, null, null);
	}
}
