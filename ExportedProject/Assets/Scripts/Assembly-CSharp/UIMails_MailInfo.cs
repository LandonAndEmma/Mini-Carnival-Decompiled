using System;
using System.Collections.Generic;
using LitJson;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Mail.C2S;
using Protocol.Role.C2S;
using UnityEngine;

public class UIMails_MailInfo : UIEntity
{
	[SerializeField]
	private GameObject _mailInfo;

	[SerializeField]
	private UILabel _titleLabel;

	[SerializeField]
	private UILabel _fromNameLabel;

	[SerializeField]
	private UILabel _contentLabel;

	[SerializeField]
	private UITexture _bigTex;

	[SerializeField]
	private UISprite _goldSprite;

	[SerializeField]
	private UISprite _crystalSprite;

	[SerializeField]
	private UILabel _awardNum;

	[SerializeField]
	private GameObject _btnAccept;

	[SerializeField]
	private GameObject _awardInfo;

	[SerializeField]
	private GameObject _awardInfo_RPG;

	[SerializeField]
	private UISprite _award_rpg_Coupon;

	[SerializeField]
	private UISprite _award_rpg_Mobility;

	[SerializeField]
	private GameObject _awardInfo_deco;

	[SerializeField]
	private UISprite _awardDecoSprite;

	[SerializeField]
	private UILabel _labelBtn_receive;

	private UIMails_MailBoxData curMailData;

