using System;
using System.Collections.Generic;
using LitJson;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.Role.S2C;
using UIGlobal;
using UnityEngine;

namespace MC_UIToolKit
{
	public static class UIGolbalStaticFun
	{
		public static uint GetSelfTID()
		{
			return ((NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role)).m_info.m_player_id;
		}

		public static UIBackpack_BoxData.EDataType BagItemPartToUIDataType(byte part)
		{
			switch (part)
			{
			case 1:
				return UIBackpack_BoxData.EDataType.Avatar_Head;
			case 2:
				return UIBackpack_BoxData.EDataType.Avatar_Body;
			case 3:
				return UIBackpack_BoxData.EDataType.Avatar_Leg;
			case 4:
				return UIBackpack_BoxData.EDataType.Decoration;
			default:
				return UIBackpack_BoxData.EDataType.None;
			}
		}

		public static UIBackpack_BoxData.EDataState BagItemStateToUIDataState(byte part)
		{
			switch (part)
			{
			case 0:
				return UIBackpack_BoxData.EDataState.NoEditNoSell;
			case 1:
				return UIBackpack_BoxData.EDataState.CanEditNoSell;
			case 2:
				return UIBackpack_BoxData.EDataState.CanEditCanSell;
			default:
				return UIBackpack_BoxData.EDataState.Unknow;
			}
		}

		public static string GetResPathByAvatarPart(EAvatarPart part)
		{
			switch (part)
			{
			case EAvatarPart.Avatar_Head:
				return "FBX/Player/BodyParts/Head01";
			case EAvatarPart.Avatar_Body:
				return "FBX/Player/BodyParts/Body01";
			case EAvatarPart.Avatar_Leg:
				return "FBX/Player/BodyParts/Leg01";
			default:
				return "FBX/Player/Part/PFB/All";
			}
		}

		public static string GetPaintResPathByAvatarPart(EAvatarPart part)
		{
			switch (part)
			{
			case EAvatarPart.Avatar_Head:
				return "FBX/Player/Part/PFB/drawHead01";
			case EAvatarPart.Avatar_Body:
				return "FBX/Player/Part/PFB/drawBody01";
			case EAvatarPart.Avatar_Leg:
				return "FBX/Player/Part/PFB/drawLeg01";
			default:
				return string.Empty;
			}
		}

		public static string GetResPathByDataType(UIBackpack_BoxData.EDataType type, string name)
		{
			switch (type)
			{
			case UIBackpack_BoxData.EDataType.Avatar_Head:
				return "FBX/Player/BodyParts/Head01";
			case UIBackpack_BoxData.EDataType.Avatar_Body:
				return "FBX/Player/BodyParts/Body01";
			case UIBackpack_BoxData.EDataType.Avatar_Leg:
				return "FBX/Player/BodyParts/Leg01";
			case UIBackpack_BoxData.EDataType.Decoration:
				return string.Empty;
			default:
				return "FBX/Player/Part/PFB/All";
			}
		}

		public static EAvatarPart UIBackpackAvatarTypeToAvatarPart(UIBackpack_BoxData.EDataType type)
		{
			switch (type)
			{
			case UIBackpack_BoxData.EDataType.Avatar_Head:
				return EAvatarPart.Avatar_Head;
			case UIBackpack_BoxData.EDataType.Avatar_Body:
				return EAvatarPart.Avatar_Body;
			case UIBackpack_BoxData.EDataType.Avatar_Leg:
				return EAvatarPart.Avatar_Leg;
			default:
				return EAvatarPart.Avatar_None;
			}
		}

