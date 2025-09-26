using UnityEngine;

public class COMA_TankCharacter_Split : MonoBehaviour
{
	public Transform parentTrans;

	public void spliteTankAndCharacter()
	{
		base.transform.parent = parentTrans;
	}
}
