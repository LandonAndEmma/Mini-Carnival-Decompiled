using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Tap")]
public class TBTap : TBComponent
{
	public int tapCount = 1;

	public Message message = new Message("OnTap");

	public event EventHandler<TBTap> OnTap;

	public bool RaiseTap(int fingerIndex, Vector2 fingerPos, int tapCount)
	{
		if (tapCount != this.tapCount)
		{
			return false;
		}
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		if (this.OnTap != null)
		{
			this.OnTap(this);
		}
		Send(message);
		return true;
	}
}
