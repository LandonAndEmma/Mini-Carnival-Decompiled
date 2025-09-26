using UnityEngine;

public class COMA_Maze_Font : MonoBehaviour
{
	public MeshRenderer[] numCom;

	private Material[] mats = new Material[10];

	private int curNumber;

	private void Awake()
	{
		for (int i = 0; i < 10; i++)
		{
			mats[i] = Object.Instantiate(Resources.Load("FBX/SceneAddition/Maze/Golds/Mat_Num" + i)) as Material;
		}
	}

	public void SetNumber(int num)
	{
		if (num != curNumber)
		{
			curNumber = num;
			numCom[0].material = mats[num % 10];
			num /= 10;
			numCom[1].material = mats[num % 10];
			num /= 10;
			numCom[2].material = mats[num % 10];
			base.animation.Play();
		}
	}
}
