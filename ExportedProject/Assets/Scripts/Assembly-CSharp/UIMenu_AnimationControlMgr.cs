using UnityEngine;

public class UIMenu_AnimationControlMgr : UI_AnimationControlMgr
{
	[SerializeField]
	private UIMarket _uiMarket;

	public int _nMenyLayer;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void EnterStart()
	{
		base.SceneBlock.m_bEnable = true;
		_uiMarket.ProcessUIEnter();
	}

	public override void EnterEnd()
	{
		base.SceneBlock.m_bEnable = false;
		_uiMarket.ProcessUIEnterEnd();
	}

	public override void ExitStart()
	{
		base.SceneBlock.m_bEnable = true;
		_uiMarket.CloseRefreshMgr();
	}

	public override void ExitEnd()
	{
		if (_nMenyLayer != 0)
		{
			if (_uiMarket.CurMenuLayer == 11 || _uiMarket.CurMenuLayer == 21 || _uiMarket.CurMenuLayer == 31 || _uiMarket.CurMenuLayer == 411 || _uiMarket.CurMenuLayer == 412 || _uiMarket.CurMenuLayer == 413)
			{
				_uiMarket.ExitMarketContainer();
			}
			_uiMarket.CurMenuLayer = _nMenyLayer;
		}
		base.SceneBlock.m_bEnable = false;
		_nMenyLayer = 0;
	}
}
