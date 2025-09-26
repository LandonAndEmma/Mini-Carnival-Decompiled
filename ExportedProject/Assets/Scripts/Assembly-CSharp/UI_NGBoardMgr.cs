using UnityEngine;

public class UI_NGBoardMgr : UIEntity
{
	[SerializeField]
	protected GameObject[] _ngBoards;

	[SerializeField]
	protected COMA_PlayerSelfCharacter _actorCmp;

	[SerializeField]
	protected GameObject _objNG;

	[SerializeField]
	protected UISprite _spriteBlockBG;

	protected override void Load()
	{
	}

	protected override void UnLoad()
	{
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected void OpenBoard(int nIndex)
	{
		for (int i = 0; i < _ngBoards.Length; i++)
		{
			_ngBoards[i].SetActive((i == nIndex) ? true : false);
		}
	}

	protected void CloseAllBoard()
	{
		for (int i = 0; i < _ngBoards.Length; i++)
		{
			_ngBoards[i].SetActive(false);
		}
	}
}
