using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using UnityEngine;

public class UIRPG_AvatarEnhanceEnhanceBtn : MonoBehaviour
{
	[SerializeField]
	private UIRPG_AvatarEnhanceMgr _avatarMgr;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (_avatarMgr != null)
		{
			if (_avatarMgr.SelAvatarData != null && _avatarMgr.SelGemData != null)
			{
				if (RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_RPGAvatarBag == UIDataBufferCenter.Instance.RPGData.m_equip_bag.Count)
				{
					string des = TUITool.StringFormat(Localization.instance.Get("zhuangbeibeibao_desc2"), RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_RPGAvatarBag);
					UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, des);
					uIMessage_CommonBoxData.Mark = "zhuangbeibeibao_desc2";
					UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
					return;
				}
				if (UIDataBufferCenter.Instance.RPGData.m_equip_capacity < RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_RPGAvatarBag && UIDataBufferCenter.Instance.RPGData.m_equip_capacity == UIDataBufferCenter.Instance.RPGData.m_equip_bag.Count)
				{
					string des2 = TUITool.StringFormat(Localization.instance.Get("zhuangbeibeibao_desc1"), RPGGlobalData.Instance.RpgMiscUnit._unitRPGAvatarBagPrice);
					UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(0, des2);
					uIMessage_CommonBoxData2.Mark = "zhuangbeibeibao_desc1";
					UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
					return;
				}
				Debug.Log("UIDataBufferCenter.Instance.RPGData.m_equip_capacity  = " + UIDataBufferCenter.Instance.RPGData.m_equip_capacity);
				Debug.Log("RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_RPGAvatarBag = " + RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_RPGAvatarBag);
				Debug.Log("UIDataBufferCenter.Instance.RPGData.m_equip_bag.Count = " + UIDataBufferCenter.Instance.RPGData.m_equip_bag.Count);
				if (_avatarMgr.CompundFee > UIDataBufferCenter.Instance.playerInfo.m_gold)
				{
					string des3 = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
					UIMessage_CommonBoxData uIMessage_CommonBoxData3 = new UIMessage_CommonBoxData(0, des3);
					uIMessage_CommonBoxData3.Mark = "Lack of Money";
					UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData3);
					return;
				}
				UIGolbalStaticFun.PopBlockOnlyMessageBox();
				EnhanceAvatarCmd enhanceAvatarCmd = new EnhanceAvatarCmd();
				enhanceAvatarCmd.m_avatar_id = _avatarMgr.SelAvatarData.ItemId;
				enhanceAvatarCmd.m_gem_level = (byte)_avatarMgr.SelGemData.CurCaptionType;
				char[] array = _avatarMgr.SelGemData.GemComposition.ToString().ToCharArray();
				enhanceAvatarCmd.m_gem1_type = byte.Parse(array[0].ToString());
				enhanceAvatarCmd.m_gem2_type = byte.Parse(array[1].ToString());
				enhanceAvatarCmd.m_gem3_type = byte.Parse(array[2].ToString());
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, enhanceAvatarCmd);
			}
			else
			{
				string des4 = TUITool.StringFormat(Localization.instance.Get("avatarfactory_desc2"));
				UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, des4);
				UIGolbalStaticFun.PopCommonMessageBox(data);
				Debug.Log("_avatarMgr.SelAvatarData or _avatarMgr.SelGemData not Data");
			}
		}
		else
		{
			Debug.Log("_avatarMgr not Init");
		}
	}
}