		public static void ParseBackpackDataToUI(BagData bagData, NGUI_COMUI.UI_Container container, NGUI_COMUI.UI_Container.EBoxSelType selectType, bool bSell, UIBackpack.EItemType itemType, bool reCreate)
		{
			if (bagData == null)
			{
				Debug.LogWarning("bagData is null!");
				return;
			}
			List<BagItem> bag_list = bagData.m_bag_list;
			int num = 72;
			if (reCreate)
			{
				container.InitContainer(selectType);
				container.ClearContainer();
				if (!bSell && itemType == UIBackpack.EItemType.All)
				{
					container.InitBoxs(num, true);
				}
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < num; i++)
				{
					if (reCreate && !bSell && itemType == UIBackpack.EItemType.All)
					{
						container.SetBoxData(i, null);
					}
					if (i < bag_list.Count)
					{
						UIBackpack_BoxData uIBackpack_BoxData = new UIBackpack_BoxData();
						uIBackpack_BoxData.DataState = BagItemStateToUIDataState(bag_list[i].m_state);
						uIBackpack_BoxData.DataType = (int)BagItemPartToUIDataType(bag_list[i].m_part);
						uIBackpack_BoxData.Unit = bag_list[i].m_unit;
						uIBackpack_BoxData.ItemId = bag_list[i].m_unique_id;
						if (uIBackpack_BoxData.DataType == 5)
						{
							num3++;
						}
						if (bSell)
						{
							if (uIBackpack_BoxData.DataState != UIBackpack_BoxData.EDataState.CanEditCanSell)
							{
								uIBackpack_BoxData = null;
							}
							else
							{
								container.AddBox();
							}
						}
						else
						{
							if (itemType == UIBackpack.EItemType.All)
							{
							}
							if (itemType == UIBackpack.EItemType.Decoration)
							{
								if (uIBackpack_BoxData.DataType == 5)
								{
									container.AddBox();
								}
								else
								{
									uIBackpack_BoxData = null;
								}
							}
							if (itemType == UIBackpack.EItemType.Avatar)
							{
								if (uIBackpack_BoxData.DataType == 2 || uIBackpack_BoxData.DataType == 3 || uIBackpack_BoxData.DataType == 4)
								{
									container.AddBox();
								}
								else
								{
									uIBackpack_BoxData = null;
								}
							}
						}
						if (uIBackpack_BoxData != null)
						{
							if (uIBackpack_BoxData.DataType == 5)
							{
								uIBackpack_BoxData.SpriteName = "deco_" + uIBackpack_BoxData.Unit;
							}
							else if (uIBackpack_BoxData.DataType == 2 || uIBackpack_BoxData.DataType == 3 || uIBackpack_BoxData.DataType == 4)
							{
								GetAvatarPartTex(UIBackpackAvatarTypeToAvatarPart((UIBackpack_BoxData.EDataType)uIBackpack_BoxData.DataType), bag_list[i].m_unit, uIBackpack_BoxData);
							}
							container.SetBoxData(num2++, uIBackpack_BoxData);
						}
					}
					else if (!bSell && itemType == UIBackpack.EItemType.All)
					{
						if (i >= bagData.m_bag_capacity)
						{
							UIBackpack_BoxData uIBackpack_BoxData2 = new UIBackpack_BoxData();
							uIBackpack_BoxData2.DataType = 0;
							container.SetBoxData(i, uIBackpack_BoxData2);
						}
					}
					else if (!bSell && itemType != UIBackpack.EItemType.All)
					{
						break;
					}
				}
				COMA_Achievement.Instance.Rich = num3;
				return;
			}
			for (int j = 0; j < container.LstBoxs.Count; j++)
			{
				if (container.LstBoxs[j].BoxData != null)
				{
					container.LstBoxs[j].BoxData.SetDirty();
				}
			}
		}

		public static void GetAvatarPartTex(EAvatarPart part, string md5, NGUI_COMUI.UI_BoxData data)
		{
			if (md5 != string.Empty)
			{
				UIDataBufferCenter.Instance.FetchFileByMD5(md5, delegate(byte[] fileData)
				{
					GameObject gameObject2 = null;
					gameObject2 = UnityEngine.Object.Instantiate(Resources.Load(GetResPathByAvatarPart(part))) as GameObject;
					Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					texture2D.LoadImage(fileData);
					texture2D.filterMode = FilterMode.Point;
					gameObject2.transform.FindChild("renderObj").gameObject.renderer.material.mainTexture = texture2D;
					IconShot.Instance.GetIconPic(gameObject2, true, delegate(Texture2D tex2D)
					{
						data.Tex = tex2D;
						data.SetDirty();
					});
				});
			}
			else
			{
				GameObject gameObject = null;
				gameObject = UnityEngine.Object.Instantiate(Resources.Load(GetResPathByAvatarPart(part))) as GameObject;
				IconShot.Instance.GetIconPic(gameObject, true, delegate(Texture2D tex2D)
				{
					data.Tex = tex2D;
					data.SetDirty();
				});
			}
		}

