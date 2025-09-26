using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using UnityEngine;

public class RPGEntity : TBaseEntity
{
	public enum EAniLST
	{
		Idle = 0,
		Attack = 1,
		Death = 2,
		Buff = 3,
		Beattack = 4
	}

	[SerializeField]
	private Transform[] _meleeAttackTran;

	[SerializeField]
	private RPGTeam _team;

	[SerializeField]
	private int _lv;

	[SerializeField]
	private float _maxHp;

	[SerializeField]
	private bool _debugDied;

	[SerializeField]
	private bool _debugHurt;

	[SerializeField]
	private bool _debugCriticalHurt;

	[SerializeField]
	private bool _debugHealing;

	[SerializeField]
	private bool _isNotifyDeath;

	[SerializeField]
	private float _curHp;

	private RPGEntity _enemy;

	[SerializeField]
	protected RPGCharacter _charac;

	[SerializeField]
	private RPGCareerUnit _careerUnit;

	[SerializeField]
	private RPGRoleAttr _baseAttr;

	[SerializeField]
	private RPGRoleAttr _calcAttr;

	[SerializeField]
	private int _curChildBoutCount;

	[SerializeField]
	private int _curBattleBoutIndex;

	[SerializeField]
	protected List<RPGTBuff> _buffList = new List<RPGTBuff>();

	[SerializeField]
	protected List<RPGTSkill> _skillLst = new List<RPGTSkill>();

	[SerializeField]
	private RPGTBuff _curShowBuffIcon;

	[SerializeField]
	private float _curBuffIconShowTime;

	[SerializeField]
	private float _maxBuffIconShowTime = 3f;

	[SerializeField]
	private bool _canUpdateBuffIcon = true;

	private List<AnimationClip> _lstAni = new List<AnimationClip>();

	private BloodBar bloodBarCom;

	[SerializeField]
	private Transform _markTrans;

	[SerializeField]
	private Transform shadowTrs;

	public Transform MeleeAttackTran
	{
		get
		{
			if (base.transform.position.x > 0f)
			{
				return _meleeAttackTran[0];
			}
			return _meleeAttackTran[1];
		}
	}

	public Transform MeleeAttackTranSelf
	{
		get
		{
			if (base.transform.position.x > 0f)
			{
				return _meleeAttackTran[2];
			}
			return _meleeAttackTran[3];
		}
	}

	public RPGTeam TeamOwner
	{
		get
		{
			return _team;
		}
		set
		{
			_team = value;
		}
	}

	public int RoleLv
	{
		get
		{
			return _lv;
		}
		set
		{
			_lv = value;
		}
	}

	public float MaxHp
	{
		get
		{
			return _maxHp;
		}
		set
		{
			_maxHp = value;
			if (_curHp > _maxHp)
			{
				CurHp = _maxHp;
			}
		}
	}

	public float CurHp
	{
		get
		{
			return _curHp;
		}
		set
		{
			_curHp = value;
			_curHp = Mathf.Clamp(_curHp, 0f, MaxHp);
			if (bloodBarCom != null)
			{
				bloodBarCom.SetBloodBar(_curHp / MaxHp);
			}
			if (!(_curHp <= 0f))
			{
				return;
			}
			RPGTBuff buff = null;
			if (IsExitBuff(124, out buff))
			{
				float num = buff.ReVerdictDeath();
				if (num > 0f)
				{
					StartCoroutine(Renaissance(num));
				}
				else if (!_isNotifyDeath)
				{
					_isNotifyDeath = true;
					_team.NotifyMembleDied(this, 0.2f);
				}
			}
			else if (!_isNotifyDeath)
			{
				_isNotifyDeath = true;
				_team.NotifyMembleDied(this, 0.2f);
			}
		}
	}

	public RPGCharacter Charac
	{
		get
		{
			return _charac;
		}
	}

	public RPGCareerUnit CareerUnit
	{
		get
		{
			return _careerUnit;
		}
		set
		{
			_careerUnit = value;
		}
	}

	public RPGRoleAttr CalcAttr
	{
		get
		{
			return _calcAttr;
		}
		set
		{
			_calcAttr = value;
		}
	}

	public int CurChildBoutCount
	{
		get
		{
			return _curChildBoutCount;
		}
		set
		{
			_curChildBoutCount = value;
		}
	}