	[SerializeField]
	private UILabel _labelMailTime;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIMails_OpenMail, this, OpenMail);
		RegisterMessage(EUIMessageID.UIMails_CloseMail, this, CloseMail);
		RegisterMessage(EUIMessageID.UIMails_AcceptMail, this, AcceptMail);
		RegisterMessage(EUIMessageID.UIMails_AcceptMailResult, this, AcceptMailResult);
		RegisterMessage(EUIMessageID.UIMails_DelMail, this, DelMail);
		RegisterMessage(EUIMessageID.UIMails_DelMailResult, this, DelMailResult);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMails_OpenMail, this);
		UnregisterMessage(EUIMessageID.UIMails_CloseMail, this);
		UnregisterMessage(EUIMessageID.UIMails_AcceptMail, this);
		UnregisterMessage(EUIMessageID.UIMails_AcceptMailResult, this);
		UnregisterMessage(EUIMessageID.UIMails_DelMail, this);
		UnregisterMessage(EUIMessageID.UIMails_DelMailResult, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	private void ParseAward(JsonData jsonData)
	{
		if (jsonData.HasMember("res"))
		{
			_awardInfo.SetActive(true);
			_awardInfo_deco.SetActive(false);
			string text = jsonData["res"].ToString();
			string[] array = text.Split(',');
			if (array[0] == "1")
			{
				_goldSprite.enabled = true;
				_crystalSprite.enabled = false;
			}
			else if (array[0] == "2")
			{
				_goldSprite.enabled = false;
				_crystalSprite.enabled = true;
			}
			_awardNum.text = array[1];
		}
		else if (jsonData.HasMember("bag"))
		{
			_awardInfo.SetActive(false);
			_awardInfo_deco.SetActive(true);
			string text2 = jsonData["bag"].ToString();
			_awardDecoSprite.spriteName = "deco_" + text2;
		}
	}

	private string ColorString(string str)
	{
		return "[99FF00]" + str + "[-]";
	}

	private string ColorString(int n)
	{
		return "[99FF00]" + n + "[-]";
	}

	private bool OpenMail(TUITelegram msg)
	{
		UIMails_MailBoxData uIMails_MailBoxData = (curMailData = msg._pExtraInfo as UIMails_MailBoxData);
		DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(uIMails_MailBoxData.MailInfo.m_time);
		_labelMailTime.text = dateTime.Year + "." + dateTime.Month + "." + dateTime.Day;
		_labelBtn_receive.text = "Collect";
		_awardInfo_RPG.SetActive(false);
		if (uIMails_MailBoxData.MailInfo.m_status == 0)
		{
			ReadMailCmd readMailCmd = new ReadMailCmd();
			readMailCmd.m_mail_id = uIMails_MailBoxData.MailInfo.m_id;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, readMailCmd);
		}
		Debug.Log((Email.Type)uIMails_MailBoxData.MailInfo.m_type);
		if (uIMails_MailBoxData.MailInfo.m_type == 2)
		{
			_btnAccept.SetActive((uIMails_MailBoxData.MailInfo.m_status != 2) ? true : false);
			string attach = uIMails_MailBoxData.MailInfo.m_attach;
			if (attach == string.Empty)
			{
				_bigTex.enabled = false;
				_titleLabel.text = uIMails_MailBoxData.MailInfo.m_title;
				_fromNameLabel.text = "Mini Carnival";
				_contentLabel.text = uIMails_MailBoxData.MailInfo.m_content;
				_awardInfo.SetActive(false);
			}
			else
			{
				JsonData jsonData = JsonMapper.ToObject<JsonData>(attach);
				int num = int.Parse(jsonData["type"].ToString());
				switch (num)
				{
				case 5:
				{
					string text3 = jsonData["nick"].ToString();
					int n2 = int.Parse(jsonData["who"].ToString());
					int id = int.Parse(jsonData["id"].ToString());
					string text4 = jsonData["avatar"].ToString();
					string[] unit = text4.Split(';');
					JsonData jsonData4 = jsonData["award"];
					ParseAward(jsonData4);
					_titleLabel.text = UIGolbalStaticFun.GetMailThemeByType((byte)num, uIMails_MailBoxData);
					_fromNameLabel.text = text3;
					_contentLabel.text = ColorString("ID:") + ColorString(n2) + " " + ColorString(text3) + " bought your avatars. Come to claim your income!";
					UIDataBufferCenter.Instance.FetchSuitByMD5(new CSuitMD5((uint)id, unit), delegate(List<byte[]> texSuits)
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
							GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(UIGolbalStaticFun.GetResPathByDataType(UIBackpack_BoxData.EDataType.Avatar_Head, string.Empty))) as GameObject;
							gameObject.transform.FindChild("renderObj").gameObject.renderer.material.mainTexture = texture2D;
							IconShot.Instance.GetIconPic(gameObject, true, delegate(Texture2D tex2D)
							{
								_bigTex.mainTexture = tex2D;
								_bigTex.enabled = true;
							});
						}
						else if (null == texture2D && null != texture2D2 && null == texture2D3)
						{
							GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load(UIGolbalStaticFun.GetResPathByDataType(UIBackpack_BoxData.EDataType.Avatar_Body, string.Empty))) as GameObject;
							gameObject2.transform.FindChild("renderObj").gameObject.renderer.material.mainTexture = texture2D2;
							IconShot.Instance.GetIconPic(gameObject2, true, delegate(Texture2D tex2D)
							{
								_bigTex.mainTexture = tex2D;
								_bigTex.enabled = true;
							});
						}
						else if (null == texture2D && null == texture2D2 && null != texture2D3)
						{
							GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load(UIGolbalStaticFun.GetResPathByDataType(UIBackpack_BoxData.EDataType.Avatar_Leg, string.Empty))) as GameObject;
							gameObject3.transform.FindChild("renderObj").gameObject.renderer.material.mainTexture = texture2D3;
							IconShot.Instance.GetIconPic(gameObject3, true, delegate(Texture2D tex2D)
							{
								_bigTex.mainTexture = tex2D;
								_bigTex.enabled = true;
							});
						}
						else
						{
							GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load(UIGolbalStaticFun.GetResPathByDataType(UIBackpack_BoxData.EDataType.None, string.Empty))) as GameObject;
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
								_bigTex.mainTexture = tex2D;
								_bigTex.enabled = true;
							});
						}
					});
					break;
				}
				case 6:
				{
					_bigTex.enabled = false;
					_titleLabel.text = uIMails_MailBoxData.MailInfo.m_title;
					_fromNameLabel.text = "Mini Carnival";
					string content = uIMails_MailBoxData.MailInfo.m_content;
					_contentLabel.text = content;
					_awardInfo.SetActive(false);
					_awardInfo_deco.SetActive(false);
					if (jsonData.HasMember("award"))
					{
						JsonData jsonData3 = jsonData["award"];
						ParseAward(jsonData3);
					}
					else
					{
						_btnAccept.SetActive(false);
					}
					break;
				}
				case 4:
				{
					_bigTex.enabled = false;
					_titleLabel.text = "Rewards for binding with FB";
					_fromNameLabel.text = "Mini Carnival";
					_contentLabel.text = "Your FB friends have been successfully imported now. Here are the rewards and wish you enjoy this game with your friends!";
					JsonData jsonData5 = jsonData["award"];
					ParseAward(jsonData5);
					break;
				}
				case 3:
				{
					_bigTex.enabled = false;
					int n3 = int.Parse(jsonData["pos"].ToString());
					_titleLabel.text = UIGolbalStaticFun.GetMailThemeByType((byte)num, uIMails_MailBoxData);
					_fromNameLabel.text = "Mini Carnival";
					_contentLabel.text = "Congrats! U ranked " + ColorString(n3) + " in [99FF00]Fishing[-] yesterday and come to claim your rewards. Good luck to u!";
					JsonData jsonData6 = jsonData["award"];
					ParseAward(jsonData6);
					break;
				}
				case 2:
				{
					_bigTex.enabled = false;
					int n = int.Parse(jsonData["pos"].ToString());
					string rankID = jsonData["rank"].ToString();
					int num3 = COMA_CommonOperation.Instance.RankIDToSceneID(rankID);
					rankID = ((num3 != 901) ? Localization.instance.Get(UI_GlobalData.Instance._strModeID[num3]) : Localization.instance.Get("paihangbang_anniu1"));
					_titleLabel.text = UIGolbalStaticFun.GetMailThemeByType((byte)num, uIMails_MailBoxData);
					_fromNameLabel.text = "Mini Carnival";
					_contentLabel.text = "Congrats! U will be rewarded for ranking the " + ColorString(n) + " in " + ColorString(rankID) + " last week.";
					JsonData jsonData2 = jsonData["award"];
					ParseAward(jsonData2);
					break;
				}
				case 1:
				{
					_labelBtn_receive.text = "Agree";
					_goldSprite.enabled = false;
					_crystalSprite.enabled = false;
					_awardNum.text = string.Empty;
					_awardInfo.SetActive(false);
					_awardInfo_deco.SetActive(false);
					int num2 = int.Parse(jsonData["who"].ToString());
					string text = jsonData["nick"].ToString();
					string text2 = jsonData["face"].ToString();
					uIMails_MailBoxData.MailInfo.m_sender_name = text;
					_titleLabel.text = UIGolbalStaticFun.GetMailThemeByType((byte)num, uIMails_MailBoxData);
					_fromNameLabel.text = text;
					_contentLabel.text = "A friend request from " + ColorString("ID:") + ColorString(num2) + " " + ColorString(text) + " , agree or not ?";
					UIDataBufferCenter.Instance.FetchFacebookIconByTID((uint)num2, delegate(Texture2D pic)
					{
						_bigTex.mainTexture = pic;
						_bigTex.enabled = true;
					});
					break;
				}
				}
			}
		}
		else if (uIMails_MailBoxData.MailInfo.m_type == 1)
		{
			_btnAccept.SetActive(false);
			_titleLabel.text = uIMails_MailBoxData.MailInfo.m_title;
			_fromNameLabel.text = "Mini Carnival";
			_contentLabel.text = uIMails_MailBoxData.MailInfo.m_content;
			_bigTex.enabled = false;
			_goldSprite.enabled = false;
			_crystalSprite.enabled = false;
			_awardNum.text = string.Empty;
			_awardInfo.SetActive(false);
			_awardInfo_RPG.SetActive(false);
		}
		else if (uIMails_MailBoxData.MailInfo.m_type == 3)
		{
			_btnAccept.SetActive((uIMails_MailBoxData.MailInfo.m_status != 2) ? true : false);
			_titleLabel.text = TUITool.StringFormat(Localization.instance.Get("youxiang_title10"), uIMails_MailBoxData.MailInfo.m_sender_name);
			_fromNameLabel.text = "Mini Carnival";
			_contentLabel.text = TUITool.StringFormat(Localization.instance.Get("youxiang_test10"), ColorString(uIMails_MailBoxData.MailInfo.m_sender_name), RPGGlobalData.Instance.RpgMiscUnit._EnergyMailNum_PerDay);
			_bigTex.enabled = false;
			_goldSprite.enabled = false;
			_crystalSprite.enabled = false;
			_awardNum.text = string.Empty;
			_awardInfo.SetActive(false);
			_awardInfo_RPG.SetActive(true);
			_award_rpg_Coupon.enabled = false;
			_award_rpg_Mobility.enabled = true;
		}
		else if (uIMails_MailBoxData.MailInfo.m_type == 4)
		{
			_btnAccept.SetActive((uIMails_MailBoxData.MailInfo.m_status != 2) ? true : false);
			_titleLabel.text = TUITool.StringFormat(Localization.instance.Get("youxiang_title12"), uIMails_MailBoxData.MailInfo.m_sender_name);
			_fromNameLabel.text = "Mini Carnival";
			_contentLabel.text = TUITool.StringFormat(Localization.instance.Get("youxiang_test12"), ColorString(uIMails_MailBoxData.MailInfo.m_sender_name), RPGGlobalData.Instance.RpgMiscUnit._couponMailNum_PerDay);
			_bigTex.enabled = false;
			_goldSprite.enabled = false;
			_crystalSprite.enabled = false;
			_awardNum.text = string.Empty;
			_awardInfo.SetActive(false);
			_awardInfo_RPG.SetActive(true);
			_award_rpg_Coupon.enabled = true;
			_award_rpg_Mobility.enabled = false;
		}
		_mailInfo.SetActive(true);
		return true;
	}

	private void RefreshMail()
	{
		if (curMailData != null && curMailData.MailInfo.m_type == 2)
		{
			_btnAccept.SetActive((curMailData.MailInfo.m_status != 2) ? true : false);
		}
	}

	private bool CloseMail(TUITelegram msg)
	{
		_mailInfo.SetActive(false);
		return true;
	}

	private bool AcceptMail(TUITelegram msg)
	{
		if (curMailData != null)
		{
			bool flag = false;
			if (curMailData.MailInfo.m_type >= 3)
			{
				flag = true;
			}
			int num = 0;
			if (!flag)
			{
				string attach = curMailData.MailInfo.m_attach;
				JsonData jsonData = JsonMapper.ToObject<JsonData>(attach);
				num = int.Parse(jsonData["type"].ToString());
			}
			Debug.Log("-----------Send:GainMailCmd " + curMailData.MailInfo.m_id + " type=" + num);
			if (!flag && num == 1)
			{
				Debug.Log("----response_friend_c");
				ResponseFriendCmd responseFriendCmd = new ResponseFriendCmd();
				responseFriendCmd.m_mail_id = curMailData.MailInfo.m_id;
				responseFriendCmd.m_op_code = 0;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, responseFriendCmd);
			}
			else
			{
				GainMailCmd gainMailCmd = new GainMailCmd();
				gainMailCmd.m_mail_id = curMailData.MailInfo.m_id;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, gainMailCmd);
				if (curMailData.MailInfo.m_type == 4)
				{
					COMA_HTTP_DataCollect.Instance.SendGetCouponNumFromFriend("1");
				}
			}
			_btnAccept.SetActive(false);
		}
		return true;
	}

	private bool AcceptMailResult(TUITelegram msg)
	{
		if (curMailData != null && curMailData.MailInfo.m_id == (uint)msg._pExtraInfo)
		{
			RefreshMail();
			if (curMailData.MailInfo.m_type == 4)
			{
				UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("youxiang_desc9"));
			}
			else if (curMailData.MailInfo.m_type == 3)
			{
				UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("youxiang_desc8"));
			}
		}
		return true;
	}

	private bool DelMail(TUITelegram msg)
	{
		if (curMailData == null)
		{
			return true;
		}
		if (curMailData.MailInfo.m_attach != string.Empty && curMailData.MailInfo.m_status != 2 && !UIGolbalStaticFun.IsFriendRequestMail(curMailData.MailInfo))
		{
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("youxiang_desc5"));
			uIMessage_CommonBoxData.Mark = "DelteMail";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		else
		{
			RealDelMail();
		}
		return true;
	}

	private void RealDelMail()
	{
		if (curMailData != null)
		{
			Debug.Log("-----------Send:DelMailCmd " + curMailData.MailInfo.m_id);
			DelMailCmd delMailCmd = new DelMailCmd();
			delMailCmd.m_mail_id = curMailData.MailInfo.m_id;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, delMailCmd);
		}
	}

	private bool DelMailResult(TUITelegram msg)
	{
		if (curMailData != null && curMailData.MailInfo.m_id == (uint)msg._pExtraInfo)
		{
			CloseMail(null);
		}
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "DelteMail":
			RealDelMail();
			break;
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
