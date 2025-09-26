using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using UnityEngine;

public class UIRPG_Map_GetMobilityBuyBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject _hideGetMobilityObj;

	[SerializeField]
	private UIRPG_CheckPointsVertexMgr _vertexMgr;

	public void OnClick()
	{
		int crystalNumRenewMobility = UIGolbalStaticFun.GetCrystalNumRenewMobility();
		int mobilityValue = RPGGlobalClock.Instance.GetMobilityValue();
		int energyValue_Max = RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max;
		if (mobilityValue >= energyValue_Max)
		{
			Debug.Log("curMobility >= maxMobility");
			string des = TUITool.StringFormat(Localization.instance.Get("energy_desc2"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, des);
			uIMessage_CommonBoxData.Mark = "MaxMobility";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			_vertexMgr.ScreenMoveScale.OnEnable();
			return;
		}
		if (crystalNumRenewMobility > 0)
		{
			_hideGetMobilityObj.SetActive(false);
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			BuyMobilityCmd extraInfo = new BuyMobilityCmd();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
		}
		_vertexMgr.ScreenMoveScale.OnEnable();
	}
}
