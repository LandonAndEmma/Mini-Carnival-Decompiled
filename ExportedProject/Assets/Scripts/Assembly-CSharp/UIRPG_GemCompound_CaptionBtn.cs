using UnityEngine;

public class UIRPG_GemCompound_CaptionBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _btnState;

	[SerializeField]
	private UIRPG_GemCompoundMgr _gemCompundMgr;

	[SerializeField]
	private UIRPG_GemCompoundMgr.ECaption _typeCaption;

	public UIRPG_GemCompoundMgr.ECaption TypeCaption
	{
		get
		{
			return _typeCaption;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetActiveBtn(bool bActive)
	{
		_btnState[0].SetActive(!bActive);
		_btnState[1].SetActive(bActive);
	}

	public void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		Debug.Log("OnClick()" + _typeCaption);
		_gemCompundMgr.HandleBtnClick(_typeCaption);
	}
}
