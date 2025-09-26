using System.Collections.Generic;
using UnityEngine;

public class UIInGame_DirTagMgr : MonoBehaviour
{
	private static UIInGame_DirTagMgr _instance;

	[SerializeField]
	private GameObject _tagPrefab;

	[SerializeField]
	private Transform _playerSelf;

	[SerializeField]
	private Vector2 _displayArea;

	[SerializeField]
	private Dictionary<int, TagObjectData> _mapOtherPlayers = new Dictionary<int, TagObjectData>();

	private int _nextValueId;

	private bool _bEnableTest;

	[SerializeField]
	private int _operN;

	[SerializeField]
	private bool _bTest;

	[SerializeField]
	private Transform[] _testRoles;

	public static UIInGame_DirTagMgr Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		TagObjectData._thresholdAngree = Mathf.Atan(_displayArea.x / _displayArea.y) * 57.29578f;
		TagObjectData._area = _displayArea;
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	private void Start()
	{
		if (_bEnableTest)
		{
			Transform[] testRoles = _testRoles;
			foreach (Transform trans in testRoles)
			{
				AddTagInfo(trans);
			}
		}
	}

	private void Update()
	{
		if (_bEnableTest && _bTest)
		{
			HideTagInfo(_operN);
			_bTest = false;
		}
		if (_playerSelf == null)
		{
			return;
		}
		Quaternion rotation = _playerSelf.rotation;
		Vector3 forward = _playerSelf.forward;
		int num = 0;
		int count = _mapOtherPlayers.Count;
		foreach (KeyValuePair<int, TagObjectData> mapOtherPlayer in _mapOtherPlayers)
		{
			mapOtherPlayer.Value.Dis = (int)Vector3.Distance(_playerSelf.position, mapOtherPlayer.Value.Pos);
			mapOtherPlayer.Value.DegreesWithRole = Vector3.Angle(forward, mapOtherPlayer.Value.Pos - _playerSelf.position);
			Quaternion quaternion = Quaternion.FromToRotation(forward, mapOtherPlayer.Value.Pos - _playerSelf.position);
			if (_playerSelf.worldToLocalMatrix.MultiplyPoint3x4(mapOtherPlayer.Value.Pos).x > 0f)
			{
				mapOtherPlayer.Value.DegreesWithRole *= -1f;
			}
			mapOtherPlayer.Value.RefreshUI();
			num++;
		}
	}

	public void SetPlayerSelf(Transform playerSelf)
	{
		_playerSelf = playerSelf;
	}

	public int AddTagInfo(Transform trans)
	{
		TagObjectData tagObjectData = new TagObjectData(trans);
		tagObjectData.TagObj = Object.Instantiate(_tagPrefab) as GameObject;
		tagObjectData.TagObj.SetActive(false);
		tagObjectData.TagObj.transform.parent = base.transform;
		tagObjectData.TagObj.transform.localPosition = new Vector3(0f, 0f, 0f);
		tagObjectData.Id = _nextValueId;
		_mapOtherPlayers.Add(tagObjectData.Id, tagObjectData);
		return _nextValueId++;
	}

	public void DelTagInfo(int nId)
	{
		if (_mapOtherPlayers.ContainsKey(nId))
		{
			TagObjectData tagObjectData = _mapOtherPlayers[nId];
			Object.Destroy(tagObjectData.TagObj);
			_mapOtherPlayers.Remove(nId);
		}
	}

	public void HideTagInfo(int nId)
	{
		if (_mapOtherPlayers.ContainsKey(nId))
		{
			TagObjectData tagObjectData = _mapOtherPlayers[nId];
			tagObjectData.TagObj.SetActive(false);
		}
	}

	public void HideAllTag()
	{
		foreach (KeyValuePair<int, TagObjectData> mapOtherPlayer in _mapOtherPlayers)
		{
			mapOtherPlayer.Value.TagObj.SetActive(false);
		}
	}

	public void ShowTagInfo(int nId)
	{
		if (_mapOtherPlayers.ContainsKey(nId))
		{
			TagObjectData tagObjectData = _mapOtherPlayers[nId];
			tagObjectData.TagObj.SetActive(true);
		}
	}

	public void ShowAllTag()
	{
		foreach (KeyValuePair<int, TagObjectData> mapOtherPlayer in _mapOtherPlayers)
		{
			mapOtherPlayer.Value.TagObj.SetActive(true);
		}
	}
}
