using MessageID;
using UnityEngine;

public class UISquareBtnMailMgr : UIEntity
{
	[SerializeField]
	private GameObject _newMailInfoObj;

	[SerializeField]
	private UILabel _newMailNumLabel;

	private void Awake()
	{
		_newMailInfoObj.SetActive(false);
	}

	private void Start()
	{
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIDataBuffer_MailDataChanged, this, MailDataChanged);
		MailDataChanged(null);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIDataBuffer_MailDataChanged, this);
	}

	protected override void Tick()
	{
	}

	private bool MailDataChanged(TUITelegram msg)
	{
		if (UIDataBufferCenter.Instance.MailBufferInfo == null)
		{
			return true;
		}
		Debug.Log("MailDataChanged;");
		_newMailNumLabel.text = UIDataBufferCenter.Instance.MailBufferInfo.m_num_of_new.ToString();
		_newMailInfoObj.SetActive((UIDataBufferCenter.Instance.MailBufferInfo.m_num_of_new > 0) ? true : false);
		return true;
	}
}
