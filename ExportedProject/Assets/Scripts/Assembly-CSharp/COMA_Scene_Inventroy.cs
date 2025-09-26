using UnityEngine;

public class COMA_Scene_Inventroy : MonoBehaviour
{
	public static UI_BoxSlot selectedSlot;

	public static int soonEditorID = -1;

	private void Awake()
	{
		if (COMA_Platform.Instance == null)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("FBX/SceneAddition/WaitingRoom/PlatformBackground1")) as GameObject;
		}
	}

	private void Start()
	{
	}
}
