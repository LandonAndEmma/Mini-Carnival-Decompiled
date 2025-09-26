using System.Collections.Generic;
using UnityEngine;

public class UIFishing_RankInfo : MonoBehaviour
{
	[SerializeField]
	private RankInfo[] _rankInfos;

	[SerializeField]
	private TUIMeshSprite[] _texIcon;

	[SerializeField]
	private TUILabel[] _labelName;

	[SerializeField]
	private TUILabel[] _labelNum;

	[SerializeField]
	private string[] _Id;

	private List<RankInfo> _lstRankInfo = new List<RankInfo>();

	public void ClearRankInfoLst()
	{
		_lstRankInfo.Clear();
	}

	public void SetRankInfo(RankInfo info, int nRank)
	{
		_texIcon[nRank].UseCustomize = true;
		_texIcon[nRank].CustomizeTexture = info._tex;
		if (info._tex != null)
		{
			_texIcon[nRank].CustomizeRect = new Rect(0f, 0f, info._tex.width, info._tex.height);
		}
		_labelName[nRank].Text = info._strName;
		_labelNum[nRank].Text = info._fNum.ToString("f0");
		_labelNum[nRank].Text += "g";
		_Id[nRank] = info._strId;
		_lstRankInfo.Add(info);
	}

	public void RefreshRankInfo(RankInfo info)
	{
		_lstRankInfo.Sort();
		for (int num = _lstRankInfo.Count - 1; num >= 3; num--)
		{
			_lstRankInfo.RemoveAt(num);
		}
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < Mathf.Min(3, _lstRankInfo.Count); i++)
		{
			if (_lstRankInfo[i]._strId == info._strId)
			{
				flag2 = true;
				if (info._fNum > _lstRankInfo[i]._fNum)
				{
					flag = true;
				}
				_lstRankInfo[i]._fNum = Mathf.Max(info._fNum, _lstRankInfo[i]._fNum);
				break;
			}
		}
		if (!flag2)
		{
			flag = true;
			_lstRankInfo.Add(info);
		}
		_lstRankInfo.Sort();
		bool flag3 = false;
		for (int j = 0; j < Mathf.Min(3, _lstRankInfo.Count); j++)
		{
			_labelName[j].Text = _lstRankInfo[j]._strName;
			_labelNum[j].Text = _lstRankInfo[j]._fNum.ToString("f0");
			_labelNum[j].Text += "g";
			_Id[j] = _lstRankInfo[j]._strId;
			if (info._strId == _Id[j])
			{
				flag3 = true;
			}
		}
		if (flag3 && flag)
		{
			NotifyCongratulation(info._strId);
		}
	}

	private void NotifyCongratulation(string id)
	{
		if (!(COMA_PlayerSelf.Instance != null))
		{
			return;
		}
		Debug.Log("========================================================NotifyCongratulation:" + id);
		COMA_Player[] componentsInChildren = COMA_PlayerSelf.Instance.transform.parent.gameObject.GetComponentsInChildren<COMA_Player>();
		if (componentsInChildren == null)
		{
			return;
		}
		Debug.Log("playerCmps=" + componentsInChildren.Length);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Debug.Log("playerCmps[i].gid=" + componentsInChildren[i].gid);
			if (componentsInChildren[i].gid == id)
			{
				Transform transform = componentsInChildren[i].transform;
				GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Interaction/Interaction_01_pfb")) as GameObject;
				gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
				gameObject.transform.parent = transform;
				gameObject.transform.localPosition = transform.GetComponent<COMA_Player>().characterCom.transform.localPosition;
				Object.DestroyObject(gameObject, 3f);
				int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
				CPopupInfo extraInfo = new CPopupInfo(0, transform.GetComponent<COMA_Player>().nickname);
				TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1018, TTelegram.SEND_MSG_IMMEDIATELY, extraInfo);
				break;
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
