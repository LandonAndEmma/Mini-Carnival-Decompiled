using UnityEngine;

public class MouseGestures : FingerGestures
{
	public int maxMouseButtons = 3;

	public override int MaxFingers
	{
		get
		{
			return maxMouseButtons;
		}
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override FingerPhase GetPhase(Finger finger)
	{
		int index = finger.Index;
		if (Input.GetMouseButton(index))
		{
			if (Input.GetMouseButtonDown(index))
			{
				return FingerPhase.Began;
			}
			if (((Vector3)(GetPosition(finger) - finger.Position)).sqrMagnitude < 1f)
			{
				return FingerPhase.Stationary;
			}
			return FingerPhase.Moved;
		}
		if (Input.GetMouseButtonUp(index))
		{
			return FingerPhase.Ended;
		}
		return FingerPhase.None;
	}

	protected override Vector2 GetPosition(Finger finger)
	{
		return Input.mousePosition;
	}
}
