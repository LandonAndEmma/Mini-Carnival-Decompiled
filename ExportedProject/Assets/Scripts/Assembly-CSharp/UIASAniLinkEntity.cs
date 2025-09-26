using MessageID;
using UnityEngine;

public class UIASAniLinkEntity : UIEntity
{
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

	public void AniEnterLinkEnd()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ASAniEnterEnd, this, null);
	}

	public void AniExitLinkEnd()
	{
		Debug.Log("AniExitLinkEnd------------" + base.gameObject.name + " Parent:" + base.gameObject.transform.parent.name);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ASAniExitEnd, this, null);
	}
}
