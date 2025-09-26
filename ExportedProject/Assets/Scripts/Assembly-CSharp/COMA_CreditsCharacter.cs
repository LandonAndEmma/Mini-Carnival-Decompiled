using UnityEngine;

public class COMA_CreditsCharacter : MonoBehaviour
{
	private float animInterval;

	private float ANIMINTERVAL = 5f;

	public string characterName = "name";

	public float nameHeight = 2.5f;

	private void Start()
	{
		base.animation["Free01"].layer = 1;
		base.animation["Free02"].layer = 1;
		base.animation["Free03"].layer = 1;
		base.animation["Free04"].layer = 1;
		base.animation["Free05"].layer = 1;
		ANIMINTERVAL = Random.Range(3, 9);
		animInterval = ANIMINTERVAL;
		GameObject gameObject = new GameObject("hurtNumberObj");
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position + new Vector3(0f, nameHeight, 0f);
		Font3D font3D = gameObject.AddComponent("Font3D") as Font3D;
		font3D.SetFont("1");
		font3D.SetString(characterName, 0.05f, 0.35f, new Vector3(0f, 0f, 0f), BoardType.Hold);
	}

	private void Update()
	{
		animInterval -= Time.deltaTime;
		if (animInterval < 0f)
		{
			animInterval = ANIMINTERVAL;
			base.animation.CrossFade("Free0" + Random.Range(1, 6));
		}
	}
}
