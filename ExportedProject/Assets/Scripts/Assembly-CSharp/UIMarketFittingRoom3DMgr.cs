using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UIMarketFittingRoom3DMgr : UIEntity
{
	public enum EOperType
	{
		Init = 0,
		Show_Charc = 1,
		Hide_Charc = 2,
		Wear = 3,
		UnWear = 4
	}

	public GameObject[] targetObj;

	public COMA_PlayerCharacter characterCom;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, this, Notify3DFittingCharc);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, this);
	}

	private bool Notify3DFittingCharc(TUITelegram msg)
	{
		if ((int)msg._pExtraInfo == 0)
		{
			base.gameObject.transform.localPosition = new Vector3(0f, 0.22f, -4f);
			base.gameObject.transform.parent.transform.localRotation = Quaternion.Euler(Vector3.zero);
			base.gameObject.transform.parent.parent.transform.localRotation = Quaternion.Euler(Vector3.zero);
			base.gameObject.transform.parent.parent.GetComponent<CameraObserve1>().Init();
			characterCom.RemoveAllAccounterment();
			for (int i = 0; i < targetObj.Length; i++)
			{
				targetObj[i].renderer.material.mainTexture = UIGolbalStaticFun.CreateWhiteTexture();
			}
			List<UIMarket_FittingBoxData> list = (List<UIMarket_FittingBoxData>)msg._pExtraInfo2;
			for (int j = 0; j < list.Count; j++)
			{
				UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIContainer_BoxOnClick, null, list[j].DataOwner);
			}
		}
		else if ((int)msg._pExtraInfo == 1)
		{
			GetComponent<Camera>().enabled = true;
		}
		else if ((int)msg._pExtraInfo == 2)
		{
			GetComponent<Camera>().enabled = false;
		}
		else if ((int)msg._pExtraInfo == 3)
		{
			UIMarket_FittingBoxData uIMarket_FittingBoxData = (UIMarket_FittingBoxData)msg._pExtraInfo2;
			if (uIMarket_FittingBoxData.Unit != string.Empty && uIMarket_FittingBoxData.SingleAvatar == -1)
			{
				Debug.Log("Wear : " + uIMarket_FittingBoxData.Unit);
				characterCom.CreateAccouterment(uIMarket_FittingBoxData.Unit);
			}
			else if (uIMarket_FittingBoxData.SingleAvatar != -1)
			{
				UIGolbalStaticFun.FillTexByMd5(uIMarket_FittingBoxData.Unit, targetObj[uIMarket_FittingBoxData.SingleAvatar]);
			}
			else
			{
				Debug.Log("Wear Avatar: " + uIMarket_FittingBoxData.Units[0] + " , " + uIMarket_FittingBoxData.Units[1] + " , " + uIMarket_FittingBoxData.Units[2]);
				for (int k = 0; k < targetObj.Length; k++)
				{
					if (uIMarket_FittingBoxData.Units[k] != string.Empty)
					{
						UIGolbalStaticFun.FillTexByMd5(uIMarket_FittingBoxData.Units[k], targetObj[k]);
					}
				}
			}
		}
		else if ((int)msg._pExtraInfo == 4)
		{
			UIMarket_FittingBoxData uIMarket_FittingBoxData2 = (UIMarket_FittingBoxData)msg._pExtraInfo2;
			if (uIMarket_FittingBoxData2.Unit != string.Empty && uIMarket_FittingBoxData2.SingleAvatar == -1)
			{
				Debug.Log("UnWear : " + uIMarket_FittingBoxData2.Unit);
				characterCom.DestroyAccoutermentExact(uIMarket_FittingBoxData2.Unit);
			}
			else if (uIMarket_FittingBoxData2.SingleAvatar != -1)
			{
				targetObj[uIMarket_FittingBoxData2.SingleAvatar].renderer.material.mainTexture = UIGolbalStaticFun.CreateWhiteTexture();
			}
			else
			{
				Debug.Log("UnWear Avatar: " + uIMarket_FittingBoxData2.Units[0] + " , " + uIMarket_FittingBoxData2.Units[1] + " , " + uIMarket_FittingBoxData2.Units[2]);
				for (int l = 0; l < targetObj.Length; l++)
				{
					if (uIMarket_FittingBoxData2.Units[l] != string.Empty)
					{
						targetObj[l].renderer.material.mainTexture = UIGolbalStaticFun.CreateWhiteTexture();
					}
				}
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_FittingRoomUnwearAvatarEnd, null, null);
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
