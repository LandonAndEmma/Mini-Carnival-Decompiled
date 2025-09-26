using MessageID;
using UnityEngine;

public class UIIAP_ButtonPurchaseIAP : MonoBehaviour
{
	[SerializeField]
	private UIIAP.EPurchaseType _btnType;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_PurchaseIAPButtonClick, null, _btnType);
	}
}
