using UnityEngine;

public class UI_GameMode
{
	public static readonly UI_GameMode Instance = new UI_GameMode();

	private string _strGameMode;

	public string GameMode
	{
		get
		{
			return _strGameMode;
		}
		set
		{
			_strGameMode = value;
			Debug.Log("Game Mode------------------: " + _strGameMode);
		}
	}

	private UI_GameMode()
	{
	}
}
