using System.Collections.Generic;
using MessageID;
using Protocol;
using UnityEngine;

public class UIMyTeamPreviewContr : UIEntity
{
	[SerializeField]
	private Transform _charc;

	[SerializeField]
	private Camera _cam;

	[SerializeField]
	private UITexture _tex_preview;

	[SerializeField]
	private RPGEntity _entity;

	[SerializeField]
	private GameObject _aureoleObj;

	[SerializeField]
	private ParticleSystem[] _particleSystems;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIMyTeam_PreviewChanged, this, PreviewChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMyTeam_PreviewChanged, this);
	}

	private bool PreviewChanged(TUITelegram msg)
	{
		uint num = (uint)msg._pExtraInfo;
		int num2 = (int)msg._pExtraInfo2;
		if (num != 0)
		{
			if (num == 40)
			{
				_cam.orthographicSize = 1.7f;
				_charc.localPosition = new Vector3(0f, -0.3f, 4f);
			}
			else
			{
				_cam.orthographicSize = 1.24f;
				_charc.localPosition = new Vector3(0f, -0.8f, 4f);
			}
			_tex_preview.enabled = true;
			_aureoleObj.SetActive(true);
			for (int i = 0; i < _particleSystems.Length; i++)
			{
				_particleSystems[i].renderer.material.color = UIRPG_DataBufferCenter.GetAureoleColorByCardGrade(UIRPG_DataBufferCenter.GetCardGradeByCardId((int)num));
			}
			if (RPGGlobalData.Instance.CareerUnitPool._dict.ContainsKey((int)num))
			{
				RPGCareerUnit careerUnit = RPGGlobalData.Instance.CareerUnitPool._dict[(int)num];
				_entity.CareerUnit = careerUnit;
				_entity.InitAnis();
				Dictionary<ulong, Equip> equip_bag = UIDataBufferCenter.Instance.RPGData.m_equip_bag;
				Debug.Log("index=" + num2);
				MemberSlot memberSlot = UIDataBufferCenter.Instance.RPGData.m_member_slot[num2];
				Equip equip = ((!equip_bag.ContainsKey(memberSlot.m_head)) ? null : equip_bag[memberSlot.m_head]);
				Equip equip2 = ((!equip_bag.ContainsKey(memberSlot.m_body)) ? null : equip_bag[memberSlot.m_body]);
				Equip equip3 = ((!equip_bag.ContainsKey(memberSlot.m_leg)) ? null : equip_bag[memberSlot.m_leg]);
				string text = ((equip == null) ? string.Empty : equip.m_md5);
				string text2 = ((equip2 == null) ? string.Empty : equip2.m_md5);
				string text3 = ((equip3 == null) ? string.Empty : equip3.m_md5);
				_entity.ClearProfile();
				if (equip != null && equip.m_md5 == string.Empty)
				{
					text = "0";
				}
				if (equip2 != null && equip2.m_md5 == string.Empty)
				{
					text2 = "0";
				}
				if (equip3 != null && equip3.m_md5 == string.Empty)
				{
					text3 = "0";
				}
				Debug.Log("head_md5=" + text);
				Debug.Log("body_md5=" + text2);
				Debug.Log("leg_md5=" + text3);
				_entity.InitProfile(text, text2, text3);
			}
		}
		else
		{
			Debug.Log("_fjakfjkajfkajkfjakjfaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
			_tex_preview.enabled = false;
			_aureoleObj.SetActive(false);
		}
		return true;
	}

	private void Awake()
	{
		_tex_preview.enabled = false;
	}

	protected override void Tick()
	{
	}
}
