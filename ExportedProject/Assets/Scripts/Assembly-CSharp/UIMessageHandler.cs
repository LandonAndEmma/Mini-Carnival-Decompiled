using UnityEngine;

public class UIMessageHandler : UICOM
{
	[SerializeField]
	protected TUIFade _fadeMgr;

	[SerializeField]
	protected TUILabel _goldLabel;

	[SerializeField]
	protected TUILabel _gemLabel;

	public UI_AnimationControl _aniControl;

	public void RefreshGoldAndCrystal()
	{
		if (_goldLabel != null)
		{
			_goldLabel.Text = COMA_Pref.Instance.GetGold().ToString();
		}
		if (_gemLabel != null)
		{
			_gemLabel.Text = COMA_Pref.Instance.GetCrystal().ToString();
		}
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}
}
