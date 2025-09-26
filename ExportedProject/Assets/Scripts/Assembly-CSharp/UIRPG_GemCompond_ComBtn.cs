using MessageID;
using Protocol.RPG.C2S;
using UnityEngine;

public class UIRPG_GemCompond_ComBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_GemCompoundBox _ownUser;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		_ownUser.MGR.CompoundBtnIndex = _ownUser.CurIndex;
		CombineGemCmd combineGemCmd = new CombineGemCmd();
		combineGemCmd.m_gem_id = (ushort)_ownUser.BOXData.GemId;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, combineGemCmd);
		COMA_HTTP_DataCollect.Instance.SendCompoundGemCount("1");
	}
}
