using MC_UIToolKit;
using Protocol;
using UnityEngine;

public class UISelfFansNum : MonoBehaviour
{
	[SerializeField]
	private UILabel _fansLabel;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		UIDataBufferCenter.Instance.FetchPlayerExtInfoByID(UIGolbalStaticFun.GetSelfTID(), delegate(ExtInfo extInfo)
		{
			if (extInfo == null)
			{
				_fansLabel.text = "fans:";
			}
			else if (extInfo.m_fans_num < 100000)
			{
				_fansLabel.text = "fans:" + extInfo.m_fans_num;
			}
			else
			{
				_fansLabel.text = "fans:" + extInfo.m_fans_num / 1000 + "K";
			}
		});
	}
}
