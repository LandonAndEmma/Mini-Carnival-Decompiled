using System.Collections.Generic;
using UnityEngine;

public class TInput_DebugCommand : MonoBehaviour
{
	private static TInput_DebugCommand _instance;

	private string stringToEnter = string.Empty;

	private bool _bShowCmdWnd;

	private Dictionary<string, TBaseCommand> _mapCmdHandle = new Dictionary<string, TBaseCommand>();

	public static TInput_DebugCommand Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public int RegisterDebugCommand(string strCmd, TBaseCommand handle)
	{
		if (_mapCmdHandle.ContainsKey(strCmd))
		{
			return -1;
		}
		_mapCmdHandle.Add(strCmd, handle);
		return 0;
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			_bShowCmdWnd = !_bShowCmdWnd;
			if (TPCInputMgr.Instance != null)
			{
				TPCInputMgr.Instance.Closed = _bShowCmdWnd;
			}
		}
	}

	private void OnGUI()
	{
		if (!_bShowCmdWnd)
		{
			return;
		}
		GUI.contentColor = Color.red;
		stringToEnter = GUI.TextField(new Rect(0f, Screen.height - 20, Screen.width, 20f), stringToEnter, 50);
		if (Event.current.type != EventType.KeyDown)
		{
			return;
		}
		if (Event.current.character == '\n')
		{
			string[] array = stringToEnter.Split(' ');
			if (array.Length - 1 > 0)
			{
				string[] array2 = new string[array.Length - 1];
				for (int i = 0; i < array.Length - 1; i++)
				{
					array2[i] = array[i + 1];
				}
				HandleCommand(array[0], array2);
			}
			else
			{
				HandleCommand(array[0], null);
			}
			stringToEnter = string.Empty;
			_bShowCmdWnd = !_bShowCmdWnd;
			if (TPCInputMgr.Instance != null)
			{
				TPCInputMgr.Instance.Closed = _bShowCmdWnd;
			}
		}
		else
		{
			stringToEnter += Event.current.character;
		}
	}

	private void HandleCommand(string cmd, string[] param)
	{
		Debug.Log("=====Enter:HandleCommand<>:" + cmd);
		if (_mapCmdHandle.ContainsKey(cmd))
		{
			Debug.Log("HandleCommand:" + cmd);
			_mapCmdHandle[cmd].Execute(param);
		}
	}
}
