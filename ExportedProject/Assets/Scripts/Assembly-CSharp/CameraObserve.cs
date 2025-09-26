using UnityEngine;

public class CameraObserve : MonoBehaviour
{
	public BodyPaint bodyPaint;

	private bool bPainting;

	private Vector3 lastPos = Vector3.zero;

	private Quaternion cmrNodeRotation = Quaternion.identity;

	private Transform cmrTrs;

	private Vector3 localEuler = Vector3.zero;

	private float distance;

	private bool bScaling;

	private float lastDistance;

	private void Start()
	{
		cmrTrs = base.transform.GetChild(0);
		localEuler = cmrTrs.localEulerAngles;
		distance = cmrTrs.localPosition.magnitude;
	}

	private void Update()
	{
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
			if (bodyPaint == null)
			{
				if (input[0].inputType == TUIInputType.Moved)
				{
					RotateCamera(input[0].position);
				}
				lastPos = Input.mousePosition;
			}
			else
			{
				Vector2 vector = new Vector2(input[0].position.x, input[0].position.y);
				if (input[0].inputType == TUIInputType.Began)
				{
					Ray ray = cmrTrs.camera.ScreenPointToRay(new Vector3(vector.x, vector.y));
					RaycastHit hitInfo;
					if (bodyPaint.paintTarget.Raycast(ray, out hitInfo, 100f))
					{
						bPainting = true;
						bodyPaint.PaintStart(hitInfo.textureCoord);
					}
				}
				else if (input[0].inputType == TUIInputType.Ended && bPainting)
				{
					bPainting = false;
					bodyPaint.PaintEnd();
				}
				if (bPainting)
				{
					Ray ray2 = cmrTrs.camera.ScreenPointToRay(new Vector3(vector.x, vector.y));
					RaycastHit hitInfo2;
					if (bodyPaint.paintTarget.Raycast(ray2, out hitInfo2, 100f))
					{
						bodyPaint.Paint(hitInfo2.textureCoord);
					}
				}
				else
				{
					if (input[0].inputType == TUIInputType.Moved)
					{
						RotateCamera(input[0].position);
					}
					lastPos = Input.mousePosition;
				}
			}
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
		Quaternion quaternion = Quaternion.Euler(localEuler);
		cmrTrs.localRotation = Quaternion.Lerp(cmrTrs.localRotation, quaternion, 0.3f);
		Vector3 to = quaternion * Vector3.back * distance;
		cmrTrs.localPosition = Vector3.Lerp(cmrTrs.localPosition, to, 0.3f);
	}
}
