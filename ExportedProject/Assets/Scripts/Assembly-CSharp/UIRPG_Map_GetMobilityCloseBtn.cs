using UnityEngine;

public class UIRPG_Map_GetMobilityCloseBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	[SerializeField]
	private GameObject _hideMobilityObj;

	public void OnClick()
	{
		_vertexMgr.ScreenMoveScale.OnEnable();
		_hideMobilityObj.SetActive(false);
	}
}
