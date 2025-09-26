using UnityEngine;

public class UIInput_AnimationControlMgr : UI_AnimationControlMgr
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void ExitEnd()
	{
		Object.DestroyObject(base.transform.root.gameObject);
	}
}
