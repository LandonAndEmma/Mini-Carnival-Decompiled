using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRPGATTRankingMgr : MonoBehaviour
{
	[SerializeField]
	private UIRPGATTRanking[] _attRaningArray;

	[SerializeField]
	private Animation _moveForwardAni;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RefreshATTRanking(List<RPGEntity> battleRoleList)
	{
		for (int i = 0; i < _attRaningArray.Length; i++)
		{
			_attRaningArray[i].RefreshUI((i >= battleRoleList.Count) ? null : battleRoleList[i]);
		}
	}

	public void RefreshATTRankingFromCur()
	{
		int num = RPGRefree.Instance.CurAttackIndex + 1;
		for (int i = 0; i < _attRaningArray.Length; i++)
		{
			if (num + i < RPGRefree.Instance.BattleRoleList.Count)
			{
				_attRaningArray[i].RefreshUI(RPGRefree.Instance.BattleRoleList[num + i]);
			}
			else
			{
				_attRaningArray[i].RefreshUI(null);
			}
		}
	}

	public void JumpToATTPos()
	{
		_attRaningArray[0].animation.Play("UI_RPGJumpToATTPos");
	}

	private IEnumerator ResetPos0()
	{
		int nStartIndex = RPGRefree.Instance.CurAttackIndex + 1;
		yield return new WaitForEndOfFrame();
		_moveForwardAni.Play();
		_attRaningArray[0].ResetPos();
		for (int i = 0; i < _attRaningArray.Length - 1; i++)
		{
			if (nStartIndex + i < RPGRefree.Instance.BattleRoleList.Count)
			{
				_attRaningArray[i].RefreshUI(RPGRefree.Instance.BattleRoleList[nStartIndex + i]);
			}
			else
			{
				_attRaningArray[i].RefreshUI(null);
			}
		}
		_attRaningArray[_attRaningArray.Length - 1].RefreshUI(null);
		if (nStartIndex + _attRaningArray.Length - 1 < RPGRefree.Instance.BattleRoleList.Count)
		{
			_attRaningArray[_attRaningArray.Length - 1].RefreshUI(RPGRefree.Instance.BattleRoleList[nStartIndex + _attRaningArray.Length - 1]);
			_attRaningArray[_attRaningArray.Length - 1].animation.Play("UI_RPGJumpToEndPos");
		}
	}

	public void JumpEnd()
	{
		StartCoroutine(ResetPos0());
	}
}
