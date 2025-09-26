using UnityEngine;

public class UIFriends_FriendRequestMgr : MonoBehaviour
{
	[SerializeField]
	private TUIScrollList_Avatar _scrollListPending;

	[SerializeField]
	private TUIClipBinder clipBinderCmp;

	[SerializeField]
	private GameObject _frPrefab;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void InitPendingList()
	{
		UIFriends component = base.transform.root.GetComponent<UIFriends>();
		if (!(component != null))
		{
			return;
		}
		_scrollListPending.Clear(true);
		for (int i = 0; i < COMA_Server_Friends.Instance.lst_requires.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(_frPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
			UIFriends_FriendRequest component2 = gameObject.GetComponent<UIFriends_FriendRequest>();
			component2.FRData = COMA_Server_Friends.Instance.lst_requires[i];
			TUIControl component3 = component2.GetComponent<TUIControl>();
			if (component3 == null)
			{
				Debug.LogError("Lack of TUIControl component!");
			}
			_scrollListPending.Add(component3);
		}
		_scrollListPending.ScrollListTo(0f);
		clipBinderCmp.SetClipRect();
	}
}
