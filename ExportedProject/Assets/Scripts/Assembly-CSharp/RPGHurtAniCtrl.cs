using UnityEngine;

public class RPGHurtAniCtrl : MonoBehaviour
{
	private Renderer[] renders = new Renderer[2];

	private float enterT;

	private float enterT1;

	private void Start()
	{
		renders[0] = base.transform.GetChild(0).renderer;
		if (base.transform.childCount > 1)
		{
			renders[1] = base.transform.GetChild(1).GetChild(0).renderer;
		}
		enterT = Time.time;
	}

	private void Update()
	{
	}

	public void AniEnd()
	{
		Object.Destroy(base.gameObject.transform.parent.gameObject, 0.1f);
	}
}
