using MessageID;
using Protocol.RPG.C2S;
using UnityEngine;

public class DebugCmd_Common : TBaseCommand
{
	private const string COMMONCOMMAND = "com";

	private const string TEST = "test";

	private const string GETGOLD = "getgold";

	private const string GETCRYSTAL = "getcrystal";

	private const string GETACCESSORIES = "getacc";

	private const string RESTART = "restart";

	private const string FACEBOOK = "facebook";

	private const string ADDRANKSCORE = "addrankscore";

	private const string FULFILLACHIEVEMENT = "fulfillachi";

	private const string SELLTEX = "selltex";

	private const string RPGWIN = "rpgwin";

	private const string RPGNOFOG = "rpgnofog";

	private const string RPGCARDS = "rpgcards";

	private const string RPGLV = "rpglv";

	private void Start()
	{
		if ((bool)TInput_DebugCommand.Instance && TInput_DebugCommand.Instance.RegisterDebugCommand("com", this) != 0)
		{
			Debug.LogError("Register command line failed!!");
		}
	}

	public override void Execute(string[] args)
	{
		if (args != null)
		{
			Debug.Log("DebugCmd_GetFishPole-Execute  args:" + args[0] + "  " + args.Length);
		}
		for (int i = 1; i < args.Length; i++)
		{
			Debug.Log(args[i]);
		}
		switch (args[0])
		{
		case "rpgcards":
		{
			if (args.Length <= 1)
			{
				break;
			}
			FirstRpgFinishCmd firstRpgFinishCmd = new FirstRpgFinishCmd();
			for (int j = 1; j < args.Length; j++)
			{
				uint num = uint.Parse(args[j]);
				if (num >= 1 && num <= 48)
				{
					firstRpgFinishCmd.m_card_list.Add(num);
				}
			}
			firstRpgFinishCmd.m_card_num = (byte)firstRpgFinishCmd.m_card_list.Count;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, firstRpgFinishCmd);
			break;
		}
		case "rpglv":
			if (args.Length > 1)
			{
				int rpg_level = int.Parse(args[1]);
				ModifyRPGLvCmd modifyRPGLvCmd = new ModifyRPGLvCmd();
				modifyRPGLvCmd.m_rpg_level = (uint)rpg_level;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyRPGLvCmd);
			}
			break;
		case "test":
			if (args.Length > 1)
			{
				int num6 = int.Parse(args[1]);
				UIMessageBoxMgr.Instance.UIMessageBox(new UIACPopupBoxData(num6 + 1));
			}
			break;
		case "getgold":
		{
			if (args.Length <= 1)
			{
				break;
			}
			int div = int.Parse(args[1]);
			COMA_Pref.Instance.AddGold(div);
			GameObject gameObject = GameObject.Find("TUI");
			if (gameObject != null)
			{
				UIMessageHandler component = gameObject.transform.root.GetComponent<UIMessageHandler>();
				if (component != null)
				{
					component.RefreshGoldAndCrystal();
				}
			}
			COMA_Pref.Instance.Save(true);
			break;
		}
		case "getcrystal":
		{
			if (args.Length <= 1)
			{
				break;
			}
			int div2 = int.Parse(args[1]);
			COMA_Pref.Instance.AddCrystal(div2);
			GameObject gameObject2 = GameObject.Find("TUI");
			if (gameObject2 != null)
			{
				UIMessageHandler component2 = gameObject2.transform.root.GetComponent<UIMessageHandler>();
				if (component2 != null)
				{
					component2.RefreshGoldAndCrystal();
				}
			}
			COMA_Pref.Instance.Save(true);
			break;
		}
		case "getacc":
		{
			if (args.Length <= 1)
			{
				break;
			}
			string text3 = args[1];
			if (text3 == string.Empty)
			{
				break;
			}
			if (COMA_Pref.Instance.PackageNullCount() < 1)
			{
				TUI_MsgBox.Instance.MessageBox(107);
				break;
			}
			COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
			cOMA_PackageItem.serialName = text3;
			if (cOMA_PackageItem.IsAccessoriesExist())
			{
				cOMA_PackageItem.itemName = text3;
				cOMA_PackageItem.part = 1;
				cOMA_PackageItem.CreateIconTexture();
				cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
				COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
				COMA_Pref.Instance.Save(true);
			}
			else
			{
				Debug.LogError("No Accessories!!");
			}
			break;
		}
		case "restart":
			Application.LoadLevel("COMA_Start");
			break;
		case "facebook":
			if (args.Length > 1)
			{
				string text2 = args[1];
				if (text2 == "feed")
				{
					UIFacebookFeedback.Instance.PublishAd();
				}
				else if (text2 == "friend")
				{
					UIFacebookFeedback.Instance.InviteFriends();
				}
			}
			break;
		case "addrankscore":
			if (args.Length > 2)
			{
				int num2 = int.Parse(args[1]);
				if (num2 < 1)
				{
					break;
				}
				int num3 = int.Parse(args[2]);
				string text = COMA_CommonOperation.Instance.SceneIDToRankID(num2);
				if (args.Length > 3)
				{
					if (args[3] == "QA")
					{
						string[] array = new string[9] { "15099", "3781", "3687", "64541", "257", "4209", "3675", "181512", "3673" };
						string[] array2 = new string[9] { "Pengqi", "iPhone5S", "Iphone5", "Zc", "P059127219", "Jjjj", "Newpad", "LOKI", "iPhone5C" };
						for (int k = 0; k < array.Length; k++)
						{
							COMA_Server_Rank.Instance.SubmitScore(array[k], num3, text, array2[k]);
						}
						Debug.Log(text + " Rank Score Add : " + num3);
					}
					else
					{
						Debug.LogError("Param Error!!");
					}
				}
				else
				{
					COMA_Server_Rank.Instance.SubmitScore(COMA_Server_ID.Instance.GID, num3, text, COMA_Pref.Instance.nickname);
					Debug.Log(text + " Rank Score Add : " + num3);
				}
			}
			else
			{
				Debug.LogError("Param Error!!");
			}
			break;
		case "fulfillachi":
			if (args.Length > 1)
			{
				int num7 = int.Parse(args[1]);
				COMA_Achievement.Instance.FulFill(num7);
				Debug.Log("Fulfill Achievement : " + num7);
			}
			break;
		case "selltex":
			if (args.Length > 1)
			{
				int num4 = int.Parse(args[1]);
				int num5 = ((args.Length <= 2) ? 1 : int.Parse(args[2]));
				if (num4 >= 0 && num4 < COMA_TexOnSale.Instance.items.Count)
				{
					Debug.Log(COMA_TexOnSale.Instance.items.Count);
					COMA_TexOnSale.Instance.items[num4].numGet -= num5;
					Debug.Log("Sell Texture Success : " + num5 + " " + COMA_TexOnSale.Instance.items[num4].tid);
				}
				else
				{
					Debug.LogError("Texture is not exist with ID:" + num4);
				}
			}
			break;
		case "rpgwin":
			if (RPGRefree.Instance != null)
			{
				TMessageDispatcher.Instance.DispatchMsg(-1, RPGRefree.Instance.GetInstanceID(), 5027, TTelegram.SEND_MSG_IMMEDIATELY, null);
			}
			break;
		case "rpgnofog":
			if (RPGRefree.Instance != null)
			{
				TMessageDispatcher.Instance.DispatchMsg(-1, RPGRefree.Instance.GetInstanceID(), 5028, TTelegram.SEND_MSG_IMMEDIATELY, null);
			}
			break;
		}
	}
}
