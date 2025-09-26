using UnityEngine;

public class COMA_Target : MonoBehaviour
{
	public float HP = 100f;

	protected float hp;

	protected virtual void Start()
	{
		hp = HP;
	}

	public virtual void BeHit(float hitAp)
	{
		if (hp < 1f)
		{
			return;
		}
		hp -= hitAp;
		if (base.gameObject.name == "TargetDiamond")
		{
			COMA_Castle_SceneController.Instance.targetCrystalCom.CrystalVolume = hp / HP;
		}
		if (hp < 1f)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Particle/effect/Crystal_Broken/Crystal_Broken")) as GameObject;
			gameObject.transform.position = new Vector3(0f, 1.1f, 0f);
			gameObject.transform.eulerAngles = new Vector3(0f, 45f, 0f);
			Object.DestroyObject(gameObject, 2f);
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.FX_Crystal_Broken, gameObject.transform);
			if (!COMA_Scene.Instance.runingGameOver)
			{
				COMA_Scene.Instance.runingGameOver = true;
				SceneTimerInstance.Instance.Add(3f, COMA_Scene.Instance.GameFinishByLocal);
			}
			Object.DestroyObject(base.gameObject);
		}
	}

	public virtual void BeHeal(float healPoint)
	{
	}
}
