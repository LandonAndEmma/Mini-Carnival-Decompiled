using UnityEngine;

public class UIRPG_Map_EnemyCloseBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	public void OnClick()
	{
		Debug.Log("UIRPG_Map_SummonFriendCloseBtn : OnClick()");
		_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].gameObject.transform.localScale = Vector3.one;
		_vertexMgr.ScreenMoveScale.OnEnable();
		if (_vertexMgr.CurVertexType == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KNpc)
		{
			_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[0].gameObject.SetActive(true);
			_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[8].gameObject.SetActive(false);
			_vertexMgr.PopUpBossObj.SetActive(false);
		}
		else if (_vertexMgr.CurVertexType == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KBoss)
		{
			if (_vertexMgr.CurVertexIndex != 99)
			{
				_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[2].gameObject.SetActive(true);
				_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[10].gameObject.SetActive(false);
			}
			else if (_vertexMgr.CurVertexIndex == 99)
			{
				_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[7].gameObject.SetActive(true);
				_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[15].gameObject.SetActive(false);
			}
			_vertexMgr.PopUpBossObj.SetActive(false);
		}
		else if (_vertexMgr.CurVertexType == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KPlayer)
		{
			_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[4].gameObject.SetActive(true);
			_vertexMgr.VertexAllDict[_vertexMgr.CurVertexIndex].DisplaySprite[12].gameObject.SetActive(false);
			_vertexMgr.PopUpPlayerObj.SetActive(false);
		}
	}
}
