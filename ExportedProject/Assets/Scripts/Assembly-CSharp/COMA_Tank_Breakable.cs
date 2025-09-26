using UnityEngine;

public class COMA_Tank_Breakable : COMA_Creation
{
	protected int _nDestoryScore = 100;

	public int _tankModeHp;

	protected int ObstacleID
	{
		get
		{
			return int.Parse(base.name);
		}
	}

	protected new void OnEnable()
	{
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	protected new void Start()
	{
		base.Start();
		creationKind = CreationKind.Enemy;
		base.hp = (HP = _tankModeHp);
	}

	public override void OnHurt(COMA_PlayerSelf from, string bulletName, float bulletAP, Vector3 push)
	{
		if (canbeDamaged(from))
		{
			Debug.Log("hurt:" + bulletName + "  bulletAP:" + bulletAP);
			// Make conditional produce a consistent numeric type (int) to avoid mixing byte and int.
			int num = !string.IsNullOrEmpty(bulletName) ? int.Parse(bulletName.Substring(1, 3)) : 0;
			doHurt(bulletAP, false);
			broadcastHit(bulletAP);
		}
	}

	protected virtual bool canbeDamaged(COMA_PlayerSelf from)
	{
		Debug.LogError("this function must be override");
		return false;
	}

	private void broadcastHit(float bulletAp)
	{
		COMA_CD_EnemyHit cOMA_CD_EnemyHit = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ENEMY_HIT) as COMA_CD_EnemyHit;
		cOMA_CD_EnemyHit.bEnemyID = (byte)ObstacleID;
		cOMA_CD_EnemyHit.attackPoint = bulletAp;
		COMA_CommandHandler.Instance.Send(cOMA_CD_EnemyHit);
	}

	public void doHurt(float bulletAp, bool bFromNet)
	{
		if (base.IsDead)
		{
			return;
		}
		base.hp -= bulletAp;
		onHpChange(base.hp);
		Debug.Log("obstacle hurt" + base.hp);
		if (base.IsDead)
		{
			onDestoryed(bFromNet);
			if (!bFromNet)
			{
				broadcastScore();
			}
		}
	}

	protected virtual void onHpChange(float fNewHp)
	{
	}

	protected virtual void onDestoryed(bool bFromNet)
	{
	}

	private void broadcastScore()
	{
	}
}
