using System.Collections.Generic;
using UnityEngine;

public class UIRankingList_RankingContainer : MonoBehaviour
{
	[SerializeField]
	private GameObject _worldRaningPrefab;

	[SerializeField]
	private GameObject _friendRaningPrefab;

	private int _nMaxRaningCount = 50;

	private List<UIRankingList_WorldList> _worldRaningList;

	private List<UIRankingList_FriendList> _friendRaningList;

	[SerializeField]
	private UIRankingList_WorldList _selfWorldRanking;

	[SerializeField]
	private Transform _transBK;

	private void Awake()
	{
		_selfWorldRanking.gameObject.SetActive(false);
	}

	private void Update()
	{
	}

	public void InitRaningContainer(int nCurGameMode, int nCurRaningMode, ref UIRankingList_RankingData[] datas)
	{
		switch (nCurRaningMode)
		{
		case 0:
			InitWorldRaning(ref datas);
			break;
		case 1:
			InitFriendRaning(ref datas);
			break;
		}
	}

	private void InitWorldRaning(ref UIRankingList_RankingData[] datas)
	{
		TUIScrollList_Avatar component = GetComponent<TUIScrollList_Avatar>();
		TUIClipBinder component2 = GetComponent<TUIClipBinder>();
		base.transform.localPosition = new Vector3(90f, -39.5f, -100f);
		component.size = new Vector2(256f, 146f);
		component.real_size = new Vector2(256f, 146f);
		component2.ClipRect.SetSize(new Vector2(256f, 146f));
		_transBK.localScale = new Vector3(16f, 9.125f, 1f);
		component.spacing = 0f;
		_worldRaningList = new List<UIRankingList_WorldList>();
		for (int i = 0; i < _nMaxRaningCount; i++)
		{
			GameObject gameObject = Object.Instantiate(_worldRaningPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
			UIRankingList_WorldList component3 = gameObject.GetComponent<UIRankingList_WorldList>();
			if (i >= datas.Length - 1)
			{
				component3.WorldRankingData = null;
			}
			else
			{
				component3.WorldRankingData = (UIRankingList_WorldRankingData)datas[i];
			}
			_worldRaningList.Add(component3);
		}
		_selfWorldRanking.gameObject.SetActive(true);
		UIRankingList_WorldRankingData worldRankingData = (UIRankingList_WorldRankingData)datas[datas.Length - 1];
		_selfWorldRanking.WorldRankingData = worldRankingData;
		component.Clear(true);
		for (int j = 0; j < _worldRaningList.Count; j++)
		{
			TUIControl component4 = _worldRaningList[j].GetComponent<TUIControl>();
			if (component4 == null)
			{
				Debug.LogError("Lack of TUIControl component!");
			}
			component.Add(component4);
		}
		component.ScrollListTo(0f);
		component2.SetClipRect();
	}

	private void InitFriendRaning(ref UIRankingList_RankingData[] datas)
	{
		Debug.Log("UIRankingList_RankingContainer----InitFriendRaning: datas Len=" + datas.Length);
		TUIScrollList_Avatar component = GetComponent<TUIScrollList_Avatar>();
		TUIClipBinder component2 = GetComponent<TUIClipBinder>();
		base.transform.localPosition = new Vector3(90f, -51.5f, -100f);
		component.size = new Vector2(256f, 170f);
		component.real_size = new Vector2(256f, 170f);
		component2.ClipRect.SetSize(new Vector2(256f, 170f));
		component.spacing = 0f;
		_transBK.localScale = new Vector3(16f, 10.625f, 1f);
		_friendRaningList = new List<UIRankingList_FriendList>();
		for (int i = 0; i < _nMaxRaningCount; i++)
		{
			GameObject gameObject = Object.Instantiate(_friendRaningPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
			UIRankingList_FriendList component3 = gameObject.GetComponent<UIRankingList_FriendList>();
			if (i >= datas.Length)
			{
				component3.FriendRankingData = null;
			}
			else
			{
				component3.FriendRankingData = (UIRankingList_FriendRankingData)datas[i];
			}
			_friendRaningList.Add(component3);
		}
		_selfWorldRanking.gameObject.SetActive(false);
		component.Clear(true);
		Debug.Log("scrollListCmp Pre cout:" + component.GetListCount() + "    _friendRaningList.Count:" + _friendRaningList.Count);
		for (int j = 0; j < _friendRaningList.Count; j++)
		{
			TUIControl component4 = _friendRaningList[j].GetComponent<TUIControl>();
			if (component4 == null)
			{
				Debug.LogError("Lack of TUIControl component!");
			}
			component.Add(component4);
		}
		Debug.Log("scrollListCmp cout:" + component.GetListCount());
		component.ScrollListTo(0f);
		component2.SetClipRect();
	}
}
