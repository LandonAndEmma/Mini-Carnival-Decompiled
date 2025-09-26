using UnityEngine;

public class COMA_Scene_Doodle : MonoBehaviour
{
	private void Awake()
	{
		if (COMA_Platform.Instance != null)
		{
			COMA_Platform.Instance.DestroyPlatform();
		}
	}
}
