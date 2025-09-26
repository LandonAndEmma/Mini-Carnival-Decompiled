using UnityEngine;

public class UIRPGDragOrPress : IgnoreTimeScale
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
		if (pressed)
		{
			Ray ray = cmrTrs.camera.ScreenPointToRay(UICamera.currentTouch.pos);
			int layerMask = 1 << LayerMask.NameToLayer("Player");
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f, layerMask))
			{
				RPGEntity component = hitInfo.collider.transform.GetComponent<RPGEntity>();
				if (component != null && component.CurHp > 0f)
				{
					RPGEntity curBoutingEntity = RPGRefree.Instance.GetCurBoutingEntity();
					if (curBoutingEntity != null && curBoutingEntity.TeamOwner != component.TeamOwner)
					{
						TMessageDispatcher.Instance.DispatchMsg(-1, curBoutingEntity.GetInstanceID(), 5016, TTelegram.SEND_MSG_IMMEDIATELY, component);
					}
				}
			}
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
			_dele(delta.x, delta.y);
		}
	}
}
