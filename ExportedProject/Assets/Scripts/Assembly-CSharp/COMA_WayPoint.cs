using UnityEngine;

public class COMA_WayPoint : MonoBehaviour
{
	private static COMA_WayPoint _instance;

	public Transform[] wayPoints;

	public static COMA_WayPoint Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}
}
