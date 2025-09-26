using System;
using UnityEngine;

public class COMA_DialogBox : MonoBehaviour
{
	public GameObject chattingBox;

	public Font3D contentCom;

	public Transform bgTrs;

	private GameObject cmrObj;

	private float cbPosY;

	public Font3D nickNameCom;

	[NonSerialized]
	public string playerNickName = string.Empty;

	private void Start()
	{
		cmrObj = GameObject.FindGameObjectWithTag("MainCamera");
		cbPosY = chattingBox.transform.localPosition.y;
		contentCom.SetFont("Chat");
		int num = contentCom.SetString(string.Empty, 0.05f, 0f, -0.3f, 1f, 2f / contentCom.transform.lossyScale.x, BoardType.Hold, AlignStyle.center);
		chattingBox.transform.localPosition = new Vector3(0f, cbPosY + (float)(num - 1) * 0.4f, 0f);
		bgTrs.localScale = new Vector3(1f, 1f, (float)num / 2f);
		if (nickNameCom != null)
		{
			nickNameCom.SetFont("1");
			nickNameCom.SetString(playerNickName, 0.05f, 0f, -0.3f, 1f, 2f / contentCom.transform.lossyScale.x, BoardType.Hold, AlignStyle.center);
		}
		Hide();
	}

	private void OnDestroy()
	{
		SceneTimerInstance.Instance.Remove(Hide);
	}

	private void Update()
	{
		if (cmrObj != null)
		{
			base.transform.LookAt(cmrObj.transform);
			if (nickNameCom != null)
			{
				nickNameCom.transform.LookAt(cmrObj.transform);
			}
		}
	}

	public void Chatting(string c)
	{
		if (playerNickName != string.Empty)
		{
			HideNickName();
		}
		Debug.Log("talking:" + c);
		int num = contentCom.SetString(c, 0.05f, 0f, -0.3f, 1f, 2f / contentCom.transform.lossyScale.x, BoardType.Hold, AlignStyle.center);
		chattingBox.transform.localPosition = new Vector3(0f, cbPosY + (float)(num - 1) * 0.4f, 0f);
		bgTrs.localScale = new Vector3(1f, 1f, (float)num / 2f);
		SceneTimerInstance.Instance.Remove(Hide);
		chattingBox.SetActive(true);
		chattingBox.animation.Stop();
		chattingBox.animation.Play();
		SceneTimerInstance.Instance.Add(8f, Hide);
	}

	public bool Hide()
	{
		chattingBox.SetActive(false);
		if (playerNickName != string.Empty)
		{
			ShowNickName();
		}
		return false;
	}

	private void ShowNickName()
	{
		if (nickNameCom != null)
		{
			chattingBox.SetActive(false);
			nickNameCom.gameObject.SetActive(true);
		}
	}

	private void HideNickName()
	{
		if (nickNameCom != null)
		{
			chattingBox.SetActive(true);
			nickNameCom.gameObject.SetActive(false);
		}
	}

	private bool IsNickNameActive()
	{
		return nickNameCom.gameObject.activeSelf;
	}

	public float GetHeadTopTextLength()
	{
		if (IsNickNameActive())
		{
			return nickNameCom.lineWidthMax;
		}
		return 2.4f;
	}

	public void SetLocPos(Vector3 v1, Vector3 v2)
	{
		base.transform.localPosition = v1;
		nickNameCom.transform.localPosition = v2;
	}

	public void ChangeLocPos(Vector3 v, bool bAdd)
	{
		if (bAdd)
		{
			base.transform.localPosition = COMA_PlayerSelf.Instance.characterCom.transform.localPosition + v;
			nickNameCom.transform.localPosition = COMA_PlayerSelf.Instance.characterCom.transform.localPosition + v;
		}
	}

	public void ChangeLocPos(Vector3 v, Vector3 v0)
	{
		base.transform.localPosition = v0 + v;
		nickNameCom.transform.localPosition = v0 + v;
	}
}
