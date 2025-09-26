using UnityEngine;

public class UIRPG_GameCoverMgr : MonoBehaviour
{
	[SerializeField]
	private int _imageWidth = 2208;

	[SerializeField]
	private int _imageHeight = 1242;

	public void Start()
	{
		Debug.Log("Screen.width : " + Screen.width);
		Debug.Log("Screen.heigth : " + Screen.height);
		Debug.Log("Screen.currentResolution.height : " + Screen.currentResolution.height);
		Debug.Log("Screen.currentResolution.refreshRate : " + Screen.currentResolution.refreshRate);
		Debug.Log("Screen.currentResolution.width : " + Screen.currentResolution.width);
		float num = (float)UIRoot.list[0].activeHeight / (float)_imageHeight;
		base.transform.localScale = new Vector3(num, num, 1f);
	}
}
