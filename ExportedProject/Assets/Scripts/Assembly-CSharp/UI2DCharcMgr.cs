using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Role.S2C;
using UnityEngine;

public class UI2DCharcMgr : UIEntity
{
	public enum EOperType
	{
		Show_Charc = 0,
		Hide_Charc = 1,
		Wear = 2,
		UnWear = 3,
		Show_Other = 4,
		Hide_Other = 5,
		Rotate = 6
	}

	public GameObject[] targetObj;

	public COMA_PlayerCharacter characterCom;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_Notify2DCharc, this, Notify2DCharc);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_Notify2DCharc, this);
	}

	private bool Notify2DCharc(TUITelegram msg)
	{
		if ((int)msg._pExtraInfo == 0)
		{
			GetComponent<Camera>().enabled = true;
			ShowEquipedProfile();
		}
		else if ((int)msg._pExtraInfo == 1)
		{
			GetComponent<Camera>().enabled = false;
			characterCom.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		}
		else if ((int)msg._pExtraInfo == 2)
		{
			UIMarket_BoxData uIMarket_BoxData = (UIMarket_BoxData)msg._pExtraInfo2;
			if (uIMarket_BoxData.DataType == 2)
			{
				if (!UIGolbalStaticFun.IsSystemShopTmp(uIMarket_BoxData.Unit))
				{
					Debug.Log("Wear : " + uIMarket_BoxData.Unit);
					characterCom.CreateAccouterment(uIMarket_BoxData.Unit);
				}
			}
			else if (uIMarket_BoxData.DataType == 1)
			{
				Debug.Log("Wear Avatar: " + uIMarket_BoxData.Units[0] + " , " + uIMarket_BoxData.Units[1] + " , " + uIMarket_BoxData.Units[2]);
				for (int i = 0; i < targetObj.Length; i++)
				{
					if (uIMarket_BoxData.Units[i] != string.Empty)
					{
						UIGolbalStaticFun.FillTexByMd5(uIMarket_BoxData.Units[i], targetObj[i]);
					}
				}
			}
		}
		else if ((int)msg._pExtraInfo != 3)
		{
			if ((int)msg._pExtraInfo == 4)
			{
				GetComponent<Camera>().enabled = true;
				WatchRoleInfo watchRoleInfo = (WatchRoleInfo)msg._pExtraInfo2;
				if (watchRoleInfo != null)
				{
					Debug.Log(" -------------------> " + watchRoleInfo.m_head);
					UIGolbalStaticFun.FillTexByMd5(watchRoleInfo.m_head, targetObj[0]);
					UIGolbalStaticFun.FillTexByMd5(watchRoleInfo.m_body, targetObj[1]);
					UIGolbalStaticFun.FillTexByMd5(watchRoleInfo.m_leg, targetObj[2]);
					characterCom.RemoveAllAccounterment();
					characterCom.CreateAccouterment(watchRoleInfo.m_head_top);
					characterCom.CreateAccouterment(watchRoleInfo.m_head_front);
					characterCom.CreateAccouterment(watchRoleInfo.m_head_back);
					characterCom.CreateAccouterment(watchRoleInfo.m_head_left);
					characterCom.CreateAccouterment(watchRoleInfo.m_head_right);
					characterCom.CreateAccouterment(watchRoleInfo.m_chest_front);
					characterCom.CreateAccouterment(watchRoleInfo.m_chest_back);
				}
				else
				{
					Debug.LogWarning("watchInfo is null!!");
				}
			}
			else if ((int)msg._pExtraInfo != 5 && (int)msg._pExtraInfo == 6)
			{
				float y = (float)msg._pExtraInfo2 * -314f * 2f / (float)Screen.width;
				Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
				characterCom.transform.rotation *= quaternion;
			}
		}
		return true;
	}

	private void Strip()
	{
		for (int i = 0; i < targetObj.Length; i++)
		{
			targetObj[i].renderer.material.mainTexture = UIGolbalStaticFun.CreateWhiteTexture();
		}
		characterCom.RemoveAllAccounterment();
	}

	private void ShowEquipedProfile()
	{
		Strip();
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		BagData bag_data = notifyRoleDataCmd.m_bag_data;
		List<BagItem> bag_list = bag_data.m_bag_list;
		for (int i = 0; i < bag_list.Count; i++)
		{
			BagItem bagItem = bag_list[i];
			if (bagItem.m_unique_id == info.m_head)
			{
				UIGolbalStaticFun.FillTexByMd5(bagItem.m_unit, targetObj[0]);
			}
			else if (bagItem.m_unique_id == info.m_body)
			{
				UIGolbalStaticFun.FillTexByMd5(bagItem.m_unit, targetObj[1]);
			}
			else if (bagItem.m_unique_id == info.m_leg)
			{
				UIGolbalStaticFun.FillTexByMd5(bagItem.m_unit, targetObj[2]);
			}
			else if (bagItem.m_unique_id == info.m_head_top || bagItem.m_unique_id == info.m_head_front || bagItem.m_unique_id == info.m_head_back || bagItem.m_unique_id == info.m_head_left || bagItem.m_unique_id == info.m_head_right || bagItem.m_unique_id == info.m_chest_front || bagItem.m_unique_id == info.m_chest_back)
			{
				characterCom.CreateAccouterment(bagItem.m_unit);
			}
		}
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
