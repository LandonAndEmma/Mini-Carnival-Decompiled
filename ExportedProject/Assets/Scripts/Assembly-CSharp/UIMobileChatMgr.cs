using System.Collections.Generic;
using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIMobileChatMgr : MonoBehaviour
{
	[SerializeField]
	private UIPanel _uiPanel;

	[SerializeField]
	private UIGrid _uiGrid;

	[SerializeField]
	private UISquare_ChatHistoryContainer _chatHistoryContainer;

	[SerializeField]
	private UISquare _uiSquare;

	private bool _needInitMobileChatArear;

	private void Start()
	{
	}

	protected void OnEnable()
	{
		_needInitMobileChatArear = true;
	}

	protected void OnDisable()
	{
		_needInitMobileChatArear = false;
	}

	private void Update()
	{
	}

	private void InitClip()
	{
		Vector2 zero = Vector2.zero;
		zero.x = Screen.width;
		zero.y = (float)Screen.height * 0.3f;
		float y = (float)(Screen.height / 2) - zero.y / 2f;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = y;
		base.transform.localPosition = localPosition;
		Vector4 clipRange = _uiPanel.clipRange;
		clipRange.z = zero.x;
		clipRange.w = zero.y;
		_uiPanel.clipRange = clipRange;
		Debug.Log("clipRange=" + clipRange);
		Vector3 position = _uiGrid.gameObject.transform.position;
		position.y = base.transform.root.localScale.y * ((float)(Screen.height / 2) - _uiGrid.cellHeight / 2f);
		_uiGrid.gameObject.transform.position = position;
		Debug.Log(base.transform.root.localScale.y);
		List<UISquare_ChatRecordBoxData> lstChatRecordBoxData = _uiSquare.LstChatRecordBoxData;
		int count = lstChatRecordBoxData.Count;
		Debug.Log("Chat BoxCount=" + count);
		_chatHistoryContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		_chatHistoryContainer.InitBoxs(count, false);
		for (int i = 0; i < count; i++)
		{
			UISquare_ChatRecordBoxData data = new UISquare_ChatRecordBoxData(lstChatRecordBoxData[i]);
			_chatHistoryContainer.SetBoxData(i, data);
		}
		Debug.Log("PostMessage=UISquare_RefreshChatHistoryControl");
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_RefreshChatHistoryControl, null, null);
	}
}
