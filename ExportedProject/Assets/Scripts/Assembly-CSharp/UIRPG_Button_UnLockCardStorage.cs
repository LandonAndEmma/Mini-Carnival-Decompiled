using MessageID;
using UnityEngine;

public class UIRPG_Button_UnLockCardStorage : MonoBehaviour
{
	[SerializeField]
	private UIRPG_CardMgr_Card_Box _owerBox;

	[SerializeField]
	private UILabel _labelPrice;

	private void OnEnable()
	{
		_labelPrice.text = RPGGlobalData.Instance.RpgMiscUnit._unitCardBagPrice.ToString();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UICardMgr_UnlockBtnClick, null, null);
	}
}
