using UnityEngine;

public class COMA_Enemy_Zombie : COMA_Enemy
{
	private COMA_Target obstacleCom;

	private Transform playerTrs;

	private COMA_PlayerSelf playerCom;

	private bool bFindPlayerLast;

	protected new void Start()
	{
		base.Start();
		AgentEnable();
		PlayMyAnimation("Move");
		playerTrs = playerCastleCom.transform;
	}

	private new void Update()
	{
		if (COMA_Scene.Instance.runingGameOver)
		{
			return;
		}
		if (base.transform.position.y < -50f || base.transform.position.x > 100f || base.transform.position.x < -100f || base.transform.position.z > 100f || base.transform.position.z < -100f)
		{
			Object.DestroyObject(base.gameObject);
		}
		else if (!cCtl.isGrounded || movePsv.y > 0f)
		{
			movePsv += Physics.gravity * Time.deltaTime;
			if (cCtl.Move(movePsv * Time.deltaTime) != CollisionFlags.None)
			{
				movePsv = Vector3.zero;
			}
			if (base.transform.position.y < 0f)
			{
				base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			}
		}
		else
		{
			if (base.IsDead)
			{
				return;
			}
			if (bFlying)
			{
				bFlying = false;
				AgentEnable();
			}
			if (base.IsFrozen)
			{
				return;
			}
			Ray ray = new Ray(base.transform.position + Vector3.up * cCtl.height, base.transform.forward);
			int layerMask = (1 << LayerMask.NameToLayer("Obstacle")) | (1 << LayerMask.NameToLayer("Target"));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, cCtl.radius * base.transform.localScale.x + 0.2f, layerMask))
			{
				obstacleCom = hitInfo.collider.gameObject.GetComponent<COMA_Target>();
				if (category != EnemyCategory.Enemy04)
				{
					PlayMyAnimation("Attack", 0.1f);
					return;
				}
				base.hp = 0f;
				PlayMyAnimation("Death", 0.1f);
				return;
			}
			bool flag = false;
			if (playerTrs != null && (playerTrs.position - base.transform.position).sqrMagnitude < view * view)
			{
				playerCom = COMA_PlayerSelf.Instance;
				if (!playerCom.IsDead)
				{
					Vector3 direction = playerTrs.position - base.transform.position;
					Ray ray2 = new Ray(base.transform.position + Vector3.up * cCtl.height, direction);
					int layerMask2 = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle")) | (1 << LayerMask.NameToLayer("Target"));
					RaycastHit hitInfo2;
					if (!Physics.Raycast(ray2, out hitInfo2, direction.magnitude, layerMask2))
					{
						if (direction.sqrMagnitude < 1.21f)
						{
							if (category != EnemyCategory.Enemy04)
							{
								PlayMyAnimation("Attack", 0.1f);
							}
							else
							{
								base.hp = 0f;
								PlayMyAnimation("Death", 0.1f);
							}
						}
						else
						{
							direction.y = 0f;
							Vector3 vector = direction.normalized * speedRun;
							if (base.IsVenom)
							{
								vector *= 0.4f;
							}
							cCtl.Move(vector * Time.deltaTime);
							base.transform.forward = Vector3.Lerp(base.transform.forward, vector, 0.1f);
						}
						flag = true;
					}
				}
			}
			if (bFindPlayerLast && !flag)
			{
				AgentEnable();
			}
			bFindPlayerLast = flag;
			if (flag)
			{
				return;
			}
			playerCom = null;
			if (curMoveTrs != null)
			{
				if ((curMoveTrs.position - base.transform.position).sqrMagnitude < 1f)
				{
					Vector3 vector2 = targetToAttack.position - base.transform.position;
					vector2.y = 0f;
					moveCur = vector2.normalized * speedRun;
					curMoveTrs = targetToAttack;
				}
				movePsv = Vector3.zero;
				moveCur.y = 0f;
				Vector3 vector3 = moveCur;
				if (base.IsVenom)
				{
					vector3 *= 0.4f;
				}
				CollisionFlags collisionFlags = cCtl.Move(vector3 * Time.deltaTime);
				if (collisionFlags == CollisionFlags.Sides)
				{
					AgentEnable();
				}
				base.transform.forward = Vector3.Lerp(base.transform.forward, moveCur, 0.1f);
			}
			else
			{
				AgentEnable();
			}
		}
	}

	protected void AgentEnable()
	{
		if (targetToAttack == null)
		{
			return;
		}
		Vector3 direction = targetToAttack.position - base.transform.position;
		Ray ray = new Ray(base.transform.position + Vector3.up * cCtl.height, direction);
		int layerMask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit hitInfo;
		if (!Physics.Raycast(ray, out hitInfo, direction.magnitude, layerMask))
		{
			curMoveTrs = targetToAttack;
			direction.y = 0f;
			moveCur = direction.normalized * speedRun;
			return;
		}
		Transform[] wayPoints = COMA_WayPoint.Instance.wayPoints;
		foreach (Transform transform in wayPoints)
		{
			direction = transform.position - base.transform.position;
			ray = new Ray(base.transform.position + Vector3.up * cCtl.height, direction);
			layerMask = 1 << LayerMask.NameToLayer("Ground");
			if (!Physics.Raycast(ray, out hitInfo, direction.magnitude, layerMask))
			{
				curMoveTrs = transform;
				direction.y = 0f;
				moveCur = direction.normalized * speedRun;
				break;
			}
		}
	}

	private void Attack()
	{
		if (!COMA_Scene.Instance.runingGameOver)
		{
			if (playerCom != null && (playerCom.transform.position - base.transform.position).sqrMagnitude < 1.45f)
			{
				playerCom.timeToExitAttack = 3f;
				playerCom.ReceiveHurt(base.ap, Vector3.zero);
			}
			else if (obstacleCom != null)
			{
				obstacleCom.BeHit(base.ap);
				obstacleCom = null;
			}
		}
	}

	private void Explode()
	{
		if (!COMA_Scene.Instance.runingGameOver)
		{
			Object.DestroyObject(base.gameObject);
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Bazooka_Brust/Bazooka_Brust")) as GameObject;
			gameObject.transform.position = base.transform.position;
			Object.DestroyObject(gameObject, 2f);
			if (playerCom != null && (playerCom.transform.position - base.transform.position).sqrMagnitude < 1.45f)
			{
				playerCom.timeToExitAttack = 3f;
				playerCom.ReceiveHurt(base.ap, Vector3.zero);
			}
			else if (obstacleCom != null)
			{
				obstacleCom.BeHit(base.ap);
				obstacleCom = null;
			}
		}
	}
}
