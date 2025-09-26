using UnityEngine;

public class UISquare_ButtonSupport : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UIOptions.Support();
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Refresh);
	}
}
