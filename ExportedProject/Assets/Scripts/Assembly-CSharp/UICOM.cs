using UnityEngine;

public class UICOM : TBaseEntity
{
	[SerializeField]
	protected static string _preSceneName;

	private static GameObject commonBoxPrefab;

	private static GameObject commonBoxPrefabOK;

	private static GameObject commonBoxPrefabNEWS;

	[SerializeField]
	private GameObject tuiControls;

	[SerializeField]
	protected GameObject _msgBoxNode;

	public static GameObject CommonBoxPrefab
	{
		get
		{
			if (commonBoxPrefab == null)
			{
				commonBoxPrefab = Resources.Load("UI/Misc/CommonMsgBox") as GameObject;
			}
			return commonBoxPrefab;
		}
	}

	public static GameObject CommonBoxPrefabOK
	{
		get
		{
			if (commonBoxPrefabOK == null)
			{
				commonBoxPrefabOK = Resources.Load("UI/Misc/CommonMsgBox2") as GameObject;
			}
			return commonBoxPrefabOK;
		}
	}

	public static GameObject CommonBoxPrefabNEWS
	{
		get
		{
			if (commonBoxPrefabNEWS == null)
			{
				commonBoxPrefabNEWS = Resources.Load("UI/Misc/GameNews") as GameObject;
			}
			return commonBoxPrefabNEWS;
		}
	}

	public GameObject TUIControls
	{
		get
		{
			if (tuiControls == null)
			{
				tuiControls = GameObject.Find("TUIControls");
			}
			return tuiControls;
		}
		set
		{
			tuiControls = value;
		}
	}

	public void EnterIAPUI(string strCurSceneName, UI_AnimationControl aniControl)
	{
		_preSceneName = strCurSceneName;
		if (aniControl != null)
		{
			aniControl.PlayExitAni("UI.IAP");
		}
		else
		{
			Application.LoadLevel("UI.IAP");
		}
	}

	protected virtual UI_MsgBox MessageBox(string strTheme, string strFurther, int nType, GameObject boxPrefab, float fLayer, string strParam)
	{
		GameObject original = ((nType == 101) ? CommonBoxPrefabOK : ((!(boxPrefab == null)) ? boxPrefab : CommonBoxPrefab));
		if (nType == 2011)
		{
			original = CommonBoxPrefabNEWS;
		}
		GameObject gameObject = Object.Instantiate(original, new Vector3(0f, 0f, fLayer), Quaternion.identity) as GameObject;
		gameObject.transform.parent = TUIControls.transform;
		UI_MsgBox component = gameObject.GetComponent<UI_MsgBox>();
		component.MsgBox(strTheme, strFurther, nType, strParam);
		return component;
	}

	protected virtual UI_MsgBox MessageBox(string strTheme, string strFurther, int nType, GameObject boxPrefab, float fLayer)
	{
		return MessageBox(strTheme, strFurther, nType, boxPrefab, fLayer, string.Empty);
	}

	protected virtual UI_MsgBox MessageBox(string strTheme)
	{
		return MessageBox(strTheme, string.Empty, 0, null, -120f);
	}

	protected virtual UI_MsgBox MessageBox(string strTheme, string strFurther)
	{
		return MessageBox(strTheme, strFurther, 0, null, -120f);
	}

	public void BtnScale(TUIControl control)
	{
		UI_ButtonScale component = control.GetComponent<UI_ButtonScale>();
		if (component != null)
		{
			component.BtnScale();
		}
		else
		{
			control.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		}
	}

	public void BtnRestoreScale(TUIControl control)
	{
		UI_ButtonScale component = control.GetComponent<UI_ButtonScale>();
		if (component != null)
		{
			component.BtnRestoreScale();
		}
		else
		{
			control.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void BtnOpenLight(TUIControl control)
	{
		UI_ButtonLight component = control.gameObject.GetComponent<UI_ButtonLight>();
		if (component != null)
		{
			component.LightOn();
		}
	}

	public void BtnCloseLight(TUIControl control)
	{
		UI_ButtonLight component = control.gameObject.GetComponent<UI_ButtonLight>();
		if (component != null)
		{
			component.LightOff();
		}
	}
}