		public static void FillTexByMd5(string md5, GameObject objPart)
		{
			if (md5 != string.Empty)
			{
				UIDataBufferCenter.Instance.FetchFileByMD5(md5, delegate(byte[] fileData)
				{
					Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					texture2D.LoadImage(fileData);
					texture2D.filterMode = FilterMode.Point;
					objPart.renderer.material.mainTexture = texture2D;
					Debug.Log("FillTexByMd5---FetchFileByMD5--" + objPart.name);
				});
			}
			else
			{
				objPart.renderer.material.mainTexture = CreateWhiteTexture();
			}
		}

		public static void GetAvatarSuitTex(CSuitMD5 suit, NGUI_COMUI.UI_BoxData data)
		{
			UIDataBufferCenter.Instance.FetchSuitByMD5(suit, delegate(List<byte[]> texSuits)
			{
				byte[] array = texSuits[0];
				byte[] array2 = texSuits[1];
				byte[] array3 = texSuits[2];
				Texture2D texture2D = null;
				Texture2D texture2D2 = null;
				Texture2D texture2D3 = null;
				if (array != null)
				{
					texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height);
					texture2D.LoadImage(array);
				}
				if (array2 != null)
				{
					texture2D2 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height);
					texture2D2.LoadImage(array2);
				}
				if (array3 != null)
				{
					texture2D3 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height);
					texture2D3.LoadImage(array3);
				}
				if (null != texture2D && null == texture2D2 && null == texture2D3)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(GetResPathByDataType(UIBackpack_BoxData.EDataType.Avatar_Head, string.Empty))) as GameObject;
					gameObject.transform.FindChild("renderObj").gameObject.renderer.material.mainTexture = texture2D;
					IconShot.Instance.GetIconPic(gameObject, true, delegate(Texture2D tex2D)
					{
						data.Tex = tex2D;
						data.SetDirty();
					});
				}
				else if (null == texture2D && null != texture2D2 && null == texture2D3)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load(GetResPathByDataType(UIBackpack_BoxData.EDataType.Avatar_Body, string.Empty))) as GameObject;
					gameObject2.transform.FindChild("renderObj").gameObject.renderer.material.mainTexture = texture2D2;
					IconShot.Instance.GetIconPic(gameObject2, true, delegate(Texture2D tex2D)
					{
						data.Tex = tex2D;
						data.SetDirty();
					});
				}
				else if (null == texture2D && null == texture2D2 && null != texture2D3)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load(GetResPathByDataType(UIBackpack_BoxData.EDataType.Avatar_Leg, string.Empty))) as GameObject;
					gameObject3.transform.FindChild("renderObj").gameObject.renderer.material.mainTexture = texture2D3;
					IconShot.Instance.GetIconPic(gameObject3, true, delegate(Texture2D tex2D)
					{
						data.Tex = tex2D;
						data.SetDirty();
					});
				}
				else
				{
					GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load(GetResPathByDataType(UIBackpack_BoxData.EDataType.None, string.Empty))) as GameObject;
					if (null != texture2D)
					{
						gameObject4.transform.FindChild("head").gameObject.renderer.material.mainTexture = texture2D;
					}
					if (null != texture2D2)
					{
						gameObject4.transform.FindChild("body").gameObject.renderer.material.mainTexture = texture2D2;
					}
					if (null != texture2D3)
					{
						gameObject4.transform.FindChild("leg").gameObject.renderer.material.mainTexture = texture2D3;
					}
					IconShot.Instance.GetIconPic(gameObject4, true, delegate(Texture2D tex2D)
					{
						data.Tex = tex2D;
						data.SetDirty();
					});
				}
			});
		}

		public static string GetMailThemeByType(byte type, UIMails_MailBoxData data)
		{
			switch (type)
			{
			case 1:
				return "A Friend Request from " + data.MailInfo.m_sender_name;
			case 2:
				return "Rewards for World Ranking";
			case 3:
				return "Rewards for Fishing";
			case 4:
				return "Rewards for binding with FB";
			case 5:
				return "Income of Selling Avatars";
			case 6:
				return data.MailInfo.m_title;
			default:
				return string.Empty;
			}
		}

		public static bool IsAvatarInFavoriteLst(uint avatarID)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			List<uint> collect_list = notifyRoleDataCmd.m_collect_list;
			for (int i = 0; i < collect_list.Count; i++)
			{
				if (avatarID == collect_list[i])
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsPlayerInFollowLst(uint playerID)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			List<uint> follow_list = notifyRoleDataCmd.m_follow_list;
			for (int i = 0; i < follow_list.Count; i++)
			{
				if (playerID == follow_list[i])
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsAvatarInPraiseLst(uint avatarID)
		{
			return UIDataBufferCenter.Instance.DictPraiseAvataLst.ContainsKey(avatarID);
		}

		public static void PraiseAvatar(uint avatarID)
		{
			if (!IsAvatarInPraiseLst(avatarID))
			{
				UIDataBufferCenter.Instance.DictPraiseAvataLst.Add(avatarID, true);
			}
		}

		public static bool IsSystemShopTmp(string name)
		{
			int result;
			switch (name)
			{
			default:
				result = ((name == "Leg01") ? 1 : 0);
				break;
			case "HBL01":
			case "Head01":
			case "Body01":
				result = 1;
				break;
			}
			return (byte)result != 0;
		}

		public static Texture2D CreateWhiteTexture()
		{
			Color[] array = new Color[COMA_TexBase.Instance.width * COMA_TexBase.Instance.height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Color(0.68f, 0.87f, 1f);
			}
			Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
			texture2D.SetPixels(array);
			texture2D.Apply(false);
			texture2D.filterMode = FilterMode.Point;
			return texture2D;
		}

		public static Texture2D CreateDefaultTexture(int i)
		{
			Texture2D result = null;
			switch (i)
			{
			case 0:
				result = Resources.Load<Texture2D>("FBX/Player/Character/Texture/T_head");
				break;
			case 1:
				result = Resources.Load<Texture2D>("FBX/Player/Character/Texture/T_body");
				break;
			case 2:
				result = Resources.Load<Texture2D>("FBX/Player/Character/Texture/T_Leg");
				break;
			}
			return result;
		}

		public static void PopBlockOnlyMessageBox()
		{
			Debug.Log("-----------------/Call PopBlockOnlyMessageBox");
			UIMessageBoxMgr.Instance.UIMessageBox(new UIMessageBlockOnlyBoxData());
		}

		public static void CloseBlockOnlyMessageBox()
		{
			Debug.Log("-----------------/Call CloseBlockOnlyMessageBox");
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_CloseBlockBox, null, null);
		}

		public static void PopBlockOnlyMessageBox(string mark)
		{
			Debug.Log("-----------------/Call PopBlockOnlyMessageBox: + " + mark);
			UIMessageBlockOnlyBoxData uIMessageBlockOnlyBoxData = new UIMessageBlockOnlyBoxData();
			uIMessageBlockOnlyBoxData.Mark = mark;
			UIMessageBoxMgr.Instance.UIMessageBox(uIMessageBlockOnlyBoxData);
		}

		public static void CloseBlockOnlyMessageBox(string mark)
		{
			Debug.Log("-----------------/Call CloseBlockOnlyMessageBox: " + mark);
			if (!UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_CloseBlockBox, null, mark))
			{
				Debug.Log("-----------------/Call CloseBlockOnlyMessageBox: " + mark + "   Add to Delay Close LST!");
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_MessageBoxDelayClose, null, mark);
			}
		}

		public static void PopBlockForTUIMessageBox()
		{
			Debug.Log("-----------------/Call PopBlockForTUIMessageBox");
			UIMessageBoxMgr.Instance.UIMessageBox(new UIMessageBlockForTUIBoxData());
		}

		public static void CloseBlockForTUIMessageBox()
		{
			Debug.Log("-----------------/Call CloseBlockForTUIMessageBox");
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_CloseBlockForTUIBox, null, null);
		}

		public static void PopIAPBlockMessageBox()
		{
			Debug.Log("-----------------/Call PopIAPBlockMessageBox");
			UIMessageBoxMgr.Instance.UIMessageBox(new UIMessageIAPBlockBoxData());
		}

		public static void CloseIAPBlockMessageBox()
		{
			Debug.Log("-----------------/Call CloseIAPBlockMessageBox");
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_CloseIAPBlockBox, null, null);
		}

		public static void PopCommonMessageBox(UIMessage_CommonBoxData data)
		{
			Debug.LogError("-----------------/Call PopCommonMessageBox");
			UIMessageBoxMgr.Instance.UIMessageBox(data);
		}

		public static void PopJoinGameMessageBox(UIJoinGameMessageBoxData data)
		{
			Debug.Log("-----------------/Call PopCommonMessageBox");
			UIMessageBoxMgr.Instance.UIMessageBox(data);
		}

		public static void PopFacebookMessageBox(UILoginFacebookMessageBoxData data)
		{
			Debug.Log("-----------------/Call PopFacebookMessageBox");
			UIMessageBoxMgr.Instance.UIMessageBox(data);
		}

		public static void PopGetItemBox(UIGetItemBoxData data)
		{
			Debug.Log("-----------------/Call PopGetItemBox");
			UIMessageBoxMgr.Instance.UIMessageBox(data);
		}

		public static bool IsItemEquiped(ulong id)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			if (id != 0L && (id == notifyRoleDataCmd.m_info.m_head || id == notifyRoleDataCmd.m_info.m_body || id == notifyRoleDataCmd.m_info.m_leg || id == notifyRoleDataCmd.m_info.m_head_top || id == notifyRoleDataCmd.m_info.m_head_front || id == notifyRoleDataCmd.m_info.m_head_back || id == notifyRoleDataCmd.m_info.m_head_left || id == notifyRoleDataCmd.m_info.m_head_right || id == notifyRoleDataCmd.m_info.m_chest_front || id == notifyRoleDataCmd.m_info.m_chest_back))
			{
				return true;
			}
			return false;
		}

		public static BagItem GetBagItemByID(ulong id)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			BagData bag_data = notifyRoleDataCmd.m_bag_data;
			for (int i = 0; i < bag_data.m_bag_list.Count; i++)
			{
				if (bag_data.m_bag_list[i].m_unique_id == id)
				{
					return bag_data.m_bag_list[i];
				}
			}
			return null;
		}

		public static string GetBagItemUINTByID(ulong id)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			BagData bag_data = notifyRoleDataCmd.m_bag_data;
			for (int i = 0; i < bag_data.m_bag_list.Count; i++)
			{
				if (bag_data.m_bag_list[i].m_unique_id == id)
				{
					return bag_data.m_bag_list[i].m_unit;
				}
			}
			return string.Empty;
		}

		public static int GetBagIndexByID(ulong id)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			BagData bag_data = notifyRoleDataCmd.m_bag_data;
			for (int i = 0; i < bag_data.m_bag_list.Count; i++)
			{
				if (bag_data.m_bag_list[i].m_unique_id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public static COMA_PackageItem ChangeBagItemToOld(BagItem item)
		{
			if (item == null)
			{
				return null;
			}
			COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
			cOMA_PackageItem.serialName = item.m_unit;
			if (COMA_PackageItem.NameToPart(cOMA_PackageItem.serialName) <= 0)
			{
			}
			return cOMA_PackageItem;
		}

		public static bool IsFriend(uint playerid)
		{
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			return notifyRoleDataCmd.m_friend_list.Contains(playerid);
		}

		public static void PopupTipsBox(string str)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UITipsBox_Popup, null, str);
		}

		public static void PopupTipsBox(string str, bool bUrgency)
		{
			if (!bUrgency)
			{
				PopupTipsBox(str);
			}
			else
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UITipsBox_Popup, null, str, true);
			}
		}

		public static DateTime ToDateSince1970(uint time)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(time);
		}

		public static int GetDayOfWeek(uint time)
		{
			Debug.Log("time:" + time);
			DateTime dateTime = ToDateSince1970(time);
			Debug.Log("Year:" + dateTime.Year);
			Debug.Log("Month:" + dateTime.Month);
			Debug.Log("Day:" + dateTime.Day);
			return (int)dateTime.DayOfWeek;
		}

		public static void PopMsgBox_LackMoney()
		{
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("shangdianjiemian_desc28"));
			uIMessage_CommonBoxData.Mark = "NotEnoughMoney";
			PopCommonMessageBox(uIMessage_CommonBoxData);
		}

		public static bool IsFriendRequestMail(Email mailInfo)
		{
			if (mailInfo.m_type == 2)
			{
				string attach = mailInfo.m_attach;
				if (attach != string.Empty)
				{
					JsonData jsonData = JsonMapper.ToObject<JsonData>(attach);
					int num = (int)jsonData["type"];
					if (num == 1)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static string GetLevelConfigNameByID(int id)
		{
			return "L_" + id + ".xml";
		}

		public static bool IsLevelConfig(string name)
		{
			if (name.StartsWith("L_"))
			{
				return true;
			}
			return false;
		}

		public static int GetLevelIDByFileName(string name)
		{
			if (name.StartsWith("L_"))
			{
				int num = name.IndexOf('.');
				return int.Parse(name.Substring(2, num - 2));
			}
			return -1;
		}

		private static void GetPlayerOwnCastleNum(ref int empty, ref int player)
		{
			MapPoint[] mapPoint = UIDataBufferCenter.Instance.RPGData.m_mapPoint;
			for (int i = 0; i < 100; i++)
			{
				if (mapPoint[i].m_status == 2)
				{
					empty++;
				}
				if (mapPoint[i].m_status == 3)
				{
					player++;
				}
			}
		}

		public static bool RequestRefreshMapData()
		{
			int empty = 0;
			int player = 0;
			GetPlayerOwnCastleNum(ref empty, ref player);
			int num = Mathf.CeilToInt((float)(empty + player) * RPGGlobalData.Instance.RpgMiscUnit._occupyParam1);
			Debug.Log("empty_castle_num " + empty);
			Debug.Log("player_castle_num " + player);
			Debug.Log("==============player_castle_sum " + num);
			if (player >= num)
			{
				Debug.Log("if (player_castle_num >= player_castle_sum)");
				return false;
			}
			uint last_refresh_time = UIDataBufferCenter.Instance.RPGData.m_last_refresh_time;
			uint correctSrvTimeUInt = RPGGlobalClock.Instance.GetCorrectSrvTimeUInt32();
			float num2 = 0f;
			Debug.Log("last_refresh_time " + last_refresh_time);
			Debug.Log("current_time " + correctSrvTimeUInt);
			num2 = ((player <= (uint)Mathf.CeilToInt((float)num * RPGGlobalData.Instance.RpgMiscUnit._occupyParam2)) ? RPGGlobalData.Instance.RpgMiscUnit._occupyParam2_1 : ((player <= (uint)Mathf.CeilToInt((float)num * RPGGlobalData.Instance.RpgMiscUnit._occupyParam3)) ? RPGGlobalData.Instance.RpgMiscUnit._occupyParam3_1 : ((player > (uint)Mathf.CeilToInt((float)num * RPGGlobalData.Instance.RpgMiscUnit._occupyParam4)) ? RPGGlobalData.Instance.RpgMiscUnit._occupyParam5_1 : RPGGlobalData.Instance.RpgMiscUnit._occupyParam4_1)));
			num2 *= 3600f;
			Debug.Log("wait_time " + num2);
			if ((float)(correctSrvTimeUInt - last_refresh_time) >= num2)
			{
				ReqRefreshPlayerLevelCmd extraInfo = new ReqRefreshPlayerLevelCmd();
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
				return true;
			}
			return false;
		}

		public static int GetCrystalNumRenewMobility()
		{
			int mobilityValue = RPGGlobalClock.Instance.GetMobilityValue();
			if (mobilityValue >= RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max)
			{
				return 0;
			}
			uint correctSrvTimeUInt = RPGGlobalClock.Instance.GetCorrectSrvTimeUInt32();
			uint num = correctSrvTimeUInt - UIDataBufferCenter.Instance.RPGData.m_mobility_time;
			int num2 = RPGGlobalData.Instance.RpgMiscUnit._energyRenewTimePerUnit * 60;
			uint num3 = (uint)(RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max * num2) - num;
			int num4 = (int)num3 / 60;
			if (num3 % 60 != 0)
			{
				num4++;
			}
			num4 = Mathf.Clamp(num4, 0, RPGGlobalData.Instance.RpgMiscUnit._energyRenewTimePerUnit * RPGGlobalData.Instance.RpgMiscUnit._energyValue_Max);
			int num5 = RPGGlobalData.Instance.RpgMiscUnit._energyRenewPricePerMinute * num4;
			Debug.Log("crystalNum=" + num5);
			return num5;
		}
	}
}
