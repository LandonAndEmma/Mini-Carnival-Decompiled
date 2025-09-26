using MessageID;
using UnityEngine;

public class UI_ButtonAlternative : MonoBehaviour
{
	[SerializeField]
	private bool _fstBtn;

	[SerializeField]
	private GameObject _normal;

	[SerializeField]
	private GameObject _sel;

	public void SetFstBtn()
	{
		_fstBtn = true;
	}

	public void SetSelected(bool bSel)
	{
		_sel.SetActive(bSel);
		_normal.SetActive(!bSel);
	}

	private bool IsSelected()
	{
		return _sel.activeSelf;
	}

	private void Awake()
	{
		if (_normal == null)
		{
			_normal = base.transform.FindChild("Part_Normal").gameObject;
		}
		if (_sel == null)
		{
			_sel = base.transform.FindChild("Part_Select").gameObject;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (!IsSelected())
		{
			SetSelected(true);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ButtonAlternativeClick, null, _fstBtn);
		}
	}
}
