using UnityEngine;

public class UIMainMenu_AnimationControlMgr : UI_AnimationControlMgr
{
	[SerializeField]
	private UIMainMenu _uiMainMenu;

	[SerializeField]
	private GameObject[] _inventoryScene;

	[SerializeField]
	private GameObject[] _mainScene;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void EnterEnd()
	{
		base.SceneBlock.m_bEnable = false;
		if (_uiMainMenu != null)
		{
			_uiMainMenu.ProcessEnterEnd();
		}
	}

	public override void ExitEnd()
	{
		base.ExitEnd();
	}
}
