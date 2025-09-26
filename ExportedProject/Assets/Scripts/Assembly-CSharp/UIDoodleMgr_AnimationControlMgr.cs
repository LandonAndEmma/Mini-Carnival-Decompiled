using UnityEngine;

public class UIDoodleMgr_AnimationControlMgr : UI_AnimationControlMgr
{
	[SerializeField]
	private UIDoodleMgr _doodleMgr;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void EnterEnd()
	{
		base.SceneBlock.m_bEnable = false;
		if (_doodleMgr != null)
		{
			_doodleMgr.ProcessEnterEnd();
		}
	}

	public override void ExitEnd()
	{
		base.ExitEnd();
	}
}
