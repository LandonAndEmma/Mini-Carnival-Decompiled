using UnityEngine;

public class COMA_Tank_Camera : COMA_CameraMove
{
	private readonly Vector2 _vCameraRange = new Vector2(15f, 15f);

	public void setCameraRotTarget(Transform rotTarget)
	{
		targetRotTrs = rotTarget;
	}

	private bool isInCameraRangeX(Vector3 vPos)
	{
		float num = Mathf.Abs(vPos.x);
		Vector2 vCameraRange = _vCameraRange;
		return num < vCameraRange.x;
	}

	private bool isInCameraRangeZ(Vector3 vPos)
	{
		float num = Mathf.Abs(vPos.z);
		Vector2 vCameraRange = _vCameraRange;
		return num < vCameraRange.y;
	}

	protected new void LateUpdate()
	{
		if (targetPosTrs != null)
		{
			Vector3 position = targetPosTrs.position;
			if (Mathf.Abs(position.y) < 0.01f)
			{
				position.y = 0f;
			}
			Vector3 position2 = posTrs.position;
			posTrs.position = Vector3.Lerp(posTrs.position, position, 0.5f);
			if (!isInCameraRangeX(posTrs.position))
			{
				posTrs.position = new Vector3(position2.x, posTrs.position.y, posTrs.position.z);
			}
			if (!isInCameraRangeZ(posTrs.position))
			{
				posTrs.position = new Vector3(posTrs.position.x, posTrs.position.y, position2.z);
			}
			if (targetRotTrs != null)
			{
				rotTrs.rotation = Quaternion.Lerp(rotTrs.rotation, targetRotTrs.rotation, 0.5f);
			}
		}
	}
}
