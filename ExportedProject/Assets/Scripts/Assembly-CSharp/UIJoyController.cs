using UnityEngine;

public class UIJoyController : IgnoreTimeScale
{
	public enum DragEffect
	{
		None = 0,
		Momentum = 1,
		MomentumAndSpring = 2
	}

	public Transform target;

	public int moveR = 130;

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
		if (moveR < 1)
		{
			moveR = 1;
		}
	}

	private void FindPanel()
	{
		mPanel = ((!(target != null)) ? null : UIPanel.Find(target.transform, false));
		if (mPanel == null)
		{
			restrictWithinPanel = false;
		}
	}

	private void OnPress(bool pressed)
	{
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !(target != null))
		{
			return;
		}
		if (pressed)
		{
			if (!mPressed)
			{
				mTouchID = UICamera.currentTouchID;
				mMomentum = Vector3.zero;
				mPressed = true;
				mStarted = false;
				mScroll = 0f;
				if (restrictWithinPanel && mPanel == null)
				{
					FindPanel();
				}
				if (restrictWithinPanel)
				{
					mBounds = NGUIMath.CalculateRelativeWidgetBounds(mPanel.cachedTransform, target);
				}
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
				Transform transform = UICamera.currentCamera.transform;
				mPlane = new Plane(((!(mPanel != null)) ? transform.rotation : mPanel.cachedTransform.rotation) * Vector3.back, UICamera.lastHit.point);
			}
		}
		else if (mPressed && mTouchID == UICamera.currentTouchID)
		{
			mPressed = false;
			if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None && dragEffect == DragEffect.MomentumAndSpring)
			{
				mPanel.ConstrainTargetToBounds(target, ref mBounds, false);
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (!mPressed || !base.enabled || !NGUITools.GetActive(base.gameObject) || !(target != null))
		{
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		float enter = 0f;
		if (!mPlane.Raycast(ray, out enter))
		{
			return;
		}
		Vector3 point = ray.GetPoint(enter);
		Vector3 vector = point - mLastPos;
		mLastPos = point;
		if (!mStarted)
		{
			mStarted = true;
			vector = Vector3.zero;
		}
		if (vector.x != 0f || vector.y != 0f)
		{
			vector = target.InverseTransformDirection(vector);
			vector.Scale(scale);
			vector = target.TransformDirection(vector);
		}
		if (dragEffect != DragEffect.None)
		{
			mMomentum = Vector3.Lerp(mMomentum, mMomentum + vector * (0.01f * momentumAmount), 0.67f);
		}
		if (restrictWithinPanel)
		{
			Vector3 localPosition = target.localPosition;
			target.position += vector;
			mBounds.center += target.localPosition - localPosition;
			if (dragEffect != DragEffect.MomentumAndSpring && mPanel.clipping != UIDrawCall.Clipping.None && mPanel.ConstrainTargetToBounds(target, ref mBounds, true))
			{
				mMomentum = Vector3.zero;
				mScroll = 0f;
			}
		}
		else
		{
			target.position += vector;
		}
	}

	private void LateUpdate()
	{
		float deltaTime = UpdateRealTimeDelta();
		if (target == null)
		{
			return;
		}
		if (mPressed)
		{
			SpringPosition component = target.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
			mScroll = 0f;
			float num = Vector3.Distance(target.localPosition, Vector3.zero);
			if (num > 0.1f)
			{
				num = (float)moveR / num;
				if (num < 1f)
				{
					target.localPosition *= num;
				}
			}
			if (_dele != null)
			{
				float x = target.localPosition.x / (float)moveR;
				float y = target.localPosition.y / (float)moveR;
				_dele(x, y);
			}
		}
		else
		{
			target.localPosition = Vector3.zero;
			if (_dele != null)
			{
				_dele(0f, 0f);
			}
		}
		NGUIMath.SpringDampen(ref mMomentum, 9f, deltaTime);
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Mathf.Sign(mScroll) != Mathf.Sign(delta))
			{
				mScroll = 0f;
			}
			mScroll += delta * scrollWheelFactor;
		}
	}
}
