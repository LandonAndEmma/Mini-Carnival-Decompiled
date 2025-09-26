using UnityEngine;

public class UIRPG_CardInfo_CloseBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _closeObj;

	public void OnClick()
	{
		Object.Destroy(_closeObj);
	}
}
