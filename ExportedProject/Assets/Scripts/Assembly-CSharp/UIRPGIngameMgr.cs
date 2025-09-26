using System.Collections.Generic;
using UnityEngine;

public class UIRPGIngameMgr : MonoBehaviour
{
	private static UIRPGIngameMgr _instance;

	[SerializeField]
	private UIRPGRoundMgr _roundMgr;

	[SerializeField]
	private UIRPGATTRankingMgr _attRankingMgr;

	public static UIRPGIngameMgr Instance
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

	public void NotifyRoundChanged(int nRound)
	{
		_roundMgr.RoundChange(nRound);
	}

	public void RefreshATTRanking(List<RPGEntity> battleRoleList)
	{
		_attRankingMgr.RefreshATTRanking(battleRoleList);
	}

	public void JumpToATTPos()
	{
		_attRankingMgr.JumpToATTPos();
	}

	public void RefreshATTRankingFromCur()
	{
		_attRankingMgr.RefreshATTRankingFromCur();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
