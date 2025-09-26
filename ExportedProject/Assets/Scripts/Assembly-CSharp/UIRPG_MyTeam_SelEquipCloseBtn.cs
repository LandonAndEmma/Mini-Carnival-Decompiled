using UnityEngine;

public class UIRPG_MyTeam_SelEquipCloseBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		_myTeamMgr.PopUpSelEquipObj.SetActive(false);
	}
}
