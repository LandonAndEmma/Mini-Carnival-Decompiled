public class UIMessage_FacebookBox : UIMessage_Box
{
	public override void FormatBoxName(int i)
	{
	}

	public override void BoxDataChanged()
	{
		UILoginFacebookMessageBoxData uILoginFacebookMessageBoxData = base.BoxData as UILoginFacebookMessageBoxData;
		if (uILoginFacebookMessageBoxData != null)
		{
		}
	}
}
