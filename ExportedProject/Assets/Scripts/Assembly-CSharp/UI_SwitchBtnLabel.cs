using UnityEngine;

public class UI_SwitchBtnLabel : MonoBehaviour
{
	[SerializeField]
	private TUILabel _switchLabel;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetLabelTxt(string strTxt, string strId)
	{
		if (_switchLabel != null)
		{
			if (strId == string.Empty)
			{
				_switchLabel.Text = strTxt;
			}
			else
			{
				_switchLabel.TextID = strId;
			}
		}
	}
}
