using UnityEngine;

public class GUI_Menu_Show : MonoBehaviour
{
	public static int retina = 1;

	public COMA_InitLocalAvatar initAvatarCom;

	private void Awake()
	{
		if (TUI.IsRetina())
		{
			retina = 2;
		}
		initAvatarCom = GetComponent<COMA_InitLocalAvatar>();
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 110 * retina, Screen.height - 120 * retina, 100 * retina, 30 * retina), "Edit Head"))
		{
			COMA_Sys.Instance.bodyIndex = 0;
			Application.LoadLevel("COMA_Menu_Paint_Head");
		}
		else if (GUI.Button(new Rect(Screen.width - 110 * retina, Screen.height - 80 * retina, 100 * retina, 30 * retina), "Edit Body"))
		{
			COMA_Sys.Instance.bodyIndex = 1;
			Application.LoadLevel("COMA_Menu_Paint_Body");
		}
		else if (GUI.Button(new Rect(Screen.width - 110 * retina, Screen.height - 40 * retina, 100 * retina, 30 * retina), "Edit Leg"))
		{
			COMA_Sys.Instance.bodyIndex = 2;
			Application.LoadLevel("COMA_Menu_Paint_Leg");
		}
	}
}
