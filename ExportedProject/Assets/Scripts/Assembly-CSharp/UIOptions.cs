using System.Collections;
using System.Collections.Generic;
using System.IO;
using MessageID;
using UIGlobal;
using UnityEngine;

public class UIOptions : UIMessageHandler
{
	private static UIOptions _instance;

	[SerializeField]
	private TUISliderEx sensitivitySlider;

	[SerializeField]
	private Transform _backBtn;

	[SerializeField]
	private GameObject _block;

	private Vector3 _oriBtnBackLocPos = Vector3.zero;

	[SerializeField]
	private Transform _oriBtnBackParent;

	[SerializeField]
	private Transform _changeNameBtnBackParent;

	private Vector3 _changeNameBtnBackLocPos = Vector3.zero;

	public float _fInitValue = 0.5f;

	[SerializeField]
	private TUILabel _clearCacheLabel;

	private bool _bInited;

	public static UIOptions Instance
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
		RefreshGoldAndCrystal();
		_block.SetActive(false);
		_oriBtnBackLocPos = _backBtn.localPosition;
		_changeNameBtnBackLocPos = _oriBtnBackLocPos;
		_changeNameBtnBackLocPos.z = -250f;
	}

	public void DisableBlock()
	{
		Debug.Log("DisableBlock");
		_block.SetActive(false);
		_backBtn.parent = _oriBtnBackParent;
		_backBtn.localPosition = _oriBtnBackLocPos;
	}

	private new void Update()
	{
		if (!_bInited)
		{
			sensitivitySlider.sliderValue = (COMA_Sys.Instance.sensitivity - 0.5f) / 1.5f;
			_bInited = true;
		}
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			COMA_Pref.Instance.Save(true);
			Debug.Log("Button_back-CommandClick");
			if (_block.activeSelf)
			{
				if (UIInputName.Instance != null)
				{
					Object.DestroyObject(UIInputName.Instance.transform.root.gameObject);
				}
				DisableBlock();
			}
			else
			{
				TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
				UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_IapEntry(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_IapEntry-CommandClick");
			EnterIAPUI("UI.Options", _aniControl);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Forum(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Reset-CommandClick");
			Application.OpenURL("http://forum.trinitigame.com/forum/viewforum.php?f=129&sid=e31a87f5019cf684cd062c7d038a69ec");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_ChangeName(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_ChangeName-CommandClick");
			COMA_Pref.Instance.Save(true);
			_block.SetActive(true);
			COMA_CommonOperation.Instance.inputKind = InputBoardCategory.Rename;
			COMA_CommonOperation.Instance.defaultInput = string.Empty;
			Application.LoadLevelAdditive("UI.InputName");
			_backBtn.localPosition = _changeNameBtnBackLocPos;
			_backBtn.parent = _changeNameBtnBackParent;
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Share(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Share-CommandClick");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public static void Support()
	{
		string text = string.Empty;
		if (COMA_Pref.Instance.playerID != string.Empty)
		{
			text = COMA_Pref.Instance.playerID;
		}
		else if (COMA_Server_ID.Instance.GID != string.Empty)
		{
			text = COMA_Server_ID.Instance.GID;
		}
		else if (GameCenter.Instance.gameCenterID != string.Empty)
		{
			text = GameCenter.Instance.gameCenterID;
		}
		else if (COMA_Sys.Instance.VID != string.Empty)
		{
			text = COMA_Sys.Instance.VID;
		}
		string text2 = "country=" + COMA_Sys.Instance.PlayerDevice.GetCountryCode();
		string text3 = "&device=android";
		string text4 = "&os=" + COMA_Sys.Instance.PlayerDevice.GetSysVersion();
		string text5 = "&game=Mini%20Carnival";
		string text6 = "&gamever=" + COMA_Sys.Instance.version;
		string text7 = text.Replace(" ", string.Empty);
		string text8 = "&code=" + text7;
		string empty = string.Empty;
		string url = "http://www.trinitigame.com/support/support.html?" + text2 + text3 + text4 + text5 + text6 + text8 + empty;
		Application.OpenURL(url);
	}

	public void HandleEventButton_Support(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Support-CommandClick");
			Support();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public static void Rate()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.trinitigame.android.google.callofminiavatar");
	}

	private IEnumerator MultiFrameClearCache()
	{
		List<string> lstCache = COMA_FileIO.SearchTexCacheFiles(1000);
		Debug.Log(lstCache.Count);
		if (lstCache.Count > 0)
		{
			Debug.Log(lstCache[0]);
		}
		for (int i = 0; i < lstCache.Count; i++)
		{
			if (lstCache[i] != "bfbe092a8f68fabfe83afbc5e961487d" || lstCache[i] != "6ba2377776d6c137ee29551baff81bb5" || lstCache[i] != "54245d0a0b0c5c8305976247da71f59f" || lstCache[i] != "9a53aef61db65e1ed1298fca0cc15a3d")
			{
				string texCachePath = COMA_FileIO.GetTexCachePath(lstCache[i]);
				if (File.Exists(texCachePath))
				{
					File.Delete(texCachePath);
				}
				_clearCacheLabel.Text = "Clearing... " + i * 100 / (lstCache.Count - 1) + "%";
			}
			yield return 0;
		}
		UITexCacherMgr.Instance.ClearCacher();
		UITexCacherMgr.Instance.InitTexCacher();
		_block.SetActive(false);
		_clearCacheLabel.gameObject.SetActive(false);
	}

	public void HandleEventButton_Review(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Review-CommandClick");
			if (UITexCacherMgr.Instance != null)
			{
				_block.SetActive(true);
				_clearCacheLabel.gameObject.SetActive(true);
				_clearCacheLabel.Text = "Clearing... 0%";
				StartCoroutine(MultiFrameClearCache());
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_Credits(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_Credits-CommandClick");
			Application.LoadLevel("COMA_Credits");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventSensitivityChanged(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == TUISliderEx.OnSliderChange)
		{
			Debug.Log("OnSliderChange : " + wparam);
			COMA_Sys.Instance.sensitivity = wparam * 1.5f + 0.5f;
		}
	}
}
