using UnityEngine;

public class COMA_PlayerSyncCharacter : COMA_PlayerCharacter
{
	public delegate void onPlayRecievedAnim(string strAnimName, float fSpeed);

	public onPlayRecievedAnim _onPlayRecieveAnim;

	protected override void Awake()
	{
		base.Awake();
	}

	protected new void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.PLAYER_ANIMATION, ReceiveAnimation);
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.PLAYER_ANIMATION, ReceiveAnimation);
		base.OnDisable();
	}

	private void ReceiveAnimation(COMA_CommandDatas commandDatas)
	{
		if (commandDatas.dataSender.Id.ToString() != playerCom.gameObject.name)
		{
			return;
		}
		COMA_CD_PlayerAnimation cOMA_CD_PlayerAnimation = commandDatas as COMA_CD_PlayerAnimation;
		if (!animIndexToName.ContainsKey(cOMA_CD_PlayerAnimation.btAnimName))
		{
			return;
		}
		string text = animIndexToName[cOMA_CD_PlayerAnimation.btAnimName];
		float fadeTime = (float)(int)cOMA_CD_PlayerAnimation.btFadeTime / 100f;
		float playSpeed = (float)(int)cOMA_CD_PlayerAnimation.btPlaySeed / 10f;
		Vector3 extra = cOMA_CD_PlayerAnimation.extra;
		if (text == "Tank_Fire" && COMA_Tank_SceneController.Instance != null)
		{
			Debug.Log("anim tank fire:" + extra);
			Transform parent = base.transform.parent;
			if (parent != null)
			{
				Debug.Log("------check mount");
				Transform parent2 = parent.parent.parent;
				if (parent2 != null && parent2.name == "Turret")
				{
					Debug.Log("-------------set extra");
					parent2.eulerAngles = extra;
				}
			}
		}
		PlayMyAnimation(text, fadeTime, playSpeed);
		playerCom.NotifyPlayerExtraProcess(commandDatas, text);
	}

	private void PlayMyAnimation(string animName, float fadeTime, float playSpeed)
	{
		if (base.animation[animName] == null)
		{
			return;
		}
		if (playSpeed == 0f)
		{
			base.animation.Stop(animName);
		}
		else
		{
			if (animName.StartsWith("Death"))
			{
				playerCom.hp = 0f;
			}
			base.animation[animName].speed = playSpeed;
			if (fadeTime == 0f)
			{
				base.animation.Play(animName);
			}
			else
			{
				base.animation.CrossFade(animName, fadeTime);
			}
		}
		if (_onPlayRecieveAnim != null)
		{
			_onPlayRecieveAnim(animName, playSpeed);
		}
	}

	private void OnWillRenderObject()
	{
		Debug.Log("OnWillRenderObject");
	}
}
