using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSkill_18 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[18];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		return 0f;
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}

	public override int Skill_DAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGTeam teamOwner = beAttack.TeamOwner;
		List<RPGEntity> list = new List<RPGEntity>();
		for (int i = 0; i < teamOwner.MemberLst.Count; i++)
		{
			RPGEntity rPGEntity = teamOwner.MemberLst[i];
			if (rPGEntity != beAttack && rPGEntity.CurHp > 0f)
			{
				list.Add(rPGEntity);
			}
		}
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		int num = Mathf.FloorToInt((float)dam * (float)(int)rPGSkillUnit.ParamLst[0] / 100f);
		for (int j = 0; j < list.Count; j++)
		{
			StartCoroutine(Sputtering(list[j], num));
			GameObject original = Resources.Load("Particle/effect/Skill/FireBall/RPG_Fireball_Lu") as GameObject;
			Vector3 position = beAttack.transform.position;
			GameObject gameObject = Object.Instantiate(original, position, Quaternion.Euler(Vector3.zero)) as GameObject;
			gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			Ammo_Fly ammo_Fly = gameObject.AddComponent<Ammo_Fly>();
			ammo_Fly.StartFly(list[j].transform, 0.3f, 0.3f);
		}
		return dam;
	}

	private IEnumerator Sputtering(RPGEntity entity, float spurtingDAM)
	{
		yield return new WaitForSeconds(0.3f);
		entity.CurHp -= spurtingDAM;
		SSHurtNum.Instance.HitNormalFont(spurtingDAM, entity.transform);
	}
}
