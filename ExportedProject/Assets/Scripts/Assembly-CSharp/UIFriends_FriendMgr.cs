using System.Collections.Generic;
using UnityEngine;

public class UIFriends_FriendMgr : MonoBehaviour
{
	private List<UIFriends_OneFriend> _friendsList;

	[SerializeField]
	private GameObject _oneFriendPrefab;

	[SerializeField]
	private TUILabel _friendNum;

	private int _friendTestIndex;

	private void Awake()
	{
	}

	private void Start()
	{
		InitFriends(null);
	}

	public void AddFriend(string gid, string strName, Texture2D tex)
	{
		UIFriends_OneFriendData uIFriends_OneFriendData = new UIFriends_OneFriendData();
		uIFriends_OneFriendData.IsAddBtn = false;
		uIFriends_OneFriendData.GID = gid;
		uIFriends_OneFriendData.FriendName = strName;
		uIFriends_OneFriendData.FriendTex2D = tex;
		AddFriend(uIFriends_OneFriendData);
	}

	public void TestAddFriend()
	{
		UIFriends_OneFriendData uIFriends_OneFriendData = new UIFriends_OneFriendData();
		uIFriends_OneFriendData.IsAddBtn = false;
		uIFriends_OneFriendData.FriendName = "jj" + _friendTestIndex++;
		AddFriend(uIFriends_OneFriendData);
	}

	public void SetFriendName(int i, string nickname)
	{
		int index = _friendsList.Count - 1 - i;
		_friendsList[index].FriendData.FriendName = nickname;
	}

	public void SetFriendTexture(int i, Texture2D tex)
	{
		int index = _friendsList.Count - 1 - i;
		_friendsList[index].FriendData.FriendTex2D = tex;
	}

	private void Update()
	{
	}

	public int InitFriends(UIFriends_OneFriendData[] datas)
	{
		_friendsList = new List<UIFriends_OneFriend>();
		UIFriends_OneFriendData friendData = new UIFriends_OneFriendData();
		GameObject gameObject = Object.Instantiate(_oneFriendPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
		UIFriends_OneFriend component = gameObject.GetComponent<UIFriends_OneFriend>();
		component.FriendData = friendData;
		_friendsList.Add(component);
		if (datas != null)
		{
			for (int i = 0; i < datas.Length; i++)
			{
				GameObject gameObject2 = Object.Instantiate(_oneFriendPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
				UIFriends_OneFriend component2 = gameObject2.GetComponent<UIFriends_OneFriend>();
				component2.FriendData = datas[i];
				_friendsList.Add(component2);
			}
		}
		_friendNum.Text = (_friendsList.Count - 1).ToString();
		TUIScrollList_Avatar component3 = GetComponent<TUIScrollList_Avatar>();
		TUIClipBinder component4 = GetComponent<TUIClipBinder>();
		component3.Clear(true);
		for (int j = 0; j < _friendsList.Count; j++)
		{
			TUIControl component5 = _friendsList[j].GetComponent<TUIControl>();
			if (component5 == null)
			{
				Debug.LogError("Lack of TUIControl component!");
			}
			component3.Add(component5);
		}
		component3.ScrollListTo(0f);
		component4.SetClipRect();
		return 0;
	}

	public int AddFriend(UIFriends_OneFriendData data)
	{
		if (data == null)
		{
			return -1;
		}
		GameObject gameObject = Object.Instantiate(_oneFriendPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
		UIFriends_OneFriend component = gameObject.GetComponent<UIFriends_OneFriend>();
		component.FriendData = data;
		_friendsList.Insert(1, component);
		_friendNum.Text = (_friendsList.Count - 1).ToString();
		TUIScrollList_Avatar component2 = GetComponent<TUIScrollList_Avatar>();
		component2.Insert(1, component.GetComponent<TUIControl>());
		component2.ScrollListTo(0f);
		TUIClipBinder component3 = GetComponent<TUIClipBinder>();
		component3.SetClipRect();
		return 0;
	}

	public int DelFriend(UIFriends_OneFriend one)
	{
		int num = _friendsList.FindIndex((UIFriends_OneFriend @object) => @object == one);
		if (num >= 1 && num < _friendsList.Count)
		{
			return DelFriend(num);
		}
		return -1;
	}

	private int DelFriend(int nIndex)
	{
		Debug.Log("----Del Friend:" + nIndex);
		_friendsList.RemoveAt(nIndex);
		_friendNum.Text = (_friendsList.Count - 1).ToString();
		TUIScrollList_Avatar component = GetComponent<TUIScrollList_Avatar>();
		component.Remove(nIndex, true);
		TUIClipBinder component2 = GetComponent<TUIClipBinder>();
		component2.SetClipRect();
		return 0;
	}
}
