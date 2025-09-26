using UnityEngine;

public class UIRPG_Map_LevelUpBlockBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _levelUpPopupObj;

	public void OnClick()
	{
		_levelUpPopupObj.SetActive(false);
	}
}