	public int CurBattleBoutIndex
	{
		get
		{
			return _curBattleBoutIndex;
		}
		set
		{
			_curBattleBoutIndex = value;
		}
	}

	public List<AnimationClip> LstAni
	{
		get
		{
			return _lstAni;
		}
	}

	public Transform MarkTrans
	{
		get
		{
			return _markTrans;
		}
	}

	public void SetEnemy(RPGEntity entity)
	{
		_enemy = entity;
	}

	public bool IsPlayer()
	{
		return RPGRefree.Instance.GetTeamIndex(TeamOwner) == 0;
	}

	private IEnumerator Renaissance(float hp)
	{
		TMessageDispatcher.Instance.DispatchMsg(GetInstanceID(), _enemy.GetInstanceID(), 5018, TTelegram.SEND_MSG_IMMEDIATELY, 3.3f);
		yield return new WaitForSeconds(0.1f);
		PlayAni(EAniLST.Buff);
		Debug.Log("------------Renaissance");
		GameObject objEffect = Object.Instantiate(Resources.Load("Particle/effect/Skill/RPG_BUFF_Undead/RPG_BUFF_Undead_Resurrection")) as GameObject;
		objEffect.transform.parent = base.transform;
		objEffect.transform.localPosition = Vector3.zero;
		objEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
		Object.Destroy(objEffect, 3f);
		yield return new WaitForSeconds(2.8f);
		Debug.Log("------------Renaissance:" + objEffect.name);
		CurHp = hp;
		SSHurtNum.Instance.HealingFont(hp, base.transform, false);
		_charac.animation.Stop();
		PlayAni(EAniLST.Idle);
	}

