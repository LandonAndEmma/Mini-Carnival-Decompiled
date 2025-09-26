using System.Collections.Generic;
using UnityEngine;

public class UITexCacherMgr : MonoBehaviour
{
	public static UITexCacherMgr Instance;

	[SerializeField]
	private Dictionary<string, byte> _texCacheMap = new Dictionary<string, byte>();

	public Texture2D texModel;

	public Texture2D texHead;

	public Texture2D texBody;

	public Texture2D texLeg;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	protected void OnEnable()
	{
		Instance = this;
	}

	protected void OnDisable()
	{
		Instance = null;
	}

	public void InitTexCacher()
	{
		List<string> list = COMA_FileIO.SearchTexCacheFiles(1000);
		for (int i = 0; i < list.Count; i++)
		{
			_texCacheMap.Add(list[i], 0);
		}
		if (!_texCacheMap.ContainsKey("bfbe092a8f68fabfe83afbc5e961487d"))
		{
			COMA_FileIO.AddFileToCache("bfbe092a8f68fabfe83afbc5e961487d", texModel.EncodeToPNG());
			_texCacheMap.Add("bfbe092a8f68fabfe83afbc5e961487d", 0);
		}
		if (!_texCacheMap.ContainsKey("6ba2377776d6c137ee29551baff81bb5"))
		{
			COMA_FileIO.AddFileToCache("6ba2377776d6c137ee29551baff81bb5", texHead.EncodeToPNG());
			_texCacheMap.Add("6ba2377776d6c137ee29551baff81bb5", 0);
		}
		if (!_texCacheMap.ContainsKey("54245d0a0b0c5c8305976247da71f59f"))
		{
			COMA_FileIO.AddFileToCache("54245d0a0b0c5c8305976247da71f59f", texBody.EncodeToPNG());
			_texCacheMap.Add("54245d0a0b0c5c8305976247da71f59f", 0);
		}
		if (!_texCacheMap.ContainsKey("9a53aef61db65e1ed1298fca0cc15a3d"))
		{
			COMA_FileIO.AddFileToCache("9a53aef61db65e1ed1298fca0cc15a3d", texLeg.EncodeToPNG());
			_texCacheMap.Add("9a53aef61db65e1ed1298fca0cc15a3d", 0);
		}
	}

	private void Start()
	{
		InitTexCacher();
	}

	private void Update()
	{
	}

	public Texture2D LoadTexFromCache(string md5)
	{
		if (_texCacheMap.ContainsKey(md5))
		{
			return COMA_FileIO.GetTexFromCache(md5);
		}
		return null;
	}

	public byte[] LoadTexBufferFromCache(string md5)
	{
		if (_texCacheMap.ContainsKey(md5))
		{
			return COMA_FileIO.GetTexBufferFromCache(md5);
		}
		return null;
	}

	public void InsertTexToCache(string md5, byte[] data)
	{
		if (_texCacheMap.ContainsKey(md5))
		{
			return;
		}
		if (_texCacheMap.Count > 1000)
		{
			int num = 0;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, byte> item in _texCacheMap)
			{
				list.Add(item.Key);
				num++;
				if (num > 100)
				{
					break;
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				DelTexFromeCache(list[i]);
			}
		}
		COMA_FileIO.AddFileToCache(md5, data);
		_texCacheMap.Add(md5, 0);
	}

	protected void DelTexFromeCache(string md5)
	{
		if (_texCacheMap.ContainsKey(md5))
		{
			_texCacheMap.Remove(md5);
			COMA_FileIO.DelTexFileFromCache(md5);
		}
	}

	public void ClearCacher()
	{
		_texCacheMap.Clear();
	}
}
