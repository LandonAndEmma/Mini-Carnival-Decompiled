using UnityEngine;

public class COMA_Camera : MonoBehaviour
{
	protected static COMA_Camera _instance;

	public Camera cmr;

	public Transform targetPosTrs;

	protected Transform targetRotTrs;

	protected Transform posTrs;

	protected Transform rotTrs;

	public GameObject cmrAnimObj;

	public static COMA_Camera Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		_instance = this;
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public virtual void CameraInit(Transform targetTrs)
	{
		targetPosTrs = targetTrs;
		targetRotTrs = targetPosTrs.FindChild("Character/Bip01/Spine_00");
	}

	public float SceneStart_CameraAnim()
	{
		return SceneStart_CameraAnim(true);
	}

	public float SceneStart_CameraAnim(bool needBreakParent)
	{
		float num = 0f;
		if (cmrAnimObj != null)
		{
			if (needBreakParent)
			{
				cmrAnimObj.transform.parent = null;
			}
			cmrAnimObj.animation.Play("CmrAnim_GameStart");
			num = cmrAnimObj.animation["CmrAnim_GameStart"].length;
			SceneTimerInstance.Instance.Add(num, CameraAnimationEnd);
		}
		return num;
	}

	public bool CameraAnimationEnd()
	{
		cmrAnimObj.SetActive(false);
		return false;
	}

	public void LockCameraToFlag(bool bLock)
	{
		if (bLock)
		{
			cmrAnimObj.transform.position = new Vector3(3.7f, 5.5f, 4.5f);
			cmrAnimObj.transform.eulerAngles = new Vector3(280f, 225f, 90f);
		}
		cmrAnimObj.SetActive(bLock);
	}
}
