using UnityEngine;

public class GotTapPointsMono : MonoBehaviour
{
	private static GotTapPointsMono m_instance;

	public int tapjoy_reward;

	public static GotTapPointsMono Instance
	{
		get
		{
			return m_instance;
		}
	}

	private void Awake()
	{
		m_instance = this;
	}

	private void GotTapPoints(int tapPoints)
	{
		tapjoy_reward = tapPoints;
		Debug.Log("Tapjoy return : " + tapPoints);
		COMA_Pref.Instance.AddCrystal(tapPoints);
	}
}
