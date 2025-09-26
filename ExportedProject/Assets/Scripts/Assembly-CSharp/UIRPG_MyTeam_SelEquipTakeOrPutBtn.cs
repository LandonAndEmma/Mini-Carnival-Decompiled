using MessageID;
using UnityEngine;

public class UIRPG_MyTeam_SelEquipTakeOrPutBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_MyTeam_SelEquipBox _selEquipBox;

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIRPG_MyTeam_SelEquipBoxData uIRPG_MyTeam_SelEquipBoxData = _selEquipBox.BoxData as UIRPG_MyTeam_SelEquipBoxData;
		UIRPG_MyTeam_SelEquipTakeOrPutData uIRPG_MyTeam_SelEquipTakeOrPutData = new UIRPG_MyTeam_SelEquipTakeOrPutData(uIRPG_MyTeam_SelEquipBoxData.EquipData);
		if (uIRPG_MyTeam_SelEquipBoxData.IsEquipBySelf)
		{
			uIRPG_MyTeam_SelEquipTakeOrPutData.IsPutOn = false;
			uIRPG_MyTeam_SelEquipBoxData.IsEquip = false;
			uIRPG_MyTeam_SelEquipBoxData.IsEquipBySelf = false;
			_selEquipBox.BoxDataChanged();
		}
		else
		{
			uIRPG_MyTeam_SelEquipTakeOrPutData.IsPutOn = true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPGTeam_SelNewAvatarClick, null, uIRPG_MyTeam_SelEquipTakeOrPutData);
	}
}
