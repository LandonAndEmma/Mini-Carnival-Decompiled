using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using UnityEngine;

public class UIRPG_MapCollectBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Coin_collectall);
		_vertexMgr.CollectBtnObj.SetActive(false);
		_vertexMgr.PopEntity.BlockMsgBox(UIMessageBoxMgr.EMessageBoxType.GetItems);
		_vertexMgr.CollectGoldQueue.Clear();
		foreach (int key in _vertexMgr.VertexPassDict.Keys)
		{
			if (_vertexMgr.VertexPassDict[key].VertexStat == UIRPG_CheckPointsVertex.UIRPG_CheckPointsVertexStat.KGold)
			{
				_vertexMgr.CollectGoldQueue.Enqueue(key);
			}
		}
		ReqGainAllGoldCmd extraInfo = new ReqGainAllGoldCmd();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
	}
}
