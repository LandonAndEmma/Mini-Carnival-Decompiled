using UnityEngine;

public class UIMessage_CommonBox : UIMessage_Box
{
	[SerializeField]
	private GameObject _onlyYes;

	[SerializeField]
	private GameObject _yesAndNo;

	[SerializeField]
	private GameObject _rateAndLater;

	[SerializeField]
	private UILabel _onlyYesLabelYes;

	[SerializeField]
	private UILabel _yesAndNoLabelYes;

	[SerializeField]
	private UILabel _yesAndNoLabelNo;

	[SerializeField]
	private UILabel _labelDes;

	public override void FormatBoxName(int i)
	{
	}

	public override void BoxDataChanged()
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = base.BoxData as UIMessage_CommonBoxData;
		if (uIMessage_CommonBoxData != null)
		{
			_onlyYes.SetActive(false);
			_yesAndNo.SetActive(false);
			_rateAndLater.SetActive(false);
			if (uIMessage_CommonBoxData.Layout == 1)
			{
				_onlyYes.SetActive(true);
			}
			else if (uIMessage_CommonBoxData.Layout == 0)
			{
				_yesAndNo.SetActive(true);
			}
			else if (uIMessage_CommonBoxData.Layout == 2)
			{
				_rateAndLater.SetActive(true);
			}
			_labelDes.text = uIMessage_CommonBoxData.Description;
			if (_labelDes.text.Length > 32)
			{
				_labelDes.pivot = UIWidget.Pivot.Left;
			}
			else
			{
				_labelDes.pivot = UIWidget.Pivot.Center;
			}
			if (uIMessage_CommonBoxData.OnlyYesLabelYes != string.Empty)
			{
				_onlyYesLabelYes.text = uIMessage_CommonBoxData.OnlyYesLabelYes;
			}
			if (uIMessage_CommonBoxData.YesAndNoLabelYes != string.Empty)
			{
				_yesAndNoLabelYes.text = uIMessage_CommonBoxData.YesAndNoLabelYes;
			}
			if (uIMessage_CommonBoxData.YesAndNoLabelNo != string.Empty)
			{
				_yesAndNoLabelNo.text = uIMessage_CommonBoxData.YesAndNoLabelNo;
			}
		}
	}
}