	public bool IsExitBuff(int secondType)
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			if (_buffList[i].SecondType == secondType)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsExitBuff(int secondType, out RPGTBuff buff)
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			if (_buffList[i].SecondType == secondType)
			{
				buff = _buffList[i];
				return true;
			}
		}
		buff = null;
		return false;
	}

	public float AddBuff(RPGTBuff buff)
	{
		float fLen = 0f;
		return AddBuff(buff, false, ref fLen, null);
	}

	public float AddBuff(RPGTBuff buff, bool needShow, ref float fLen, RPGEntity owner)
	{
		if (!buff.IsOverlap && IsExitBuff(buff.SecondType))
		{
			Object.Destroy(buff);
			fLen = 0f;
			return -1f;
		}
		buff.RPGEntityOwner = this;
		if (buff.SecondType == 114 && owner == null)
		{
			buff.SetHideBuffIconPermanent();
		}
		_buffList.Add(buff);
		if (buff.GetBuffEffect() != null)
		{
			buff.GetBuffEffect().SetActive(false);
		}
		if (needShow)
		{
			if (owner != null && owner == buff.RPGEntityOwner)
			{
				fLen = PlayAni(EAniLST.Buff);
			}
			else
			{
				fLen = ((!(fLen > 0f)) ? 0f : fLen);
			}
		}
		else
		{
			fLen = 0f;
		}
		RefreshBuffIcon(null);
		ReCalcAttr();
		return buff.Id;
	}

	public int RemoveBuff(int buffId)
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			if (_buffList[i].GetInstanceID() == buffId)
			{
				RefreshBuffIcon(_buffList[i]);
				Object.Destroy(_buffList[i]);
				_buffList.RemoveAt(i);
				ReCalcAttr();
				return i;
			}
		}
		return 0;
	}

	private void ShowCurBuffIconObj()
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			if (_buffList[i].GetBuffEffect() != null && _buffList[i].IsNeedShowBuffIcon)
			{
				_buffList[i].GetBuffEffect().SetActive((_buffList[i] == _curShowBuffIcon) ? true : false);
			}
		}
	}

	protected bool ShowFirstBuffIcon()
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			if (_buffList[i].GetBuffEffect() != null && _buffList[i].IsNeedShowBuffIcon)
			{
				_curShowBuffIcon = _buffList[i];
				_curBuffIconShowTime = Time.time;
				ShowCurBuffIconObj();
				return true;
			}
		}
		return false;
	}

	protected bool NextBuffIcon()
	{
		if (_curShowBuffIcon == null)
		{
			return ShowFirstBuffIcon();
		}
		bool flag = false;
		for (int i = 0; i < _buffList.Count; i++)
		{
			if (_buffList[i] == _curShowBuffIcon)
			{
				flag = true;
			}
			else if (flag && _buffList[i].GetBuffEffect() != null)
			{
				_curShowBuffIcon = _buffList[i];
				_curBuffIconShowTime = Time.time;
				ShowCurBuffIconObj();
				return true;
			}
		}
		return ShowFirstBuffIcon();
	}

	protected void UpdateBuffIcon()
	{
		if (_canUpdateBuffIcon && (_curShowBuffIcon == null || (_curShowBuffIcon != null && Time.time - _curBuffIconShowTime >= _maxBuffIconShowTime)))
		{
			_canUpdateBuffIcon = NextBuffIcon();
		}
	}

	protected void RefreshBuffIcon(RPGTBuff removeBuff)
	{
		if (removeBuff == _curShowBuffIcon)
		{
			_canUpdateBuffIcon = NextBuffIcon();
		}
	}

	public float NotifyBuffBeginBount(RPGRole_PlayerState_Begin_Bout state)
	{
		float result = 0f;
		for (int i = 0; i < _buffList.Count; i++)
		{
			float num = _buffList[i].NotifyBuffBegin_Bout(state, (RPGCenterController_Auto)_centerController);
			if (num > 0f)
			{
				result = num;
			}
		}
		return result;
	}

	public void NotifyBuffEndBount()
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			_buffList[i].NotifyBuffEnd_Bout();
		}
	}

	public void ReCalcAttr()
	{
		RPGRoleAttr rPGRoleAttr = new RPGRoleAttr();
		for (int i = 0; i < _buffList.Count; i++)
		{
			rPGRoleAttr += _buffList[i].GetOffsetResult(_baseAttr);
		}
		CalcAttr = _baseAttr + rPGRoleAttr;
		MaxHp = CalcAttr.Attrs[3];
		if (_team.Refree.CurMainState <= RPGRefree.EBattleMainState.ShowBuff)
		{
			CurHp = MaxHp;
		}
	}

	public static float GetHPByRoleAttr(float fStr, float fDex, float fInt)
	{
		return fStr * 6f + fDex * 3f + fInt * 2f;
	}

	public void InitAttr()
	{
		_baseAttr = new RPGRoleAttr();
		_baseAttr.Attrs[0] = (float)(4 + _lv) * CareerUnit.AttrValue[0];
		_baseAttr.Attrs[1] = (float)(4 + _lv) * CareerUnit.AttrValue[1];
		_baseAttr.Attrs[2] = (float)(4 + _lv) * CareerUnit.AttrValue[2];
		_baseAttr.Attrs[3] = GetHPByRoleAttr(_baseAttr.Attrs[0], _baseAttr.Attrs[1], _baseAttr.Attrs[2]);
		float num = 0f;
		for (int i = 0; i <= 2; i++)
		{
			if (i != (int)CareerUnit.MainAttr)
			{
				num += _baseAttr.Attrs[i];
			}
		}
		_baseAttr.Attrs[4] = _baseAttr.Attrs[(int)CareerUnit.MainAttr] * 1.5f + num;
		_baseAttr.Attrs[5] = 35f - 40000f / (1150f + _baseAttr.Attrs[1]) + (35f - 40000f / (1150f + _baseAttr.Attrs[2]));
		_baseAttr.Attrs[6] = 5f;
		_baseAttr.Attrs[7] = 100f;
		_baseAttr.Attrs[8] = 20f - 40000f / (2000f + _baseAttr.Attrs[1]);
		_baseAttr.Attrs[9] = 1.5f + (1f - 400f / (400f + _baseAttr.Attrs[2]));
		_baseAttr.Attrs[10] = 0f;
		_baseAttr.Attrs[11] = 0f;
		_baseAttr.Attrs[12] = 1f;
		_baseAttr.Attrs[13] = _baseAttr.Attrs[1];
		_baseAttr.Attrs[14] = 0f;
		_baseAttr.Attrs[15] = 0f;
		_baseAttr.Attrs[16] = 0f;
		_baseAttr.Attrs[17] = 0f;
		_baseAttr.Attrs[18] = 0f;
		_baseAttr.Attrs[19] = 0f;
		_baseAttr.Attrs[20] = 0f;
		_baseAttr.Attrs[21] = 0f;
		_baseAttr.Attrs[22] = 0f;
		_baseAttr.Attrs[23] = 0f;
		CalcAttr = new RPGRoleAttr(_baseAttr);
		MaxHp = CalcAttr.Attrs[3];
		CurHp = MaxHp;
		Debug.Log("InitAttr:" + CalcAttr.Attrs[3]);
	}

	public void InitAnis()
	{
		int careerId = CareerUnit.CareerId;
		if (!RPGGlobalData.Instance.CareerAniPool._dict.ContainsKey(careerId))
		{
			return;
		}
		_lstAni.Clear();
		List<RPGCareerAnimationUnit> list = RPGGlobalData.Instance.CareerAniPool._dict[careerId];
		for (int i = 0; i < list.Count; i++)
		{
			string text = "FBX/Player/Character/Animation/RPG/" + list[i]._name;
			Debug.Log(text);
			AnimationClip animationClip = Object.Instantiate(Resources.Load(text)) as AnimationClip;
			if (animationClip == null)
			{
				Debug.LogError(text + "  does not exist!");
			}
			animationClip.name = list[i]._name;
			animationClip.wrapMode = list[i]._wrapMode;
			_charac.animation.AddClip(animationClip, list[i]._name);
			if (list[i]._default)
			{
				AnimationState animationState = _charac.animation[animationClip.name];
				animationState.layer = -1;
				_charac.animation.clip = animationClip;
				_charac.animation.Play();
			}
			if (animationClip.name == "Samurai_Attack01")
			{
				AnimationState animationState2 = _charac.animation[animationClip.name];
				animationState2.speed = 2f;
			}
			_lstAni.Add(animationClip);
		}
	}

	public void InitProfile()
	{
		List<RPGDressUnit> dressUnitLst = CareerUnit.DressUnitLst;
		for (int i = 0; i < dressUnitLst.Count; i++)
		{
			if (dressUnitLst[i]._bodyPart == RPGCharacter.ERPGPart.decoration)
			{
				Debug.Log(dressUnitLst[i]._partPath);
				GameObject gameObject = Object.Instantiate(Resources.Load(dressUnitLst[i]._partPath)) as GameObject;
				_charac.EquipDecoration(dressUnitLst[i]._decoPart, gameObject.transform);
			}
			else if (dressUnitLst[i]._bodyPart == RPGCharacter.ERPGPart.head)
			{
				Object obj = Resources.Load(dressUnitLst[i]._partPath);
				if (obj == null)
				{
					obj = Resources.Load("FBX/Player/Character/Texture/T_head");
				}
				Texture2D tex = Object.Instantiate(obj) as Texture2D;
				_charac.EquipTexture(dressUnitLst[i]._bodyPart, tex);
			}
			else if (dressUnitLst[i]._bodyPart == RPGCharacter.ERPGPart.body)
			{
				Object obj2 = Resources.Load(dressUnitLst[i]._partPath);
				if (obj2 == null)
				{
					obj2 = Resources.Load("FBX/Player/Character/Texture/T_body");
				}
				Texture2D tex2 = Object.Instantiate(obj2) as Texture2D;
				_charac.EquipTexture(dressUnitLst[i]._bodyPart, tex2);
			}
			else if (dressUnitLst[i]._bodyPart == RPGCharacter.ERPGPart.leg)
			{
				Object obj3 = Resources.Load(dressUnitLst[i]._partPath);
				if (obj3 == null)
				{
					obj3 = Resources.Load("FBX/Player/Character/Texture/T_Leg");
				}
				Texture2D tex3 = Object.Instantiate(obj3) as Texture2D;
				_charac.EquipTexture(dressUnitLst[i]._bodyPart, tex3);
			}
		}
	}

	public void ClearProfile()
	{
		for (int i = 0; i < 9; i++)
		{
			Transform decoByPart = Charac.GetDecoByPart((RPGCharacter.EDecorationPart)i);
			for (int num = decoByPart.childCount - 1; num >= 0; num--)
			{
				Transform child = decoByPart.GetChild(num);
				child.parent = null;
				Object.DestroyObject(child.gameObject);
			}
		}
		_charac.EquipTexture(RPGCharacter.ERPGPart.head, UIGolbalStaticFun.CreateDefaultTexture(0));
		_charac.EquipTexture(RPGCharacter.ERPGPart.body, UIGolbalStaticFun.CreateDefaultTexture(1));
		_charac.EquipTexture(RPGCharacter.ERPGPart.leg, UIGolbalStaticFun.CreateDefaultTexture(2));
	}

	public void InitProfile(string head, string body, string leg)
	{
		List<RPGDressUnit> dressLst = CareerUnit.DressUnitLst;
		for (int i = 0; i < dressLst.Count; i++)
		{
			if (dressLst[i]._bodyPart == RPGCharacter.ERPGPart.decoration)
			{
				Debug.Log(dressLst[i]._partPath);
				GameObject gameObject = Object.Instantiate(Resources.Load(dressLst[i]._partPath)) as GameObject;
				_charac.EquipDecoration(dressLst[i]._decoPart, gameObject.transform);
			}
			else if (dressLst[i]._bodyPart == RPGCharacter.ERPGPart.head)
			{
				Texture2D tex = null;
				if (head == string.Empty)
				{
					Object obj = Resources.Load(dressLst[i]._partPath);
					if (obj == null)
					{
						obj = Resources.Load("FBX/Player/Character/Texture/T_head");
					}
					tex = Object.Instantiate(obj) as Texture2D;
					_charac.EquipTexture(dressLst[i]._bodyPart, tex);
				}
				else if (head == "0")
				{
					_charac.EquipTexture(dressLst[i]._bodyPart, UIGolbalStaticFun.CreateWhiteTexture());
				}
				else
				{
					int dressLstIndex_head = i;
					UIDataBufferCenter.Instance.FetchFileByMD5(head, delegate(byte[] fileData)
					{
						tex = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height);
						tex.LoadImage(fileData);
						tex.filterMode = FilterMode.Point;
						_charac.EquipTexture(dressLst[dressLstIndex_head]._bodyPart, tex);
					});
				}
			}
			else if (dressLst[i]._bodyPart == RPGCharacter.ERPGPart.body)
			{
				Texture2D tex2 = null;
				if (body == string.Empty)
				{
					Object obj2 = Resources.Load(dressLst[i]._partPath);
					if (obj2 == null)
					{
						obj2 = Resources.Load("FBX/Player/Character/Texture/T_body");
					}
					tex2 = Object.Instantiate(obj2) as Texture2D;
					_charac.EquipTexture(dressLst[i]._bodyPart, tex2);
				}
				else if (body == "0")
				{
					_charac.EquipTexture(dressLst[i]._bodyPart, UIGolbalStaticFun.CreateWhiteTexture());
				}
				else
				{
					int dressLstIndex_body = i;
					UIDataBufferCenter.Instance.FetchFileByMD5(body, delegate(byte[] fileData)
					{
						tex2 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height);
						tex2.LoadImage(fileData);
						tex2.filterMode = FilterMode.Point;
						_charac.EquipTexture(dressLst[dressLstIndex_body]._bodyPart, tex2);
					});
				}
			}
			else
			{
				if (dressLst[i]._bodyPart != RPGCharacter.ERPGPart.leg)
				{
					continue;
				}
				Texture2D tex3 = null;
				if (leg == string.Empty)
				{
					Object obj3 = Resources.Load(dressLst[i]._partPath);
					if (obj3 == null)
					{
						obj3 = Resources.Load("FBX/Player/Character/Texture/T_Leg");
					}
					tex3 = Object.Instantiate(obj3) as Texture2D;
					_charac.EquipTexture(dressLst[i]._bodyPart, tex3);
				}
				else if (leg == "0")
				{
					_charac.EquipTexture(dressLst[i]._bodyPart, UIGolbalStaticFun.CreateWhiteTexture());
				}
				else
				{
					int dressLstIndex_leg = i;
					UIDataBufferCenter.Instance.FetchFileByMD5(leg, delegate(byte[] fileData)
					{
						tex3 = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height);
						tex3.LoadImage(fileData);
						tex3.filterMode = FilterMode.Point;
						_charac.EquipTexture(dressLst[dressLstIndex_leg]._bodyPart, tex3);
					});
				}
			}
		}
	}

	public void InitEquipment(RPGEquipmentUnit unit)
	{
		if (unit == null)
		{
			return;
		}
		if (unit._SuckHP != null && unit._SuckHP._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff = null;
			rPGTBuff = base.gameObject.AddComponent<RPGBuff_Equipment_SuckHP>();
			rPGTBuff.SecondType = 310;
			rPGTBuff.IsOverlap = false;
			((RPGBuff_Equipment_SuckHP)rPGTBuff)._suckR = unit._SuckHP._promoteValue;
			if (AddBuff(rPGTBuff) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff.name + " to " + base.gameObject.name);
			}
		}
		if (unit._LimitRHP != null && unit._LimitRHP._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff2 = null;
			rPGTBuff2 = base.gameObject.AddComponent<RPGBuff_Equipment_LimitRHP>();
			rPGTBuff2.SecondType = 311;
			rPGTBuff2.IsOverlap = false;
			((RPGBuff_Equipment_LimitRHP)rPGTBuff2)._RHPR = unit._LimitRHP._promoteValue;
			if (AddBuff(rPGTBuff2) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff2.name + " to " + base.gameObject.name);
			}
		}
		if (unit._boutLimit_ATT != null && unit._boutLimit_ATT._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff3 = null;
			rPGTBuff3 = base.gameObject.AddComponent<RPGBuff_Equipment_ATT>();
			rPGTBuff3.SecondType = 300;
			rPGTBuff3.IsOverlap = true;
			rPGTBuff3.BoutCount = unit._boutLimit_ATT._boutCount;
			rPGTBuff3.OffsetAttr.Attrs[4] = unit._boutLimit_ATT._promoteValue;
			if (AddBuff(rPGTBuff3) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff3.name + " to " + base.gameObject.name);
			}
		}
		if (unit._boutLimit_DEF != null && unit._boutLimit_DEF._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff4 = null;
			rPGTBuff4 = base.gameObject.AddComponent<RPGBuff_Equipment_DEF>();
			rPGTBuff4.SecondType = 301;
			rPGTBuff4.IsOverlap = true;
			rPGTBuff4.BoutCount = unit._boutLimit_DEF._boutCount;
			rPGTBuff4.OffsetAttr.Attrs[5] = unit._boutLimit_DEF._promoteValue;
			if (AddBuff(rPGTBuff4) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff4.name + " to " + base.gameObject.name);
			}
		}
		if (unit._ATT != null && unit._ATT._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff5 = null;
			rPGTBuff5 = base.gameObject.AddComponent<RPGBuff_Equipment_ATT>();
			rPGTBuff5.SecondType = 300;
			rPGTBuff5.IsOverlap = true;
			rPGTBuff5.OffsetAttr.Attrs[4] = unit._ATT._promoteValue;
			if (AddBuff(rPGTBuff5) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff5.name + " to " + base.gameObject.name);
			}
		}
		if (unit._DEF != null && unit._DEF._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff6 = null;
			rPGTBuff6 = base.gameObject.AddComponent<RPGBuff_Equipment_DEF>();
			rPGTBuff6.SecondType = 301;
			rPGTBuff6.IsOverlap = true;
			rPGTBuff6.OffsetAttr.Attrs[5] = unit._DEF._promoteValue;
			if (AddBuff(rPGTBuff6) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff6.name + " to " + base.gameObject.name);
			}
		}
		if (unit._DodgeRate != null && unit._DodgeRate._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff7 = null;
			rPGTBuff7 = base.gameObject.AddComponent<RPGBuff_Equipment_DodgeRate>();
			rPGTBuff7.SecondType = 302;
			rPGTBuff7.IsOverlap = false;
			rPGTBuff7.OffsetAttr.Attrs[6] = unit._DodgeRate._promoteValue;
			if (AddBuff(rPGTBuff7) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff7.name + " to " + base.gameObject.name);
			}
		}
		if (unit._CriticalRate != null && unit._CriticalRate._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff8 = null;
			rPGTBuff8 = base.gameObject.AddComponent<RPGBuff_Equipment_CriticalRate>();
			rPGTBuff8.SecondType = 303;
			rPGTBuff8.IsOverlap = false;
			rPGTBuff8.OffsetAttr.Attrs[8] = unit._CriticalRate._promoteValue;
			if (AddBuff(rPGTBuff8) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff8.name + " to " + base.gameObject.name);
			}
		}
		if (unit._ATIndex != null && unit._ATIndex._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff9 = null;
			rPGTBuff9 = base.gameObject.AddComponent<RPGBuff_Equipment_ATIndex>();
			rPGTBuff9.SecondType = 304;
			rPGTBuff9.IsOverlap = false;
			rPGTBuff9.OffsetAttr.Attrs[7] = unit._ATIndex._promoteValue;
			if (AddBuff(rPGTBuff9) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff9.name + " to " + base.gameObject.name);
			}
		}
		if (unit._HP != null && unit._HP._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff10 = null;
			rPGTBuff10 = base.gameObject.AddComponent<RPGBuff_Equipment_HP>();
			rPGTBuff10.SecondType = 305;
			rPGTBuff10.IsOverlap = false;
			rPGTBuff10.OffsetAttr.Attrs[3] = unit._HP._promoteValue;
			if (AddBuff(rPGTBuff10) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff10.name + " to " + base.gameObject.name);
			}
		}
		if (unit._CriticalMultiplier != null && unit._CriticalMultiplier._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff11 = null;
			rPGTBuff11 = base.gameObject.AddComponent<RPGBuff_Equipment_CriticalMultiplier>();
			rPGTBuff11.SecondType = 306;
			rPGTBuff11.IsOverlap = false;
			rPGTBuff11.OffsetAttr.Attrs[9] = unit._CriticalMultiplier._promoteValue;
			if (AddBuff(rPGTBuff11) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff11.name + " to " + base.gameObject.name);
			}
		}
		if (unit._ITDValue != null && unit._ITDValue._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff12 = null;
			rPGTBuff12 = base.gameObject.AddComponent<RPGBuff_Equipment_ITDValue>();
			rPGTBuff12.SecondType = 307;
			rPGTBuff12.IsOverlap = false;
			rPGTBuff12.OffsetAttr.Attrs[23] = unit._ITDValue._promoteValue;
			if (AddBuff(rPGTBuff12) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff12.name + " to " + base.gameObject.name);
			}
		}
		if (unit._FrozenResistRate != null && unit._FrozenResistRate._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff13 = null;
			rPGTBuff13 = base.gameObject.AddComponent<RPGBuff_Equipment_FrozenResistRate>();
			rPGTBuff13.SecondType = 308;
			rPGTBuff13.IsOverlap = false;
			rPGTBuff13.OffsetAttr.Attrs[16] = unit._FrozenResistRate._promoteValue;
			if (AddBuff(rPGTBuff13) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff13.name + " to " + base.gameObject.name);
			}
		}
		if (unit._StunResistRate != null && unit._StunResistRate._promoteValue > 0f)
		{
			RPGTBuff rPGTBuff14 = null;
			rPGTBuff14 = base.gameObject.AddComponent<RPGBuff_Equipment_StunResistRate>();
			rPGTBuff14.SecondType = 309;
			rPGTBuff14.IsOverlap = false;
			rPGTBuff14.OffsetAttr.Attrs[17] = unit._StunResistRate._promoteValue;
			if (AddBuff(rPGTBuff14) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff14.name + " to " + base.gameObject.name);
			}
		}
	}

	public RPGTSkill EquipSkill(int skillId)
	{
		RPGTSkill rPGTSkill = RPGGlobalFun.EquipSkillById(skillId, this);
		rPGTSkill.SkillOwner = this;
		_skillLst.Add(rPGTSkill);
		return rPGTSkill;
	}

	public RPGTSkill GetAttackSkill()
	{
		if (_skillLst.Count > 0)
		{
			return _skillLst[0];
		}
		return null;
	}

	public RPGTSkill GetSkillByIndex(int n)
	{
		if (n < 0 || n >= _skillLst.Count)
		{
			return null;
		}
		return _skillLst[n];
	}

	public int GetSkillCount()
	{
		return _skillLst.Count;
	}

	public bool IsExistSkill(int skillId)
	{
		for (int i = 0; i < _skillLst.Count; i++)
		{
			if (_skillLst[i].SkillUnit.SkillId == skillId)
			{
				return true;
			}
		}
		return false;
	}

	public float PlayAni(EAniLST ani)
	{
		if (CareerUnit.CareerId == 28 || CareerUnit.CareerId == 510)
		{
			RPGBishopBook componentInChildren = base.transform.GetComponentInChildren<RPGBishopBook>();
			if (componentInChildren != null)
			{
				switch (ani)
				{
				case EAniLST.Buff:
					componentInChildren.PlayBuffAni();
					break;
				case EAniLST.Attack:
					componentInChildren.PlayAttackAni();
					break;
				}
			}
		}
		if ((int)ani < LstAni.Count)
		{
			if (LstAni[(int)ani].length <= 0.2f || ani == EAniLST.Attack)
			{
				if (LstAni[(int)ani].name.StartsWith("Blank"))
				{
					return 0.5f;
				}
				if (LstAni[(int)ani].name.StartsWith("Dead01"))
				{
					Charac.animation.CrossFade(LstAni[(int)ani].name, 0.3f);
					return LstAni[(int)ani].length;
				}
				Charac.animation.Play(LstAni[(int)ani].name);
			}
			else
			{
				if (LstAni[(int)ani].name.StartsWith("Blank"))
				{
					return 0.5f;
				}
				Charac.animation.CrossFade(LstAni[(int)ani].name, 0.1f);
			}
			return LstAni[(int)ani].length;
		}
		return 0f;
	}

	public void StopAni()
	{
		Charac.animation.Stop();
	}

	public void TriggerAttack(float delay)
	{
		((RPGCenterController_Auto)_centerController).RealAttack(delay);
	}

	public void TriggerAttack()
	{
		TriggerAttack(0f);
	}

	public void TriggerAttackEffect()
	{
		GetAttackSkill().Skill_AttackEffect((RPGCenterController_Auto)_centerController);
	}

	public void TriggerAttackEffect_Launch()
	{
		GetAttackSkill().Skill_AttackEffect_Launch((RPGCenterController_Auto)_centerController);
	}

	public void TriggerBuffer_BeAttack(RPGEntity attack, int dam)
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			_buffList[i].BeAttackAppend(attack, dam);
		}
	}

	public void TriggerBuffer_Attack(RPGEntity attack, int dam)
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			_buffList[i].AttackAppend(attack, dam);
		}
	}

	public void TriggerBuffer_Limit()
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			_buffList[i].ActionLimit();
		}
	}

	private bool IsInLimitBoutBuff()
	{
		for (int i = 0; i < _buffList.Count; i++)
		{
			if (_buffList[i].SecondType == 220)
			{
				return true;
			}
		}
		return false;
	}

	public int IsCanBeginBout()
	{
		if (CurHp > 0f && !IsInLimitBoutBuff())
		{
			return 1;
		}
		if (CurHp <= 0f)
		{
			return -1;
		}
		return 0;
	}

	public bool AttackTimeOffset(ref float f)
	{
		if (CareerUnit.CareerId == 46)
		{
			f += 0.3f;
			return true;
		}
		if (CareerUnit.CareerId == 42 || CareerUnit.CareerId == 515)
		{
			f += 1f;
			return true;
		}
		return false;
	}

	public void InitBlood(BloodBar.BarColor color)
	{
		bloodBarCom.InitBloodBar(color);
	}

	private void Awake()
	{
		_centerController = new RPGCenterController_Auto(this);
		Vector3 position = base.transform.position + new Vector3(0f, 2.4f, 0f);
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Common/BloodBar/PFB_RPG_Frame"), position, Quaternion.identity) as GameObject;
		gameObject.transform.parent = base.transform;
		bloodBarCom = gameObject.GetComponent<BloodBar>();
		_maxBuffIconShowTime = Random.Range(2.5f, 3f);
	}

	protected void UpdateShadow()
	{
		Vector3 position = shadowTrs.position;
		position.x = MarkTrans.position.x;
		position.z = MarkTrans.position.z;
		shadowTrs.position = position;
	}

	private new void Update()
	{
		base.Update();
		UpdateShadow();
		if (_debugDied)
		{
			CurHp = 0f;
			_debugDied = false;
		}
		if (_debugHurt)
		{
			SSHurtNum.Instance.HitNormalFont(150f, base.transform);
			_debugHurt = false;
		}
		if (_debugCriticalHurt)
		{
			SSHurtNum.Instance.HitCriticalFont(150f, base.transform);
			_debugCriticalHurt = false;
		}
		if (_debugHealing)
		{
			SSHurtNum.Instance.HealingFont(150f, base.transform);
			_debugHealing = false;
		}
		UpdateBuffIcon();
	}
}
