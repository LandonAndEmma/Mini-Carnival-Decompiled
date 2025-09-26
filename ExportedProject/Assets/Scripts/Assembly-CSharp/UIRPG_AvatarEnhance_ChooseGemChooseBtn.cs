using MessageID;
using UnityEngine;

public class UIRPG_AvatarEnhance_ChooseGemChooseBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_AvatarEnhance_ChooseGemBox _chooseGemBox;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		if (_chooseGemBox != null)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPGAvatar_SelGemCompoundClick, null, _chooseGemBox.BoxData);
		}
	}
}
