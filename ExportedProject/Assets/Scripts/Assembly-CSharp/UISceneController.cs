using System.Collections.Generic;
using MessageID;
using UIGlobal;
using UnityEngine;

public class UISceneController : UIEntity
{
	[SerializeField]
	private Dictionary<string, bool> _curSceneLst = new Dictionary<string, bool>();

	[SerializeField]
	private Dictionary<string, UIEntity> _sceneToEntityMap = new Dictionary<string, UIEntity>();

	[SerializeField]
	private string _fstUISceneName;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_InitSceneEntityMap, this, UIInitSceneEntityMap);
		RegisterMessage(EUIMessageID.UI_ClearSceneEntityMap, this, UClearSceneEntityMap);
		RegisterMessage(EUIMessageID.UI_LoadLevel, this, UILoadLevel);
		RegisterMessage(EUIMessageID.UI_UnloadLevel, this, UIUnloadLevel);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_InitSceneEntityMap, this);
		UnregisterMessage(EUIMessageID.UI_ClearSceneEntityMap, this);
		UnregisterMessage(EUIMessageID.UI_LoadLevel, this);
		UnregisterMessage(EUIMessageID.UI_UnloadLevel, this);
	}

	private void LoadLevel_AddOnly(string loadName)
	{
		if (IsExistScene(loadName))
		{
			if (_sceneToEntityMap[loadName] != null)
			{
				_curSceneLst[loadName] = true;
				_sceneToEntityMap[loadName].gameObject.SetActive(true);
			}
		}
		else
		{
			Application.LoadLevelAdditive(loadName);
			_curSceneLst.Add(loadName, true);
		}
	}

	private bool UILoadLevel(TUITelegram msg)
	{
		if (null != IconShot.Instance)
		{
			IconShot.Instance.ClearRenderQueue();
		}
		TLoadScene tLoadScene = msg._pExtraInfo as TLoadScene;
		string sceneName = tLoadScene._sceneName;
		switch (tLoadScene._param)
		{
		case ELoadLevelParam.AddOnly:
			LoadLevel_AddOnly(sceneName);
			break;
		case ELoadLevelParam.AddHidePre:
			Debug.Log("AddHidePre + " + sceneName + " Exist Count=" + _curSceneLst.Count);
			Application.LoadLevel(sceneName);
			_curSceneLst.Clear();
			_curSceneLst.Add(sceneName, true);
			break;
		case ELoadLevelParam.LoadOnly:
			_curSceneLst.Clear();
			Application.LoadLevel(sceneName);
			break;
		case ELoadLevelParam.LoadOnlyAnDestroyPre:
			Application.LoadLevel(sceneName);
			_curSceneLst.Clear();
			_curSceneLst.Add(sceneName, true);
			break;
		}
		return true;
	}

	private bool UIUnloadLevel(TUITelegram msg)
	{
		return true;
	}

	private bool UIInitSceneEntityMap(TUITelegram msg)
	{
		UIScene uIScene = msg._pExtraInfo as UIScene;
		if (uIScene != null)
		{
			string key = uIScene.gameObject.name;
			if (_sceneToEntityMap.ContainsKey(key))
			{
				_sceneToEntityMap[key] = uIScene;
			}
			else
			{
				_sceneToEntityMap.Add(key, uIScene);
			}
		}
		return true;
	}

	private bool UClearSceneEntityMap(TUITelegram msg)
	{
		UIScene uIScene = msg._pExtraInfo as UIScene;
		if (uIScene != null)
		{
			string key = uIScene.gameObject.name;
			if (_sceneToEntityMap.ContainsKey(key))
			{
				_sceneToEntityMap[key] = null;
			}
		}
		return true;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.transform.gameObject);
		if (_fstUISceneName != string.Empty)
		{
			_curSceneLst.Add(_fstUISceneName, true);
		}
	}

	protected override void Tick()
	{
	}

	protected bool IsExistScene(string name)
	{
		foreach (string key in _curSceneLst.Keys)
		{
			if (name == key)
			{
				return true;
			}
		}
		return false;
	}

	protected void HideAllExclude(string inName)
	{
		string[] array = new string[_curSceneLst.Count];
		_curSceneLst.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			if (text != inName)
			{
				_curSceneLst[array[i]] = false;
				Debug.Log(array[i]);
				GameObject gameObject = GameObject.Find(array[i]);
				if (gameObject != null)
				{
					gameObject.SetActive(_curSceneLst[array[i]]);
				}
			}
		}
	}
}
