using UnityEngine;

public class UISquare_ButtonCloseChatFriendSel : MonoBehaviour
{
	public GameObject tuiBlock;

	public GameObject _objOwner;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
		if (_objOwner != null)
		{
			_objOwner.SetActive(false);
		}
		else
		{
			base.transform.parent.gameObject.SetActive(false);
		}
		if (tuiBlock != null)
		{
			tuiBlock.SetActive(false);
		}
	}
}
