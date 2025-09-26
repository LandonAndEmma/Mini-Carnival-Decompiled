using MessageID;
using UnityEngine;

public class UIMarket_FittingRoomButtonClose : MonoBehaviour
{
	[SerializeField]
	private GameObject _objOwner;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, null, UIMarketFittingRoom3DMgr.EOperType.Hide_Charc);
		_objOwner.SetActive(false);
	}
}
