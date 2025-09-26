using UnityEngine;

public class UIRPG_MyTeamSwitchBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_MyTeamMgr _myTeamMgr;

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_myTeamMgr.MyTeamContainer.CurSelBox.BoxData.DataType != 2)
		{
			_myTeamMgr.PopUpSelCardObj.SetActive(true);
			_myTeamMgr.SelCardSelBtnObj.SetActive(false);
		}
	}
}
