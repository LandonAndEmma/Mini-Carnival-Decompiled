using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UITipsMessageBoxMgr : UIEntity
{
	private const byte c_MaxTipsCount = 6;

	private const byte c_Height = 30;

	[SerializeField]
	private GameObject _prefabBox;

	private List<UIMessage_OnlyTipsBox> _lstTipsBox = new List<UIMessage_OnlyTipsBox>();

	[SerializeField]
	private bool _bTestAddTips;

	private int tipsIndex;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UITipsBox_Popup, this, PopUpTipsBox);
		RegisterMessage(EUIMessageID.UITipsBox_Del, this, DelTipsBox);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UITipsBox_Popup, this);
		UnregisterMessage(EUIMessageID.UITipsBox_Del, this);
		for (int i = 0; i < _lstTipsBox.Count; i++)
		{
			Object.DestroyObject(_lstTipsBox[i].gameObject);
		}
		_lstTipsBox.Clear();
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	private void Start()
	{
	}

	protected override void Tick()
	{
		if (_bTestAddTips)
		{
			UIGolbalStaticFun.PopupTipsBox("test" + tipsIndex++);
			_bTestAddTips = false;
		}
	}

	private bool PopUpTipsBox(TUITelegram msg)
	{
		string tipsDes = msg._pExtraInfo as string;
		GameObject gameObject = Object.Instantiate(_prefabBox, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		UIMessage_OnlyTipsBox component = gameObject.GetComponent<UIMessage_OnlyTipsBox>();
		component.TipsDes = tipsDes;
		component.TipsColor = new Color(1f, 0.42f, 0f);
		if (msg._pExtraInfo2 != null)
		{
			component.TipsColor = new Color(1f, 0f, 0f);
		}
		_lstTipsBox.Insert(0, component);
		int num = 0;
		for (int num2 = _lstTipsBox.Count - 1; num2 >= 0; num2--)
		{
			_lstTipsBox[num2].gameObject.transform.localPosition = new Vector3(0f, (num2 < 6) ? (num++ * 30) : 10000, 0f);
		}
		return true;
	}

	private bool DelTipsBox(TUITelegram msg)
	{
		GameObject gameObject = msg._pExtraInfo as GameObject;
		for (int i = 0; i < _lstTipsBox.Count; i++)
		{
			if (_lstTipsBox[i].gameObject == gameObject)
			{
				Debug.Log("Del One Tips box!");
				_lstTipsBox.RemoveAt(i);
				Object.DestroyObject(gameObject);
				break;
			}
		}
		return true;
	}
}
