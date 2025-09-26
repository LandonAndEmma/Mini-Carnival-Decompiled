using UnityEngine;

public class TUI_MsgBox : UICOM
{
	private static TUI_MsgBox _instance;

	private float depth = -420f;

	public static TUI_MsgBox Instance
	{
		get
		{
			return _instance;
		}
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
	}

	public UI_MsgBox MessageBox(int id)
	{
		return MessageBox(id, null);
	}

	public UI_MsgBox MessageBox(int id, GameObject tuiObj, params object[] args)
	{
		if (tuiObj != null)
		{
			base.TUIControls = tuiObj;
		}
		UI_MsgBox result = null;
		switch (id)
		{
		case 101:
		{
			string strTheme3 = TUITextManager.Instance().GetString("fengmian_lianjietishi2");
			result = MessageBox(strTheme3, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 102:
		{
			string text3 = TUITextManager.Instance().GetString("fengmian_lianjietishi1");
			Debug.Log(text3);
			result = MessageBox(text3, string.Empty, 101, null, depth + 20f, string.Empty);
			break;
		}
		case 103:
		{
			string strTheme = TUITextManager.Instance().GetString("fengmian_lianjietishi6");
			result = MessageBox(strTheme, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 104:
		{
			string strTheme2 = TUITextManager.Instance().GetString("guanbitishi_zhujiemian");
			result = MessageBox(strTheme2, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 105:
		{
			string strTheme40 = TUITextManager.Instance().GetString("shangdianjiemian_desc10");
			result = MessageBox(strTheme40, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 106:
		{
			string strTheme39 = TUITextManager.Instance().GetString("shangdianjiemian_desc8");
			result = MessageBox(strTheme39, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 107:
		{
			string strTheme38 = TUITextManager.Instance().GetString("jiaoyijiemian_desc3");
			result = MessageBox(strTheme38, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 108:
		{
			string strTheme37 = TUITextManager.Instance().GetString("shangdianjiemian_desc11");
			result = MessageBox(strTheme37, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 109:
		{
			string strTheme36 = TUITextManager.Instance().GetString("shangdianjiemian_desc9");
			result = MessageBox(strTheme36, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 110:
		{
			string strTheme35 = TUITextManager.Instance().GetString("jiaoyijiemian_desc11");
			result = MessageBox(strTheme35, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 111:
		{
			string strTheme34 = TUITextManager.Instance().GetString("beibaojiemian_desc5");
			result = MessageBox(strTheme34, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 112:
		{
			string strTheme33 = TUITextManager.Instance().GetString("chongzhijilu_zhujiemian");
			result = MessageBox(strTheme33, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 113:
		{
			string strTheme32 = TUITextManager.Instance().GetString("NoviceProcess_22");
			result = MessageBox(strTheme32, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 114:
		{
			string strTheme31 = TUITextManager.Instance().GetString("jiaoyijiemian_desc18");
			result = MessageBox(strTheme31, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 115:
		{
			string strTheme30 = TUITextManager.Instance().GetString("beibaojiemian_desc9");
			result = MessageBox(strTheme30, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 116:
		{
			GameObject boxPrefab = Resources.Load("UI/Misc/CommonMsgBox3") as GameObject;
			string strTheme29 = TUITextManager.Instance().GetString("tuisongwenzi02");
			result = MessageBox(strTheme29, string.Empty, 0, boxPrefab, depth, string.Empty);
			break;
		}
		case 117:
		{
			string strTheme28 = TUITextManager.Instance().GetString("fangjianmantishi_dengdaijiemian");
			result = MessageBox(strTheme28, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 120:
		{
			string text4 = COMA_Sys.Instance.marketRefreshInterval.ToString();
			string strTheme27 = TUITool.StringFormat(TUITextManager.Instance().GetString("fengmian_lianjietishi10"), text4);
			result = MessageBox(strTheme27, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 121:
		{
			string strTheme26 = TUITextManager.Instance().GetString("fengmian_lianjietishi9");
			result = MessageBox(strTheme26, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 122:
		{
			string strTheme25 = TUITextManager.Instance().GetString("fengmian_lianjietishi12");
			result = MessageBox(strTheme25, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 123:
		{
			string strTheme24 = TUITool.StringFormat(TUITextManager.Instance().GetString("beibaojiemian_desc6"), args[0].ToString(), args[1].ToString());
			result = MessageBox(strTheme24, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 124:
		{
			string strTheme23 = TUITextManager.Instance().GetString("fengmian_lianjietishi13");
			result = MessageBox(strTheme23, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 125:
		{
			string strTheme22 = TUITextManager.Instance().GetString("beibaojiemian_desc10");
			result = MessageBox(strTheme22, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 126:
		{
			string strTheme21 = TUITextManager.Instance().GetString("fengmian_lianjietishi14");
			result = MessageBox(strTheme21, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 127:
		{
			string strTheme20 = TUITextManager.Instance().GetString("fengmian_lianjietishi15");
			result = MessageBox(strTheme20, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 128:
		{
			string strTheme19 = TUITextManager.Instance().GetString("fengmian_lianjietishi16");
			result = MessageBox(strTheme19, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 129:
		{
			string strTheme18 = TUITextManager.Instance().GetString("fengmian_lianjietishi18");
			result = MessageBox(strTheme18, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 130:
		{
			string strTheme17 = TUITextManager.Instance().GetString("fengmian_lianjietishi17");
			result = MessageBox(strTheme17, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 131:
		{
			string strTheme16 = TUITextManager.Instance().GetString("haoyoujiemian_desc3");
			result = MessageBox(strTheme16, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 132:
		{
			string strTheme15 = TUITextManager.Instance().GetString("haoyoujiemian_desc7");
			result = MessageBox(strTheme15, string.Empty, 101, null, depth + 5f, string.Empty);
			break;
		}
		case 133:
		{
			string strTheme14 = TUITextManager.Instance().GetString("beibaojiemian_desc11");
			result = MessageBox(strTheme14, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 134:
		{
			string strTheme13 = TUITextManager.Instance().GetString("beibaojiemian_desc18");
			result = MessageBox(strTheme13, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 135:
		{
			string strTheme12 = TUITextManager.Instance().GetString("tanchukuang_desc5");
			result = MessageBox(strTheme12, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 136:
		{
			string strTheme11 = TUITextManager.Instance().GetString("tanchukuang_desc6");
			result = MessageBox(strTheme11, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 137:
		{
			string strTheme10 = TUITextManager.Instance().GetString("haoyoujiemian_desc7");
			result = MessageBox(strTheme10, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 223:
		{
			string strTheme9 = TUITextManager.Instance().GetString("beibaojiemian_desc12");
			result = MessageBox(strTheme9, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 224:
		{
			string strTheme8 = TUITextManager.Instance().GetString("beibaojiemian_desc13");
			result = MessageBox(strTheme8, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 228:
		{
			string strTheme7 = TUITool.StringFormat(TUITextManager.Instance().GetString("beibaojiemian_desc19"), args[0].ToString(), args[1].ToString());
			result = MessageBox(strTheme7, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 229:
		{
			string strTheme6 = TUITextManager.Instance().GetString("diaoyudaojiemian_desc1");
			result = MessageBox(strTheme6, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 230:
		{
			string strTheme5 = TUITextManager.Instance().GetString("tanchukuang_desc7");
			result = MessageBox(strTheme5, string.Empty, 101, null, depth, string.Empty);
			break;
		}
		case 231:
		{
			string strTheme4 = TUITextManager.Instance().GetString("diaoyudaojiemian_desc19");
			result = MessageBox(strTheme4, string.Empty, 0, null, depth, string.Empty);
			break;
		}
		case 1211:
		{
			string tipContent2 = COMA_Sys.Instance.tipContent;
			if (tipContent2 != string.Empty)
			{
				result = MessageBox(tipContent2, string.Empty, 2011, null, depth + 10f, string.Empty);
			}
			break;
		}
		case 1212:
		{
			string text = TUITextManager.Instance().GetString("fengmian_lianjietishi11");
			string text2 = text;
			string tipContent = COMA_Sys.Instance.tipContent;
			if (tipContent != string.Empty)
			{
				text2 = tipContent;
			}
			if (text2 != string.Empty)
			{
				result = MessageBox(text2, string.Empty, 2011, null, depth + 10f, string.Empty);
			}
			break;
		}
		}
		return result;
	}

	public UI_MsgBox MessageBox(int id, GameObject tuiObj)
	{
		return MessageBox(id, tuiObj, null);
	}

	public void TipBox(int id, int num, string itemName, Texture2D tex)
	{
		TipBox(id, num, itemName, tex, string.Empty);
	}

	public void TipBox(int id, int num, string itemName, Texture2D tex, string serialName)
	{
		switch (id)
		{
		case 0:
		{
			GameObject gameObject5 = Object.Instantiate(Resources.Load("UI/InGameCommon/GetItemUI"), new Vector3(0f, 0f, -300f), Quaternion.identity) as GameObject;
			gameObject5.transform.parent = base.TUIControls.transform;
			UI_GetItemUIMgr component5 = gameObject5.GetComponent<UI_GetItemUIMgr>();
			component5.ItemType = 0;
			component5.ItemNum = num;
			component5.ItemName = "Gold";
			break;
		}
		case 1:
		{
			GameObject gameObject4 = Object.Instantiate(Resources.Load("UI/InGameCommon/GetItemUI"), new Vector3(0f, 0f, -300f), Quaternion.identity) as GameObject;
			gameObject4.transform.parent = base.TUIControls.transform;
			UI_GetItemUIMgr component4 = gameObject4.GetComponent<UI_GetItemUIMgr>();
			component4.ItemType = 0;
			component4.ItemNum = -num;
			component4.ItemName = "tCrystal";
			break;
		}
		case 2:
		{
			GameObject gameObject3 = Object.Instantiate(Resources.Load("UI/InGameCommon/GetItemUI"), new Vector3(0f, 0f, -300f), Quaternion.identity) as GameObject;
			gameObject3.transform.parent = base.TUIControls.transform;
			UI_GetItemUIMgr component3 = gameObject3.GetComponent<UI_GetItemUIMgr>();
			component3.ItemType = 1;
			component3.ItemName = itemName;
			if (tex != null)
			{
				component3.ItemTexture = tex;
			}
			else
			{
				component3.SetItemTex(COMA_Scene_Shop.iconPrename + serialName);
			}
			break;
		}
		case 3:
		{
			GameObject gameObject2 = Object.Instantiate(Resources.Load("UI/InGameCommon/GetItemUI"), new Vector3(0f, 0f, -300f), Quaternion.identity) as GameObject;
			gameObject2.transform.parent = base.TUIControls.transform;
			UI_GetItemUIMgr component2 = gameObject2.GetComponent<UI_GetItemUIMgr>();
			component2.ItemType = 0;
			component2.ItemNum = num;
			component2.ItemName2 = "Gold";
			break;
		}
		case 4:
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("UI/InGameCommon/GetItemUI"), new Vector3(0f, 0f, -300f), Quaternion.identity) as GameObject;
			gameObject.transform.parent = base.TUIControls.transform;
			UI_GetItemUIMgr component = gameObject.GetComponent<UI_GetItemUIMgr>();
			component.ItemType = 0;
			component.ItemNum = -num;
			component.ItemName2 = "tCrystal";
			break;
		}
		}
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_BuyItem);
	}
}
