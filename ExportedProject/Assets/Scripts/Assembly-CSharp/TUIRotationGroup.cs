using UnityEngine;

public class TUIRotationGroup : TUIControlImpl
{
	public static int CommandDown;

	public static int CommandMove = 1;

	public static int CommandUp = 2;

	protected Vector2 moveSpeed;

	protected int fingerId = -1;

	protected Vector2 fingerPosition = Vector2.zero;

	protected bool touch;

	protected bool move;

	protected bool scroll;

	protected Vector2 lastPosition = Vector2.zero;

	public override bool HandleInput(TUIInput input)
	{
		bool result = true;
		switch (input.inputType)
		{
		case TUIInputType.Began:
			result = HandleInputBegan(input);
			break;
		case TUIInputType.Moved:
			result = HandleInputMoved(input);
			break;
		case TUIInputType.Ended:
			result = HandleInputEnded(input);
			break;
		}
		base.HandleInput(input);
		return result;
	}

	private bool HandleInputBegan(TUIInput input)
	{
		if (PtInControl(input.position))
		{
			fingerId = input.fingerId;
			fingerPosition = input.position;
			touch = true;
			move = false;
			scroll = false;
			lastPosition = fingerPosition;
			moveSpeed = Vector2.zero;
			PostEvent(this, CommandDown, 0f, 0f, input);
		}
		return false;
	}

	private bool HandleInputMoved(TUIInput input)
	{
		if (input.fingerId != fingerId)
		{
			return false;
		}
		float wparam = input.position.x - fingerPosition.x;
		float num = input.position.y - fingerPosition.y;
		PostEvent(this, CommandMove, wparam, 0f, input);
		fingerPosition = input.position;
		return true;
	}

	private bool HandleInputEnded(TUIInput input)
	{
		bool result = false;
		if (touch)
		{
			float wparam = ((input.position.x - lastPosition.x > 10f) ? 1 : 0);
			float num = Mathf.Abs(input.position.x - lastPosition.x) / 100f;
			num = ((!(num <= 1f)) ? num : 1f);
			Debug.Log("Slot----> " + num);
			PostEvent(this, CommandUp, wparam, num, input);
			result = true;
		}
		fingerId = -1;
		fingerPosition = Vector2.zero;
		touch = false;
		move = false;
		lastPosition = Vector2.zero;
		return result;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
