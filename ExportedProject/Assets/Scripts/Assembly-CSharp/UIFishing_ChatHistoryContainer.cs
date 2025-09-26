using System.Collections.Generic;
using UnityEngine;

public class UIFishing_ChatHistoryContainer : MonoBehaviour
{
	[SerializeField]
	private GameObject _oneWordsPrefab;

	[SerializeField]
	private GameObject _oneWordsPrefabIn;

	[SerializeField]
	private GameObject _oneWordsPrefabOut;

	private List<ChatHistoryInfo> _queueHistory = new List<ChatHistoryInfo>();

	public int _nType;

	private void Awake()
	{
		if (_nType == 1)
		{
			_oneWordsPrefab = _oneWordsPrefabOut;
		}
	}

	private void Start()
	{
		_queueHistory.Clear();
	}

	public void InitScrollList(Vector2 newSize, float newY)
	{
		TUIScrollList_Avatar component = GetComponent<TUIScrollList_Avatar>();
		TUIClipBinder component2 = GetComponent<TUIClipBinder>();
		component.size = newSize;
		component.real_size = newSize;
		component2.ClipRect.Size = newSize;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = newY;
		base.transform.localPosition = localPosition;
		component.ScrollListTo(1f);
		component2.SetClipRect();
	}

	public void InitScrollList()
	{
		TUIScrollList_Avatar component = GetComponent<TUIScrollList_Avatar>();
		TUIClipBinder component2 = GetComponent<TUIClipBinder>();
		component.ScrollListTo(1f);
		component2.SetClipRect();
	}

	public int AddOneWords(string name, string words, string id)
	{
		int num = 50;
		if (_queueHistory.Count >= num)
		{
			_queueHistory.RemoveAt(0);
		}
		_queueHistory.Add(new ChatHistoryInfo(name, words, id));
		TUIScrollList_Avatar component = GetComponent<TUIScrollList_Avatar>();
		if (component.GetListCount() >= num)
		{
			List<TUIControl> tUIControlONListObjsNoSort = component.GetTUIControlONListObjsNoSort(true);
			for (int i = 0; i < num; i++)
			{
				UIFishing_ChatOneWords component2 = tUIControlONListObjsNoSort[i].GetComponent<UIFishing_ChatOneWords>();
				if (component2 != null)
				{
					component2._strOwerId = _queueHistory[i]._strID;
					component2.Name = _queueHistory[i]._strName;
					component2.Words = _queueHistory[i]._strWords;
				}
			}
		}
		else
		{
			GameObject gameObject = Object.Instantiate(_oneWordsPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
			UIFishing_ChatOneWords component3 = gameObject.GetComponent<UIFishing_ChatOneWords>();
			component3._strOwerId = id;
			component3.Name = name;
			component3.Words = words;
			component.Add(gameObject.GetComponent<TUIControl>());
		}
		RefreshScrollList();
		Debug.Log(">>>>>>>>>>>AddOneWords END");
		return 0;
	}

	public void RefreshScrollList()
	{
		TUIScrollList_Avatar component = GetComponent<TUIScrollList_Avatar>();
		component.ScrollListTo(1f);
		TUIClipBinder component2 = GetComponent<TUIClipBinder>();
		component2.SetClipRect();
	}

	private void Update()
	{
	}
}
