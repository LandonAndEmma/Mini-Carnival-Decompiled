public class COMA_Buff
{
	public enum Buff
	{
		None = 0,
		Ice = 1,
		Confused = 2,
		Venom = 3,
		TankEffect = 4,
		GetInvincible = 5,
		Invincible = 6,
		GetSpeedUp = 7,
		SpeedUp = 8,
		GetFlash = 9,
		GetBulletIgnore = 10,
		Flash = 11,
		Evil = 12,
		Hide = 13,
		Heal = 14,
		Doodle = 15,
		BeGod = 16,
		GoldAdsorb = 17,
		Exhaust = 18,
		Inverted = 19,
		EffectInverted = 20,
		RideRocket = 21,
		Glue = 22,
		Mine = 23,
		SelfBlast = 24,
		Blood_Stun = 25
	}

	private static COMA_Buff _instance;

	public float lastTime_ice = 3f;

	public float lastTime_confused = 4f;

	public float lastTime_venom = 4f;

	public float lastTime_tank_speedUp = 5f;

	public float lastTime_invincible = 10f;

	public float lastTime_speedUp = 10f;

	public float lastTime_doubleScore = 30f;

	public float lastTime_oneShot = 15f;

	public float lastTime_hide = 10f;

	public float lastTime_flashHitGround = 0.5f;

	public float lastTime_run_speedUp = 3f;

	public float lastTime_run_flashHitGround = 1f;

	public float lastTime_run_goldAdsorb = 5f;

	public float lastTime_run_exhaust = 5f;

	public float lastTime_run_inverted = 3f;

	public float lastTime_run_ride_rocket = 8f;

	public float lastTime_run_glue = 10f;

	public float lastTime_blood_shotRed = 0.2f;

	public float lastTime_blood_stun = 2f;

	public static COMA_Buff Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_Buff();
			}
			return _instance;
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}
}
