using System;
using System.Collections.Generic;
using Facebook;
using LitJson;
using MC_UIToolKit;
using MessageID;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using UnityEngine;

public class UIFacebookFeedback : MonoBehaviour
{
	public enum ConnectKind
	{
		Login = 0,
		Refresh = 1
	}

	private delegate void LoginSuccessCallBack();

	private static UIFacebookFeedback _instance;

	[NonSerialized]
	public ConnectKind tipMark;

	private LoginSuccessCallBack loginFinish;

	private bool isInit;

	public string FriendSelectorTitle = string.Empty;

	public string FriendSelectorMessage = string.Empty;

	public string FriendSelectorFilters = "[\"all\",\"app_users\",\"app_non_users\"]";

	public string FriendSelectorData = "{}";

	public string FriendSelectorExcludeIds = string.Empty;

	public string FriendSelectorMax = string.Empty;

	private string FeedToId = string.Empty;

	private string FeedLink = "https://play.google.com/store/apps/details?id=com.trinitigame.android.google.callofminiavatar";

	private string FeedLinkName = "Go on a tour of a Carnival Fantasy floating in the air!";

	private string FeedLinkCaption = "Mini Carnival";

	private string FeedLinkDescription = "Design a unique avatar by yourself in a creative way, and enjoy numerous interesting mini games! I'm having a lot of fun in Mini Carnival. Join me now!";

	private string FeedPicture = "https://graph.facebook.com/253674194784825/picture?type=large";

	private string FeedMediaSource = string.Empty;

	private string FeedActionName = string.Empty;

	private string FeedActionLink = string.Empty;

	private string FeedReference = string.Empty;

	private bool IncludeFeedProperties;

	private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();

	public static UIFacebookFeedback Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		CallFBInit();
	}

	public bool HasInit()
	{
		return isInit;
	}

	public bool HasLogin()
	{
		return FB.IsLoggedIn;
	}

	public void LoginFacebook()
	{
		if (FB.IsLoggedIn)
		{
			BindFacebook();
			return;
		}
		loginFinish = BindFacebook;
		CallFBLogin();
	}

	public void LogoutFacebook()
	{
		CallFBLogout();
	}

	public void InviteFriends()
	{
		if (FB.IsLoggedIn)
		{
			CallAppRequestAsFriendSelector();
			return;
		}
		tipMark = ConnectKind.Login;
		loginFinish = CallAppRequestAsFriendSelector;
		CallFBLogin();
	}

	public void PublishAd()
	{
		if (FB.IsLoggedIn)
		{
			CallFBFeed();
			return;
		}
		tipMark = ConnectKind.Login;
		loginFinish = CallFBFeed;
		CallFBLogin();
	}

	private void BindFacebook()
	{
		if (tipMark == ConnectKind.Login)
		{
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fb_desc4"));
			UIGolbalStaticFun.PopCommonMessageBox(data);
		}
		else if (tipMark == ConnectKind.Refresh)
		{
			UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("fb_desc5"));
			UIGolbalStaticFun.PopCommonMessageBox(data2);
		}
		BindFacebookInfo();
		ImportFacebookFriends();
	}

	private void BindFacebookInfo()
	{
		FB.API("/me/picture", HttpMethod.GET, delegate(FBResult response)
		{
			if (response.Texture == null)
			{
				Debug.LogWarning("Facebook picture is null!!");
			}
			else
			{
				Texture2D texture = response.Texture;
				byte[] buffer = texture.EncodeToPNG();
				UIDataBufferCenter.Instance.UploadFile(0uL, buffer, delegate(string texMD5)
				{
					COMA_GC_TID.Instance.fbLocal = FB.UserId;
					COMA_GC_TID.Instance.Save();
					SetFaceImageCmd extraInfo = new SetFaceImageCmd
					{
						m_facebookid = FB.UserId,
						m_md5 = texMD5
					};
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
					NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
					if (notifyRoleDataCmd.m_hasInportFB == 0)
					{
						SetImportFBMarkCmd extraInfo2 = new SetImportFBMarkCmd();
						UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo2);
						notifyRoleDataCmd.m_hasInportFB = 1;
					}
					UIDataBufferCenter.Instance.playerInfo.m_facebook_id = FB.UserId;
					UIDataBufferCenter.Instance.playerInfo.m_face_image_md5 = texMD5;
				});
			}
		});
	}

	private void ImportFacebookFriends()
	{
		FB.API("/me/friends", HttpMethod.GET, delegate(FBResult response)
		{
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response.Text);
			JsonData jsonData2 = jsonData["data"];
			List<string> list = new List<string>();
			for (int i = 0; i < jsonData2.Count; i++)
			{
				list.Add(jsonData2[i]["id"].ToString());
			}
			string text = JsonMapper.ToJson(list);
			Debug.Log(text);
			SearchRoleByFBInfoCmd extraInfo = new SearchRoleByFBInfoCmd
			{
				m_param = text
			};
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
		});
	}

	private void CallFBInit()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}

	private void OnInitComplete()
	{
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		isInit = true;
	}

	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}

	private void CallFBLogin()
	{
		FB.Login("email,read_friendlists,publish_actions", LoginCallback);
	}

	private void LoginCallback(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.LogError("Error Response:\n" + result.Error);
		}
		else if (!FB.IsLoggedIn)
		{
			if (tipMark == ConnectKind.Login)
			{
				UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fb_desc3"));
				UIGolbalStaticFun.PopCommonMessageBox(data);
			}
			else if (tipMark == ConnectKind.Refresh)
			{
				UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("fb_desc6"));
				UIGolbalStaticFun.PopCommonMessageBox(data2);
			}
		}
		else
		{
			if (loginFinish != null)
			{
				loginFinish();
				loginFinish = null;
			}
			LoginFacebook();
		}
	}

	private void CallFBLogout()
	{
		FB.Logout();
	}

	private void FriendSelectCallback(FBResult result)
	{
		if (string.IsNullOrEmpty(result.Error))
		{
			ImportFacebookFriends();
		}
	}

	private void CallAppRequestAsFriendSelector()
	{
		int? maxRecipients = null;
		if (FriendSelectorMax != string.Empty)
		{
			try
			{
				maxRecipients = int.Parse(FriendSelectorMax);
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
		}
		string[] excludeIds = ((!(FriendSelectorExcludeIds == string.Empty)) ? FriendSelectorExcludeIds.Split(',') : null);
		List<object> filters = null;
		FB.AppRequest(FriendSelectorMessage, null, filters, excludeIds, maxRecipients, FriendSelectorData, FriendSelectorTitle, FriendSelectCallback);
	}

	private void FeedCallback(FBResult result)
	{
		if (string.IsNullOrEmpty(result.Error))
		{
			Debug.Log("Feed Success : " + result.Text);
		}
		else
		{
			Debug.Log("Feed Fail : " + result.Error);
		}
	}

	private void CallFBFeed()
	{
		Dictionary<string, string[]> properties = null;
		if (IncludeFeedProperties)
		{
			properties = FeedProperties;
		}
		FB.Feed(FeedToId, FeedLink, FeedLinkName, FeedLinkCaption, FeedLinkDescription, FeedPicture, FeedMediaSource, FeedActionName, FeedActionLink, FeedReference, properties, FeedCallback);
	}
}
