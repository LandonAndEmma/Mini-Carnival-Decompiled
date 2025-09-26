using UnityEngine;

public class UI_PlayerInfo : MonoBehaviour
{
	private static UI_PlayerInfo instance;

	private string nickname;

	public static UI_PlayerInfo Instance
	{
		get
		{
			return instance;
		}
	}

	public string Nickname
	{
		get
		{
			return nickname;
		}
		set
		{
			nickname = value;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
