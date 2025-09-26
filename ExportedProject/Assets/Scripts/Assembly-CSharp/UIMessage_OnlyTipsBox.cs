using MessageID;
using UnityEngine;

public class UIMessage_OnlyTipsBox : MonoBehaviour
{
	[SerializeField]
	private UILabel _labelDes;

	[SerializeField]
	private float _fAwakeTime;

	private bool _bDel;

	public string TipsDes
	{
		set
		{
			_labelDes.text = value;
		}
	}

	public Color TipsColor
	{
		set
		{
			_labelDes.color = value;
		}
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
		if (!_bDel)
		{
			_bDel = true;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UITipsBox_Del, null, base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (!_bDel)
		{
			_bDel = true;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UITipsBox_Del, null, base.gameObject);
		}
	}

	public void EndAni()
	{
		Debug.Log("EndAni:" + base.gameObject.name);
		_bDel = true;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UITipsBox_Del, null, base.gameObject);
	}

	private void Update()
	{
		if (_fAwakeTime < 3f)
		{
			_fAwakeTime += Time.deltaTime;
			if (_fAwakeTime >= 3f)
			{
				GetComponent<Animation>().Stop();
				GetComponent<Animation>().Play();
			}
		}
	}
}
