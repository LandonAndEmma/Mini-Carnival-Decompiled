using UnityEngine;

[AddComponentMenu("FingerGestures/Gesture Recognizers/Long Press")]
public class LongPressGestureRecognizer : AveragedGestureRecognizer
{
	public float Duration = 1f;

	public float MoveTolerance = 5f;

	private float startTime;

	public float StartTime
	{
		get
		{
			return startTime;
		}
	}

	public event EventDelegate<LongPressGestureRecognizer> OnLongPress;

	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
		startTime = Time.time;
	}

	protected override GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != RequiredFingerCount)
		{
			return GestureState.Failed;
		}
		float num = Time.time - startTime;
		if (num >= Duration)
		{
			RaiseOnLongPress();
			return GestureState.Recognized;
		}
		if (touches.GetAverageDistanceFromStart() > MoveTolerance)
		{
			return GestureState.Failed;
		}
		return GestureState.InProgress;
	}

	protected void RaiseOnLongPress()
	{
		if (this.OnLongPress != null)
		{
			this.OnLongPress(this);
		}
	}
}
