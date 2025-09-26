using UnityEngine;

public class COMA_Scene_MainMenu : MonoBehaviour
{
	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Menu);
		if (COMA_Platform.Instance == null)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/WaitingRoom/PlatformBackground2")) as GameObject;
		}
	}
}
