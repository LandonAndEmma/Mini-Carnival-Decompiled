using UnityEngine;

public class COMA_BlackHouse_DoorController : MonoBehaviour
{
	public GameObject doorObj;

	private void Start()
	{
	}

	private void OpenDoor()
	{
		doorObj.animation.Play();
	}

	private void Update()
	{
	}
}
