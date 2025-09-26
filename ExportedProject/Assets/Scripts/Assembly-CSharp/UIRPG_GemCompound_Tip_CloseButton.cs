using UnityEngine;

public class UIRPG_GemCompound_Tip_CloseButton : MonoBehaviour
{
	[SerializeField]
	private GameObject _closeObj;

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		_closeObj.SetActive(false);
	}
}
