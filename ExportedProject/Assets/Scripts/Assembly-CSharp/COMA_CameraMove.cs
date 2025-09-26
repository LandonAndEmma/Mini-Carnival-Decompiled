using UnityEngine;

public class COMA_CameraMove : COMA_Camera
{
	private Vector3 defaultLocalPosition = Vector3.zero;

	public Transform anchorTrs;

	private float defaultDistance;

	private Quaternion defaultLocalRotation = Quaternion.identity;

	private Quaternion confusedRotation = new Quaternion(0f, 0f, 1f, 0f);

	private void OnEnable()
	{
		if (COMA_Camera._instance == null)
		{
			COMA_Camera._instance = this;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnDisable()
	{
		COMA_Camera._instance = null;
	}

	private void Start()
	{
		if (targetPosTrs != null)
		{
			CameraInit(targetPosTrs);
		}
		posTrs = base.transform;
		rotTrs = base.transform.FindChild("CameraRot");
		defaultLocalPosition = cmr.transform.localPosition;
		anchorTrs.LookAt(cmr.transform);
		defaultDistance = (defaultLocalPosition - anchorTrs.localPosition).magnitude;
		defaultLocalRotation = cmr.transform.localRotation;
	}

	public override void CameraInit(Transform targetTrs)
	{
		base.CameraInit(targetTrs);
		base.transform.position = targetPosTrs.position;
	}

	protected void LateUpdate()
	{
		if (targetPosTrs != null)
		{
			Vector3 position = targetPosTrs.position;
			if (Mathf.Abs(position.y) < 0.01f)
			{
				position.y = 0f;
			}
			posTrs.position = Vector3.Lerp(posTrs.position, position, 0.5f);
			if (targetRotTrs != null)
			{
				rotTrs.rotation = Quaternion.Lerp(rotTrs.rotation, targetRotTrs.rotation, 0.5f);
			}
		}
		Ray ray = new Ray(anchorTrs.position, anchorTrs.forward);
		int layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));
		RaycastHit hitInfo;
		Vector3 localPosition = ((!Physics.Raycast(ray, out hitInfo, defaultDistance, layerMask) || (!(targetRotTrs != null) && !(Application.loadedLevelName == "COMA_Scene_WaitingRoom") && !(Application.loadedLevelName == "UI.Square"))) ? defaultLocalPosition : rotTrs.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point - anchorTrs.forward * 0.2f));
		cmr.transform.localPosition = localPosition;
		if (COMA_PlayerSelf.Instance != null)
		{
			if (COMA_PlayerSelf.Instance.IsConfused)
			{
				cmr.transform.localRotation = Quaternion.Lerp(cmr.transform.localRotation, defaultLocalRotation * confusedRotation, 0.2f);
			}
			else
			{
				cmr.transform.localRotation = Quaternion.Lerp(cmr.transform.localRotation, defaultLocalRotation, 0.2f);
			}
		}
	}
}
