using UnityEngine;

public class UIMoveController : IgnoreTimeScale
{
	public enum DragEffect
	{
		None = 0,
		Momentum = 1,
		MomentumAndSpring = 2
	}

	public OnDelegateJoyStick _dele;

	public OnDelegateJoyStickTo _joyStick;

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

	private bool bDrag;

	public Transform cmrTrs;

	private void Start()
	{
		cmrTrs = GameObject.Find("Main Camera").transform;
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
		if (!pressed && !bDrag)
		{
			Ray ray = cmrTrs.camera.ScreenPointToRay(UICamera.currentTouch.pos);
			int layerMask = 1 << LayerMask.NameToLayer("Ground");
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f, layerMask) && hitInfo.collider.name == "floor_01")
			{
				Debug.Log(hitInfo.point);
				_joyStick(0f, 0f, hitInfo.point);
			}
		}
		if (!pressed)
		{
			bDrag = false;
		}
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
		if (mPressed && base.enabled && NGUITools.GetActive(base.gameObject) && (delta.x != 0f || delta.y != 0f) && _dele != null)
		{
			bDrag = true;
			_dele(delta.x, delta.y);
		}
	}
}
