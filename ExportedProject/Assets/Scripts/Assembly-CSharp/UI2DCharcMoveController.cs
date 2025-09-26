using MessageID;
using UnityEngine;

public class UI2DCharcMoveController : IgnoreTimeScale
{
	public enum DragEffect
	{
		None = 0,
		Momentum = 1,
		MomentumAndSpring = 2
	}

	public OnDelegateJoyStick _dele;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public bool restrictWithinPanel;

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	private Plane mPlane;

	private Vector3 mLastPos;

	private UIPanel mPanel;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private int mTouchID;

	private bool mStarted;

	private void Start()
	{
	}

	private void FindPanel()
	{
		mPanel = null;
		if (mPanel == null)
		{
			restrictWithinPanel = false;
		}
	}

	private void OnPress(bool pressed)
	{
		if (!base.enabled || !NGUITools.GetActive(base.gameObject))
		{
			return;
		}
		if (pressed)
		{
			if (!mPressed)
			{
				mPressed = true;
			}
		}
		else if (mPressed && mTouchID == UICamera.currentTouchID)
		{
			mPressed = false;
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (mPressed && base.enabled && NGUITools.GetActive(base.gameObject) && (delta.x != 0f || delta.y != 0f))
		{
			if (_dele != null)
			{
				_dele(delta.x, delta.y);
			}
			if (delta.x != 0f)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Notify2DCharc, null, UI2DCharcMgr.EOperType.Rotate, delta.x);
			}
		}
	}
}
