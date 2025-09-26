using UnityEngine;

public class UIFriends_FriendRequestData
{
	private string _gid = string.Empty;

	private string _nickname;

	private string _url_archive = string.Empty;

	private string _url_texture = string.Empty;

	public int[] TInPack = new int[3] { -1, -1, -1 };

	public string[] accounterments = new string[7]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	private Texture2D[] _textures = new Texture2D[3];

	public string GID
	{
		get
		{
			return _gid;
		}
		set
		{
			_gid = value;
		}
	}

	public string nickname
	{
		get
		{
			return _nickname;
		}
		set
		{
			_nickname = value;
		}
	}

	public string url_archive
	{
		get
		{
			return _url_archive;
		}
		set
		{
			_url_archive = value;
		}
	}

	public string url_texture
	{
		get
		{
			return _url_texture;
		}
		set
		{
			_url_texture = value;
		}
	}

	public Texture2D[] textures
	{
		get
		{
			return _textures;
		}
		set
		{
			_textures = value;
		}
	}
}
