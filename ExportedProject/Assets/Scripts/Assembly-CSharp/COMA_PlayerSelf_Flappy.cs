using System;
using UnityEngine;

public class COMA_PlayerSelf_Flappy : COMA_PlayerSelf
{
	private Vector3 curGravity = new Vector3(0f, -40f, 0f);

	private Vector3 jumpVel = new Vector3(0f, 12f, 0f);

	private Vector2 angle = new Vector2(-30f, 50f);

	private float angleAccel = 210f;

	private float curAngle;

	private float skyLine = 7f;

	private float groundLine = -4f;

	public ParticleSystem ptl_rocketFire;

	private float deathHold;

	protected new void Start()
	{
		base.hp = HP;
		try
		{
			for (int i = 0; i < characterCom.bodyObjs.Length; i++)
			{
				characterCom.bodyObjs[i].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[i]].texture;
			}
		}
		catch (Exception)
		{
		}
	}

	private new void Update()
	{
		if (base.transform.localPosition.y > skyLine)
		{
			base.transform.localPosition = new Vector3(0f, skyLine, 0f);
		}
	}

	private void LateUpdate()
	{
		if (COMA_Sys.Instance.bCoverUpdate || IsGrounded)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			UI_Jump();
		}
		Vector3 vector = new Vector3(0f, 0f, 0f);
		if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y) > 0.2f)
		{
			moveCur = vector.normalized * speedRun;
		}
		else
		{
			moveInput = Vector2.zero;
			moveCur = Vector3.zero;
		}
		movePsv += curGravity * Time.deltaTime;
		if (deathHold > 0f)
		{
			movePsv = Vector3.zero;
			deathHold -= Time.deltaTime;
		}
		Vector3 localPosition = base.transform.localPosition;
		localPosition += (moveCur + movePsv) * Time.deltaTime;
		localPosition.z = 0f;
		localPosition.x = 0f;
		if (localPosition.y < groundLine)
		{
			localPosition.y = groundLine;
			OnGround();
			OnDead();
			IsGrounded = true;
		}
		base.transform.localPosition = localPosition;
		if (movePsv.y < 0f)
		{
			curAngle += angleAccel * Time.deltaTime;
			if (curAngle > angle.y)
			{
				curAngle = angle.y;
			}
			base.transform.localEulerAngles = new Vector3(curAngle, 0f, 0f);
		}
	}

	public override bool HandleMessage(TTelegram msg)
	{
		if (COMA_Sys.Instance.bCoverUIInput)
		{
			return true;
		}
		if (msg._nMsgId != 30001)
		{
			if (msg._nMsgId == 30000)
			{
				TPCInputEvent tPCInputEvent = (TPCInputEvent)msg._pExtraInfo;
				Debug.Log(string.Concat("Key: ", tPCInputEvent.code, " ", tPCInputEvent.type));
				if (tPCInputEvent.type == EventType.KeyUp && tPCInputEvent.code == KeyCode.R)
				{
					OnRelive();
				}
			}
			else if (msg._nMsgId == 30003)
			{
				TPCInputEvent tPCInputEvent2 = (TPCInputEvent)msg._pExtraInfo;
				if (tPCInputEvent2.code == KeyCode.Mouse0 && tPCInputEvent2.type == EventType.KeyDown)
				{
					UI_Jump();
				}
			}
		}
		return true;
	}

	public override void UI_Jump()
	{
		if (!base.IsDead)
		{
			ptl_rocketFire.Play(true);
			movePsv = jumpVel;
			characterCom.PlayMyAnimation("Fly_up", string.Empty);
			curAngle = angle.x;
			base.transform.localEulerAngles = new Vector3(curAngle, 0f, 0f);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("---------------------------------OnTriggerEnter : " + other.name);
		switch (other.name)
		{
		case "Collider":
			OnDead();
			break;
		case "Collider_ScoreArea":
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Get_Gold, base.transform, 2f, false);
			base.score++;
			Debug.Log("Score:" + base.score);
			COMA_Flappy_SceneController.Instance.UpdateCurrentScore(base.score);
			if (base.score >= 100)
			{
				COMA_Achievement.Instance.FlappyFly100++;
			}
			if (base.score >= 200)
			{
				COMA_Achievement.Instance.FlappyFly200++;
			}
			other.collider.enabled = false;
			break;
		}
	}

	private void OnDead()
	{
		if (!base.IsDead)
		{
			deathHold = 0.2f;
			base.hp = 0f;
			COMA_Flappy_SceneController.Instance.Stop();
			base.rigidbody.Sleep();
		}
	}

	private void OnGround()
	{
		COMA_Flappy_SceneController.Instance.Ground();
	}

	public override bool OnRelive()
	{
		base.rigidbody.WakeUp();
		base.hp = HP;
		IsGrounded = false;
		base.transform.localPosition = Vector3.zero;
		movePsv = Vector3.zero;
		curAngle = 0f;
		base.transform.localEulerAngles = Vector3.zero;
		return false;
	}
}
