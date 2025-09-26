using UnityEngine;

public class CameraObserve1 : MonoBehaviour
{
	private Quaternion cmrNodeRotation = Quaternion.identity;

	[SerializeField]
	public Transform cmrTrs;

	private Vector3 localEuler = Vector3.zero;

	private Vector3 lastPos = Vector3.zero;

	private bool bScaling;

	private float lastDistance;

	private float distance;

	public Camera _camera;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		localEuler = cmrTrs.localEulerAngles;
		distance = Mathf.Abs(cmrTrs.localPosition.z);
	}

	private void Update()
	{
		if (!_camera.enabled)
		{
			cmrTrs.localPosition = new Vector3(0f, 0.8840484f, -2f);
			return;
		}
		TUIInput[] input = TUIInputManager.GetInput();
		if (input.Length >= 2)
		{
			if (!bScaling)
			{
				lastDistance = Vector2.Distance(input[0].position, input[1].position);
				bScaling = true;
			}
			else if (input[0].inputType == TUIInputType.Moved || input[1].inputType == TUIInputType.Moved)
			{
				float num = Vector2.Distance(input[0].position, input[1].position);
				float num2 = lastDistance / num;
				if (num2 > 1.05f)
				{
					num2 = 1.05f;
				}
				if (num2 < 0.95f)
				{
					num2 = 0.95f;
				}
				distance *= num2;
				if (distance < 1f)
				{
					distance = 1f;
				}
				if (distance > 5f)
				{
					distance = 5f;
				}
				lastDistance = num;
			}
		}
		else if (input.Length == 1)
		{
			if (input[0].inputType == TUIInputType.Moved)
			{
				RotateCamera(input[0].position);
			}
			lastPos = Input.mousePosition;
		}
		else
		{
			bScaling = false;
		}
		UpdateCamera();
	}

	private void RotateCamera(Vector2 point)
	{
		float y = (point.x - lastPos.x) * 314f / (float)Screen.width;
		cmrNodeRotation = Quaternion.Euler(0f, y, 0f);
		base.transform.rotation *= cmrNodeRotation;
		float num = (0f - (point.y - lastPos.y)) * 157f / (float)Screen.height;
		localEuler.x += num;
		if (localEuler.x > 80f)
		{
			localEuler.x = 80f;
		}
		if (localEuler.x < -80f)
		{
			localEuler.x = -80f;
		}
	}

	private void UpdateCamera()
	{
		cmrTrs.localPosition = new Vector3(0f, 0.8840484f, -1f * distance);
	}
}
