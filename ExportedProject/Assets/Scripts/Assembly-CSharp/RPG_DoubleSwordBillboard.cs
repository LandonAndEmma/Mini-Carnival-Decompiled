using UnityEngine;

public class RPG_DoubleSwordBillboard : MonoBehaviour
{
	private Transform cam;

	private void Start()
	{
		cam = GameObject.Find("Main Camera").transform;
	}

	private void Update()
	{
		if (Application.loadedLevelName.StartsWith("COMA_Scene_RPG"))
		{
			base.transform.rotation = Quaternion.Euler(0f - cam.rotation.eulerAngles.x, 0f, 0f);
		}
	}
}
